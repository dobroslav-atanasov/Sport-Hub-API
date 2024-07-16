namespace SportHub.Converters.OlympicGames.SportConverters;

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

public class GymnasticsConverter : BaseSportConverter
{
    public GymnasticsConverter(IOlympediaService olympediaService, IDateService dateService, IDataCacheService dataCacheService, IRegExpService regExpService, IMapper mapper,
        INormalizeService normalizeService, OlympicGamesRepository<Team> teamRepository, OlympicGamesRepository<Participation> participationRepository,
        OlympicGamesRepository<Athlete> athleteRepository, OlympicGamesRepository<Result> resultRepository)
        : base(olympediaService, dateService, dataCacheService, regExpService, mapper, normalizeService, teamRepository, participationRepository, athleteRepository, resultRepository)
    {
    }

    public override async Task ProcessAsync(Options options)
    {
        switch (options.Discipline.Name)
        {
            case DisciplineConstants.ARTISTIC_GYMNASTICS:
                await this.ProcessArtisticGymnasticsAsync(options);
                break;
        }
    }

    private async Task ProcessArtisticGymnasticsAsync(Options options)
    {
        var rounds = new List<Round<ArtisticGymnastics>>();

        if (options.Event.IsTeamEvent)
        {
            await Console.Out.WriteLineAsync($"{options.Game.Year} - {options.Event.Name}");
            if (options.Game.Year <= 1996)
            {
                var firstRoundData = options.Rounds.FirstOrDefault();
                var round = this.CreateRound<ArtisticGymnastics>(firstRoundData, options.Event.Name, null);
                await this.SetArtisticGymnasticsTeamsAsync(round, firstRoundData, options.Event.Id, options.Game.Year, null);

                foreach (var roundData in options.Rounds.Skip(1))
                {
                    await this.SetArtisticGymnasticsTeamsAsync(round, roundData, options.Event.Id, options.Game.Year, roundData.Name);
                }

                rounds.Add(round);
            }
            else
            {
                foreach (var roundData in options.Rounds)
                {
                    if (options.Rounds.Count > 1 && roundData.Order == 1)
                    {
                        continue;
                    }

                    var round = this.CreateRound<ArtisticGymnastics>(roundData, options.Event.Name, null);
                    await this.SetArtisticGymnasticsTeamsAsync(round, roundData, options.Event.Id, options.Game.Year, null);
                    var documentNumbers = this.OlympediaService.FindResults(roundData.Html);

                    foreach (var number in documentNumbers)
                    {
                        var document = options.Documents.FirstOrDefault(x => x.Id == number);
                        var documentTitle = this.CleanDocumentTitle(document.Title);
                        var judges = await this.GetJudgesAsync(document.Html, documentTitle);
                        round.Judges.AddRange(judges);
                        await this.SetArtisticGymnasticsTeamsAsync(round, document.Rounds.FirstOrDefault(), options.Event.Id, options.Game.Year, documentTitle);
                    }

                    rounds.Add(round);
                }
            }
        }
        else
        {
            if (options.Game.Year <= 1932)
            {
                var firstRoundData = options.Rounds.FirstOrDefault();
                var round = this.CreateRound<ArtisticGymnastics>(firstRoundData, options.Event.Name, null);
                await this.SetArtisticGymnasticsAthletesAsync(round, firstRoundData, options.Event.Id, options.Game.Year, null);

                foreach (var roundData in options.Rounds.Skip(1))
                {
                    await this.SetArtisticGymnasticsAthletesAsync(round, roundData, options.Event.Id, options.Game.Year, roundData.Name);
                }

                rounds.Add(round);
            }
            else
            {
                foreach (var roundData in options.Rounds)
                {
                    if (options.Rounds.Count > 1 && roundData.Order == 1)
                    {
                        continue;
                    }

                    var round = this.CreateRound<ArtisticGymnastics>(roundData, options.Event.Name, null);
                    await this.SetArtisticGymnasticsAthletesAsync(round, roundData, options.Event.Id, options.Game.Year, null);
                    var documentNumbers = this.OlympediaService.FindResults(roundData.Html);

                    foreach (var number in documentNumbers)
                    {
                        var document = options.Documents.FirstOrDefault(x => x.Id == number);
                        var documentTitle = this.CleanDocumentTitle(document.Title);
                        var judges = await this.GetJudgesAsync(document.Html, documentTitle);
                        round.Judges.AddRange(judges);
                        await this.SetArtisticGymnasticsAthletesAsync(round, document.Rounds.FirstOrDefault(), options.Event.Id, options.Game.Year, documentTitle);
                    }

                    rounds.Add(round);
                }
            }
        }

        await this.ProcessJsonAsync(rounds, options);
    }

