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

public class CyclingConverter : BaseSportConverter
{
    public CyclingConverter(IOlympediaService olympediaService, IDateService dateService, IDataCacheService dataCacheService, IRegExpService regExpService, IMapper mapper,
        INormalizeService normalizeService, OlympicGamesRepository<Team> teamRepository, OlympicGamesRepository<Participation> participationRepository,
        OlympicGamesRepository<Athlete> athleteRepository, OlympicGamesRepository<Result> resultRepository)
        : base(olympediaService, dateService, dataCacheService, regExpService, mapper, normalizeService, teamRepository, participationRepository, athleteRepository, resultRepository)
    {
    }

    public override async Task ProcessAsync(Options options)
    {
        switch (options.Discipline.Name)
        {
            case DisciplineConstants.CYCLING_BMX_FREESTYLE:
                await this.ProcessCyclingBMXFreestyleAsync(options);
                break;
            case DisciplineConstants.CYCLING_BMX_RACING:
                await this.ProcessCyclingBMXRacingAsync(options);
                break;
            case DisciplineConstants.CYCLING_MOUNTAIN_BIKE:
                await this.ProcessCyclingMountainBikeAsync(options);
                break;
            case DisciplineConstants.CYCLING_ROAD:
                await this.ProcessCyclingRoadAsync(options);
                break;
            case DisciplineConstants.CYCLING_TRACK:
                await this.ProcessCyclingTrackAsync(options);
                break;
        }
    }

    private async Task ProcessCyclingTrackAsync(Options options)
    {
        var rounds = new List<Round<CyclingRoad>>();
        await Console.Out.WriteLineAsync($"{options.Game.Year} - {options.Event.Name}");

        var allRounds = options.Rounds;
        if (options.Rounds.Count != 1)
        {
            allRounds = options.Rounds.Skip(1).ToList();
        }

        foreach (var roundData in allRounds)
        {
            var round = this.CreateRound<CyclingRoad>(roundData, options.Event.Name, null);
            //await this.SetCyclingTrackAthletesAsync(round, roundData, options);
            foreach (var item in roundData.Indexes)
            {
                Console.WriteLine(item.Key);
            }
            rounds.Add(round);
        }

        await this.ProcessJsonAsync(rounds, options);
    }

    private async Task SetCyclingTrackAthletesAsync(Round<CyclingRoad> round, RoundDataModel roundData, Options options)
    {
    }

    private async Task ProcessCyclingRoadAsync(Options options)
    {
        var rounds = new List<Round<CyclingRoad>>();
        await Console.Out.WriteLineAsync($"{options.Game.Year} - {options.Event.Name}");

        var roundData = options.Rounds.FirstOrDefault();
        var track = this.SetCyclingRoadTrack(options.HtmlDocument.ParsedText);
        var round = this.CreateRound<CyclingRoad>(roundData, options.Event.Name, track);
        await this.SetCyclingRoadAthletesAsync(round, roundData, options);
        rounds.Add(round);

        await this.ProcessJsonAsync(rounds, options);
    }

    private Track SetCyclingRoadTrack(string html)
    {
        var length = this.RegExpService.MatchFirstGroup(html, @"Distance:(.*?)<br>");
        var inter1 = this.RegExpService.MatchFirstGroup(html, @"Intermediate 1:(.*?)<br>");
        var inter2 = this.RegExpService.MatchFirstGroup(html, @"Intermediate 2:(.*?)<br>");
        var inter3 = this.RegExpService.MatchFirstGroup(html, @"Intermediate 3:(.*?)<br>");
        var inter4 = this.RegExpService.MatchFirstGroup(html, @"Intermediate 4:(.*?)<br>");
        var inter5 = this.RegExpService.MatchFirstGroup(html, @"Intermediate 5:(.*?) km");

        var track = new Track
        {
            Length = this.RegExpService.MatchInt(length),
        };

        if (inter1 != null)
        {
            track.Intermediates.Add(new Intermediate { Number = 1, Kilometers = this.RegExpService.MatchDouble(inter1) });
        }

        if (inter2 != null)
        {
            track.Intermediates.Add(new Intermediate { Number = 2, Kilometers = this.RegExpService.MatchDouble(inter2) });
        }

        if (inter3 != null)
        {
            track.Intermediates.Add(new Intermediate { Number = 3, Kilometers = this.RegExpService.MatchDouble(inter3) });
        }

        if (inter4 != null)
        {
            track.Intermediates.Add(new Intermediate { Number = 4, Kilometers = this.RegExpService.MatchDouble(inter4) });
        }

        if (inter5 != null)
        {
            track.Intermediates.Add(new Intermediate { Number = 5, Kilometers = this.RegExpService.MatchDouble(inter5) });
        }

        return track;
    }

