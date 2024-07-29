namespace SportHub.Converters.OlympicGames.Olympedia;

using Microsoft.Extensions.Logging;

using SportHub.Common.Extensions;
using SportHub.Data.Models.Converters.OlympicGames;
using SportHub.Data.Models.DbEntities.Crawlers;
using SportHub.Data.Models.Enumerations.OlympicGames;
using SportHub.Services.Data.CrawlerStorageDb.Interfaces;
using SportHub.Services.Data.OlympicGamesDb.Interfaces;
using SportHub.Services.Interfaces;

public abstract class BaseOlympediaConverter : BaseConverter
{
    public BaseOlympediaConverter(ILogger<BaseConverter> logger, ICrawlersService crawlersService, ILogsService logsService, IGroupsService groupsService, IZipService zipService
        , IRegExpService regExpService, INormalizeService normalizeService, IOlympediaService olympediaService, IDataCacheService dataCacheService)
        : base(logger, crawlersService, logsService, groupsService, zipService)
    {
        this.NormalizeService = normalizeService;
        this.OlympediaService = olympediaService;
        this.RegExpService = regExpService;
        this.DataCacheService = dataCacheService;
    }

    protected INormalizeService NormalizeService { get; }

    protected IOlympediaService OlympediaService { get; }

    protected IRegExpService RegExpService { get; }

    protected IDataCacheService DataCacheService { get; }

    protected List<ConverterModel> PrepareConverterModels(Group group)
    {
        var models = new List<ConverterModel>();

        foreach (var document in group.Documents)
        {
            var model = new ConverterModel
            {
                HtmlDocument = this.CreateHtmlDocument(document),
                Order = document.Order
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

            models.Add(model);
        }

        return models;
    }

    private void SetIsForbiddenEvent(ConverterModel model)
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

    private void SetEventInfo(ConverterModel model)
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

    private void SetHeader(ConverterModel model)
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

    //protected GameCache GetGameFromDatabase(HtmlDocument htmlDocument)
    //{
    //    var headers = htmlDocument.DocumentNode.SelectSingleNode("//ol[@class='breadcrumb']");
    //    var match = this.RegExpService.Match(headers.OuterHtml, @"<a href=""\/editions\/(?:\d+)"">(\d+)\s*(\w+)\s*Olympics<\/a>");

    //    if (match != null)
    //    {
    //        var year = int.Parse(match.Groups[1].Value);
    //        var type = match.Groups[2].Value.Trim();

    //        if (type.ToLower() == "equestrian")
    //        {
    //            type = "Summer";
    //        }

    //        var gameType = type.ToEnum<GameTypeEnum>();
    //        return null;
    //        //var game = this.DataCacheService.Games.FirstOrDefault(g => g.Year == year && g.Type == gameType);

    //        //return game;
    //    }

    //    return null;
    //}

    //protected DisciplineCache GetDisciplineFromDatabase(HtmlDocument htmlDocument)
    //{
    //    var headers = htmlDocument.DocumentNode.SelectSingleNode("//ol[@class='breadcrumb']");
    //    var name = this.RegExpService.MatchFirstGroup(headers.OuterHtml, @"<a href=""\/editions\/[\d]+\/sports\/(?:.*?)"">(.*?)<\/a>");
    //    var eventName = this.RegExpService.MatchFirstGroup(headers.OuterHtml, @"<li\s*class=""active"">(.*?)<\/li>");

    //    if (name != null && eventName != null)
    //    {
    //        if (name.ToLower() == "wrestling")
    //        {
    //            if (eventName.ToLower().Contains("freestyle"))
    //            {
    //                name = "Wrestling Freestyle";
    //            }
    //            else
    //            {
    //                name = "Wrestling Greco-Roman";
    //            }
    //        }
    //        else if (name.ToLower() == "canoe marathon")
    //        {
    //            name = "Canoe Sprint";
    //        }

    //        var discipline = this.DataCacheService.Disciplines.FirstOrDefault(d => d.Name == name);

    //        return discipline;
    //    }

    //    return null;
    //}

    //protected Dictionary<string, int> GetHeaderIndexes(HtmlDocument document)
    //{
    //    var headers = document
    //        .DocumentNode
    //        .SelectSingleNode("//table[@class='table table-striped']/thead/tr")
    //        .Elements("th")
    //        .Select(x => x.InnerText.ToLower().Trim())
    //        //.Where(x => !string.IsNullOrEmpty(x))
    //        .ToList();

    //    var indexes = this.OlympediaService.FindIndexes(headers);

    //    return indexes;
    //}

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

    private void AddInfo(ConverterModel model)
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