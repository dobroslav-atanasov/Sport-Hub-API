namespace SportHub.Converters.OlympicGames;

using HtmlAgilityPack;

using Microsoft.Extensions.Logging;

using SportHub.Common.Extensions;
using SportHub.Converters;
using SportHub.Data.Models.Cache;
using SportHub.Data.Models.Converters.OlympicGames;
using SportHub.Data.Models.Entities.OlympicGames.Enumerations;
using SportHub.Services.Data.CrawlerStorageDb.Interfaces;
using SportHub.Services.Data.OlympicGamesDb.Interfaces;
using SportHub.Services.Interfaces;

public abstract class BaseOlympediaConverter : BaseConverter
{
    public BaseOlympediaConverter(ILogger<BaseConverter> logger, ICrawlersService crawlersService, ILogsService logsService, IGroupsService groupsService, IZipService zipService
        , IRegExpService regExpService, INormalizeService normalizeService, IOlympediaService olympediaService, IDataCacheService dataCacheService)
        : base(logger, crawlersService, logsService, groupsService, zipService)
    {
        this.RegExpService = regExpService;
        this.NormalizeService = normalizeService;
        this.OlympediaService = olympediaService;
        this.DataCacheService = dataCacheService;
    }

    protected IRegExpService RegExpService { get; }

    protected INormalizeService NormalizeService { get; }

    protected IOlympediaService OlympediaService { get; }

    protected IDataCacheService DataCacheService { get; }

    protected GameCache GetGameFromDatabase(HtmlDocument htmlDocument)
    {
        var headers = htmlDocument.DocumentNode.SelectSingleNode("//ol[@class='breadcrumb']");
        var match = this.RegExpService.Match(headers.OuterHtml, @"<a href=""\/editions\/(?:\d+)"">(\d+)\s*(\w+)\s*Olympics<\/a>");

        if (match != null)
        {
            var year = int.Parse(match.Groups[1].Value);
            var type = match.Groups[2].Value.Trim();

            if (type.ToLower() == "equestrian")
            {
                type = "Summer";
            }

            var olympicGameType = type.ToEnum<OlympicGameTypeEnum>();// this.DataCacheService.OlympicGameTypes.FirstOrDefault(x => x.Name.Equals(type, StringComparison.CurrentCultureIgnoreCase));
            var game = this.DataCacheService.Games.FirstOrDefault(g => g.Year == year && g.OlympicGameTypeId == (int)olympicGameType);

            return game;
        }

        return null;
    }

    protected DisciplineCache GetDisciplineFromDatabase(HtmlDocument htmlDocument)
    {
        var headers = htmlDocument.DocumentNode.SelectSingleNode("//ol[@class='breadcrumb']");
        var name = this.RegExpService.MatchFirstGroup(headers.OuterHtml, @"<a href=""\/editions\/[\d]+\/sports\/(?:.*?)"">(.*?)<\/a>");
        var eventName = this.RegExpService.MatchFirstGroup(headers.OuterHtml, @"<li\s*class=""active"">(.*?)<\/li>");

        if (name != null && eventName != null)
        {
            if (name.ToLower() == "wrestling")
            {
                if (eventName.ToLower().Contains("freestyle"))
                {
                    name = "Wrestling Freestyle";
                }
                else
                {
                    name = "Wrestling Greco-Roman";
                }
            }
            else if (name.ToLower() == "canoe marathon")
            {
                name = "Canoe Sprint";
            }

            var discipline = this.DataCacheService.Disciplines.FirstOrDefault(d => d.Name == name);

            return discipline;
        }

        return null;
    }

