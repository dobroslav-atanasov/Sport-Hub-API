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

public class BiathlonConverter : BaseSportConverter
{
    public BiathlonConverter(IOlympediaService olympediaService, IDateService dateService, IDataCacheService dataCacheService, IRegExpService regExpService, IMapper mapper,
        INormalizeService normalizeService, OlympicGamesRepository<Team> teamRepository, OlympicGamesRepository<Participation> participationRepository,
        OlympicGamesRepository<Athlete> athleteRepository, OlympicGamesRepository<Result> resultRepository)
        : base(olympediaService, dateService, dataCacheService, regExpService, mapper, normalizeService, teamRepository, participationRepository, athleteRepository, resultRepository)
    {
    }

    public override async Task ProcessAsync(Options options)
    {
        var rounds = new List<Round<Biathlon>>();
        var roundData = options.Rounds.FirstOrDefault();
        var track = this.ExtractTrack(roundData.HtmlDocument);
        var round = this.CreateRound<Biathlon>(roundData, options.Event.Name, track);

        if (options.Event.IsTeamEvent)
        {
            await this.SetTeamsAsync(round, roundData, options.Event.Id);
            rounds.Add(round);
        }
        else
        {
            await this.SetAthletesAsync(round, roundData, options.Event.Id);
            rounds.Add(round);
        }

        await this.ProcessJsonAsync(rounds, options);
    }

    private async Task SetTeamsAsync(Round<Biathlon> round, RoundDataModel roundData, Guid eventId)
    {
        Biathlon team = null;
        foreach (var row in roundData.Rows.Skip(1))
        {
            var noc = this.OlympediaService.FindNOCCode(row.OuterHtml);
            var data = row.Elements("td").ToList();

            if (noc != null)
            {
                var nocCache = this.DataCacheService.NOCs.FirstOrDefault(x => x.Code == noc);
                var dbTeam = await this.TeamRepository.GetAsync(x => x.NOCId == nocCache.Id && x.EventId == eventId);

                team = new Biathlon
                {
                    Id = dbTeam.Id,
                    Name = dbTeam.Name,
                    NOC = noc,
                    FinishStatus = this.OlympediaService.FindFinishStatus(row.OuterHtml),
                    Time = this.GetTime(roundData.Indexes, ConverterConstants.Time, data),
                    Misses = this.GetInt(roundData.Indexes, ConverterConstants.Misses, data),
                    ExtraShots = this.GetInt(roundData.Indexes, ConverterConstants.ExtraShots, data),
                };

                round.Teams.Add(team);
            }
            else
            {
                var athleteModel = this.OlympediaService.FindAthlete(data[roundData.Indexes[ConverterConstants.Name]].OuterHtml);
                var participant = await this.ParticipationRepository.GetAsync(x => x.Code == athleteModel.Code && x.EventId == eventId);
                var athlete = new Biathlon
                {
                    Id = participant.Id,
                    Name = athleteModel.Name,
                    NOC = noc,
                    Code = athleteModel.Code,
                    FinishStatus = this.OlympediaService.FindFinishStatus(row.OuterHtml),
                    Number = this.GetInt(roundData.Indexes, ConverterConstants.Number, data),
                    Position = this.GetString(roundData.Indexes, ConverterConstants.Position, data),
                    Time = this.GetTime(roundData.Indexes, ConverterConstants.Time, data),
                    Misses = this.GetInt(roundData.Indexes, ConverterConstants.Misses, data),
                    ExtraShots = this.GetInt(roundData.Indexes, ConverterConstants.ExtraShots, data),
                    Exchange = this.GetTime(roundData.Indexes, ConverterConstants.Exchange, data),
                };

                if (roundData.Indexes.ContainsKey(ConverterConstants.Shooting1))
                {
                    athlete.Shootings.Add(new Shooting
                    {
                        Number = 1,
                        Misses = this.GetInt(roundData.Indexes, ConverterConstants.Shooting1Misses, data),
                        Time = this.GetTime(roundData.Indexes, ConverterConstants.Shooting1, data),
                        ExtraShots = this.GetInt(roundData.Indexes, ConverterConstants.ExtraShots, data),
                    });
                }

                if (roundData.Indexes.ContainsKey(ConverterConstants.Shooting2))
                {
                    athlete.Shootings.Add(new Shooting
                    {
                        Number = 2,
                        Misses = this.GetInt(roundData.Indexes, ConverterConstants.Shooting2Misses, data),
                        Time = this.GetTime(roundData.Indexes, ConverterConstants.Shooting2, data),
                        ExtraShots = this.GetInt(roundData.Indexes, ConverterConstants.ExtraShots, data),
                    });
                }

                team.Athletes.Add(athlete);
            }
        }
    }