    private async Task SetCyclingRoadAthletesAsync(Round<CyclingRoad> round, RoundDataModel roundData, Options options)
    {
        foreach (var row in roundData.Rows.Skip(1))
        {
            var noc = this.OlympediaService.FindNOCCode(row.OuterHtml);
            var data = row.Elements("td").ToList();

            CyclingRoad cycling = null;
            if (options.Event.IsTeamEvent)
            {
                var athleteModels = this.OlympediaService.FindAthletes(row.OuterHtml);

                if (athleteModels.Count == 0)
                {
                    var nocCache = this.DataCacheService.NOCs.FirstOrDefault(x => x.Code == noc);
                    var teamName = this.GetString(roundData.Indexes, ConverterConstants.Name, data);
                    var dbTeam = await this.TeamRepository.GetAsync(x => x.NOCId == nocCache.Id && x.EventId == options.Event.Id);

                    cycling = new CyclingRoad
                    {
                        Id = dbTeam.Id,
                        Name = dbTeam.Name,
                    };

                    round.Teams.Add(cycling);
                }
                else
                {
                    var team = round.Teams.Last();

                    if (athleteModels.Count == 1)
                    {
                        var athleteModel = this.OlympediaService.FindAthlete(data[roundData.Indexes[ConverterConstants.Name]].OuterHtml);
                        if (athleteModel != null)
                        {
                            var participant = await this.ParticipationRepository.GetAsync(x => x.Code == athleteModel.Code && x.EventId == options.Event.Id);
                            cycling = new CyclingRoad
                            {
                                Id = participant.Id,
                                Name = athleteModel.Name,
                                Code = athleteModel.Code,
                            };

                            team.Athletes.Add(cycling);
                        }
                    }
                    else
                    {
                        foreach (var athleteModel in athleteModels)
                        {
                            var participant = await this.ParticipationRepository.GetAsync(x => x.Code == athleteModel.Code && x.EventId == options.Event.Id);
                            team.Athletes.Add(new CyclingRoad
                            {
                                Id = participant.Id,
                                Name = athleteModel.Name,
                                Code = athleteModel.Code,
                            });
                        }
                    }
                }
            }
            else
            {
                var athleteModel = this.OlympediaService.FindAthlete(data[roundData.Indexes[ConverterConstants.Name]].OuterHtml);
                if (athleteModel != null)
                {
                    var participant = await this.ParticipationRepository.GetAsync(x => x.Code == athleteModel.Code && x.EventId == options.Event.Id);

                    cycling = new CyclingRoad
                    {
                        Id = participant.Id,
                        Name = athleteModel.Name,
                        Code = athleteModel.Code,
                        Number = this.GetInt(roundData.Indexes, ConverterConstants.Number, data),
                    };

                    round.Athletes.Add(cycling);
                }
            }

            if (cycling != null)
            {
                cycling.NOC = noc;
                cycling.FinishStatus = this.OlympediaService.FindFinishStatus(row.OuterHtml);
                cycling.Time = this.GetTime(roundData.Indexes, ConverterConstants.Time, data);
                cycling.Time ??= this.GetTime(roundData.Indexes, ConverterConstants.AdjustedTime, data);

                if (roundData.Indexes.ContainsKey(ConverterConstants.Margin))
                {
                    var margin = this.GetTime(roundData.Indexes, ConverterConstants.Margin, data);
                    if (cycling.Time == null && margin != null)
                    {
                        cycling.Time = round.Athletes.FirstOrDefault().Time + margin;
                    }
                }

                if (roundData.Indexes.ContainsKey(ConverterConstants.TimeMargin))
                {
                    if (round.Athletes.Count == 1)
                    {
                        cycling.Time = this.GetTime(roundData.Indexes, ConverterConstants.TimeMargin, data);
                    }
                    else
                    {
                        var timeMargin = this.GetString(roundData.Indexes, ConverterConstants.TimeMargin, data);
                        if (timeMargin == "same time")
                        {
                            cycling.Time = round.Athletes[round.Athletes.Count - 2].Time;
                        }
                        else
                        {
                            var time = this.GetTime(roundData.Indexes, ConverterConstants.TimeMargin, data);
                            cycling.Time = round.Athletes.FirstOrDefault().Time + time;
                        }
                    }
                }

                if (roundData.Indexes.ContainsKey(ConverterConstants.Intermediate1))
                {
                    cycling.Intermediates.Add(new CyclingRoadIntermediate { Number = 1, Time = this.GetTime(roundData.Indexes, ConverterConstants.Intermediate1, data) });
                }

                if (roundData.Indexes.ContainsKey(ConverterConstants.Intermediate2))
                {
                    cycling.Intermediates.Add(new CyclingRoadIntermediate { Number = 2, Time = this.GetTime(roundData.Indexes, ConverterConstants.Intermediate2, data) });
                }

                if (roundData.Indexes.ContainsKey(ConverterConstants.Intermediate3))
                {
                    cycling.Intermediates.Add(new CyclingRoadIntermediate { Number = 3, Time = this.GetTime(roundData.Indexes, ConverterConstants.Intermediate3, data) });
                }

                if (roundData.Indexes.ContainsKey(ConverterConstants.Intermediate4))
                {
                    cycling.Intermediates.Add(new CyclingRoadIntermediate { Number = 4, Time = this.GetTime(roundData.Indexes, ConverterConstants.Intermediate4, data) });
                }

                if (roundData.Indexes.ContainsKey(ConverterConstants.Intermediate5))
                {
                    cycling.Intermediates.Add(new CyclingRoadIntermediate { Number = 5, Time = this.GetTime(roundData.Indexes, ConverterConstants.Intermediate6, data) });
                }
            }
        }
    }

