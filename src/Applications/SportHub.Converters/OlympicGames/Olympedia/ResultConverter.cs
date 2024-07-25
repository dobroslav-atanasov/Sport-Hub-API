namespace SportHub.Converters.OlympicGames.Olympedia;

using System;
using System.Threading.Tasks;

using HtmlAgilityPack;

using Microsoft.Extensions.Logging;

using SportHub.Common.Constants;
using SportHub.Converters.OlympicGames.Olympedia.SportConverters;
using SportHub.Data.Models.Cache;
using SportHub.Data.Models.Converters.OlympicGames;
using SportHub.Data.Models.DbEntities.Crawlers;
using SportHub.Data.Models.Enumerations.OlympicGames;
using SportHub.Services.Data.CrawlerStorageDb.Interfaces;
using SportHub.Services.Data.OlympicGamesDb.Interfaces;
using SportHub.Services.Interfaces;

public class ResultConverter : BaseOlympediaConverter
{
    private readonly IDateService dateService;
    private readonly BasketballConverter basketballConverter;
    private readonly SkiingConverter skiingConverter;
    private readonly ArcheryConverter archeryConverter;
    private readonly GymnasticsConverter gymnasticsConverter;
    private readonly AthleticsConverter athleticsConverter;
    private readonly AquaticsConverter aquaticsConverter;
    private readonly BadmintonConverter badmintonConverter;
    private readonly BaseballSoftballConverter baseballSoftballConverter;
    private readonly OldSportsConverter oldSportsConverter;
    private readonly VolleyballConverter volleyballConverter;
    private readonly BiathlonConverter biathlonConverter;
    private readonly BobsleighConverter bobsleighConverter;
    private readonly BoxingConverter boxingConverter;
    private readonly CanoeConverter canoeConverter;
    private readonly CurlingConverter curlingConverter;
    private readonly CyclingConverter cyclingConverter;
    private readonly EquestrianConverter equestrianConverter;
    private readonly FencingConverter fencingConverter;
    private readonly SkatingConverter skatingConverter;
    private readonly FootballConverter footballConverter;

    public ResultConverter(ILogger<BaseConverter> logger, ICrawlersService crawlersService, ILogsService logsService, IGroupsService groupsService, IZipService zipService,
        IRegExpService regExpService, INormalizeService normalizeService, IOlympediaService olympediaService, IDataCacheService dataCacheService,
        IDateService dateService, BasketballConverter basketballConverter, SkiingConverter skiingConverter, ArcheryConverter archeryConverter, GymnasticsConverter gymnasticsConverter,
        AthleticsConverter athleticsConverter, AquaticsConverter aquaticsConverter, BadmintonConverter badmintonConverter, BaseballSoftballConverter baseballSoftballConverter,
        OldSportsConverter oldSportsConverter, VolleyballConverter volleyballConverter, BiathlonConverter biathlonConverter, BobsleighConverter bobsleighConverter,
        BoxingConverter boxingConverter, CanoeConverter canoeConverter, CurlingConverter curlingConverter, CyclingConverter cyclingConverter, EquestrianConverter equestrianConverter,
        FencingConverter fencingConverter, SkatingConverter skatingConverter, FootballConverter footballConverter)
        : base(logger, crawlersService, logsService, groupsService, zipService, regExpService, normalizeService, olympediaService, dataCacheService)
    {
        this.dateService = dateService;
        this.basketballConverter = basketballConverter;
        this.skiingConverter = skiingConverter;
        this.archeryConverter = archeryConverter;
        this.gymnasticsConverter = gymnasticsConverter;
        this.athleticsConverter = athleticsConverter;
        this.aquaticsConverter = aquaticsConverter;
        this.badmintonConverter = badmintonConverter;
        this.baseballSoftballConverter = baseballSoftballConverter;
        this.oldSportsConverter = oldSportsConverter;
        this.volleyballConverter = volleyballConverter;
        this.biathlonConverter = biathlonConverter;
        this.bobsleighConverter = bobsleighConverter;
        this.boxingConverter = boxingConverter;
        this.canoeConverter = canoeConverter;
        this.curlingConverter = curlingConverter;
        this.cyclingConverter = cyclingConverter;
        this.equestrianConverter = equestrianConverter;
        this.fencingConverter = fencingConverter;
        this.skatingConverter = skatingConverter;
        this.footballConverter = footballConverter;
    }

