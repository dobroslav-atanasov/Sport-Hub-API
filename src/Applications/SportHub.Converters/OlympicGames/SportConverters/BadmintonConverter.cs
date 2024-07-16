namespace SportHub.Converters.OlympicGames.SportConverters;

using AutoMapper;

using SportHub.Data.Models.Converters.OlympicGames;
using SportHub.Data.Models.Converters.OlympicGames.Base;
using SportHub.Data.Models.Converters.OlympicGames.Disciplines;
using SportHub.Data.Models.DbEntities.OlympicGames;
using SportHub.Data.Repositories;
using SportHub.Services.Data.OlympicGamesDb.Interfaces;
using SportHub.Services.Interfaces;

public class BadmintonConverter : BaseSportConverter
{
    public BadmintonConverter(IOlympediaService olympediaService, IDateService dateService, IDataCacheService dataCacheService, IRegExpService regExpService, IMapper mapper,
        INormalizeService normalizeService, OlympicGamesRepository<Team> teamRepository, OlympicGamesRepository<Participation> participationRepository,
        OlympicGamesRepository<Athlete> athleteRepository, OlympicGamesRepository<Result> resultRepository)
        : base(olympediaService, dateService, dataCacheService, regExpService, mapper, normalizeService, teamRepository, participationRepository, athleteRepository, resultRepository)
    {
    }

    public override async Task ProcessAsync(Options options)
    {
        var rounds = new List<Round<Badminton>>();

        foreach (var roundData in options.Rounds.Skip(1))
        {
            var round = this.CreateRound<Badminton>(roundData, options.Event.Name, null);
            await this.SetMatchesAsync(round, options, roundData);
            rounds.Add(round);
        }

        await this.ProcessJsonAsync(rounds, options);
    }

    private async Task SetMatchesAsync(Round<Badminton> round, Options options, RoundDataModel roundData)
    {
        foreach (var row in roundData.Rows.Skip(1).Where(x => this.OlympediaService.FindResultNumber(x.OuterHtml) != 0))
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
                IsDoubles = true,
                HomeName = options.Game.Year <= 1996 ? data[2].OuterHtml : data[3].OuterHtml,
                HomeNOC = options.Game.Year <= 1996 ? data[3].OuterHtml : data[4].OuterHtml,
                Result = options.Game.Year <= 1996 ? data[4].OuterHtml : data[5].OuterHtml,
                AwayName = options.Game.Year <= 1996 ? data[5].OuterHtml : data[6].OuterHtml,
                AwayNOC = options.Game.Year <= 1996 ? data[6].OuterHtml : data[7].OuterHtml,
                AnyParts = true,
                RoundType = roundData.Type,
                RoundSubType = roundData.SubType,
                Location = options.Game.Year <= 1996 ? null : data[2].InnerText,
            };
            var matchModel = await this.GetMatchAsync(matchInputModel);

            if (options.Event.IsTeamEvent)
            {
                var match = this.Mapper.Map<TeamMatch<Badminton>>(matchModel);
                match.Judges = await this.GetJudgesAsync(options, match.ResultId);
                round.TeamMatches.Add(match);
            }
            else
            {
                var match = this.Mapper.Map<AthleteMatch<Badminton>>(matchModel);
                match.Judges = await this.GetJudgesAsync(options, match.ResultId);
                round.AthleteMatches.Add(match);
            }
        }
    }

    private async Task<List<Judge>> GetJudgesAsync(Options options, int resultId)
    {
        var document = options.Documents.FirstOrDefault(x => x.Id == resultId);
        if (document != null)
        {
            var judges = await this.GetJudgesAsync(document.HtmlDocument.ParsedText);
            return judges;
        }

        return [];
    }
}