    private async Task ProcessCyclingMountainBikeAsync(Options options)
    {
        var rounds = new List<Round<CyclingMountainBike>>();
        await Console.Out.WriteLineAsync($"{options.Game.Year} - {options.Event.Name}");

        var roundData = options.Rounds.FirstOrDefault();
        var track = this.SetCyclingMountainBikeTrack(options.HtmlDocument.ParsedText);
        var round = this.CreateRound<CyclingMountainBike>(roundData, options.Event.Name, track);
        await this.SetCyclingMountainBikeAthletesAsync(round, roundData, options);
        rounds.Add(round);

        await this.ProcessJsonAsync(rounds, options);
    }

    private Track SetCyclingMountainBikeTrack(string html)
    {
        var length = this.RegExpService.MatchFirstGroup(html, @"Distance:(.*?)<br>");
        var inter1 = this.RegExpService.MatchFirstGroup(html, @"Intermediate 1:(.*?)<br>");
        var inter2 = this.RegExpService.MatchFirstGroup(html, @"Intermediate 2:(.*?)<br>");
        var inter3 = this.RegExpService.MatchFirstGroup(html, @"Intermediate 3:(.*?)<br>");
        var inter4 = this.RegExpService.MatchFirstGroup(html, @"Intermediate 4:(.*?)<br>");
        var inter5 = this.RegExpService.MatchFirstGroup(html, @"Intermediate 5:(.*?)<br>");
        var inter6 = this.RegExpService.MatchFirstGroup(html, @"Intermediate 6:(.*?)<br>");
        var inter7 = this.RegExpService.MatchFirstGroup(html, @"Intermediate 7:(.*?) km");

        var track = new Track
        {
            Length = this.RegExpService.MatchInt(length),
        };

        if (inter1 != null)
        {
            track.Intermediates.Add(new Intermediate { Number = 1, Kilometers = this.RegExpService.MatchDouble(inter1) });
        }

        if (inter2 != null)
        {
            track.Intermediates.Add(new Intermediate { Number = 2, Kilometers = this.RegExpService.MatchDouble(inter2) });
        }

        if (inter3 != null)
        {
            track.Intermediates.Add(new Intermediate { Number = 3, Kilometers = this.RegExpService.MatchDouble(inter3) });
        }

        if (inter4 != null)
        {
            track.Intermediates.Add(new Intermediate { Number = 4, Kilometers = this.RegExpService.MatchDouble(inter4) });
        }

        if (inter5 != null)
        {
            track.Intermediates.Add(new Intermediate { Number = 5, Kilometers = this.RegExpService.MatchDouble(inter5) });
        }

        if (inter6 != null)
        {
            track.Intermediates.Add(new Intermediate { Number = 6, Kilometers = this.RegExpService.MatchDouble(inter6) });
        }
        if (inter7 != null)
        {
            track.Intermediates.Add(new Intermediate { Number = 7, Kilometers = this.RegExpService.MatchDouble(inter7) });
        }

        return track;
    }