    private async Task SetArtisticGymnasticsAthletesAsync(Round<ArtisticGymnastics> round, RoundDataModel roundData, Guid eventId, int year, string info)
    {
        foreach (var row in roundData.Rows.Skip(1))
        {
            var data = row.Elements("td").ToList();
            var athleteModel = this.OlympediaService.FindAthlete(data[roundData.Indexes[ConverterConstants.Name]].OuterHtml);

            if (string.IsNullOrEmpty(info))
            {
                if (athleteModel != null)
                {
                    var participant = await this.ParticipationRepository.GetAsync(x => x.Code == athleteModel.Code && x.EventId == eventId);
                    var noc = this.OlympediaService.FindNOCCode(data[roundData.Indexes[ConverterConstants.NOC]].OuterHtml);
                    var athlete = new ArtisticGymnastics
                    {
                        Id = participant != null ? participant.Id : Guid.Empty,
                        Name = athleteModel.Name,
                        NOC = noc,
                        Code = athleteModel.Code,
                        FinishStatus = this.OlympediaService.FindFinishStatus(row.OuterHtml),
                        IsQualified = this.OlympediaService.IsQualified(row.OuterHtml),
                        Points = this.GetDouble(roundData.Indexes, ConverterConstants.Points, data),
                        CompulsoryPoints = this.GetDouble(roundData.Indexes, ConverterConstants.CompulsaryPoints, data),
                        Height = this.GetDouble(roundData.Indexes, ConverterConstants.Height, data),
                        OptionalPoints = this.GetDouble(roundData.Indexes, ConverterConstants.OptionalPoints, data),
                        Time = this.GetDouble(roundData.Indexes, ConverterConstants.ShotPut, data),
                        Vault1 = this.GetDouble(roundData.Indexes, ConverterConstants.Vault1, data),
                        Vault2 = this.GetDouble(roundData.Indexes, ConverterConstants.Vault2, data),
                        VaultOff1 = this.GetDouble(roundData.Indexes, ConverterConstants.VaultOff1, data),
                        VaultOff2 = this.GetDouble(roundData.Indexes, ConverterConstants.VaultOff2, data),
                        FirstTimeTrial = this.GetDouble(roundData.Indexes, ConverterConstants.FirstTimeTrial, data),
                        SecondTimeTrial = this.GetDouble(roundData.Indexes, ConverterConstants.SecondTimeTrial, data),
                        ThirdTimeTrial = this.GetDouble(roundData.Indexes, ConverterConstants.ThirdTimeTrial, data),
                    };

                    if (year == 2008)
                    {
                        this.SetArtisticGymnasticsScores(athlete, roundData, data);
                    }

                    round.Athletes.Add(athlete);
                }
            }
            else
            {
                var athlete = round.Athletes.FirstOrDefault(x => x.Code == athleteModel.Code);
                athlete?.Scores.Add(new ArtisticGymnasticsScore
                {
                    Points = this.GetDouble(roundData.Indexes, ConverterConstants.Points, data),
                    CompulsoryPoints = this.GetDouble(roundData.Indexes, ConverterConstants.CompulsaryPoints, data),
                    OptionalPoints = this.GetDouble(roundData.Indexes, ConverterConstants.OptionalPoints, data),
                    Time = this.GetDouble(roundData.Indexes, ConverterConstants.Time, data),
                    Vault1 = this.GetDouble(roundData.Indexes, ConverterConstants.Vault1, data),
                    Vault2 = this.GetDouble(roundData.Indexes, ConverterConstants.Vault2, data),
                    Distance = this.GetDouble(roundData.Indexes, ConverterConstants.Distance, data),
                    DScore = this.GetDouble(roundData.Indexes, ConverterConstants.Dscore, data),
                    EScore = this.GetDouble(roundData.Indexes, ConverterConstants.Escore, data),
                    FinalPoints = this.GetDouble(roundData.Indexes, ConverterConstants.Final, data),
                    LinePenalty = this.GetDouble(roundData.Indexes, ConverterConstants.LinePenalty, data),
                    OtherPenalty = this.GetDouble(roundData.Indexes, ConverterConstants.OtherPenalty, data),
                    Penalty = this.GetDouble(roundData.Indexes, ConverterConstants.Penalty, data),
                    QualificationHalfPoints = this.GetDouble(roundData.Indexes, ConverterConstants.HalfQualificationPoints, data),
                    QualificationPoints = this.GetDouble(roundData.Indexes, ConverterConstants.Qualification, data),
                    TimePenalty = this.GetDouble(roundData.Indexes, ConverterConstants.TimePenalty, data),
                    Name = info
                });
            }
        }
    }

