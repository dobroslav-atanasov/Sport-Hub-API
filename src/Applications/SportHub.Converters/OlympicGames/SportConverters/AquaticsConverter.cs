namespace SportHub.Converters.OlympicGames.SportConverters;

using System;

using AutoMapper;

using SportHub.Common.Constants;
using SportHub.Data.Models.Converters.OlympicGames;
using SportHub.Data.Models.Converters.OlympicGames.Base;
using SportHub.Data.Models.Converters.OlympicGames.Disciplines;
using SportHub.Data.Models.DbEntities.OlympicGames;
using SportHub.Data.Repositories;
using SportHub.Services.Data.OlympicGamesDb.Interfaces;
using SportHub.Services.Interfaces;

public class AquaticsConverter : BaseSportConverter
{
    public AquaticsConverter(IOlympediaService olympediaService, IDateService dateService, IDataCacheService dataCacheService, IRegExpService regExpService, IMapper mapper,
        INormalizeService normalizeService, OlympicGamesRepository<Team> teamRepository, OlympicGamesRepository<Participation> participationRepository,
        OlympicGamesRepository<Athlete> athleteRepository, OlympicGamesRepository<Result> resultRepository)
        : base(olympediaService, dateService, dataCacheService, regExpService, mapper, normalizeService, teamRepository, participationRepository, athleteRepository, resultRepository)
    {
    }

    public override async Task ProcessAsync(Options options)
    {
        switch (options.Discipline.Name)
        {
            case DisciplineConstants.ARTISTIC_SWIMMING:
                await this.ProcessArtisticSwimmingAsync(options);
                break;
            case DisciplineConstants.DIVING:
                await this.ProcessDivingAsync(options);
                break;
        }
    }

    private async Task ProcessDivingAsync(Options options)
    {
        var rounds = new List<Round<Diving>>();
        await Console.Out.WriteLineAsync($"{options.Game.Year} - {options.Event.Name}");

        var allRounds = options.Rounds;
        if (options.Rounds.Count != 1)
        {
            allRounds = options.Rounds.Skip(1).ToList();
        }

        foreach (var roundData in allRounds)
        {
            var round = this.CreateRound<Diving>(roundData, options.Event.Name, null);
            var judges = await this.GetJudgesAsync(allRounds.Count == 1 ? options.HtmlDocument.ParsedText : roundData.Html);
            round.Judges = judges;
            await this.SetDivingAthletesAsync(round, roundData, options, null);
            rounds.Add(round);

            var documentNumbers = this.OlympediaService.FindResults(allRounds.Count == 1 ? options.HtmlDocument.ParsedText : roundData.Html);
            foreach (var number in documentNumbers)
            {
                var document = options.Documents.FirstOrDefault(x => x.Id == number && !x.Title.Contains("Summary", StringComparison.CurrentCultureIgnoreCase));
                if (document != null)
                {
                    await this.SetDivingAthletesAsync(round, document.Rounds.FirstOrDefault(), options, document.Title);
                }
            }
        }

        await this.ProcessJsonAsync(rounds, options);
    }