    private async Task SetCyclingMountainBikeAthletesAsync(Round<CyclingMountainBike> round, RoundDataModel roundData, Options options)
    {
        foreach (var row in roundData.Rows.Skip(1))
        {
            var noc = this.OlympediaService.FindNOCCode(row.OuterHtml);
            var data = row.Elements("td").ToList();

            var athleteModel = this.OlympediaService.FindAthlete(data[roundData.Indexes[ConverterConstants.Name]].OuterHtml);
            if (athleteModel != null)
            {
                var participant = await this.ParticipationRepository.GetAsync(x => x.Code == athleteModel.Code && x.EventId == options.Event.Id);

                var athlete = new CyclingMountainBike
                {
                    Id = participant.Id,
                    Name = athleteModel.Name,
                    Code = athleteModel.Code,
                    NOC = noc,
                    FinishStatus = this.OlympediaService.FindFinishStatus(row.OuterHtml),
                    Time = this.GetTime(roundData.Indexes, ConverterConstants.Time, data),
                };

                var margin = this.GetTime(roundData.Indexes, ConverterConstants.Margin, data);
                if (athlete.Time == null && margin != null)
                {
                    athlete.Time = round.Athletes.FirstOrDefault().Time + margin;
                }

                if (roundData.Indexes.ContainsKey(ConverterConstants.Intermediate1))
                {
                    athlete.Intermediates.Add(new CyclingMountainBIkeIntermediate { Number = 1, Time = this.GetTime(roundData.Indexes, ConverterConstants.Intermediate1, data) });
                }

                if (roundData.Indexes.ContainsKey(ConverterConstants.Intermediate2))
                {
                    athlete.Intermediates.Add(new CyclingMountainBIkeIntermediate { Number = 2, Time = this.GetTime(roundData.Indexes, ConverterConstants.Intermediate2, data) });
                }

                if (roundData.Indexes.ContainsKey(ConverterConstants.Intermediate3))
                {
                    athlete.Intermediates.Add(new CyclingMountainBIkeIntermediate { Number = 3, Time = this.GetTime(roundData.Indexes, ConverterConstants.Intermediate3, data) });
                }

                if (roundData.Indexes.ContainsKey(ConverterConstants.Intermediate4))
                {
                    athlete.Intermediates.Add(new CyclingMountainBIkeIntermediate { Number = 4, Time = this.GetTime(roundData.Indexes, ConverterConstants.Intermediate4, data) });
                }

                if (roundData.Indexes.ContainsKey(ConverterConstants.Intermediate5))
                {
                    athlete.Intermediates.Add(new CyclingMountainBIkeIntermediate { Number = 5, Time = this.GetTime(roundData.Indexes, ConverterConstants.Intermediate6, data) });
                }

                if (roundData.Indexes.ContainsKey(ConverterConstants.Intermediate6))
                {
                    athlete.Intermediates.Add(new CyclingMountainBIkeIntermediate { Number = 6, Time = this.GetTime(roundData.Indexes, ConverterConstants.Intermediate6, data) });
                }

                if (roundData.Indexes.ContainsKey(ConverterConstants.Intermediate7))
                {
                    athlete.Intermediates.Add(new CyclingMountainBIkeIntermediate { Number = 7, Time = this.GetTime(roundData.Indexes, ConverterConstants.Intermediate7, data) });
                }

                round.Athletes.Add(athlete);
            }
        }
    }

    private async Task ProcessCyclingBMXRacingAsync(Options options)
    {
        var rounds = new List<Round<CyclingBMXRacing>>();
        await Console.Out.WriteLineAsync($"{options.Game.Year} - {options.Event.Name}");

        foreach (var roundData in options.Rounds.Skip(1))
        {
            var round = this.CreateRound<CyclingBMXRacing>(roundData, options.Event.Name, null);
            await this.SetCyclingBMXRacingleAthletesAsync(round, roundData, options);
            rounds.Add(round);
        }

        await this.ProcessJsonAsync(rounds, options);
    }