    private void SetArtisticGymnasticsScores(ArtisticGymnastics artisticGymnastics, RoundDataModel roundData, List<HtmlNode> data)
    {
        var floorExercise = this.GetDouble(roundData.Indexes, ConverterConstants.FloorExercise, data);
        if (floorExercise != null)
        {
            artisticGymnastics.Scores.Add(new ArtisticGymnasticsScore { Points = floorExercise, Name = "Floor Exercise" });
        }

        var horseVault = this.GetDouble(roundData.Indexes, ConverterConstants.HorseVault, data);
        if (horseVault != null)
        {
            artisticGymnastics.Scores.Add(new ArtisticGymnasticsScore { Points = horseVault, Name = "Horse Vault" });
        }

        var parallelBars = this.GetDouble(roundData.Indexes, ConverterConstants.ParallelBars, data);
        if (parallelBars != null)
        {
            artisticGymnastics.Scores.Add(new ArtisticGymnasticsScore { Points = parallelBars, Name = "Parallel Bars" });
        }

        var horizontalBar = this.GetDouble(roundData.Indexes, ConverterConstants.HorizontalBar, data);
        if (horizontalBar != null)
        {
            artisticGymnastics.Scores.Add(new ArtisticGymnasticsScore { Points = horizontalBar, Name = "Horizontal Bar" });
        }

        var rings = this.GetDouble(roundData.Indexes, ConverterConstants.Rings, data);
        if (rings != null)
        {
            artisticGymnastics.Scores.Add(new ArtisticGymnasticsScore { Points = rings, Name = "Rings" });
        }

        var pommellHorse = this.GetDouble(roundData.Indexes, ConverterConstants.PommellHorse, data);
        if (pommellHorse != null)
        {
            artisticGymnastics.Scores.Add(new ArtisticGymnasticsScore { Points = pommellHorse, Name = "Pommell Horse" });
        }

        var unevenBars = this.GetDouble(roundData.Indexes, ConverterConstants.UnevenBars, data);
        if (unevenBars != null)
        {
            artisticGymnastics.Scores.Add(new ArtisticGymnasticsScore { Points = unevenBars, Name = "Uneven Bars" });
        }

        var balanceBeam = this.GetDouble(roundData.Indexes, ConverterConstants.BalanceBeam, data);
        if (balanceBeam != null)
        {
            artisticGymnastics.Scores.Add(new ArtisticGymnasticsScore { Points = balanceBeam, Name = "Balance beam" });
        }
    }

