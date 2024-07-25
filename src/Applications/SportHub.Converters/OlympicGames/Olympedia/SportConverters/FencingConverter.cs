namespace SportHub.Converters.OlympicGames.Olympedia.SportConverters;

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

public class FencingConverter : BaseSportConverter
{
    public FencingConverter(IOlympediaService olympediaService, IDateService dateService, IDataCacheService dataCacheService, IRegExpService regExpService, IMapper mapper,
        INormalizeService normalizeService, OlympicGamesRepository<Team> teamRepository, OlympicGamesRepository<Participation> participationRepository,
        OlympicGamesRepository<Athlete> athleteRepository, OlympicGamesRepository<Result> resultRepository)
        : base(olympediaService, dateService, dataCacheService, regExpService, mapper, normalizeService, teamRepository, participationRepository, athleteRepository, resultRepository)
    {
    }

    public override async Task ProcessAsync(Options options)
    {
        var rounds = new List<Round<Fencing>>();
        await Console.Out.WriteLineAsync($"{options.Game.Year} - {options.Event.Name}");

        var allRounds = options.Rounds;
        if (options.Rounds.Count != 1)
        {
            allRounds = options.Rounds.Skip(1).ToList();
        }

        foreach (var roundData in allRounds)
        {
            var round = this.CreateRound<Fencing>(roundData, options.Event.Name, null);
            round.Judges = await this.GetJudgesAsync(roundData.Html);
            await this.SetMatchesAsync(round, roundData, options);
            rounds.Add(round);
        }

        await this.ProcessJsonAsync(rounds, options);
    }

    private async Task SetMatchesAsync(Round<Fencing> round, RoundDataModel roundData, Options options)
    {
        foreach (var row in roundData.Rows)
        {
            var data = row.Elements("td").ToList();

            if (data.Count != 0 && (data[0].InnerText.StartsWith("Bout ") || data[0].InnerText.StartsWith("Match ") || data[0].InnerText.StartsWith("Pool ") || data[0].InnerText.StartsWith("Final Pool")))
            {
                if (options.Event.IsTeamEvent)
                {
                    await this.SetTeamMatchAsync(round, roundData, row, data, options);
                }
                else
                {
                    await this.SetAthleteMatchAsync(round, roundData, row, data, options);
                }
            }
            else if (data.Count != 0)
            {
                var noc = this.OlympediaService.FindNOCCode(row.OuterHtml);
                Fencing fencing = null;
                if (options.Event.IsTeamEvent)
                {
                    if (noc != null)
                    {
                        var nocCache = this.DataCacheService.NOCs.FirstOrDefault(x => x.Code == noc);
                        var teamName = this.GetString(roundData.Indexes, ConverterConstants.Name, data);
                        var dbTeam = await this.TeamRepository.GetAsync(x => x.NOCId == nocCache.Id && x.EventId == options.Event.Id);

                        if (dbTeam != null)
                        {
                            fencing = new Fencing
                            {
                                Id = dbTeam.Id,
                                Name = dbTeam.Name,
                                GroupNumber = roundData.Number,
                            };

                            round.Teams.Add(fencing);
                        }
                    }
                }
                else
                {
                    var athleteModel = this.OlympediaService.FindAthlete(data[roundData.Indexes[ConverterConstants.Name]].OuterHtml);
                    if (athleteModel != null)
                    {
                        var participant = await this.ParticipationRepository.GetAsync(x => x.Code == athleteModel.Code && x.EventId == options.Event.Id);
                        if (participant != null)
                        {
                            fencing = new Fencing
                            {
                                Id = participant.Id,
                                Name = athleteModel.Name,
                                Code = athleteModel.Code,
                                GroupNumber = roundData.Number,
                            };

                            round.Athletes.Add(fencing);
                        }
                    }
                }

                if (fencing != null)
                {
                    fencing.NOC = noc;
                    fencing.FinishStatus = this.OlympediaService.FindFinishStatus(row.OuterHtml);
                    fencing.IsQualified = this.OlympediaService.IsQualified(row.OuterHtml);
                    fencing.Points = this.GetInt(roundData.Indexes, ConverterConstants.Points, data);
                    fencing.Wins = this.GetInt(roundData.Indexes, ConverterConstants.Wins, data);
                    fencing.Draws = this.GetInt(roundData.Indexes, ConverterConstants.Draw, data);
                    fencing.Losses = this.GetInt(roundData.Indexes, ConverterConstants.Losses, data);
                    fencing.BarrageWins = this.GetInt(roundData.Indexes, ConverterConstants.BarrageBoutsWon, data);
                    fencing.BarrageLosses = this.GetInt(roundData.Indexes, ConverterConstants.BarrageBoutsLoss, data);
                    fencing.TouchesDelivered = this.GetInt(roundData.Indexes, ConverterConstants.TouchesDelivered, data);
                    fencing.TouchesReceived = this.GetInt(roundData.Indexes, ConverterConstants.TouchesReceived, data);
                }
            }
        }
    }