    protected override async Task ProcessGroupAsync(Group group)
    {
        var model = this.PrepareConverterModel(group);
        //var htmlDocument = this.CreateHtmlDocument(group.Documents.Single(x => x.Order == 1));
        var documents = group.Documents.Where(x => x.Order != 1).OrderBy(x => x.Order);
        //var originalEventName = htmlDocument.DocumentNode.SelectSingleNode("//ol[@class='breadcrumb']/li[@class='active']").InnerText;
        //var gameCache = this.GetGameFromDatabase(htmlDocument);
        //var disciplineCache = this.GetDisciplineFromDatabase(htmlDocument);
        //var eventModel = this.CreateEventModel(originalEventName, gameCache, disciplineCache);
        if (model.IsValidEvent)
        {
            //var eventCache = this.DataCacheService
            //    .Events
            //    .FirstOrDefault(x => x.OriginalName == model.EventInfo.OriginalName && x.GameId == eventModel.GameId && x.DisciplineId == eventModel.DisciplineId);

            if (model.EventCache != null)
            {
                var roundDataModels = this.ExtractRoundDataModels(model.HtmlDocument, model.EventCache);
                var documentDataModels = this.ExtractDocumentDataModels(documents, model.EventCache);

                var options = new Options
                {
                    Discipline = model.DisciplineCache,
                    Event = model.EventCache,
                    Game = model.GameCache,
                    HtmlDocument = model.HtmlDocument,
                    Rounds = roundDataModels,
                    Documents = documentDataModels
                };

                // MonoBob 2022 dont have participants in db

                switch (model.DisciplineCache.Name)
                {
                    case DisciplineConstants.BASKETBALL_3X3:
                    case DisciplineConstants.BASKETBALL:
                        await this.basketballConverter.ProcessAsync(options);
                        break;
                    case DisciplineConstants.ALPINE_SKIING:
                    case DisciplineConstants.CROSS_COUNTRY_SKIING:
                        await this.skiingConverter.ProcessAsync(options);
                        break;
                    case DisciplineConstants.ARCHERY:
                        await this.archeryConverter.ProcessAsync(options);
                        break;
                    case DisciplineConstants.ARTISTIC_GYMNASTICS:
                        await this.gymnasticsConverter.ProcessAsync(options);
                        break;
                    case DisciplineConstants.ARTISTIC_SWIMMING:
                    case DisciplineConstants.DIVING:
                        await this.aquaticsConverter.ProcessAsync(options);
                        break;
                    case DisciplineConstants.ATHLETICS:
                        await this.athleticsConverter.ProcessAsync(options);
                        break;
                    case DisciplineConstants.BADMINTON:
                        await this.badmintonConverter.ProcessAsync(options);
                        break;
                    case DisciplineConstants.BASEBALL:
                    case DisciplineConstants.SOFTBALL:
                        await this.baseballSoftballConverter.ProcessAsync(options);
                        break;
                    case DisciplineConstants.BASQUE_PELOTA:
                    case DisciplineConstants.CRICKET:
                    case DisciplineConstants.CROQUET: // TODO
                        await this.oldSportsConverter.ProcessAsync(options);
                        break;
                    case DisciplineConstants.BEACH_VOLLEYBALL:
                    case DisciplineConstants.VOLLEYBALL:
                        await this.volleyballConverter.ProcessAsync(options);
                        break;
                    case DisciplineConstants.BIATHLON:
                        await this.biathlonConverter.ProcessAsync(options);
                        break;
                    case DisciplineConstants.BOBSLEIGH:
                    case DisciplineConstants.SKELETON:
                        await this.bobsleighConverter.ProcessAsync(options);
                        break;
                    case DisciplineConstants.BOXING:
                        await this.boxingConverter.ProcessAsync(options);
                        break;
                    case DisciplineConstants.CANOE_SLALOM:
                    case DisciplineConstants.CANOE_SPRINT:
                        await this.canoeConverter.ProcessAsync(options);
                        break;
                    case DisciplineConstants.CURLING:
                        await this.curlingConverter.ProcessAsync(options);
                        break;
                    case DisciplineConstants.CYCLING_BMX_FREESTYLE:
                    case DisciplineConstants.CYCLING_BMX_RACING:
                    case DisciplineConstants.CYCLING_MOUNTAIN_BIKE:
                    case DisciplineConstants.CYCLING_ROAD:
                    case DisciplineConstants.CYCLING_TRACK: // TODO
                        await this.cyclingConverter.ProcessAsync(options);
                        break;
                    case DisciplineConstants.EQUESTRIAN_DRESSAGE: // TODO
                    case DisciplineConstants.EQUESTRIAN_DRIVING:  // TODO
                    case DisciplineConstants.EQUESTRIAN_EVENTING: // TODO
                    case DisciplineConstants.EQUESTRIAN_JUMPING:  // TODO
                    case DisciplineConstants.EQUESTRIAN_VAULTING: // TODO
                        await this.equestrianConverter.ProcessAsync(options);
                        break;
                    case DisciplineConstants.FENCING:
                        await this.fencingConverter.ProcessAsync(options);
                        break;
                    case DisciplineConstants.FIGURE_SKATING:
                        await this.skatingConverter.ProcessAsync(options); // TODO
                        break;
                    case DisciplineConstants.FOOTBALL:
                        await this.footballConverter.ProcessAsync(options);
                        break;
                }
            }
        }
    }

