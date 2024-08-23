namespace SportHub.Converters.OlympicGames.Olympedia.Base;

using Microsoft.Extensions.Logging;

using SportHub.Common.Extensions;
using SportHub.Common.Helpers;
using SportHub.Data.Entities.Crawlers;
using SportHub.Data.Models.Converters.OlympicGames;
using SportHub.Data.Models.Converters.OlympicGames.Olympedia;
using SportHub.Services.Data.CrawlerStorageDb.Interfaces;
using SportHub.Services.Data.OlympicGamesDb.Interfaces;
using SportHub.Services.Interfaces;

public abstract class OlympediaConverter : BaseConverter
{
    protected OlympediaConverter(ILogger<BaseConverter> logger, ICrawlersService crawlersService, ILogsService logsService, IGroupsService groupsService, IZipService zipService,
        INormalizeService normalizeService, IDataCacheService dataCacheService)
        : base(logger, crawlersService, logsService, groupsService, zipService, normalizeService, dataCacheService)
    {
    }

    protected ConverterModel<DocumentConverterModel> PrepareConverterModel(Group group)
    {
        var model = new ConverterModel<DocumentConverterModel>
        {
            Identifier = group.Identifier,
            CrawlerId = group.CrawlerId,
            Name = group.Name,
        };

        foreach (var document in group.Documents.OrderBy(x => x.Order).Take(1))
        {
            var documentModel = new DocumentConverterModel
            {
                HtmlDocument = this.CreateHtmlDocument(document),
            };

            documentModel.Title = documentModel.HtmlDocument.DocumentNode.SelectSingleNode("//h1").InnerText.Trim();

            if (int.TryParse(new Uri(document.Url).Segments.Last(), out var pageId))
            {
                documentModel.PageId = pageId;
            }

            this.SetHeader(documentModel);
            this.SetEventInfo(documentModel);
            this.SetIsForbiddenEvent(documentModel);

            if (documentModel.IsValidEvent && !documentModel.EventInfo.IsForbidden)
            {
                var code = this.GenerateEventCode(documentModel.Game.Type, documentModel.Game.Year, documentModel.Discipline.Code, this.GetGender(documentModel.EventInfo.Gender), this.NormalizeService.GetShortEventName(documentModel.EventInfo.Name));
                var eventCache = this.DataCacheService
                    .Events
                    .FirstOrDefault(x => x.Code == code);

                documentModel.Event = eventCache;

                this.SetPhaseData(documentModel);
            }

            model.Documents.Add(document.Order, documentModel);
        }

        return model;
    }

    private void SetPhaseData(DocumentConverterModel model)
    {
        var html = model.HtmlDocument.DocumentNode.SelectSingleNode("//table[@class='table table-striped']")?.OuterHtml;
        var format = RegExpHelper.MatchFirstGroup(model.HtmlDocument.DocumentNode.OuterHtml, @"<th>Format<\/th>\s*<td(?:.*?)>(.*?)<\/td>");
        var dateString = RegExpHelper.MatchFirstGroup(model.HtmlDocument.DocumentNode.OuterHtml, @"<th>\s*Date\s*<\/th>\s*<td>(.*?)<\/td>");
        var dateModel = DateHelper.ParseDate(dateString);

        var order = 1;
        var firstPhase = new PhaseData
        {
            Order = order++,
            EventId = model.Event.Id,
            EventCode = model.Event.Code,
            Html = html,
            //Format = format,
            From = dateModel.From,
            To = dateModel.To,
            Name = "Standing",
            Code = $"{model.Event.Code}FNL--"
            //Type = RoundEnum.FinalRound,
            //SubType = RoundEnum.None
        };

        this.ExtractRows(firstPhase);
        model.Phases.Add(firstPhase);
        html = model.HtmlDocument.ParsedText;
        html = html.Replace("<table class=\"biodata\"></table>", string.Empty);

        var matches = RegExpHelper.Matches(html, @"<(h2|h3)>(.*?)<\/(h2|h3)>");
        var list = File.ReadAllLines("asd.txt").ToList();
        list.Add($"{model.Event.Code}|{firstPhase.Name}|{firstPhase.Code}");

        foreach (System.Text.RegularExpressions.Match match in matches)
        {
            var text = match.Groups[2].Value;
            text = RegExpHelper.CutHtml(text);
            text = RegExpHelper.Replace(text, @"\((.*)\)", string.Empty).Trim();

            var code = this.NormalizeService.MapPhase(text);
            var phase = new PhaseData
            {
                EventId = model.Event.Id,
                EventCode = model.Event.Code,
                Order = order++,
                Name = text,
                Code = $"{model.Event.Code}{code.PadRight(5, '-')}"
            };

            list.Add($"{model.Event.Code}|{text}|{phase.Code}");
        }

        File.WriteAllLines("asd.txt", list);
    }