    private async Task SetDivingAthletesAsync(Round<Diving> round, RoundDataModel roundData, Options options, string info)
    {
        foreach (var row in roundData.Rows.Skip(1))
        {
            var noc = this.OlympediaService.FindNOCCode(row.OuterHtml);
            var data = row.Elements("td").ToList();

            Diving diving = null;
            if (options.Event.IsTeamEvent)
            {
                if (string.IsNullOrEmpty(info))
                {
                    var athleteModels = this.OlympediaService.FindAthletes(data[roundData.Indexes[ConverterConstants.Name]].OuterHtml);

                    if (noc != null)
                    {
                        var nocCache = this.DataCacheService.NOCs.FirstOrDefault(x => x.Code == noc);
                        var dbTeam = await this.TeamRepository.GetAsync(x => x.NOCId == nocCache.Id && x.EventId == options.Event.Id);

                        diving = new Diving
                        {
                            Id = dbTeam.Id,
                            Name = dbTeam.Name,
                            NOC = noc,
                        };

                        round.Teams.Add(diving);

                        if (athleteModels.Count != 0)
                        {
                            foreach (var athleteModel in athleteModels)
                            {
                                var participant = await this.ParticipationRepository.GetAsync(x => x.Code == athleteModel.Code && x.EventId == options.Event.Id);
                                diving.Athletes.Add(new Diving
                                {
                                    Id = participant.Id,
                                    Name = athleteModel.Name,
                                    NOC = noc,
                                    Code = athleteModel.Code,
                                });
                            }
                        }
                    }
                    else
                    {
                        foreach (var athleteModel in athleteModels)
                        {
                            var participant = await this.ParticipationRepository.GetAsync(x => x.Code == athleteModel.Code && x.EventId == options.Event.Id);
                            var team = round.Teams.Last();
                            team.Athletes.Add(new Diving
                            {
                                Id = participant.Id,
                                Name = athleteModel.Name,
                                NOC = noc,
                                Code = athleteModel.Code,
                            });
                        }
                    }
                }
                else
                {
                    if (noc == null)
                    {
                        continue;
                    }

                    diving = round.Teams.FirstOrDefault(x => x.NOC == noc);
                }
            }
            else
            {
                var athleteModel = this.OlympediaService.FindAthlete(data[roundData.Indexes[ConverterConstants.Name]].OuterHtml);

                if (string.IsNullOrEmpty(info))
                {
                    if (athleteModel != null)
                    {
                        var participant = await this.ParticipationRepository.GetAsync(x => x.Code == athleteModel.Code && x.EventId == options.Event.Id);

                        diving = new Diving
                        {
                            Id = participant.Id,
                            Name = athleteModel.Name,
                            Code = athleteModel.Code,
                        };

                        round.Athletes.Add(diving);
                    }
                }
                else
                {
                    diving = round.Athletes.FirstOrDefault(x => x.Code == athleteModel.Code);
                }
            }

            if (diving != null && string.IsNullOrEmpty(info))
            {
                diving.NOC = noc;
                diving.FinishStatus = this.OlympediaService.FindFinishStatus(row.OuterHtml);
                diving.IsQualified = this.OlympediaService.IsQualified(row.OuterHtml);
                diving.Order = this.GetInt(roundData.Indexes, ConverterConstants.Order, data);
                diving.Points = this.GetDouble(roundData.Indexes, ConverterConstants.Points, data);
                diving.CompulsoryPoints = this.GetDouble(roundData.Indexes, ConverterConstants.CompulsaryPoints, data);
                diving.Ordinals = this.GetDouble(roundData.Indexes, ConverterConstants.Ordinals, data);
                diving.FinalPoints = this.GetDouble(roundData.Indexes, ConverterConstants.Final, data);
                diving.QualificationPoints = this.GetDouble(roundData.Indexes, ConverterConstants.Qualification, data);
                diving.SemiFinalsPoints = this.GetDouble(roundData.Indexes, ConverterConstants.Semifinals, data);

                //if (roundData.Indexes.ContainsKey(ConverterConstants.Dive1))
                //{
                //    diving.Dives.Add(new Dive { Number = 1, Points = this.GetDouble(roundData.Indexes, ConverterConstants.Dive1, data) });
                //}

                //if (roundData.Indexes.ContainsKey(ConverterConstants.Dive2))
                //{
                //    diving.Dives.Add(new Dive { Number = 2, Points = this.GetDouble(roundData.Indexes, ConverterConstants.Dive2, data) });
                //}

                //if (roundData.Indexes.ContainsKey(ConverterConstants.Dive3))
                //{
                //    diving.Dives.Add(new Dive { Number = 3, Points = this.GetDouble(roundData.Indexes, ConverterConstants.Dive3, data) });
                //}

                //if (roundData.Indexes.ContainsKey(ConverterConstants.Dive4))
                //{
                //    diving.Dives.Add(new Dive { Number = 4, Points = this.GetDouble(roundData.Indexes, ConverterConstants.Dive4, data) });
                //}

                //if (roundData.Indexes.ContainsKey(ConverterConstants.Dive5))
                //{
                //    diving.Dives.Add(new Dive { Number = 5, Points = this.GetDouble(roundData.Indexes, ConverterConstants.Dive5, data) });
                //}

                //if (roundData.Indexes.ContainsKey(ConverterConstants.Dive6))
                //{
                //    diving.Dives.Add(new Dive { Number = 6, Points = this.GetDouble(roundData.Indexes, ConverterConstants.Dive6, data) });
                //}
            }

            if (!string.IsNullOrEmpty(info))
            {
                var match = this.RegExpService.Match(info, @"Dive\s*#(\d+)");
                if (match != null)
                {
                    diving.Dives.Add(new Dive
                    {
                        Number = int.Parse(match.Groups[1].Value),
                        Points = this.GetDouble(roundData.Indexes, ConverterConstants.Points, data),
                        Ordinals = this.GetDouble(roundData.Indexes, ConverterConstants.Ordinals, data),
                        Difficulty = this.GetDouble(roundData.Indexes, ConverterConstants.Difficulty, data),
                        Name = this.GetString(roundData.Indexes, ConverterConstants.Dive, data),
                        ExecutionJudge1Score = this.GetDouble(roundData.Indexes, ConverterConstants.ExecutionJudge1, data),
                        ExecutionJudge2Score = this.GetDouble(roundData.Indexes, ConverterConstants.ExecutionJudge2, data),
                        ExecutionJudge3Score = this.GetDouble(roundData.Indexes, ConverterConstants.ExecutionJudge3, data),
                        ExecutionJudge4Score = this.GetDouble(roundData.Indexes, ConverterConstants.ExecutionJudge4, data),
                        ExecutionJudge5Score = this.GetDouble(roundData.Indexes, ConverterConstants.ExecutionJudge5, data),
                        ExecutionJudge6Score = this.GetDouble(roundData.Indexes, ConverterConstants.ExecutionJudge6, data),
                        ExecutionJudge7Score = this.GetDouble(roundData.Indexes, ConverterConstants.ExecutionJudge7, data),
                        SynchronizationJudge1Score = this.GetDouble(roundData.Indexes, ConverterConstants.Synchronization1, data),
                        SynchronizationJudge2Score = this.GetDouble(roundData.Indexes, ConverterConstants.Synchronization2, data),
                        SynchronizationJudge3Score = this.GetDouble(roundData.Indexes, ConverterConstants.Synchronization3, data),
                        SynchronizationJudge4Score = this.GetDouble(roundData.Indexes, ConverterConstants.Synchronization4, data),
                        SynchronizationJudge5Score = this.GetDouble(roundData.Indexes, ConverterConstants.Synchronization5, data),
                    });
                }
            }
        }
    }

