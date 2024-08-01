namespace SportHub.Converters;

using System.Text;

using Dasync.Collections;

using HtmlAgilityPack;

using Microsoft.Extensions.Logging;

using SportHub.Common.Extensions;
using SportHub.Data.Models.Converters.OlympicGames;
using SportHub.Data.Models.Converters.OlympicGames.Olympedia.Base;
using SportHub.Data.Models.Converters.OlympicGames.Paris2024.Base;
using SportHub.Data.Models.DbEntities.Crawlers;
using SportHub.Data.Models.Enumerations.OlympicGames;
using SportHub.Services.Data.CrawlerStorageDb.Interfaces;
using SportHub.Services.Interfaces;

public abstract class BaseConverter
{
    private readonly ILogger<BaseConverter> logger;
    private readonly ICrawlersService crawlersService;
    private readonly ILogsService logsService;
    private readonly IGroupsService groupsService;
    private readonly IZipService zipService;

    public BaseConverter(ILogger<BaseConverter> logger, ICrawlersService crawlersService, ILogsService logsService, IGroupsService groupsService, IZipService zipService,
        INormalizeService normalizeService, IRegExpService regExpService)
    {
        this.logger = logger;
        this.crawlersService = crawlersService;
        this.logsService = logsService;
        this.groupsService = groupsService;
        this.zipService = zipService;
        this.NormalizeService = normalizeService;
        this.RegExpService = regExpService;
    }

    protected ConverterModel Model { get; set; }

    protected INormalizeService NormalizeService { get; }

    protected IRegExpService RegExpService { get; }

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

                    this.PrepareConverterModel(group, crawlerName);
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

    protected HtmlDocument CreateHtmlDocument(Document document)
    {
        var encoding = Encoding.GetEncoding(document.Encoding);
        var html = encoding.GetString(document.Content).Decode();
        var htmlDocument = new HtmlDocument();
        htmlDocument.LoadHtml(html);

        return htmlDocument;
    }

    private void PrepareConverterModel(Group group, string crawlerName)
    {
        this.Model = new ConverterModel
        {
            Identifier = group.Identifier,
            CrawlerId = group.CrawlerId,
            Name = group.Name,
        };

        if (crawlerName.Contains("Paris2024"))
        {
            foreach (var document in group.Documents)
            {
                var encoding = Encoding.GetEncoding(document.Encoding);
                this.Model.Paris2024Documents.Add(document.Order,
                    new Paris2024ConverterModel
                    {
                        Encoding = encoding,
                        Json = encoding.GetString(document.Content)
                    });
            }
        }
        else if (crawlerName.Contains("Olympedia"))
        {
            foreach (var document in group.Documents)
            {
                var model = new OlympediaDocumentModel
                {
                    HtmlDocument = this.CreateHtmlDocument(document),
                };

                model.Title = model.HtmlDocument.DocumentNode.SelectSingleNode("//h1").InnerText.Trim();

                if (int.TryParse(new Uri(document.Url).Segments.Last(), out var pageId))
                {
                    model.PageId = pageId;
                }

                this.SetHeader(model);
                this.SetEventInfo(model);
                this.SetIsForbiddenEvent(model);

                //var eventCache = this.DataCacheService
                //    .Events
                //    .FirstOrDefault(x => x.OriginalName == model.EventInfo.OriginalName && x.GameId == model.GameCache.Id && x.DisciplineId == model.DisciplineCache.Id);

                //model.EventCache = eventCache;

                this.Model.OlympediaDocuments.Add(document.Order, model);
            }
        }
    }

    private void SetIsForbiddenEvent(OlympediaDocumentModel model)
    {
        var list = new List<string>
        {
            "1900-Archery-None Event, Men",
            "1920-Shooting-None Event, Men",
            "1904-Artistic Gymnastics-Individual All-Around, Field Sports, Men"
        };

        var isForbidden = list.Any(x => x == $"{model.GameCache?.Year}-{model.DisciplineCache?.Name}-{model.EventInfo?.OriginalName}");
        model.EventInfo.IsForbidden = isForbidden;
    }

