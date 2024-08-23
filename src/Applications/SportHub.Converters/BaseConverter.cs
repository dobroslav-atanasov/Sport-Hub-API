namespace SportHub.Converters;

using System.Text;

using Dasync.Collections;

using HtmlAgilityPack;

using Microsoft.Extensions.Logging;

using SportHub.Common.Extensions;
using SportHub.Data.Entities.Crawlers;
using SportHub.Data.Models.Converters.OlympicGames;
using SportHub.Data.Models.Enumerations.OlympicGames;
using SportHub.Services.Data.CrawlerStorageDb.Interfaces;
using SportHub.Services.Data.OlympicGamesDb.Interfaces;
using SportHub.Services.Interfaces;

public abstract class BaseConverter
{
    private readonly ILogger<BaseConverter> logger;
    private readonly ICrawlersService crawlersService;
    private readonly ILogsService logsService;
    private readonly IGroupsService groupsService;
    private readonly IZipService zipService;

    public BaseConverter(ILogger<BaseConverter> logger, ICrawlersService crawlersService, ILogsService logsService, IGroupsService groupsService, IZipService zipService,
        INormalizeService normalizeService, IDataCacheService dataCacheService)
    {
        this.logger = logger;
        this.crawlersService = crawlersService;
        this.logsService = logsService;
        this.groupsService = groupsService;
        this.zipService = zipService;
        this.NormalizeService = normalizeService;
        this.DataCacheService = dataCacheService;
    }

    protected INormalizeService NormalizeService { get; }

    protected IDataCacheService DataCacheService { get; }

    protected abstract Task ProcessGroupAsync(Group group);

    public async Task ConvertAsync(string crawlerName)
    {
        this.logger.LogInformation($"Converter: {crawlerName} start.");

        try
        {
            var crawlerId = await this.crawlersService.GetCrawlerIdAsync(crawlerName);
            var identifiers = await this.logsService.GetLogIdentifiersAsync(crawlerId);

            //            identifiers = new List<Guid>
            //            {
            //            };

            await identifiers.ParallelForEachAsync(async identifier =>
            {
                try
                {
                    var group = await this.groupsService.GetGroupAsync(identifier);
                    var zipModels = this.zipService.UnzipGroup(group.Content);
                    foreach (var document in group.Documents)
                    {
                        var zipModel = zipModels.First(z => z.Name == document.Name);
                        document.Content = zipModel.Content;
                    }

                    await this.ProcessGroupAsync(group);
                }
                catch (Exception ex)
                {
                    this.logger.LogError(ex, $"Group was not process: {identifier};");
                }
            }, maxDegreeOfParallelism: 1);
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, $"Failed to process documents from converter: {crawlerName};");
        }

        this.logger.LogInformation($"Converter: {crawlerName} end.");
    }

    protected string GenerateCode(CodeInfo codeInfo, CodeEnum to)
    {
        var code = $"{codeInfo.GameType}-{codeInfo.Year}-{codeInfo.Discipline}-{codeInfo.Gender}-{codeInfo.Event}".PadRight(26, '-');

        switch (to)
        {
            case CodeEnum.Phase:
                code = $"{code}{codeInfo.Phase}".PadRight(39, '-');
                break;
            case CodeEnum.Unit:
                code = $"{code}{codeInfo.Phase}-{codeInfo.Unit}";
                break;
        }
        return code;
    }

    protected HtmlDocument CreateHtmlDocument(Document document)
    {
        var encoding = Encoding.GetEncoding(document.Encoding);
        var html = encoding.GetString(document.Content).Decode();
        var htmlDocument = new HtmlDocument();
        htmlDocument.LoadHtml(html);

        return htmlDocument;
    }

    protected string GetGender(string text)
    {
        var gender = string.Empty;
        switch (text)
        {
            case "Men's": gender = "M"; break;
            case "Women's": gender = "W"; break;
            case "Mixed": gender = "X"; break;
            case "Open": gender = "O"; break;
        }

        return gender;
    }
}