    private async Task ProcessArtisticSwimmingAsync(Options options)
    {
        var rounds = new List<Round<ArtisticSwimming>>();

        foreach (var roundData in options.Rounds)
        {
            if (options.Rounds.Count > 1 && roundData.Order == 1)
            {
                continue;
            }

            var round = this.CreateRound<ArtisticSwimming>(roundData, options.Event.Name, null);
            await this.SetArtisticSwimmingTeamsAsync(round, roundData, options.Event.Id, options.Event.IsTeamEvent);

            if (options.Rounds.Count == 1)
            {
                foreach (var document in options.Documents)
                {
                    var subRoundType = this.NormalizeService.MapRoundData(document.Title);
                    roundData.SubType = subRoundType.Type;
                    var currentRound = this.CreateRound<ArtisticSwimming>(roundData, options.Event.Name, null);
                    var judges = await this.GetJudgesAsync(document.Html, document.Title);
                    currentRound.Judges.AddRange(judges);
                    await this.SetArtisticSwimmingTeamsAsync(currentRound, document.Rounds.FirstOrDefault(), options.Event.Id, options.Event.IsTeamEvent);
                    rounds.Add(currentRound);
                }
            }
            else
            {
                var documentNumbers = this.OlympediaService.FindResults(roundData.Html);
                foreach (var number in documentNumbers)
                {
                    var document = options.Documents.FirstOrDefault(x => x.Id == number);
                    var subRoundType = this.NormalizeService.MapRoundData(document.Title);
                    roundData.SubType = subRoundType.Type;
                    var currentRound = this.CreateRound<ArtisticSwimming>(roundData, options.Event.Name, null);
                    var judges = await this.GetJudgesAsync(document.Html, document.Title);
                    currentRound.Judges.AddRange(judges);
                    await this.SetArtisticSwimmingTeamsAsync(currentRound, document.Rounds.FirstOrDefault(), options.Event.Id, options.Event.IsTeamEvent);
                    rounds.Add(currentRound);
                }
            }

            rounds.Add(round);
        }

        await this.ProcessJsonAsync(rounds, options);
    }

