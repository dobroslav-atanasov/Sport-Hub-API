namespace SportData.Converters.OlympicGames.SportConverters;

using System;
using System.Threading.Tasks;

using AutoMapper;

using SportData.Common.Constants;
using SportData.Data.Models.Converters.OlympicGames;
using SportData.Data.Models.Converters.OlympicGames.Base;
using SportData.Data.Models.Converters.OlympicGames.Disciplines;
using SportData.Data.Models.Entities.Enumerations;
using SportData.Data.Models.Entities.OlympicGames;
using SportData.Data.Repositories;
using SportData.Services.Data.OlympicGamesDb.Interfaces;
using SportData.Services.Interfaces;

public class OldSportsConverter : BaseSportConverter
{
    public OldSportsConverter(IOlympediaService olympediaService, IDateService dateService, IDataCacheService dataCacheService, IRegExpService regExpService, IMapper mapper,
        INormalizeService normalizeService, OlympicGamesRepository<Team> teamRepository, OlympicGamesRepository<Participation> participationRepository,
        OlympicGamesRepository<Athlete> athleteRepository, OlympicGamesRepository<Result> resultRepository)
        : base(olympediaService, dateService, dataCacheService, regExpService, mapper, normalizeService, teamRepository, participationRepository, athleteRepository, resultRepository)
    {
    }

    public override async Task ProcessAsync(Options options)
    {
        switch (options.Discipline.Name)
        {
            case DisciplineConstants.BASQUE_PELOTA:
                await this.ProcessBasquePelotaAsync(options);
                break;
            case DisciplineConstants.CRICKET:
                await this.ProcessCricketAsync(options);
                break;
            case DisciplineConstants.CROQUET:
                await this.ProcessCroquetAsync(options);
                break;
        }
    }

    private async Task ProcessCroquetAsync(Options options)
    {
        var rounds = new List<Round<CanoeSprint>>();
        await Console.Out.WriteLineAsync($"{options.Game.Year} - {options.Event.Name}");

        var allRounds = options.Rounds;
        if (options.Rounds.Count != 1)
        {
            allRounds = options.Rounds.Skip(1).ToList();
        }

        foreach (var roundData in allRounds)
        {
            var round = this.CreateRound<CanoeSprint>(roundData, options.Event.Name, null);

            rounds.Add(round);
        }

        await this.ProcessJsonAsync(rounds, options);
    }

    private async Task ProcessCricketAsync(Options options)
    {
        var rounds = new List<Round<Cricket>>();

        foreach (var roundData in options.Rounds.Skip(1))
        {
            var round = this.CreateRound<Cricket>(roundData, options.Event.Name, null);
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
                    IsDoubles = false,
                    HomeName = data[2].OuterHtml,
                    HomeNOC = data[3].OuterHtml,
                    Result = data[4].OuterHtml,
                    AwayName = data[5].OuterHtml,
                    AwayNOC = data[6].OuterHtml,
                    AnyParts = false,
                    RoundType = roundData.Type,
                    RoundSubType = roundData.SubType,
                    Location = null,
                };
                var matchModel = await this.GetMatchAsync(matchInputModel);
                var match = this.Mapper.Map<TeamMatch<Cricket>>(matchModel);
                match.Team1.MatchResult = MatchResultType.Win;
                match.Team2.MatchResult = MatchResultType.Lose;

                round.TeamMatches.Add(match);
            }

            rounds.Add(round);
        }

        await this.ProcessJsonAsync(rounds, options);
    }

    private async Task ProcessBasquePelotaAsync(Options options)
    {
        var rounds = new List<Round<BasquePelota>>();
        var roundData = options.Rounds.FirstOrDefault();
        var round = this.CreateRound<BasquePelota>(roundData, options.Event.Name, null);

        foreach (var row in roundData.Rows.Skip(1))
        {
            var data = row.Elements("td").ToList();
            var teamName = data[roundData.Indexes[ConverterConstants.Name]].InnerText;
            var noc = this.OlympediaService.FindNOCCode(row.OuterHtml);
            var nocCache = this.DataCacheService.NOCs.FirstOrDefault(x => x.Code == noc);
            var athleteModels = this.OlympediaService.FindAthletes(row.OuterHtml);
            var dbTeam = await this.TeamRepository.GetAsync(x => x.Name == teamName && x.NOCId == nocCache.Id && x.EventId == options.Event.Id);

            var team = new BasquePelota
            {
                Id = dbTeam.Id,
                Name = dbTeam.Name,
                NOC = noc,
                FinishStatus = this.OlympediaService.FindStatus(row.OuterHtml),
            };

            foreach (var athleteModel in athleteModels)
            {
                var participant = await this.ParticipationRepository.GetAsync(x => x.Code == athleteModel.Code && x.EventId == options.Event.Id);
                var athlete = new BasquePelota
                {
                    Id = participant.Id,
                    Code = athleteModel.Code,
                    Name = athleteModel.Name,
                    NOC = team.NOC,
                    FinishStatus = this.OlympediaService.FindStatus(row.OuterHtml),
                };

                team.Athletes.Add(athlete);
            }

            round.Teams.Add(team);
        }

        rounds.Add(round);

        await this.ProcessJsonAsync(rounds, options);
    }
}