    private void SetEventInfo(OlympediaDocumentModel model)
    {
        if (model.GameCache != null && model.DisciplineCache != null)
        {
            model.EventInfo.OriginalName = model.Header.Event;
            model.EventInfo.Name = this.NormalizeService.NormalizeEventName(model);

            var parts = model.EventInfo.Name.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();
            var gender = parts.Last().Trim();
            model.EventInfo.Name = string.Join("|", parts.Take(parts.Count - 1).Select(x => x.Trim()).ToList());

            this.AddInfo(model);

            if (model.DisciplineCache.Name == "Wrestling Freestyle")
            {
                model.EventInfo.Name = model.EventInfo.Name.Replace("Freestyle", string.Empty);
            }
            else if (model.DisciplineCache.Name == "Wrestling Greco-Roman")
            {
                model.EventInfo.Name = model.EventInfo.Name.Replace("Greco-Roman", string.Empty);
            }

            if (this.RegExpService.IsMatch(model.EventInfo.Name, @"Team"))
            {
                model.EventInfo.Name = this.RegExpService.Replace(model.EventInfo.Name, @"Team", string.Empty);
                model.EventInfo.Name = $"Team|{model.EventInfo.Name}";
            }

            if (this.RegExpService.IsMatch(model.EventInfo.Name, @"Individual"))
            {
                model.EventInfo.Name = this.RegExpService.Replace(model.EventInfo.Name, @"Individual", string.Empty);
                model.EventInfo.Name = $"Individual|{model.EventInfo.Name}";
            }

            var nameParts = model.EventInfo.Name.Split(new[] { " ", "|" }, StringSplitOptions.RemoveEmptyEntries).Select(x => x.UpperFirstChar()).ToList();
            var name = string.Join(" ", nameParts);

            model.EventInfo.Name = $"{gender} {name}";
            var prefix = this.ConvertEventPrefix(gender);
            model.EventInfo.NormalizedName = $"{prefix} {name.ToLower()}";
        }
        else
        {
            model.IsValidEvent = false;
        }
    }

    private void SetHeader(OlympediaDocumentModel model)
    {
        var headers = model.HtmlDocument.DocumentNode.SelectSingleNode("//ol[@class='breadcrumb']");
        var gameMatch = this.RegExpService.Match(headers?.OuterHtml, @"<a href=""\/editions\/(?:\d+)"">(\d+)\s*(\w+)\s*Olympics<\/a>");

        if (gameMatch != null)
        {
            model.Header.GameYear = int.Parse(gameMatch.Groups[1].Value);
            var type = gameMatch.Groups[2].Value.Trim();

            if (type.Equals("equestrian", StringComparison.CurrentCultureIgnoreCase))
            {
                type = "Summer";
            }

            model.Header.GameType = type.ToEnum<GameTypeEnum>();
            //model.GameCache = this.DataCacheService.Games.FirstOrDefault(x => x.Year == model.Header.GameYear && x.Type == model.Header.GameType);
        }

        var discipline = this.RegExpService.MatchFirstGroup(headers?.OuterHtml, @"<a href=""\/editions\/[\d]+\/sports\/(?:.*?)"">(.*?)<\/a>");
        var @event = this.RegExpService.MatchFirstGroup(headers?.OuterHtml, @"<li\s*class=""active"">(.*?)<\/li>");

        if (discipline != null && @event != null)
        {
            if (discipline.Equals("wrestling", StringComparison.CurrentCultureIgnoreCase))
            {
                if (@event.Contains("freestyle", StringComparison.CurrentCultureIgnoreCase))
                {
                    discipline = "Wrestling Freestyle";
                }
                else
                {
                    discipline = "Wrestling Greco-Roman";
                }
            }
            else if (discipline.Equals("canoe marathon", StringComparison.CurrentCultureIgnoreCase))
            {
                discipline = "Canoe Sprint";
            }

            model.Header.Discipline = discipline;
            model.Header.Event = @event;
            //model.DisciplineCache = this.DataCacheService.Disciplines.FirstOrDefault(x => x.Name == model.Header.Discipline);
        }
    }

    private string ConvertEventPrefix(string gender)
    {

        switch (gender.ToLower())
        {
            case "men":
                gender = "Men's";
                break;
            case "women":
                gender = "Women's";
                break;
            case "mixed":
            case "open":
                gender = "Mixed";
                break;
        }

        return gender;
    }

    private void AddInfo(OlympediaDocumentModel model)
    {
        var match = this.RegExpService.Match(model.EventInfo.Name, @"\(.*?\)");
        if (match != null)
        {
            var text = match.Groups[0].Value;
            model.EventInfo.Name = model.EventInfo.Name.Replace(text, string.Empty).Trim();

            var poundMatch = this.RegExpService.Match(text, @"(\+|-)([\d\.]+)\s*pounds");
            var kilogramMatch = this.RegExpService.Match(text, @"(\+|-)([\d\.]+)\s*kilograms");
            var otherMatch = this.RegExpService.Match(text, @"\((.*?)\)");
            if (poundMatch != null)
            {
                var weight = double.Parse(poundMatch.Groups[2].Value).ConvertPoundToKilograms();
                model.EventInfo.AdditionalInfo = $"{poundMatch.Groups[1].Value.Trim()}{weight:F2}kg";
            }
            else if (kilogramMatch != null)
            {
                var weight = double.Parse(kilogramMatch.Groups[2].Value);
                model.EventInfo.AdditionalInfo = $"{kilogramMatch.Groups[1].Value.Trim()}{weight}kg";
            }
            else if (otherMatch != null)
            {
                model.EventInfo.AdditionalInfo = otherMatch.Value.Replace("(", string.Empty).Replace(")", string.Empty).Trim();
            }
        }
    }
}