    private void ExtractRows(PhaseData phaseData)
    {
        var rows = phaseData.HtmlDocument.DocumentNode.SelectNodes("//table[@class='table table-striped']//tr");
        if (rows == null)
        {
            return;
        }

        var headers = rows.First()
            .Elements("th")
            .Where(x => OlympediaHelper.FindAthlete(x.OuterHtml) == null)
            .Select(x => x.InnerText)
            .ToList();

        phaseData.Headers = headers;
        phaseData.Rows = rows;
        phaseData.Indexes = OlympediaHelper.GetIndexes(headers);
    }

    private void SetIsForbiddenEvent(DocumentConverterModel model)
    {
        var list = new List<string>
        {
            "1900-Archery-None Event, Men",
            "1920-Shooting-None Event, Men",
            "1904-Artistic Gymnastics-Individual All-Around, Field Sports, Men"
        };

        var isForbidden = list.Any(x => x == $"{model.Game?.Year}-{model.Discipline?.Name}-{model.EventInfo?.OriginalName}");
        model.EventInfo.IsForbidden = isForbidden;
    }

    private void SetEventInfo(DocumentConverterModel model)
    {
        if (model.Game != null && model.Discipline != null)
        {
            model.EventInfo.OriginalName = model.Header.Event;
            model.EventInfo.Name = this.NormalizeService.NormalizeEventName(model);

            var parts = model.EventInfo.Name.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();
            var gender = parts.Last().Trim();
            model.EventInfo.Name = string.Join("|", parts.Take(parts.Count - 1).Select(x => x.Trim()).ToList());

            this.SetDescription(model);

            if (RegExpHelper.IsMatch(model.EventInfo.Name, @"Team"))
            {
                model.EventInfo.Name = RegExpHelper.Replace(model.EventInfo.Name, @"Team", string.Empty);
                model.EventInfo.Name = $"Team|{model.EventInfo.Name}";
            }

            if (RegExpHelper.IsMatch(model.EventInfo.Name, @"Individual"))
            {
                model.EventInfo.Name = RegExpHelper.Replace(model.EventInfo.Name, @"Individual", string.Empty);
                model.EventInfo.Name = $"Individual|{model.EventInfo.Name}";
            }

            var nameParts = model.EventInfo.Name.Split(new[] { " ", "|" }, StringSplitOptions.RemoveEmptyEntries).Select(x => x.UpperFirstChar()).ToList();
            var name = string.Join(" ", nameParts);

            switch (gender)
            {
                case "Men":
                    gender = "Men's";
                    break;
                case "Women":
                    gender = "Women's";
                    break;
            }

            var categoryMatch = RegExpHelper.Match(model.EventInfo.Description, @"([\d\.]+)kg");
            if (categoryMatch != null)
            {
                var description = model.EventInfo.Description;
                model.EventInfo.Description = name;
                name = description.Replace("-", string.Empty);
            }

            if (model.Discipline.Name == "Wrestling" && model.EventInfo.OriginalName.Contains("Greco-Roman"))
            {
                name = $"Greco-Roman {name}";
                model.EventInfo.Description = model.EventInfo.Description?.Replace("Greco-Roman", string.Empty).Trim();
            }

            if (model.Discipline.Name == "Wrestling" && model.EventInfo.OriginalName.Contains("Freestyle"))
            {
                name = $"Freestyle {name}";
                model.EventInfo.Description = model.EventInfo.Description?.Replace("Freestyle", string.Empty).Trim();
            }

            if (model.Game.Year == 1900 && model.EventInfo.Name == "Coxed Four")
            {
                name = $"{name} {model.EventInfo.Description}";
            }

            if ((model.Game.Year == 1948 || model.Game.Year == 1972) && model.EventInfo.Name == "Two Person Keelboat")
            {
                name = $"{name} {model.EventInfo.Description}";
            }

            if (model.Game.Year == 1972 && model.EventInfo.Name == "Three Person Keelboat")
            {
                name = $"{name} {model.EventInfo.Description}";
            }

            model.EventInfo.Name = name;
            model.EventInfo.Gender = gender;
            model.EventInfo.FullName = $"{gender} {name}";
        }
        else
        {
            model.IsValidEvent = false;
        }
    }