    private async Task SetTeamMatchAsync(Round<Fencing> round, RoundDataModel roundData, HtmlNode row, List<HtmlNode> data, Options options)
    {
        if (data.Count != 5)
        {
            var matchInputModel = new MatchInputModel
            {
                Row = row.OuterHtml,
                Number = data[0].OuterHtml,
                Date = data.Count == 4 ? null : data[1].InnerText,
                Year = options.Game.Year,
                EventId = options.Event.Id,
                IsTeam = options.Event.IsTeamEvent,
                IsDoubles = false,
                HomeName = data.Count == 4 ? null : data[3].OuterHtml,
                HomeNOC = data.Count == 4 ? data[1].OuterHtml : data[4].OuterHtml,
                Result = data.Count == 4 ? data[2].OuterHtml : data[5].OuterHtml,
                AwayName = data.Count == 4 ? null : data[6].OuterHtml,
                AwayNOC = data.Count == 4 ? data[3].OuterHtml : data[7].OuterHtml,
                AnyParts = false,
                RoundType = roundData.Type,
                RoundSubType = roundData.SubType,
                Location = data.Count == 4 ? null : data[2].InnerText,
            };

            var matchModel = await this.GetMatchAsync(matchInputModel);
            var match = this.Mapper.Map<TeamMatch<Fencing>>(matchModel);
            var document = options.Documents.FirstOrDefault(x => x.Id == match.ResultId);
            if (document != null && match.Team2.NOC != null)
            {
                match.Judges = await this.GetJudgesAsync(document.Html);
                match.MatchTime = this.DateService.ParseTime(this.RegExpService.MatchFirstGroup(document.Html, @"Match Time: (\d+:\d+)"));

                var roundDataModels = document.Rounds;
                if (roundDataModels.Count == 4)
                {
                    roundDataModels = document.Rounds.Skip(1).ToList();
                }

                await this.SetAthletesAsync(match.Team1, roundDataModels.ElementAtOrDefault(0), options.Event.Id);
                await this.SetAthletesAsync(match.Team2, roundDataModels.ElementAtOrDefault(1), options.Event.Id);

                var lastRoundData = roundDataModels.ElementAtOrDefault(2);
                if (lastRoundData != null)
                {
                    foreach (var currentRow in lastRoundData.Rows)
                    {
                        var currentData = currentRow.Elements("td").ToList();
                        await this.SetAthleteMatchAsync(round, lastRoundData, currentRow, currentData, options);
                    }
                }
            }

            round.TeamMatches.Add(match);
        }
    }

    private async Task SetAthleteMatchAsync(Round<Fencing> round, RoundDataModel roundData, HtmlNode row, List<HtmlNode> data, Options options)
    {
        var noc = this.OlympediaService.FindNOCCodes(row.OuterHtml);
        if (noc.Count != 0)
        {
            MatchInputModel matchInputModel = null;
            if (options.Event.IsTeamEvent && options.Game.Year >= 1996)
            {
                matchInputModel = new MatchInputModel
                {
                    Row = row.OuterHtml,
                    Number = data[0].OuterHtml,
                    Date = data[1].InnerText,
                    Year = options.Game.Year,
                    EventId = options.Event.Id,
                    IsTeam = false,
                    IsDoubles = false,
                    HomeName = !roundData.Indexes.ContainsKey(ConverterConstants.Location) ? data[2].OuterHtml : data[3].OuterHtml,
                    HomeNOC = !roundData.Indexes.ContainsKey(ConverterConstants.Location) ? data[3].OuterHtml : data[4].OuterHtml,
                    Result = !roundData.Indexes.ContainsKey(ConverterConstants.Location) ? data[4].OuterHtml : data[5].OuterHtml,
                    AwayName = !roundData.Indexes.ContainsKey(ConverterConstants.Location) ? data[5].OuterHtml : data[6].OuterHtml,
                    AwayNOC = !roundData.Indexes.ContainsKey(ConverterConstants.Location) ? data[6].OuterHtml : data[7].OuterHtml,
                    AnyParts = false,
                    RoundType = roundData.Type,
                    RoundSubType = roundData.SubType,
                    Location = !roundData.Indexes.ContainsKey(ConverterConstants.Location) ? null : data[2].InnerText,
                };
            }
            else
            {
                matchInputModel = new MatchInputModel
                {
                    Row = row.OuterHtml,
                    Number = data[0].OuterHtml,
                    Date = data.Count is 6 or 7 ? null : data[1].InnerText,
                    Year = options.Game.Year,
                    EventId = options.Event.Id,
                    IsTeam = false,
                    IsDoubles = false,
                    HomeName = data.Count is 6 or 7 ? data[1].OuterHtml : data[3].OuterHtml,
                    HomeNOC = data.Count is 6 or 7 ? data[2].OuterHtml : data[4].OuterHtml,
                    Result = data.Count is 6 or 7 ? data[3].OuterHtml : data[5].OuterHtml,
                    AwayName = data.Count is 6 or 7 ? data[4].OuterHtml : data[6].OuterHtml,
                    AwayNOC = data.Count is 6 or 7 ? data[5].OuterHtml : data[7].OuterHtml,
                    AnyParts = false,
                    RoundType = roundData.Type,
                    RoundSubType = roundData.SubType,
                    Location = data.Count is 6 or 7 ? null : data[2].InnerText,
                };
            }

            var matchModel = await this.GetMatchAsync(matchInputModel);
            var match = this.Mapper.Map<AthleteMatch<Fencing>>(matchModel);
            var document = options.Documents.FirstOrDefault(x => x.Id == match.ResultId);
            if (document != null && match.Athlete2.Code != 0)
            {
                match.Judges = await this.GetJudgesAsync(document.Html);
                match.MatchTime = this.DateService.ParseTime(this.RegExpService.MatchFirstGroup(document.Html, @"Match Time: (\d+:\d+)"));
                this.SetAthleteMatchDocument(match, document.Rounds.LastOrDefault());
            }

            round.AthleteMatches.Add(match);
        }
    }