    private async Task SetArtisticSwimmingTeamsAsync(Round<ArtisticSwimming> round, RoundDataModel roundData, Guid eventId, bool isTeamEvent)
    {
        ArtisticSwimming team = null;

        foreach (var row in roundData.Rows.Skip(1))
        {
            var noc = this.OlympediaService.FindNOCCode(row.OuterHtml);
            var data = row.Elements("td").ToList();

            if (isTeamEvent)
            {
                var athleteModels = this.OlympediaService.FindAthletes(row.OuterHtml);

                if (noc != null)
                {
                    var teamName = data[roundData.Indexes[ConverterConstants.Name]].InnerText;
                    var nocCache = this.DataCacheService.NOCs.FirstOrDefault(x => x.Code == noc);
                    var dbTeam = await this.TeamRepository.GetAsync(x => x.Name == teamName && x.NOCId == nocCache.Id && x.EventId == eventId);
                    dbTeam ??= await this.TeamRepository.GetAsync(x => x.NOCId == nocCache.Id && x.EventId == eventId);

                    if (dbTeam != null)
                    {
                        team = new ArtisticSwimming
                        {
                            Id = dbTeam.Id,
                            Name = dbTeam.Name,
                            NOC = noc,
                            FinishStatus = this.OlympediaService.FindFinishStatus(row.OuterHtml),
                            IsQualified = this.OlympediaService.IsQualified(row.OuterHtml),
                            Points = this.GetDouble(roundData.Indexes, ConverterConstants.Points, data),
                            FigurePoints = this.GetDouble(roundData.Indexes, ConverterConstants.Figures, data),
                            MusicalRoutinePoints = this.GetDouble(roundData.Indexes, ConverterConstants.MusicalRoutinePoints, data),
                            FreeRoutinePoints = this.GetDouble(roundData.Indexes, ConverterConstants.FreeRoutine, data),
                            TechnicalRoutinePoints = this.GetDouble(roundData.Indexes, ConverterConstants.TechnicalRoutine, data),
                            ArtisticImpression = this.GetDouble(roundData.Indexes, ConverterConstants.ArtisticImpression, data),
                            ArtisticImpressionChoreography = this.GetDouble(roundData.Indexes, ConverterConstants.ArtisticImpressionChoreographyPoints, data),
                            ArtisticImpressionJudge1 = this.GetDouble(roundData.Indexes, ConverterConstants.ArtisticImpressionJudge1Points, data),
                            ArtisticImpressionJudge2 = this.GetDouble(roundData.Indexes, ConverterConstants.ArtisticImpressionJudge2Points, data),
                            ArtisticImpressionJudge3 = this.GetDouble(roundData.Indexes, ConverterConstants.ArtisticImpressionJudge3Points, data),
                            ArtisticImpressionJudge4 = this.GetDouble(roundData.Indexes, ConverterConstants.ArtisticImpressionJudge4Points, data),
                            ArtisticImpressionJudge5 = this.GetDouble(roundData.Indexes, ConverterConstants.ArtisticImpressionJudge5Points, data),
                            ArtisticImpressionMusicInterpretation = this.GetDouble(roundData.Indexes, ConverterConstants.ArtisticImpressionMusicInterpretationPoints, data),
                            ArtisticImpressionMannerOfPresentation = this.GetDouble(roundData.Indexes, ConverterConstants.ArtisticImpressionMannerofPresentationPoints, data),
                            Difficulty = this.GetDouble(roundData.Indexes, ConverterConstants.Difficulty, data),
                            DifficultyJudge1 = this.GetDouble(roundData.Indexes, ConverterConstants.DifficultyJudge1, data),
                            DifficultyJudge2 = this.GetDouble(roundData.Indexes, ConverterConstants.DifficultyJudge2, data),
                            DifficultyJudge3 = this.GetDouble(roundData.Indexes, ConverterConstants.DifficultyJudge3, data),
                            DifficultyJudge4 = this.GetDouble(roundData.Indexes, ConverterConstants.DifficultyJudge4, data),
                            DifficultyJudge5 = this.GetDouble(roundData.Indexes, ConverterConstants.DifficultyJudge5, data),
                            Execution = this.GetDouble(roundData.Indexes, ConverterConstants.DifficultyJudge5, data),
                            ExecutionJudge1 = this.GetDouble(roundData.Indexes, ConverterConstants.ExecutionJudge1, data),
                            ExecutionJudge2 = this.GetDouble(roundData.Indexes, ConverterConstants.ExecutionJudge2, data),
                            ExecutionJudge3 = this.GetDouble(roundData.Indexes, ConverterConstants.ExecutionJudge3, data),
                            ExecutionJudge4 = this.GetDouble(roundData.Indexes, ConverterConstants.ExecutionJudge4, data),
                            ExecutionJudge5 = this.GetDouble(roundData.Indexes, ConverterConstants.ExecutionJudge5, data),
                            ExecutionJudge6 = this.GetDouble(roundData.Indexes, ConverterConstants.ExecutionJudge6, data),
                            ExecutionJudge7 = this.GetDouble(roundData.Indexes, ConverterConstants.ExecutionJudge7, data),
                            OverallImpression = this.GetDouble(roundData.Indexes, ConverterConstants.OverallImpression, data),
                            OverallImpressionJudge1 = this.GetDouble(roundData.Indexes, ConverterConstants.OverallImpressionJudge1, data),
                            OverallImpressionJudge2 = this.GetDouble(roundData.Indexes, ConverterConstants.OverallImpressionJudge2, data),
                            OverallImpressionJudge3 = this.GetDouble(roundData.Indexes, ConverterConstants.OverallImpressionJudge3, data),
                            OverallImpressionJudge4 = this.GetDouble(roundData.Indexes, ConverterConstants.OverallImpressionJudge4, data),
                            OverallImpressionJudge5 = this.GetDouble(roundData.Indexes, ConverterConstants.OverallImpressionJudge5, data),
                            OverallImpressionJudge6 = this.GetDouble(roundData.Indexes, ConverterConstants.OverallImpressionJudge6, data),
                            OverallImpressionJudge7 = this.GetDouble(roundData.Indexes, ConverterConstants.OverallImpressionJudge7, data),
                            Penalties = this.GetDouble(roundData.Indexes, ConverterConstants.Penalties, data),
                            RequiredElementPenalty = this.GetDouble(roundData.Indexes, ConverterConstants.RequiredElementPenalty, data),
                            Routine1Points = this.GetDouble(roundData.Indexes, ConverterConstants.Routine1, data),
                            Routine2Points = this.GetDouble(roundData.Indexes, ConverterConstants.Routine2, data),
                            Routine3Points = this.GetDouble(roundData.Indexes, ConverterConstants.Routine3, data),
                            Routine4Points = this.GetDouble(roundData.Indexes, ConverterConstants.Routine4, data),
                            Routine5Points = this.GetDouble(roundData.Indexes, ConverterConstants.Routine5, data),
                            Routine1DegreeOfDifficulty = this.GetDouble(roundData.Indexes, ConverterConstants.Routine1DegreeOfDifficulty, data),
                            Routine2DegreeOfDifficulty = this.GetDouble(roundData.Indexes, ConverterConstants.Routine2DegreeOfDifficulty, data),
                            Routine3DegreeOfDifficulty = this.GetDouble(roundData.Indexes, ConverterConstants.Routine3DegreeOfDifficulty, data),
                            Routine4DegreeOfDifficulty = this.GetDouble(roundData.Indexes, ConverterConstants.Routine4DegreeOfDifficulty, data),
                            Routine5DegreeOfDifficulty = this.GetDouble(roundData.Indexes, ConverterConstants.Routine5DegreeOfDifficulty, data),
                            ReducedPoints = this.GetDouble(roundData.Indexes, ConverterConstants.ReducedPoints, data),
                            TechnicalMerit = this.GetDouble(roundData.Indexes, ConverterConstants.TechnicalMerit, data),
                            TechnicalMeritDifficulty = this.GetDouble(roundData.Indexes, ConverterConstants.TechnicalMeritDifficultyPoints, data),
                            TechnicalMeritExecution = this.GetDouble(roundData.Indexes, ConverterConstants.TechnicalMeritExecutionPoints, data),
                            TechnicalMeritSynchronization = this.GetDouble(roundData.Indexes, ConverterConstants.TechnicalMeritSynchronizationPoints, data),
                        };

                        team.Execution ??= this.GetDouble(roundData.Indexes, ConverterConstants.ExecutionPoints, data);

                        if (athleteModels.Count != 0)
                        {
                            foreach (var athleteModel in athleteModels)
                            {
                                var participant = await this.ParticipationRepository.GetAsync(x => x.Code == athleteModel.Code && x.EventId == eventId);
                                var athlete = new ArtisticSwimming
                                {
                                    Id = participant != null ? participant.Id : Guid.Empty,
                                    Code = athleteModel.Code,
                                    Name = athleteModel.Name,
                                    NOC = noc,
                                    FinishStatus = this.OlympediaService.FindFinishStatus(row.OuterHtml),
                                    IsQualified = this.OlympediaService.IsQualified(row.OuterHtml),
                                };

                                team.Athletes.Add(athlete);
                            }
                        }

                        round.Teams.Add(team);
                    }
                }
                else
                {
                    foreach (var athleteModel in athleteModels)
                    {
                        var participant = await this.ParticipationRepository.GetAsync(x => x.Code == athleteModel.Code && x.EventId == eventId);
                        var athlete = new ArtisticSwimming
                        {
                            Id = participant != null ? participant.Id : Guid.Empty,
                            Code = athleteModel.Code,
                            Name = athleteModel.Name,
                            NOC = noc,
                            FinishStatus = this.OlympediaService.FindFinishStatus(row.OuterHtml),
                            IsQualified = this.OlympediaService.IsQualified(row.OuterHtml),
                        };

                        team.Athletes.Add(athlete);
                    }
                }
            }
            else
            {
                var athleteModel = this.OlympediaService.FindAthlete(row.OuterHtml);
                var participant = await this.ParticipationRepository.GetAsync(x => x.Code == athleteModel.Code && x.EventId == eventId);
                var athlete = new ArtisticSwimming
                {
                    Id = participant != null ? participant.Id : Guid.Empty,
                    Code = athleteModel.Code,
                    Name = athleteModel.Name,
                    NOC = noc,
                    FinishStatus = this.OlympediaService.FindFinishStatus(row.OuterHtml),
                    IsQualified = this.OlympediaService.IsQualified(row.OuterHtml),
                    Points = this.GetDouble(roundData.Indexes, ConverterConstants.Points, data),
                    FigurePoints = this.GetDouble(roundData.Indexes, ConverterConstants.Figures, data),
                    MusicalRoutinePoints = this.GetDouble(roundData.Indexes, ConverterConstants.MusicalRoutinePoints, data),
                    FreeRoutinePoints = this.GetDouble(roundData.Indexes, ConverterConstants.FreeRoutine, data),
                    TechnicalRoutinePoints = this.GetDouble(roundData.Indexes, ConverterConstants.TechnicalRoutine, data),
                };

                round.Athletes.Add(athlete);
            }
        }
    }
}