    private async Task SetAthletesAsync(Round<Biathlon> round, RoundDataModel roundData, Guid eventId)
    {
        foreach (var row in roundData.Rows.Skip(1))
        {
            var data = row.Elements("td").ToList();
            var athleteModel = this.OlympediaService.FindAthlete(data[roundData.Indexes[ConverterConstants.Name]].OuterHtml);

            if (athleteModel != null)
            {
                var participant = await this.ParticipationRepository.GetAsync(x => x.Code == athleteModel.Code && x.EventId == eventId);
                var noc = this.OlympediaService.FindNOCCode(data[roundData.Indexes[ConverterConstants.NOC]].OuterHtml);
                var athlete = new Biathlon
                {
                    Id = participant.Id,
                    Name = athleteModel.Name,
                    NOC = noc,
                    Code = athleteModel.Code,
                    FinishStatus = this.OlympediaService.FindFinishStatus(row.OuterHtml),
                    Number = this.GetInt(roundData.Indexes, ConverterConstants.Number, data),
                    Time = this.GetTime(roundData.Indexes, ConverterConstants.Time, data),
                    Skiing = this.GetTime(roundData.Indexes, ConverterConstants.Skiing, data),
                    Misses = this.GetInt(roundData.Indexes, ConverterConstants.Misses, data),
                    StartBehind = this.GetTime(roundData.Indexes, ConverterConstants.StartBehind, data),
                    Penalties = this.GetInt(roundData.Indexes, ConverterConstants.Penalties, data),
                };

                athlete.Time ??= this.GetTime(roundData.Indexes, ConverterConstants.AdjustedTime, data);
                athlete.Skiing ??= this.GetTime(roundData.Indexes, ConverterConstants.Race, data);

                if (roundData.Indexes.ContainsKey(ConverterConstants.Shooting1))
                {
                    athlete.Shootings.Add(new Shooting
                    {
                        Number = 1,
                        Misses = this.GetInt(roundData.Indexes, ConverterConstants.Shooting1Misses, data),
                        Time = this.GetTime(roundData.Indexes, ConverterConstants.Shooting1, data),
                        Penalties = this.GetInt(roundData.Indexes, ConverterConstants.Shooting1Penalties, data),
                    });
                }

                if (roundData.Indexes.ContainsKey(ConverterConstants.Shooting2))
                {
                    athlete.Shootings.Add(new Shooting
                    {
                        Number = 2,
                        Misses = this.GetInt(roundData.Indexes, ConverterConstants.Shooting2Misses, data),
                        Time = this.GetTime(roundData.Indexes, ConverterConstants.Shooting2, data),
                        Penalties = this.GetInt(roundData.Indexes, ConverterConstants.Shooting2Penalties, data),
                    });
                }

                if (roundData.Indexes.ContainsKey(ConverterConstants.Shooting3))
                {
                    athlete.Shootings.Add(new Shooting
                    {
                        Number = 3,
                        Misses = this.GetInt(roundData.Indexes, ConverterConstants.Shooting3Misses, data),
                        Time = this.GetTime(roundData.Indexes, ConverterConstants.Shooting3, data),
                        Penalties = this.GetInt(roundData.Indexes, ConverterConstants.Shooting3Penalties, data),
                    });
                }

                if (roundData.Indexes.ContainsKey(ConverterConstants.Shooting4))
                {
                    athlete.Shootings.Add(new Shooting
                    {
                        Number = 4,
                        Misses = this.GetInt(roundData.Indexes, ConverterConstants.Shooting4Misses, data),
                        Time = this.GetTime(roundData.Indexes, ConverterConstants.Shooting4, data),
                        Penalties = this.GetInt(roundData.Indexes, ConverterConstants.Shooting4Penalties, data),
                    });
                }

                round.Athletes.Add(athlete);
            }
        }
    }

    private Track ExtractTrack(HtmlDocument htmlDocument)
    {
        var length = this.RegExpService.MatchFirstGroup(htmlDocument.DocumentNode.OuterHtml, @"Course Length:(.*?)<br>");
        var height = this.RegExpService.MatchFirstGroup(htmlDocument.DocumentNode.OuterHtml, @"Height Differential:(.*?)<br>");
        var climb = this.RegExpService.MatchFirstGroup(htmlDocument.DocumentNode.OuterHtml, @"Maximum Climb:(.*?)<br>");
        var shooting1 = this.RegExpService.MatchFirstGroup(htmlDocument.DocumentNode.OuterHtml, @"Shooting 1:(.*?)<br>");
        var shooting2 = this.RegExpService.MatchFirstGroup(htmlDocument.DocumentNode.OuterHtml, @"Shooting 2:(.*?)<br>");
        var shooting3 = this.RegExpService.MatchFirstGroup(htmlDocument.DocumentNode.OuterHtml, @"Shooting 3:(.*?)<br>");
        var shooting4 = this.RegExpService.MatchFirstGroup(htmlDocument.DocumentNode.OuterHtml, @"Shooting 4:(.*?)<br>");
        var totalClimbing = this.RegExpService.MatchFirstGroup(htmlDocument.DocumentNode.OuterHtml, @"Total Climbing:(.*?)<\/td>");

        var track = new Track
        {
            HeightDifference = this.RegExpService.MatchInt(height),
            Length = this.RegExpService.MatchInt(length),
            MaximumClimb = this.RegExpService.MatchInt(climb),
            TotalClimb = this.RegExpService.MatchInt(totalClimbing),
        };

        if (shooting1 != null)
        {
            track.ShootingInfos.Add(new ShootingInfo { Number = 1, Info = shooting1 });
        }

        if (shooting2 != null)
        {
            track.ShootingInfos.Add(new ShootingInfo { Number = 2, Info = shooting2 });
        }

        if (shooting3 != null)
        {
            track.ShootingInfos.Add(new ShootingInfo { Number = 3, Info = shooting3 });
        }

        if (shooting4 != null)
        {
            track.ShootingInfos.Add(new ShootingInfo { Number = 4, Info = shooting4 });
        }

        return track;
    }
}