    private List<RoundDataModel> ExtractRoundDataModels(HtmlDocument htmlDocument, EventCache eventCache)
    {
        var standingTableHtml = htmlDocument.DocumentNode.SelectSingleNode("//table[@class='table table-striped']")?.OuterHtml;
        var format = this.RegExpService.MatchFirstGroup(htmlDocument.DocumentNode.OuterHtml, @"<th>Format<\/th>\s*<td(?:.*?)>(.*?)<\/td>");
        var dateString = this.RegExpService.MatchFirstGroup(htmlDocument.DocumentNode.OuterHtml, @"<th>\s*Date\s*<\/th>\s*<td>(.*?)<\/td>");
        var dateModel = this.dateService.ParseDate(dateString);

        var order = 1;
        var standingModel = new RoundDataModel
        {
            Order = order++,
            EventId = eventCache.Id,
            Html = standingTableHtml,
            Format = format,
            From = dateModel.From,
            To = dateModel.To,
            Name = "Standing",
            Type = RoundEnum.FinalRound,
            SubType = RoundEnum.None
        };

        this.ExtractRows(standingModel);
        var models = new List<RoundDataModel> { standingModel };
        var html = htmlDocument.ParsedText;
        html = html.Replace("<table class=\"biodata\"></table>", string.Empty);

        var matches = this.RegExpService.Matches(html, @"<(h2|h3)>(.*?)<\/(h2|h3)>");
        if (matches.Count != 0)
        {
            foreach (System.Text.RegularExpressions.Match match in matches)
            {
                var roundName = match.Groups[2].Value.Trim();
                roundName = this.RegExpService.CutHtml(roundName);

                var currentMatch = this.RegExpService.Match(roundName, @"(.*?)\s*(\(.*?\))");
                var round = string.Empty;
                var info = string.Empty;

                if (currentMatch != null)
                {
                    round = currentMatch.Groups[1].Value.Trim();
                    info = currentMatch.Groups[2].Value.Trim();
                }
                else
                {
                    round = roundName;
                }

                dateModel = this.dateService.ParseDate(info);
                var roundDataModel = this.NormalizeService.MapRoundData(round);
                roundDataModel.NameHtml = match.Groups[0].Value;
                roundDataModel.Order = order++;
                roundDataModel.From = dateModel.From;
                roundDataModel.To = dateModel.To;

                models.Add(roundDataModel);
            }
        }

        for (var i = 1; i < models.Count; i++)
        {
            var pattern = $"({models[i].NameHtml.Replace("(", @"\(").Replace(")", @"\)").Replace("/", @"\/")})";
            if (i != models.Count - 1)
            {
                pattern += $"(.*?)({models[i + 1].NameHtml.Replace("(", @"\(").Replace(")", @"\)").Replace("/", @"\/")})";
            }
            else
            {
                pattern += $"(.*)";
            }

            var match = this.RegExpService.Match(html, pattern);
            if (match != null)
            {
                var replacePattern = $@"{match.Groups[1].Value}{match.Groups[2].Value}"
                    .Replace("(", @"\(")
                    .Replace(")", @"\)")
                    .Replace("/", @"\/")
                    .Replace("[", @"\[")
                    .Replace("]", @"\]")
                    .Replace("/", @"\/")
                    .Replace("-", @"\-");
                html = html.Replace($@"{match.Groups[1].Value}{match.Groups[2].Value}", string.Empty); /*this.RegExpService.Replace(html, replacePattern, string.Empty);*/
                var currentHtml = match.Groups[2].Value;
                var formatMatch = this.RegExpService.Match(currentHtml, @"<th>Format<\/th>\s*<td(?:.*?)>(.*?)<\/td>");
                formatMatch ??= this.RegExpService.Match(currentHtml, @"<p>\s*<i>(.*?)<\/i>\s*<\/p>");
                if (formatMatch != null)
                {
                    format = formatMatch.Groups[2].Value.Trim();
                    currentHtml = this.RegExpService.Replace(currentHtml, formatMatch.Groups[0].Value, string.Empty);
                    models[i].Format = format;
                }

                var splitMatch = this.RegExpService.Match(currentHtml, @"(?:Splits<\/h4>|Split Times<\/h2>)\s*<table class=""table table-striped"">(.*?)<\/table>");
                if (splitMatch != null)
                {
                    models[i].SplitsHtml = splitMatch.Groups[0].Value;
                    currentHtml = currentHtml.Replace(splitMatch.Groups[0].Value, string.Empty); /*this.RegExpService.Replace(currentHtml, splitMatch.Groups[0].Value, string.Empty);*/
                }

                models[i].Html = currentHtml;
                //models[i].Html = $"<table>{currentHtml}</table>";
                dateString = this.RegExpService.MatchFirstGroup(currentHtml, @"<th>\s*Date\s*<\/th>\s*<td>(.*?)<\/td>");
                dateModel = this.dateService.ParseDate(dateString);
                if (dateModel.From != null && models[i].From == null)
                {
                    models[i].From = dateModel.From;
                    models[i].To = dateModel.To;
                }

                if (models[i].Type == RoundEnum.PreliminaryRound && (models[i].SubType == RoundEnum.Group || models[i].SubType == RoundEnum.Pool || models[i].SubType == RoundEnum.Heat))
                {
                    RoundDataModel previousRound = null;
                    for (var z = i - 1; z >= 0; z--)
                    {
                        if ((models[z].Type != RoundEnum.PreliminaryRound && models[z].SubType != RoundEnum.Group && models[z].SubType != RoundEnum.Pool && models[z].SubType != RoundEnum.Heat) || models[z].Type == RoundEnum.PreliminaryRound)
                        {
                            previousRound = models[z];
                            break;
                        }
                    }

                    models[i].Type = previousRound.Type;
                    models[i].Format = previousRound.Format;
                    if (models[i].From == null)
                    {
                        models[i].From = previousRound.From;
                    }
                    if (models[i].To == null)
                    {
                        models[i].To = previousRound.To;
                    }
                }

                this.ExtractRows(models[i]);
            }
        }

        models = models.Where(x => x.Rows != null && x.Rows.Count > 0).ToList();
        return models;
    }

