namespace SportHub.Converters;

using System.Text;

using Dasync.Collections;

using HtmlAgilityPack;

using Microsoft.Extensions.Logging;

using SportHub.Common.Extensions;
using SportHub.Data.Models.Entities.Crawlers;
using SportHub.Services.Data.CrawlerStorageDb.Interfaces;
using SportHub.Services.Interfaces;

public abstract class BaseConverter
{
    private readonly ICrawlersService crawlersService;
    private readonly ILogsService logsService;
    private readonly IGroupsService groupsService;
    private readonly IZipService zipService;

    public BaseConverter(ILogger<BaseConverter> logger, ICrawlersService crawlersService, ILogsService logsService, IGroupsService groupsService, IZipService zipService)
    {
        this.Logger = logger;
        this.crawlersService = crawlersService;
        this.logsService = logsService;
        this.groupsService = groupsService;
        this.zipService = zipService;
    }

    protected ILogger<BaseConverter> Logger { get; }

    protected abstract Task ProcessGroupAsync(Group group);

    public async Task ConvertAsync(string crawlerName)
    {
        this.Logger.LogInformation($"Converter: {crawlerName} start.");

        try
        {
            var crawlerId = await this.crawlersService.GetCrawlerIdAsync(crawlerName);
            var identifiers = await this.logsService.GetLogIdentifiersAsync(crawlerId);

            identifiers = new List<Guid>
            {
Guid.Parse("40d1b281-8f24-451f-9309-c960f7ace814"),
Guid.Parse("7958614a-8764-44f2-b01e-f029d8a2aa1e"),
Guid.Parse("38d0c4ad-0381-4722-aade-c43209f2e6c3"),
Guid.Parse("d812052c-6ede-459d-b214-84dca85f660f"),
Guid.Parse("3d12e018-01f1-4306-babf-a6b8018e1aac"),
Guid.Parse("e95ca427-540e-4bdf-8515-ebcd3558ac63"),
Guid.Parse("b0b40fac-90c9-47a6-a32f-721c4e4f7b46"),
Guid.Parse("2508985b-acde-4b01-9edc-fe2eaa9e24a1"),
Guid.Parse("f37f7051-04ec-43d9-a742-d71dd8f14f3d"),
Guid.Parse("f3d0d36c-6f9d-4135-92f8-86974edb807e"),
Guid.Parse("f05667b9-2590-4287-9725-5b973d2a9945"),
Guid.Parse("4bf6c34c-2674-46ee-bdad-0f9022504594"),
Guid.Parse("aa803634-f6b5-4d94-a079-172e4807a932"),
Guid.Parse("1885be6f-fb77-464c-9e1c-0795e0e75214"),
Guid.Parse("7752d437-f121-449c-9d85-b23bf24f9d71"),
Guid.Parse("fcd087db-86c7-4828-94f5-914373712c41"),
Guid.Parse("5eb46747-c805-49f1-8478-490063b19f2e"),
Guid.Parse("52ab85b8-310e-4087-a047-26e7edd5e143"),
Guid.Parse("1bc429c1-36e3-42a1-89a3-4c3f2f57e20d"),
Guid.Parse("17790cfd-fc0b-4cbb-907e-d44221cacb1f"),
Guid.Parse("70dd0f14-ada8-4ea3-9ff6-8f5e34f13342"),
Guid.Parse("087b8fab-572b-4dac-b655-31d12040d92b"),
Guid.Parse("5567d1bc-14e0-44cf-894b-fe8a3969d22a"),
Guid.Parse("e72dc7f2-051a-4589-ba37-e5822cae8627"),
Guid.Parse("918bedb2-0bd0-4581-a380-d6114408809c"),
Guid.Parse("508d44cd-7bdd-49f4-b415-a5f1ca65f06b"),
Guid.Parse("9c375449-a876-4782-bcef-d7dfad7902a1"),
Guid.Parse("27d6b080-a03f-48c2-b629-12ef5aa3696f"),
Guid.Parse("6496c589-e160-4dad-9a5e-35aab11ff4d3"),
Guid.Parse("b93970b5-f0c4-4dd0-bef2-659c9147eddc"),
Guid.Parse("ed3c5054-b51d-4c1c-8526-daa00aad87f5"),
Guid.Parse("b6d6d4bc-6072-4960-9b01-d9913fa3a1f5"),
Guid.Parse("607a9e7e-5706-40bc-8176-ed5980064cb9"),
Guid.Parse("0f9d99b6-1337-4ed8-b3a0-6c1724c55e91"),



            };

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
                    this.Logger.LogError(ex, $"Group was not process: {identifier};");
                }
            }, maxDegreeOfParallelism: 1);
        }
        catch (Exception ex)
        {
            this.Logger.LogError(ex, $"Failed to process documents from converter: {crawlerName};");
        }

        this.Logger.LogInformation($"Converter: {crawlerName} end.");
    }

    protected HtmlDocument CreateHtmlDocument(Document document)
    {
        var encoding = Encoding.GetEncoding(document.Encoding);
        var html = encoding.GetString(document.Content).Decode();
        var htmlDocument = new HtmlDocument();
        htmlDocument.LoadHtml(html);

        return htmlDocument;
    }
}