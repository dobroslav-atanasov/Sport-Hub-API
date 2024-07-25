namespace SportHub.Converters.OlympicGames.Olympedia.SportConverters;

using System.Threading.Tasks;

using AutoMapper;

using HtmlAgilityPack;

using SportHub.Common.Constants;
using SportHub.Data.Models.Converters.OlympicGames;
using SportHub.Data.Models.Converters.OlympicGames.Base;
using SportHub.Data.Models.Converters.OlympicGames.Disciplines;
using SportHub.Data.Models.DbEntities.OlympicGames;
using SportHub.Data.Repositories;
using SportHub.Services.Data.OlympicGamesDb.Interfaces;
using SportHub.Services.Interfaces;

public class BoxingConverter : BaseSportConverter
{
    public BoxingConverter(IOlympediaService olympediaService, IDateService dateService, IDataCacheService dataCacheService, IRegExpService regExpService, IMapper mapper,
        INormalizeService normalizeService, OlympicGamesRepository<Team> teamRepository, OlympicGamesRepository<Participation> participationRepository,
        OlympicGamesRepository<Athlete> athleteRepository, OlympicGamesRepository<Result> resultRepository)
        : base(olympediaService, dateService, dataCacheService, regExpService, mapper, normalizeService, teamRepository, participationRepository, athleteRepository, resultRepository)
    {
    }

    public override async Task ProcessAsync(Options options)
    {
        var rounds = new List<Round<Boxing>>();

        foreach (var roundData in options.Rounds.Skip(1))
        {
            var round = this.CreateRound<Boxing>(roundData, options.Event.Name, null);
            await this.SetMatchesAsync(round, options, roundData);
            rounds.Add(round);
        }
        await this.ProcessJsonAsync(rounds, options);
    }

    private async Task SetMatchesAsync(Round<Boxing> round, Options options, RoundDataModel roundData)
    {
        foreach (var row in roundData.Rows.Skip(1))
        {
            var data = row.Elements("td").ToList();
            var matchInputModel = new MatchInputModel
            {
                Row = row.OuterHtml,
                Number = data[0].OuterHtml,
                Date = data[1].InnerText,
                Year = options.Game.Year,
                EventId = options.Event.Id,
                IsTeam = options.Event.IsTeamEvent,
                IsDoubles = false,
                HomeName = options.Game.Year != 2000 ? data[2].OuterHtml : data[3].OuterHtml,
                HomeNOC = options.Game.Year != 2000 ? data[3].OuterHtml : data[4].OuterHtml,
                Result = options.Game.Year != 2000 ? data[4].InnerText : data[5].InnerText,
                AwayName = options.Game.Year != 2000 ? data[5].OuterHtml : data[6].OuterHtml,
                AwayNOC = options.Game.Year != 2000 ? data[6].OuterHtml : data[7].OuterHtml,
                AnyParts = false,
                RoundType = roundData.Type,
                RoundSubType = roundData.SubType,
                Location = options.Game.Year != 2000 ? null : data[2].InnerText,
            };

            var matchModel = await this.GetMatchAsync(matchInputModel);
            var match = this.Mapper.Map<AthleteMatch<Boxing>>(matchModel);
            var boxingDecision = this.MapBoxingResult(matchInputModel.Result);
            var document = options.Documents.FirstOrDefault(x => x.Id == match.ResultId);
            if (document != null && match.Athlete2.Code != 0)
            {
                match.Judges = await this.GetJudgesAsync(document.Html);
                var scoreData = document.Rounds.FirstOrDefault();
                this.SetScore(match.Athlete1, scoreData.Rows.ElementAtOrDefault(1), scoreData.Indexes, boxingDecision);
                this.SetScore(match.Athlete2, scoreData.Rows.ElementAtOrDefault(2), scoreData.Indexes, BoxingDecision.None);
                this.SetAdditionalScore(match.Athlete1, document.Rounds.LastOrDefault());
            }

            round.AthleteMatches.Add(match);
        }
    }

    private void SetAdditionalScore(Boxing athlete, RoundDataModel roundData)
    {
        foreach (var row in roundData.Rows)
        {
            var headers = row.Elements("th").ToList();
            var data = row.Elements("td").ToList();

            switch (headers[0].InnerText.Trim())
            {
                case "Match Result":
                    athlete.Decision = this.MapBoxingResult(data[0].InnerText);
                    break;
                case "Round":
                    athlete.InRound = this.RegExpService.MatchInt(data[0].InnerText);
                    break;
                case "Time":
                    athlete.Time = data[0].InnerText.Trim();
                    break;
            }
        }
    }

    private void SetScore(Boxing athlete, HtmlNode htmlNode, Dictionary<string, int> indexes, BoxingDecision decision)
    {
        var data = htmlNode.Elements("td").ToList();

        athlete.Points = this.GetInt(indexes, ConverterConstants.Score, data);
        athlete.Points ??= this.GetInt(indexes, ConverterConstants.JudgesFavoring, data);
        athlete.TotalPoints = this.GetInt(indexes, ConverterConstants.Judges, data);
        athlete.Decision = decision;
        athlete.Trunks = this.GetString(indexes, ConverterConstants.Trunks, data);
        athlete.Judge1 = this.GetInt(indexes, ConverterConstants.Judge1, data);
        athlete.Judge2 = this.GetInt(indexes, ConverterConstants.Judge2, data);
        athlete.Judge3 = this.GetInt(indexes, ConverterConstants.Judge3, data);
        athlete.Judge4 = this.GetInt(indexes, ConverterConstants.Judge4, data);
        athlete.Judge5 = this.GetInt(indexes, ConverterConstants.Judge5, data);
        athlete.Round1 = this.GetInt(indexes, ConverterConstants.Round1, data);
        athlete.Round2 = this.GetInt(indexes, ConverterConstants.Round2, data);
        athlete.Round3 = this.GetInt(indexes, ConverterConstants.Round3, data);
        athlete.Round4 = this.GetInt(indexes, ConverterConstants.Round4, data);
    }

    private BoxingDecision MapBoxingResult(string text)
    {
        var result = BoxingDecision.None;
        switch (text.Trim())
        {
            case "Decision": result = BoxingDecision.Decision; break;
            case "Disqualification":
            case "Disqualified":
            case "Dq":
                result = BoxingDecision.Disqualification; break;
            case "Knockout":
            case "Knock-out":
                result = BoxingDecision.Knockout; break;
            case "No contest": result = BoxingDecision.NoContest; break;
            case "Walkover": result = BoxingDecision.Walkover; break;
            case "Retired": result = BoxingDecision.Retirement; break;
            case "Referee stops contest": result = BoxingDecision.RefereeStopsContest; break;
            case "Referee stops contest (head blow)": result = BoxingDecision.RefereeStopsContestHeadBlow; break;
            case "Referee stops contest (injured)": result = BoxingDecision.RefereeStopsContestInjured; break;
            case "Referee stops contest (outclassed)": result = BoxingDecision.RefereeStopsContestOutclassed; break;
            case "Referee stops contest (outscored)": result = BoxingDecision.RefereeStopsContestOutscored; break;
        }

        return result;
    }
}