    private void ExtractRows(RoundDataModel round)
    {
        var rows = round.HtmlDocument.DocumentNode.SelectNodes("//table[@class='table table-striped']//tr");
        //if (rows == null)
        //{
        //    var htmlDocument = new HtmlDocument();
        //    //htmlDocument.LoadHtml(table.OriginalHtml);
        //    rows = htmlDocument.DocumentNode.SelectNodes("//table[@class='table table-striped']//tr");
        //}

        if (rows == null)
        {
            return;
        }

        var headers = rows.First().Elements("th").Where(x => this.OlympediaService.FindAthlete(x.OuterHtml) == null).Select(x => x.InnerText).ToList();
        round.Headers = headers;
        round.Rows = rows;
        round.Indexes = this.OlympediaService.GetIndexes(headers);
    }

    private List<DocumentDataModel> ExtractDocumentDataModels(IOrderedEnumerable<Document> documents, EventCache eventCache)
    {
        var models = new List<DocumentDataModel>();
        foreach (var document in documents)
        {
            var htmlDocument = this.CreateHtmlDocument(document);
            var title = htmlDocument.DocumentNode.SelectSingleNode("//h1").InnerText;
            title = title.Replace(eventCache.OriginalName, string.Empty).Replace("–", string.Empty).Trim();
            var dateString = this.RegExpService.MatchFirstGroup(htmlDocument.DocumentNode.OuterHtml, @"<th>\s*Date\s*<\/th>\s*<td>(.*?)<\/td>");
            var dateModel = this.dateService.ParseDate(dateString);
            var format = this.RegExpService.MatchFirstGroup(htmlDocument.DocumentNode.OuterHtml, @"<th>Format<\/th>\s*<td(?:.*?)>(.*?)<\/td>");
            var id = int.Parse(new Uri(document.Url).Segments.Last());
            var round = this.NormalizeService.MapRoundData(title);

            var documentDataModel = new DocumentDataModel
            {
                Id = id,
                Title = title,
                Html = htmlDocument.ParsedText,
                HtmlDocument = htmlDocument,
                From = dateModel.From,
                To = dateModel.To,
                Format = format,
                Type = round.Type,
                SubType = round.SubType,
                Number = round.Number,
                Info = round.Info,
            };

            var nodes = htmlDocument.DocumentNode.SelectNodes("//table[@class='table table-striped']");
            if (nodes != null)
            {
                var order = 1;
                foreach (var node in nodes)
                {
                    var roundDataModel = new RoundDataModel
                    {
                        Html = node.OuterHtml,
                        Order = order++,
                    };

                    this.ExtractRows(roundDataModel);
                    documentDataModel.Rounds.Add(roundDataModel);
                }
            }

            models.Add(documentDataModel);
        }

        return models;
    }
}