    private async Task SetCyclingBMXRacingleAthletesAsync(Round<CyclingBMXRacing> round, RoundDataModel roundData, Options options)
    {
        foreach (var row in roundData.Rows.Skip(1))
        {
            var noc = this.OlympediaService.FindNOCCode(row.OuterHtml);
            var data = row.Elements("td").ToList();

            var athleteModel = this.OlympediaService.FindAthlete(data[roundData.Indexes[ConverterConstants.Name]].OuterHtml);
            if (athleteModel != null)
            {
                var participant = await this.ParticipationRepository.GetAsync(x => x.Code == athleteModel.Code && x.EventId == options.Event.Id);

                var athlete = new CyclingBMXRacing
                {
                    Id = participant.Id,
                    Name = athleteModel.Name,
                    Code = athleteModel.Code,
                    NOC = noc,
                    FinishStatus = this.OlympediaService.FindFinishStatus(row.OuterHtml),
                    IsQualified = this.OlympediaService.IsQualified(row.OuterHtml),
                    Points = this.GetInt(roundData.Indexes, ConverterConstants.Points, data),
                    Time = this.GetTime(roundData.Indexes, ConverterConstants.Time, data),
                    Run1Points = this.GetInt(roundData.Indexes, ConverterConstants.Run1, data),
                    Run2Points = this.GetInt(roundData.Indexes, ConverterConstants.Run2, data),
                    Run3Points = this.GetInt(roundData.Indexes, ConverterConstants.Run3, data),
                    Race1 = this.GetTime(roundData.Indexes, ConverterConstants.Race1, data),
                    Race2 = this.GetTime(roundData.Indexes, ConverterConstants.Race2, data),
                    Race3 = this.GetTime(roundData.Indexes, ConverterConstants.Race3, data),
                    Race4 = this.GetTime(roundData.Indexes, ConverterConstants.Race4, data),
                    Race5 = this.GetTime(roundData.Indexes, ConverterConstants.Race5, data),
                };

                athlete.Points ??= this.GetInt(roundData.Indexes, ConverterConstants.TotalPoints, data);
                athlete.Time ??= this.GetTime(roundData.Indexes, ConverterConstants.BestTime, data);

                round.Athletes.Add(athlete);
            }
        }
    }

    private async Task ProcessCyclingBMXFreestyleAsync(Options options)
    {
        var rounds = new List<Round<CyclingBMXFreestyle>>();
        await Console.Out.WriteLineAsync($"{options.Game.Year} - {options.Event.Name}");

        foreach (var roundData in options.Rounds.Skip(1))
        {
            var round = this.CreateRound<CyclingBMXFreestyle>(roundData, options.Event.Name, null);
            var judges = await this.GetJudgesAsync(roundData.Html);
            round.Judges = judges;
            await this.SetCyclingBMXFreestyleAthletesAsync(round, roundData, options);
            rounds.Add(round);
        }

        await this.ProcessJsonAsync(rounds, options);
    }

    private async Task SetCyclingBMXFreestyleAthletesAsync(Round<CyclingBMXFreestyle> round, RoundDataModel roundData, Options options)
    {
        foreach (var row in roundData.Rows.Skip(1))
        {
            var noc = this.OlympediaService.FindNOCCode(row.OuterHtml);
            var data = row.Elements("td").ToList();

            var athleteModel = this.OlympediaService.FindAthlete(data[roundData.Indexes[ConverterConstants.Name]].OuterHtml);
            if (athleteModel != null)
            {
                var participant = await this.ParticipationRepository.GetAsync(x => x.Code == athleteModel.Code && x.EventId == options.Event.Id);

                var athlete = new CyclingBMXFreestyle
                {
                    Id = participant.Id,
                    Name = athleteModel.Name,
                    Code = athleteModel.Code,
                    NOC = noc,
                    FinishStatus = this.OlympediaService.FindFinishStatus(row.OuterHtml),
                    Points = this.GetDouble(roundData.Indexes, ConverterConstants.Points, data),
                    Run1 = this.GetDouble(roundData.Indexes, ConverterConstants.Run1, data),
                    Run2 = this.GetDouble(roundData.Indexes, ConverterConstants.Run2, data),
                };

                round.Athletes.Add(athlete);
            }
        }
    }
}