    private void SetHeader(DocumentConverterModel model)
    {
        var headers = model.HtmlDocument.DocumentNode.SelectSingleNode("//ol[@class='breadcrumb']");
        var gameMatch = RegExpHelper.Match(headers?.OuterHtml, @"<a href=""\/editions\/(?:\d+)"">(\d+)\s*(\w+)\s*Olympics<\/a>");

        if (gameMatch != null)
        {
            model.Header.GameYear = int.Parse(gameMatch.Groups[1].Value);
            var type = gameMatch.Groups[2].Value.Trim();

            if (type.Equals("equestrian", StringComparison.CurrentCultureIgnoreCase))
            {
                type = "Summer";
            }

            model.Header.GameType = type;
            model.Game = this.DataCacheService.Games.FirstOrDefault(x => x.Year == model.Header.GameYear && x.Type == model.Header.GameType);
        }

        var discipline = RegExpHelper.MatchFirstGroup(headers?.OuterHtml, @"<a href=""\/editions\/[\d]+\/sports\/(?:.*?)"">(.*?)<\/a>");
        var @event = RegExpHelper.MatchFirstGroup(headers?.OuterHtml, @"<li\s*class=""active"">(.*?)<\/li>");

        if (discipline != null && @event != null)
        {
            if (discipline.StartsWith("Equestrian"))
            {
                var name = discipline.Replace("Equestrian", string.Empty).Trim();
                @event = $"{name} {@event}";
            }

            switch (discipline)
            {
                case "Canoe Marathon":
                    discipline = "Canoe Sprint";
                    break;
                case "Equestrian Dressage":
                case "Equestrian Driving":
                case "Equestrian Eventing":
                case "Equestrian Jumping":
                case "Equestrian Vaulting":
                    discipline = "Equestrian";
                    break;
                case "Trampolining":
                    discipline = "Trampoline Gymnastics";
                    break;
            }

            model.Header.Discipline = discipline;
            model.Header.Event = @event;
            model.Discipline = this.DataCacheService.Disciplines.FirstOrDefault(x => x.Name == model.Header.Discipline);
        }
    }

    private void SetDescription(DocumentConverterModel model)
    {
        var match = RegExpHelper.Match(model.EventInfo.Name, @"\(.*?\)");
        if (match != null)
        {
            var text = match.Groups[0].Value;
            model.EventInfo.Name = model.EventInfo.Name.Replace(text, string.Empty).Trim();

            var poundMatch = RegExpHelper.Match(text, @"(\+|-)([\d\.]+)\s*pounds");
            var kilogramMatch = RegExpHelper.Match(text, @"(\+|-)([\d\.]+)\s*kilograms");
            var otherMatch = RegExpHelper.Match(text, @"\((.*?)\)");
            if (poundMatch != null)
            {
                var weight = double.Parse(poundMatch.Groups[2].Value).ConvertPoundToKilograms();
                model.EventInfo.Description = $"{poundMatch.Groups[1].Value.Trim()}{weight:F2}kg";
            }
            else if (kilogramMatch != null)
            {
                var weight = double.Parse(kilogramMatch.Groups[2].Value);
                model.EventInfo.Description = $"{kilogramMatch.Groups[1].Value.Trim()}{weight}kg";
            }
            else if (otherMatch != null)
            {
                model.EventInfo.Description = otherMatch.Value.Replace("(", string.Empty).Replace(")", string.Empty).Trim();
            }
        }
    }

    protected string GenerateEventCode(string type, int year, string discipline, string gender, string shortName)
    {
        var code = $"{type.Substring(0, 1)}-{year}-{discipline}-{gender}-{shortName}";
        code = code.PadRight(24, '-');
        return code;
    }
}