    private async Task SetArtisticGymnasticsTeamsAsync(Round<ArtisticGymnastics> round, RoundDataModel roundData, Guid eventId, int year, string info)
    {
        ArtisticGymnastics team = null;
        string lastNOC = null;
        foreach (var row in roundData.Rows.Skip(1))
        {
            var noc = this.OlympediaService.FindNOCCode(row.OuterHtml);
            var data = row.Elements("td").ToList();
            if (noc != null)
            {
                lastNOC = noc;
                if (string.IsNullOrEmpty(info))
                {
                    var teamName = data[roundData.Indexes[ConverterConstants.Name]].InnerText;
                    var nocCache = this.DataCacheService.NationalOlympicCommittees.FirstOrDefault(x => x.Code == noc);
                    var dbTeam = await this.TeamRepository.GetAsync(x => x.Name == teamName && x.NationalOlympicCommitteeId == nocCache.Id && x.EventId == eventId);
                    dbTeam ??= await this.TeamRepository.GetAsync(x => x.NationalOlympicCommitteeId == nocCache.Id && x.EventId == eventId);

                    team = new ArtisticGymnastics
                    {
                        Id = dbTeam.Id,
                        Name = dbTeam.Name,
                        NOC = noc,
                        FinishStatus = this.OlympediaService.FindFinishStatus(row.OuterHtml),
                        IsQualified = this.OlympediaService.IsQualified(row.OuterHtml),
                        ApparatusPoints = this.GetDouble(roundData.Indexes, ConverterConstants.ApparatusPoints, data),
                        CompulsoryPoints = this.GetDouble(roundData.Indexes, ConverterConstants.CompulsaryPoints, data),
                        DrillPoints = this.GetDouble(roundData.Indexes, ConverterConstants.DrillPoints, data),
                        GroupExercisePoints = this.GetDouble(roundData.Indexes, ConverterConstants.GroupExercise, data),
                        Vault1 = this.GetDouble(roundData.Indexes, ConverterConstants.HorseVault, data),
                        HalfTeamPoints = this.GetDouble(roundData.Indexes, ConverterConstants.HalfTeamPoints, data),
                        OptionalPoints = this.GetDouble(roundData.Indexes, ConverterConstants.OptionalPoints, data),
                        LongJumpPoints = this.GetDouble(roundData.Indexes, ConverterConstants.LongJump, data),
                        PrecisionPoins = this.GetDouble(roundData.Indexes, ConverterConstants.PrecisionPoints, data),
                        Points = this.GetDouble(roundData.Indexes, ConverterConstants.Points, data),
                        ShotPutPoints = this.GetDouble(roundData.Indexes, ConverterConstants.ShotPut, data),
                        Yards100Points = this.GetDouble(roundData.Indexes, ConverterConstants.Y100, data),
                    };

                    team.Points ??= this.GetDouble(roundData.Indexes, ConverterConstants.TeamPoints, data);

                    this.SetArtisticGymnasticsScores(team, roundData, data);

                    round.Teams.Add(team);
                }
                else
                {
                    if (year < 2012)
                    {
                        var currentTeam = round.Teams.FirstOrDefault(x => x.NOC == noc);
                        currentTeam?.Scores.Add(new ArtisticGymnasticsScore
                        {
                            ApparatusPoints = this.GetDouble(roundData.Indexes, ConverterConstants.ApparatusPoints, data),
                            CompulsoryPoints = this.GetDouble(roundData.Indexes, ConverterConstants.CompulsaryPoints, data),
                            DrillPoints = this.GetDouble(roundData.Indexes, ConverterConstants.DrillPoints, data),
                            GroupExercisePoints = this.GetDouble(roundData.Indexes, ConverterConstants.GroupExercise, data),
                            Vault1 = this.GetDouble(roundData.Indexes, ConverterConstants.HorseVault, data),
                            HalfTeamPoints = this.GetDouble(roundData.Indexes, ConverterConstants.HalfTeamPoints, data),
                            OptionalPoints = this.GetDouble(roundData.Indexes, ConverterConstants.OptionalPoints, data),
                            LongJumpPoints = this.GetDouble(roundData.Indexes, ConverterConstants.LongJump, data),
                            PrecisionPoins = this.GetDouble(roundData.Indexes, ConverterConstants.PrecisionPoints, data),
                            Points = this.GetDouble(roundData.Indexes, ConverterConstants.Points, data),
                            ShotPutPoints = this.GetDouble(roundData.Indexes, ConverterConstants.ShotPut, data),
                            Yards100Points = this.GetDouble(roundData.Indexes, ConverterConstants.Y100, data),
                            Name = info
                        });
                    }
                }
            }
            else
            {
                var athleteModels = this.OlympediaService.FindAthletes(row.OuterHtml);
                if (athleteModels.Count > 0)
                {
                    foreach (var athleteModel in athleteModels)
                    {
                        var participant = await this.ParticipationRepository.GetAsync(x => x.Code == athleteModel.Code && x.EventId == eventId);

                        if (string.IsNullOrEmpty(info))
                        {
                            var athlete = new ArtisticGymnastics
                            {
                                Id = participant != null ? participant.Id : Guid.Empty,
                                Code = athleteModel.Code,
                                Name = athleteModel.Name,
                                NOC = team.NOC,
                                FinishStatus = this.OlympediaService.FindFinishStatus(row.OuterHtml),
                                IsQualified = this.OlympediaService.IsQualified(row.OuterHtml),
                            };

                            if (athleteModels.Count == 1)
                            {
                                athlete.IndividualPoints = this.GetDouble(roundData.Indexes, ConverterConstants.IndividualPoints, data);
                                athlete.ApparatusPoints = this.GetDouble(roundData.Indexes, ConverterConstants.ApparatusPoints, data);
                                athlete.CompulsoryPoints = this.GetDouble(roundData.Indexes, ConverterConstants.CompulsaryPoints, data);
                                athlete.DrillPoints = this.GetDouble(roundData.Indexes, ConverterConstants.DrillPoints, data);
                                athlete.GroupExercisePoints = this.GetDouble(roundData.Indexes, ConverterConstants.GroupExercise, data);
                                athlete.Vault1 = this.GetDouble(roundData.Indexes, ConverterConstants.HorseVault, data);
                                athlete.OptionalPoints = this.GetDouble(roundData.Indexes, ConverterConstants.OptionalPoints, data);
                                athlete.LongJumpPoints = this.GetDouble(roundData.Indexes, ConverterConstants.LongJump, data);
                                athlete.PrecisionPoins = this.GetDouble(roundData.Indexes, ConverterConstants.PrecisionPoints, data);
                                athlete.Points = this.GetDouble(roundData.Indexes, ConverterConstants.Points, data);
                                athlete.ShotPutPoints = this.GetDouble(roundData.Indexes, ConverterConstants.ShotPut, data);
                                athlete.Yards100Points = this.GetDouble(roundData.Indexes, ConverterConstants.Y100, data);
                            }

                            if (year == 2008)
                            {
                                this.SetArtisticGymnasticsScores(athlete, roundData, data);
                            }

                            team.Athletes.Add(athlete);
                        }
                        else
                        {
                            var currentTeam = round.Teams.FirstOrDefault(x => x.NOC == lastNOC);
                            var athlete = currentTeam.Athletes.FirstOrDefault(x => x.Code == athleteModel.Code);
                            athlete?.Scores.Add(new ArtisticGymnasticsScore
                            {
                                Points = this.GetDouble(roundData.Indexes, ConverterConstants.Points, data),
                                Time = this.GetDouble(roundData.Indexes, ConverterConstants.ShotPut, data),
                                DScore = this.GetDouble(roundData.Indexes, ConverterConstants.Dscore, data),
                                EScore = this.GetDouble(roundData.Indexes, ConverterConstants.Escore, data),
                                LinePenalty = this.GetDouble(roundData.Indexes, ConverterConstants.LinePenalty, data),
                                OtherPenalty = this.GetDouble(roundData.Indexes, ConverterConstants.OtherPenalty, data),
                                Penalty = this.GetDouble(roundData.Indexes, ConverterConstants.Penalty, data),
                                Name = info
                            });
                        }
                    }
                }
            }
        }
    }

    private string CleanDocumentTitle(string title)
    {
        title = title.Replace("Final, ", string.Empty)
            .Replace("Qualification, ", string.Empty)
            .Replace("Qualifying, ", string.Empty)
            .Replace("Horse Vault", "Vault");

        return title;
    }
}