    protected EventModel CreateEventModel(string originalEventName, GameCache gameCache, DisciplineCache disciplineCache)
    {
        if (gameCache != null && disciplineCache != null)
        {
            var eventModel = new EventModel
            {
                OriginalName = originalEventName,
                GameId = gameCache.Id,
                GameYear = gameCache.Year,
                DisciplineId = disciplineCache.Id,
                DisciplineName = disciplineCache.Name,
                Name = this.NormalizeService.NormalizeEventName(originalEventName, gameCache.Year, disciplineCache.Name)
            };

            var parts = eventModel.Name.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();
            var gender = parts.Last().Trim();
            eventModel.Name = string.Join("|", parts.Take(parts.Count - 1).Select(x => x.Trim()).ToList());

            this.AddInfo(eventModel);

            if (disciplineCache.Name == "Wrestling Freestyle")
            {
                eventModel.Name = eventModel.Name.Replace("Freestyle", string.Empty);
            }
            else if (disciplineCache.Name == "Wrestling Greco-Roman")
            {
                eventModel.Name = eventModel.Name.Replace("Greco-Roman", string.Empty);
            }

            if (this.RegExpService.IsMatch(eventModel.Name, @"Team"))
            {
                eventModel.Name = this.RegExpService.Replace(eventModel.Name, @"Team", string.Empty);
                eventModel.Name = $"Team|{eventModel.Name}";
            }

            if (this.RegExpService.IsMatch(eventModel.Name, @"Individual"))
            {
                eventModel.Name = this.RegExpService.Replace(eventModel.Name, @"Individual", string.Empty);
                eventModel.Name = $"Individual|{eventModel.Name}";
            }

            var nameParts = eventModel.Name.Split(new[] { " ", "|" }, StringSplitOptions.RemoveEmptyEntries).Select(x => x.UpperFirstChar()).ToList();
            var name = string.Join(" ", nameParts);

            eventModel.Name = $"{gender} {name}";
            var prefix = this.ConvertEventPrefix(gender);
            eventModel.NormalizedName = $"{prefix} {name.ToLower()}";

            return eventModel;
        }

        return null;
    }

    protected bool CheckForbiddenEvent(string eventName, string disciplineName, int year)
    {
        var list = new List<string>
        {
            "1900-Archery-None Event, Men",
            "1920-Shooting-None Event, Men",
            "1904-Artistic Gymnastics-Individual All-Around, Field Sports, Men"
        };

        var isForbidden = list.Any(x => x == $"{year}-{disciplineName}-{eventName}");
        return isForbidden;
    }

    protected Dictionary<string, int> GetHeaderIndexes(HtmlDocument document)
    {
        var headers = document
            .DocumentNode
            .SelectSingleNode("//table[@class='table table-striped']/thead/tr")
            .Elements("th")
            .Select(x => x.InnerText.ToLower().Trim())
            //.Where(x => !string.IsNullOrEmpty(x))
            .ToList();

        var indexes = this.OlympediaService.FindIndexes(headers);

        return indexes;
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

    private void AddInfo(EventModel eventModel)
    {
        var match = this.RegExpService.Match(eventModel.Name, @"\(.*?\)");
        if (match != null)
        {
            var text = match.Groups[0].Value;
            eventModel.Name = eventModel.Name.Replace(text, string.Empty).Trim();

            var poundMatch = this.RegExpService.Match(text, @"(\+|-)([\d\.]+)\s*pounds");
            var kilogramMatch = this.RegExpService.Match(text, @"(\+|-)([\d\.]+)\s*kilograms");
            var otherMatch = this.RegExpService.Match(text, @"\((.*?)\)");
            if (poundMatch != null)
            {
                var weight = double.Parse(poundMatch.Groups[2].Value.Replace(".", ",")).ConvertPoundToKilograms();
                eventModel.AdditionalInfo = $"{poundMatch.Groups[1].Value.Trim()}{weight:F2}kg";
            }
            else if (kilogramMatch != null)
            {
                var weight = double.Parse(kilogramMatch.Groups[2].Value.Replace(".", ","));
                eventModel.AdditionalInfo = $"{kilogramMatch.Groups[1].Value.Trim()}{weight}kg";
            }
            else if (otherMatch != null)
            {
                eventModel.AdditionalInfo = otherMatch.Value.Replace("(", string.Empty).Replace(")", string.Empty).Trim();
            }
        }
    }
}