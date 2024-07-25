namespace SportHub.Converters.OlympicGames.Olympedia.SportConverters;

using System;
using System.Threading.Tasks;

using AutoMapper;

using SportHub.Common.Constants;
using SportHub.Data.Models.Converters.OlympicGames;
using SportHub.Data.Models.Converters.OlympicGames.Base;
using SportHub.Data.Models.Converters.OlympicGames.Disciplines;
using SportHub.Data.Models.DbEntities.OlympicGames;
using SportHub.Data.Repositories;
using SportHub.Services.Data.OlympicGamesDb.Interfaces;
using SportHub.Services.Interfaces;

public class VolleyballConverter : BaseSportConverter
{
    public VolleyballConverter(IOlympediaService olympediaService, IDateService dateService, IDataCacheService dataCacheService, IRegExpService regExpService, IMapper mapper,
        INormalizeService normalizeService, OlympicGamesRepository<Team> teamRepository, OlympicGamesRepository<Participation> participationRepository,
        OlympicGamesRepository<Athlete> athleteRepository, OlympicGamesRepository<Result> resultRepository)
        : base(olympediaService, dateService, dataCacheService, regExpService, mapper, normalizeService, teamRepository, participationRepository, athleteRepository, resultRepository)
    {
    }

    public override async Task ProcessAsync(Options options)
    {
        switch (options.Discipline.Name)
        {
            case DisciplineConstants.BEACH_VOLLEYBALL:
                await this.ProcessBeachVolleyballAsync(options);
                break;
            case DisciplineConstants.VOLLEYBALL:
                //await this.ProcessVolleyballAsync(options);
                break;
        }
    }

    private async Task ProcessBeachVolleyballAsync(Options options)
    {
        var rounds = new List<Round<BeachVolleyball>>();
        await Console.Out.WriteLineAsync($"{options.Game.Year} - {options.Event.Name}");

        foreach (var roundData in options.Rounds.Skip(1))
        {
            var round = this.CreateRound<BeachVolleyball>(roundData, options.Event.Name, null);
            await this.SetBeachVolleyballMatchesAsync(round, options, roundData);
            rounds.Add(round);
        }

        await this.ProcessJsonAsync(rounds, options);
    }

    private async Task SetBeachVolleyballMatchesAsync(Round<BeachVolleyball> round, Options options, RoundDataModel roundData)
    {
        var year = options.Game.Year;
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
                HomeName = year is 1996 or 2004 or 2020 ? data[2].OuterHtml : data[3].OuterHtml,
                HomeNOC = year is 1996 or 2004 or 2020 ? data[3].OuterHtml : data[4].OuterHtml,
                Result = year is 1996 or 2004 or 2020 ? data[4].OuterHtml : data[5].OuterHtml,
                AwayName = year is 1996 or 2004 or 2020 ? data[5].OuterHtml : data[6].OuterHtml,
                AwayNOC = year is 1996 or 2004 or 2020 ? data[6].OuterHtml : data[7].OuterHtml,
                AnyParts = true,
                RoundType = roundData.Type,
                RoundSubType = roundData.SubType,
                Location = year is 1996 or 2004 or 2020 ? null : data[2].InnerText,
            };
            var matchModel = await this.GetMatchAsync(matchInputModel);

            var match = this.Mapper.Map<TeamMatch<BeachVolleyball>>(matchModel);
            var document = options.Documents.FirstOrDefault(x => x.Id == match.ResultId);
            if (document != null)
            {
                match.Judges = await this.GetJudgesAsync(document.HtmlDocument.ParsedText);
                match.Attendance = this.RegExpService.MatchInt(this.RegExpService.MatchFirstGroup(document.HtmlDocument.ParsedText, @"<th>Attendance<\/th>\s*<td(?:.*?)>(.*?)<\/td>"));

                await this.SetBeachVolleyballStatisticsAsync(match.Team1, document.Rounds.ElementAtOrDefault(1), options.Event.Id, options.Game.Year);
                await this.SetBeachVolleyballStatisticsAsync(match.Team2, document.Rounds.ElementAtOrDefault(2), options.Event.Id, options.Game.Year);
            }

            round.TeamMatches.Add(match);
        }
    }

    private async Task SetBeachVolleyballStatisticsAsync(BeachVolleyball team, RoundDataModel roundData, Guid eventId, int year)
    {
        if (roundData == null)
        {
            return;
        }

        foreach (var row in roundData.Rows.Skip(1))
        {
            var data = row.Elements("td").ToList();
            var athleteModel = this.OlympediaService.FindAthlete(row.OuterHtml);
            if (athleteModel == null)
            {
                if (year >= 2012)
                {
                    team.ServiceAttempts = this.RegExpService.MatchInt(data[1].InnerText);
                    team.ServiceFaults = this.RegExpService.MatchInt(data[2].InnerText);
                    team.ServiceAces = this.RegExpService.MatchInt(data[3].InnerText);
                    team.AttackAttempts = this.RegExpService.MatchInt(data[5].InnerText);
                    team.AttackSuccesses = this.RegExpService.MatchInt(data[6].InnerText);
                    team.BlockSuccesses = this.RegExpService.MatchInt(data[7].InnerText);
                    team.OpponentErrors = this.RegExpService.MatchInt(data[8].InnerText);
                    team.TotalPoints = this.RegExpService.MatchInt(data[9].InnerText);
                    team.DigSuccesses = this.RegExpService.MatchInt(data[10].InnerText);
                }
            }
            else
            {
                var participant = await this.ParticipationRepository.GetAsync(x => x.Code == athleteModel.Code && x.EventId == eventId);
                var athlete = new BeachVolleyball
                {
                    Id = participant.Id,
                    Name = athleteModel.Name,
                    NOC = team.NOC,
                    Code = athleteModel.Code,
                    Number = this.GetInt(roundData.Indexes, ConverterConstants.Number, data),
                    Position = this.GetString(roundData.Indexes, ConverterConstants.Position, data),
                    ServiceAces = this.GetInt(roundData.Indexes, ConverterConstants.ServiceAces, data),
                    ServiceAttempts = this.GetInt(roundData.Indexes, ConverterConstants.ServiceAttempts, data),
                    ServiceFaults = this.GetInt(roundData.Indexes, ConverterConstants.ServiceFaults, data),
                    FastestServe = this.GetInt(roundData.Indexes, ConverterConstants.FastestServe, data),
                    AttackAttempts = this.GetInt(roundData.Indexes, ConverterConstants.AttackAttempts, data),
                    AttackSuccesses = this.GetInt(roundData.Indexes, ConverterConstants.AttackSuccesses, data),
                    BlockSuccesses = this.GetInt(roundData.Indexes, ConverterConstants.BlockSuccesses, data),
                    DigSuccesses = this.GetInt(roundData.Indexes, ConverterConstants.DigSuccesses, data),

                };

                team.Athletes.Add(athlete);
            }
        }
    }
}