    private async Task SetAthletesAsync(Fencing team, RoundDataModel roundData, Guid eventId)
    {
        if (roundData != null)
        {
            foreach (var row in roundData.Rows.Skip(1))
            {
                var athleteModel = this.OlympediaService.FindAthlete(row.OuterHtml);
                if (athleteModel != null)
                {
                    var participation = await this.ParticipationRepository.GetAsync(x => x.Code == athleteModel.Code && x.EventId == eventId);

                    team.Athletes.Add(new Fencing
                    {
                        Id = participation.Id,
                        Name = athleteModel.Name,
                        Code = athleteModel.Code,
                        NOC = team.NOC
                    });
                }
            }

            if (roundData.Indexes.ContainsKey(ConverterConstants.YellowCard))
            {
                var data = roundData.Rows.Last().Elements("td").ToList();
                team.YellowCards = this.RegExpService.MatchInt(data[2].InnerText);
            }

            if (roundData.Indexes.ContainsKey(ConverterConstants.RedCard))
            {
                var data = roundData.Rows.Last().Elements("td").ToList();
                team.RedCards = this.RegExpService.MatchInt(data[3].InnerText);
            }
        }
    }

    private void SetAthleteMatchDocument(AthleteMatch<Fencing> match, RoundDataModel roundData)
    {
        if (roundData != null)
        {
            foreach (var row in roundData.Rows.Skip(1))
            {
                var data = row.Elements("td").ToList();
                var header = row.Elements("th").FirstOrDefault();

                if (header != null)
                {
                    switch (header.InnerText.Trim())
                    {
                        case "Yellow Cards":
                            match.Athlete1.YellowCards = this.RegExpService.MatchInt(data.ElementAtOrDefault(0)?.InnerText);
                            match.Athlete2.YellowCards = this.RegExpService.MatchInt(data.ElementAtOrDefault(1)?.InnerText);
                            break;
                        case "Red Cards":
                            match.Athlete1.RedCards = this.RegExpService.MatchInt(data.ElementAtOrDefault(0)?.InnerText);
                            match.Athlete2.RedCards = this.RegExpService.MatchInt(data.ElementAtOrDefault(1)?.InnerText);
                            break;
                        case "Period 1":
                            match.Athlete1.Period1Points = this.RegExpService.MatchInt(data.ElementAtOrDefault(0)?.InnerText);
                            match.Athlete2.Period1Points = this.RegExpService.MatchInt(data.ElementAtOrDefault(1)?.InnerText);
                            break;
                        case "Period 2":
                            match.Athlete1.Period2Points = this.RegExpService.MatchInt(data.ElementAtOrDefault(0)?.InnerText);
                            match.Athlete2.Period2Points = this.RegExpService.MatchInt(data.ElementAtOrDefault(1)?.InnerText);
                            break;
                        case "Period 3":
                            match.Athlete1.Period3Points = this.RegExpService.MatchInt(data.ElementAtOrDefault(0)?.InnerText);
                            match.Athlete2.Period3Points = this.RegExpService.MatchInt(data.ElementAtOrDefault(1)?.InnerText);
                            break;
                    }
                }
            }
        }
    }
}