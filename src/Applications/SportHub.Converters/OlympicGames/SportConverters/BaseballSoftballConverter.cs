namespace SportHub.Converters.OlympicGames.SportConverters;

using System;
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

public class BaseballSoftballConverter : BaseSportConverter
{
    public BaseballSoftballConverter(IOlympediaService olympediaService, IDateService dateService, IDataCacheService dataCacheService, IRegExpService regExpService,
        IMapper mapper, INormalizeService normalizeService, OlympicGamesRepository<Team> teamRepository, OlympicGamesRepository<Participation> participationRepository,
        OlympicGamesRepository<Athlete> athleteRepository, OlympicGamesRepository<Result> resultRepository)
        : base(olympediaService, dateService, dataCacheService, regExpService, mapper, normalizeService, teamRepository, participationRepository, athleteRepository, resultRepository)
    {
    }

    public override async Task ProcessAsync(Options options)
    {
        var rounds = new List<Round<Baseball>>();

        foreach (var roundData in options.Rounds.Skip(1))
        {
            var round = this.CreateRound<Baseball>(roundData, options.Event.Name, null);
            await this.SetMatchesAsync(round, options, roundData);
            rounds.Add(round);
        }

        await this.ProcessJsonAsync(rounds, options);
    }

    private async Task SetMatchesAsync(Round<Baseball> round, Options options, RoundDataModel roundData)
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
                HomeName = options.Game.Year is not 2004 and not 2008 ? data[2].OuterHtml : data[3].OuterHtml,
                HomeNOC = options.Game.Year is not 2004 and not 2008 ? data[3].OuterHtml : data[4].OuterHtml,
                Result = options.Game.Year is not 2004 and not 2008 ? data[4].OuterHtml : data[5].OuterHtml,
                AwayName = options.Game.Year is not 2004 and not 2008 ? data[5].OuterHtml : data[6].OuterHtml,
                AwayNOC = options.Game.Year is not 2004 and not 2008 ? data[6].OuterHtml : data[7].OuterHtml,
                AnyParts = false,
                RoundType = roundData.Type,
                RoundSubType = roundData.SubType,
                Location = options.Game.Year is not 2004 and not 2008 ? null : data[2].InnerText,
            };
            var matchModel = await this.GetMatchAsync(matchInputModel);
            var match = this.Mapper.Map<TeamMatch<Baseball>>(matchModel);

            var document = options.Documents.FirstOrDefault(x => x.Id == match.ResultId);
            if (document != null)
            {
                match.Attendance = this.RegExpService.MatchInt(this.RegExpService.MatchFirstGroup(document.HtmlDocument.ParsedText, @"<th>Attendance<\/th>\s*<td(?:.*?)>(.*?)<\/td>"));
                match.Judges = await this.GetJudgesAsync(document.HtmlDocument.ParsedText);

                if (options.Game.Year <= 2008)
                {
                    this.SetInningsData(match.Team1, document.Rounds[1].Rows.Skip(document.Rounds[1].Rows.Count - 2).Take(1).FirstOrDefault(), options.Game.Year);
                    this.SetInningsData(match.Team2, document.Rounds[2].Rows.Skip(document.Rounds[2].Rows.Count - 2).Take(1).FirstOrDefault(), options.Game.Year);
                }
                else
                {
                    this.SetInningsData(match.Team1, document.Rounds.FirstOrDefault().Rows.ElementAtOrDefault(1), options.Game.Year);
                    this.SetInningsData(match.Team2, document.Rounds.FirstOrDefault().Rows.ElementAtOrDefault(2), options.Game.Year);
                }

                await this.SetStatisticsAsync(options.Event.Id, match.Team1, document.Rounds[1], options.Game.Year);
                await this.SetStatisticsAsync(options.Event.Id, match.Team2, document.Rounds[2], options.Game.Year);
            }

            round.TeamMatches.Add(match);
        }
    }

    private async Task SetStatisticsAsync(Guid eventId, Baseball team, RoundDataModel roundData, int year)
    {
        foreach (var row in roundData.Rows.Skip(1))
        {
            var data = row.Elements("td").ToList();
            var athleteModel = this.OlympediaService.FindAthlete(data[roundData.Indexes[ConverterConstants.Name]].OuterHtml);
            if (athleteModel != null)
            {
                var participant = await this.ParticipationRepository.GetAsync(x => x.Code == athleteModel.Code && x.EventId == eventId);
                var athlete = new Baseball
                {
                    Id = participant != null ? participant.Id : Guid.Empty,
                    Name = athleteModel.Name,
                    NOC = team.NOC,
                    Code = athleteModel.Code,
                    Position = this.GetString(roundData.Indexes, ConverterConstants.Position, data),
                    Number = this.GetInt(roundData.Indexes, ConverterConstants.Number, data),
                    CaughtStealing = year > 2008 ? this.RegExpService.MatchInt(data[4].InnerText) : this.RegExpService.MatchInt(data[14].InnerText),
                    StolenBases = year > 2008 ? this.RegExpService.MatchInt(data[5].InnerText) : this.RegExpService.MatchInt(data[13].InnerText),
                    SacrificeHits = year > 2008 ? this.RegExpService.MatchInt(data[6].InnerText) : this.RegExpService.MatchInt(data[15].InnerText),
                    SacrificeFlies = year > 2008 ? this.RegExpService.MatchInt(data[7].InnerText) : this.RegExpService.MatchInt(data[16].InnerText),
                    WinLostSave = year > 2008 ? data[8].InnerText : data[17].InnerText,
                    InningsPitched = year > 2008 ? this.RegExpService.MatchDecimal(data[9].InnerText) : this.RegExpService.MatchDecimal(data[18].InnerText),
                    EarnedRunsAllowed = year > 2008 ? this.RegExpService.MatchInt(data[10].InnerText) : this.RegExpService.MatchInt(data[19].InnerText),
                    RunsAllowed = year > 2008 ? this.RegExpService.MatchInt(data[11].InnerText) : this.RegExpService.MatchInt(data[20].InnerText),
                    HitsAllowed = year > 2008 ? this.RegExpService.MatchInt(data[12].InnerText) : this.RegExpService.MatchInt(data[21].InnerText),
                    HomeRunsAllowed = year > 2008 ? this.RegExpService.MatchInt(data[13].InnerText) : this.RegExpService.MatchInt(data[22].InnerText),
                    Strikeouts = year > 2008 ? this.RegExpService.MatchInt(data[14].InnerText) : this.RegExpService.MatchInt(data[23].InnerText),
                    BasesOnBalls = year > 2008 ? this.RegExpService.MatchInt(data[15].InnerText) : this.RegExpService.MatchInt(data[24].InnerText),
                    WildPitches = year > 2008 ? this.RegExpService.MatchInt(data[16].InnerText) : this.RegExpService.MatchInt(data[25].InnerText),
                    Putouts = year > 2008 ? this.RegExpService.MatchInt(data[17].InnerText) : this.RegExpService.MatchInt(data[26].InnerText),
                    Assists = year > 2008 ? this.RegExpService.MatchInt(data[18].InnerText) : this.RegExpService.MatchInt(data[27].InnerText),
                    Errors = year > 2008 ? this.RegExpService.MatchInt(data[19].InnerText) : this.RegExpService.MatchInt(data[28].InnerText),
                    AtBats = year > 2008 ? null : this.RegExpService.MatchInt(data[4].InnerText),
                    Runs = year > 2008 ? null : this.RegExpService.MatchInt(data[5].InnerText),
                    Hits = year > 2008 ? null : this.RegExpService.MatchInt(data[6].InnerText),
                    Doubles = year > 2008 ? null : this.RegExpService.MatchInt(data[7].InnerText),
                    Triples = year > 2008 ? null : this.RegExpService.MatchInt(data[8].InnerText),
                    HomeRuns = year > 2008 ? null : this.RegExpService.MatchInt(data[9].InnerText),
                    RBIs = year > 2008 ? null : this.RegExpService.MatchInt(data[10].InnerText),
                    Walks = year > 2008 ? null : this.RegExpService.MatchInt(data[11].InnerText),
                    BattingStrikeouts = year > 2008 ? null : this.RegExpService.MatchInt(data[12].InnerText),
                };

                team.Athletes.Add(athlete);
            }
        }
    }

    private void SetInningsData(Baseball team, HtmlNode node, int year)
    {
        var data = node.Elements("td").ToList();
        if (year > 2008)
        {
            team.Runs = this.RegExpService.MatchInt(data[3].InnerText);
            team.Innings.Add(new Inning { Number = 1, Points = this.RegExpService.MatchInt(data.ElementAtOrDefault(4)?.InnerText) });
            team.Innings.Add(new Inning { Number = 2, Points = this.RegExpService.MatchInt(data.ElementAtOrDefault(5)?.InnerText) });
            team.Innings.Add(new Inning { Number = 3, Points = this.RegExpService.MatchInt(data.ElementAtOrDefault(6)?.InnerText) });
            team.Innings.Add(new Inning { Number = 4, Points = this.RegExpService.MatchInt(data.ElementAtOrDefault(7)?.InnerText) });
            team.Innings.Add(new Inning { Number = 5, Points = this.RegExpService.MatchInt(data.ElementAtOrDefault(8)?.InnerText) });
            team.Innings.Add(new Inning { Number = 6, Points = this.RegExpService.MatchInt(data.ElementAtOrDefault(9)?.InnerText) });
            team.Innings.Add(new Inning { Number = 7, Points = this.RegExpService.MatchInt(data.ElementAtOrDefault(10)?.InnerText) });
            team.Innings.Add(new Inning { Number = 8, Points = this.RegExpService.MatchInt(data.ElementAtOrDefault(11)?.InnerText) });
            team.Innings.Add(new Inning { Number = 9, Points = this.RegExpService.MatchInt(data.ElementAtOrDefault(12)?.InnerText) });
            team.Innings.Add(new Inning { Number = 10, Points = this.RegExpService.MatchInt(data.ElementAtOrDefault(13)?.InnerText) });
        }
        else
        {
            team.Innings.Add(new Inning { Number = 1, Points = this.RegExpService.MatchInt(data[1].InnerText) });
            team.Innings.Add(new Inning { Number = 2, Points = this.RegExpService.MatchInt(data[2].InnerText) });
            team.Innings.Add(new Inning { Number = 3, Points = this.RegExpService.MatchInt(data[3].InnerText) });
            team.Innings.Add(new Inning { Number = 4, Points = this.RegExpService.MatchInt(data[4].InnerText) });
            team.Innings.Add(new Inning { Number = 5, Points = this.RegExpService.MatchInt(data[5].InnerText) });
            team.Innings.Add(new Inning { Number = 6, Points = this.RegExpService.MatchInt(data[6].InnerText) });
            team.Innings.Add(new Inning { Number = 7, Points = this.RegExpService.MatchInt(data[7].InnerText) });
            team.Innings.Add(new Inning { Number = 8, Points = this.RegExpService.MatchInt(data[8].InnerText) });
            team.Innings.Add(new Inning { Number = 9, Points = this.RegExpService.MatchInt(data[9].InnerText) });
            team.Innings.Add(new Inning { Number = 10, Points = this.RegExpService.MatchInt(data[10].InnerText) });
            team.Innings.Add(new Inning { Number = 11, Points = this.RegExpService.MatchInt(data[11].InnerText) });
            team.Innings.Add(new Inning { Number = 12, Points = this.RegExpService.MatchInt(data[12].InnerText) });
            team.Innings.Add(new Inning { Number = 13, Points = this.RegExpService.MatchInt(data[13].InnerText) });
            team.Innings.Add(new Inning { Number = 14, Points = this.RegExpService.MatchInt(data[14].InnerText) });
            team.Innings.Add(new Inning { Number = 15, Points = this.RegExpService.MatchInt(data[15].InnerText) });
            team.Runs = this.RegExpService.MatchInt(data[16].InnerText);
            team.Hits = this.RegExpService.MatchInt(data[17].InnerText);
            team.Errors = this.RegExpService.MatchInt(data[18].InnerText);
            team.LOB = this.RegExpService.MatchInt(data[19].InnerText);
        }

        team.Innings = team.Innings.Where(x => x.Points != null).ToList();
    }
}