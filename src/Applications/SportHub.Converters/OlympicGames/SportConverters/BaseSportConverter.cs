namespace SportHub.Converters.OlympicGames.SportConverters;

using System;
using System.Text.Json;
using System.Text.Json.Serialization;

using AutoMapper;

using HtmlAgilityPack;

using SportHub.Common.Constants;
using SportHub.Data.Models.Converters.OlympicGames;
using SportHub.Data.Models.Converters.OlympicGames.Base;
using SportHub.Data.Models.Converters.OlympicGames.Disciplines;
using SportHub.Data.Models.DbEntities.Enumerations;
using SportHub.Data.Models.DbEntities.OlympicGames;
using SportHub.Data.Models.Enumerations.OlympicGames;
using SportHub.Data.Repositories;
using SportHub.Services.Data.OlympicGamesDb.Interfaces;
using SportHub.Services.Interfaces;

public abstract class BaseSportConverter
{
    private readonly OlympicGamesRepository<Result> resultRepository;

    public BaseSportConverter(IOlympediaService olympediaService, IDateService dateService, IDataCacheService dataCacheService, IRegExpService regExpService, IMapper mapper,
        INormalizeService normalizeService, OlympicGamesRepository<Team> teamRepository, OlympicGamesRepository<Participation> participationRepository,
        OlympicGamesRepository<Athlete> athleteRepository, OlympicGamesRepository<Result> resultRepository)
    {
        this.OlympediaService = olympediaService;
        this.DateService = dateService;
        this.DataCacheService = dataCacheService;
        this.RegExpService = regExpService;
        this.Mapper = mapper;
        this.NormalizeService = normalizeService;
        this.TeamRepository = teamRepository;
        this.ParticipationRepository = participationRepository;
        this.AthleteRepository = athleteRepository;
        this.resultRepository = resultRepository;
    }

    protected IOlympediaService OlympediaService { get; }
    protected IDateService DateService { get; }
    protected IDataCacheService DataCacheService { get; }
    protected IRegExpService RegExpService { get; }
    protected IMapper Mapper { get; }
    protected INormalizeService NormalizeService { get; }
    protected OlympicGamesRepository<Team> TeamRepository { get; }
    protected OlympicGamesRepository<Participation> ParticipationRepository { get; }
    protected OlympicGamesRepository<Athlete> AthleteRepository { get; }

    public abstract Task ProcessAsync(Options options);

    protected Round<TModel> CreateRound<TModel>(RoundDataModel roundDataModel, string eventName, Track track)
    {
        return new Round<TModel>
        {
            FromDate = roundDataModel.From,
            ToDate = roundDataModel.To,
            Format = roundDataModel.Format,
            EventName = eventName,
            Info = roundDataModel.Info,
            Name = roundDataModel.Name,
            Number = roundDataModel.Number,
            Type = roundDataModel.Type,
            SubType = roundDataModel.SubType,
            Track = track
        };
    }

    protected async Task<MatchModel> GetMatchAsync(MatchInputModel input)
    {
        var match = new MatchModel();

        if (!string.IsNullOrEmpty(input.Number))
        {
            match.Number = this.OlympediaService.FindMatchNumber(input.Number);
        }

        if (!string.IsNullOrEmpty(input.Location))
        {
            match.Location = input.Location;
        }

        if (!string.IsNullOrEmpty(input.Date))
        {
            var dateModel = this.DateService.ParseDate(input.Date, input.Year);
            match.Date = dateModel.From;
        }

        match.Decision = this.OlympediaService.FindDecision(input.Row);
        match.Info = this.OlympediaService.FindMatchInfo(input.Number);
        match.ResultId = this.OlympediaService.FindResultNumber(input.Number);
        match.Medal = this.OlympediaService.FindMedal(input.Number, input.RoundType);

        if (input.IsTeam)
        {
            var homeTeamNOCCode = this.OlympediaService.FindNOCCode(input.HomeNOC);
            var homeTeamNOC = this.DataCacheService.NOCs.FirstOrDefault(x => x.Code == homeTeamNOCCode);

            Team homeTeam = null;
            if (input.IsDoubles)
            {
                var athleteModels = this.OlympediaService.FindAthletes(input.HomeName);
                var participant = await this.ParticipationRepository.GetAsync(x => x.Code == athleteModels.FirstOrDefault().Code && x.EventId == input.EventId);
                //homeTeam = await this.TeamRepository.GetAsync(x => x.NationalOlympicCommitteeId == homeTeamNOC.Id && x.Squads.Any(x => x.ParticipationId == participant.Id));
            }
            else
            {
                homeTeam = await this.TeamRepository.GetAsync(x => x.NOCId == homeTeamNOC.Id && x.EventId == input.EventId);
            }

            if (homeTeam == null)
            {
                ;
            }
            else
            {
                match.Team1.Id = homeTeam.Id;
                match.Team1.Name = homeTeam.Name;
                match.Team1.NOC = homeTeamNOCCode;
                match.Team1.Seed = this.OlympediaService.FindSeedNumber(input.HomeName);

                if (match.Decision == DecisionEnum.None)
                {
                    var awayTeamNOCCode = this.OlympediaService.FindNOCCode(input.AwayNOC);
                    if (awayTeamNOCCode != null)
                    {
                        var awayTeamNOC = this.DataCacheService.NOCs.FirstOrDefault(x => x.Code == awayTeamNOCCode);
                        Team awayTeam = null;
                        if (input.IsDoubles)
                        {
                            var athleteModels = this.OlympediaService.FindAthletes(input.AwayName);
                            var participant = await this.ParticipationRepository.GetAsync(x => x.Code == athleteModels.FirstOrDefault().Code && x.EventId == input.EventId);
                            //awayTeam = await this.TeamRepository.GetAsync(x => x.NationalOlympicCommitteeId == awayTeamNOC.Id && x.Squads.Any(x => x.ParticipationId == participant.Id));
                        }
                        else
                        {
                            awayTeam = await this.TeamRepository.GetAsync(x => x.NOCId == awayTeamNOC.Id && x.EventId == input.EventId);
                        }

                        if (awayTeam == null)
                        {
                            ;
                        }
                        else
                        {
                            match.Team2.Id = awayTeam.Id;
                            match.Team2.Name = awayTeam.Name;
                            match.Team2.NOC = awayTeamNOCCode;
                            match.Team2.Seed = this.OlympediaService.FindSeedNumber(input.AwayName);
                        }
                    }
                }
            }
        }
        else
        {
            var homeAthleteModel = this.OlympediaService.FindAthlete(input.HomeName);
            var homeAthleteNOCCode = this.OlympediaService.FindNOCCode(input.HomeNOC);
            var homeAthleteNOC = this.DataCacheService.NOCs.FirstOrDefault(x => x.Code == homeAthleteNOCCode);
            if (homeAthleteModel != null)
            {
                var homeAthlete = await this.ParticipationRepository.GetAsync(x => x.Code == homeAthleteModel.Code && x.EventId == input.EventId);
                homeAthlete ??= await this.ParticipationRepository.GetAsync(x => x.Code == homeAthleteModel.Code && x.EventId == input.EventId && x.NOCId == homeAthleteNOC.Id);

                match.Team1.Id = homeAthlete.Id;
                match.Team1.Name = homeAthleteModel.Name;
                match.Team1.NOC = homeAthleteNOCCode;
                match.Team1.Code = homeAthleteModel.Code;
                match.Team1.Seed = this.OlympediaService.FindSeedNumber(input.HomeName);

                if (match.Decision == DecisionEnum.None)
                {
                    var awayAthleteModel = this.OlympediaService.FindAthlete(input.AwayName);
                    var awayAthleteNOCCode = this.OlympediaService.FindNOCCode(input.AwayNOC);
                    if (awayAthleteModel != null && awayAthleteNOCCode != null)
                    {
                        var awayAthleteNOC = this.DataCacheService.NOCs.FirstOrDefault(x => x.Code == awayAthleteNOCCode);
                        var awayAthlete = await this.ParticipationRepository.GetAsync(x => x.Code == awayAthleteModel.Code && x.EventId == input.EventId);
                        awayAthlete ??= await this.ParticipationRepository.GetAsync(x => x.Code == awayAthleteModel.Code && x.EventId == input.EventId && x.NOCId == awayAthleteNOC.Id);

                        if (awayAthlete != null)
                        {
                            match.Team2.Id = awayAthlete.Id;
                            match.Team2.Name = awayAthleteModel.Name;
                            match.Team2.NOC = awayAthleteNOCCode;
                            match.Team2.Code = awayAthleteModel.Code;
                            match.Team2.Seed = this.OlympediaService.FindSeedNumber(input.AwayName);
                        }
                    }
                }
            }
        }

        if (match.Decision == DecisionEnum.None && match.Team2.NOC != null)
        {
            input.Result = input.Result.Replace("[", string.Empty).Replace("]", string.Empty);
            var isDone = false;
            if (input.AnyParts)
            {
                var partsMatch = this.RegExpService.Match(input.Result, @"(\d+)\s*-\s*(\d+)\s*,\s*(\d+)\s*-\s*(\d+)\s*,\s*(\d+)\s*-\s*(\d+)");
                if (partsMatch != null)
                {
                    match.Team1.Parts = [int.Parse(partsMatch.Groups[1].Value), int.Parse(partsMatch.Groups[3].Value), int.Parse(partsMatch.Groups[5].Value)];
                    match.Team2.Parts = [int.Parse(partsMatch.Groups[2].Value), int.Parse(partsMatch.Groups[4].Value), int.Parse(partsMatch.Groups[6].Value)];
                    isDone = true;
                }
                partsMatch = this.RegExpService.Match(input.Result, @"(\d+)\s*-\s*(\d+)\s*,\s*(\d+)\s*-\s*(\d+)");
                if (partsMatch != null && !isDone)
                {
                    match.Team1.Parts = [int.Parse(partsMatch.Groups[1].Value), int.Parse(partsMatch.Groups[3].Value)];
                    match.Team2.Parts = [int.Parse(partsMatch.Groups[2].Value), int.Parse(partsMatch.Groups[4].Value)];
                    isDone = true;
                }
                partsMatch = this.RegExpService.Match(input.Result, @"(\d+)\s*-\s*(\d+)");
                if (partsMatch != null && !isDone)
                {
                    match.Team1.Parts = [int.Parse(partsMatch.Groups[1].Value)];
                    match.Team2.Parts = [int.Parse(partsMatch.Groups[2].Value)];
                    isDone = true;
                }

                for (var i = 0; i < 5; i++)
                {
                    var team1Points = match.Team1.Parts.ElementAtOrDefault(i);
                    var team2Points = match.Team2.Parts.ElementAtOrDefault(i);

                    if (team1Points != null && team2Points != null)
                    {
                        if (team1Points > team2Points)
                        {
                            match.Team1.Points++;
                        }
                        else
                        {
                            match.Team2.Points++;
                        }
                    }
                }

                this.SetWinAndLose(match);
            }
            else
            {
                var regexMatch = this.RegExpService.Match(input.Result, @"(\d+)\s*(?:-|–|—)\s*(\d+)");
                if (regexMatch != null)
                {
                    match.Team1.Points = int.Parse(regexMatch.Groups[1].Value.Trim());
                    match.Team2.Points = int.Parse(regexMatch.Groups[2].Value.Trim());

                    this.OlympediaService.SetWinAndLose(match);
                }

                regexMatch = this.RegExpService.Match(input.Result, @"(\d+)\.(\d+)\s*(?:-|–|—)\s*(\d+)\.(\d+)");
                if (regexMatch != null)
                {
                    match.Team1.Time = this.DateService.ParseTime($"{regexMatch.Groups[1].Value}.{regexMatch.Groups[2].Value}");
                    match.Team2.Time = this.DateService.ParseTime($"{regexMatch.Groups[3].Value}.{regexMatch.Groups[4].Value}");

                    if (match.Team1.Time < match.Team2.Time)
                    {
                        match.Team1.MatchResult = MatchResultType.Win;
                        match.Team2.MatchResult = MatchResultType.Lose;
                    }
                    else if (match.Team1.Time > match.Team2.Time)
                    {
                        match.Team1.MatchResult = MatchResultType.Lose;
                        match.Team2.MatchResult = MatchResultType.Win;
                    }
                }

                regexMatch = this.RegExpService.Match(input.Result, @"(\d+)\.(\d+)\s*(?:-|–|—)\s*DNF");
                if (regexMatch != null)
                {
                    match.Team1.Time = this.DateService.ParseTime($"{regexMatch.Groups[1].Value}.{regexMatch.Groups[2].Value}");
                    match.Team1.MatchResult = MatchResultType.Win;
                    match.Team2.MatchResult = MatchResultType.Lose;
                }

                regexMatch = this.RegExpService.Match(input.Result, @"DNF\s*(?:-|–|—)\s*(\d+)\.(\d+)");
                if (regexMatch != null)
                {
                    match.Team2.Time = this.DateService.ParseTime($"{regexMatch.Groups[1].Value}.{regexMatch.Groups[2].Value}");
                    match.Team2.MatchResult = MatchResultType.Win;
                    match.Team1.MatchResult = MatchResultType.Lose;
                }
            }
        }

        return match;
    }

    public void SetWinAndLose(MatchModel match)
    {
        if (match.Team1.Points > match.Team2.Points)
        {
            match.Team1.MatchResult = MatchResultType.Win;
            match.Team2.MatchResult = MatchResultType.Lose;
        }
        else if (match.Team1.Points < match.Team2.Points)
        {
            match.Team1.MatchResult = MatchResultType.Lose;
            match.Team2.MatchResult = MatchResultType.Win;
        }
        else if (match.Team1.Points == match.Team2.Points)
        {
            match.Team1.MatchResult = MatchResultType.Draw;
            match.Team2.MatchResult = MatchResultType.Draw;
        }
    }

    protected async Task<List<Judge>> GetJudgesAsync(string html, string info = null)
    {
        var matches = this.RegExpService.Matches(html, @"<tr class=""(?:referees|hidden_referees)""(?:.*?)<th>(.*?)<\/th>(.*?)<\/tr>");
        var judges = new List<Judge>();
        foreach (System.Text.RegularExpressions.Match match in matches)
        {
            //var judgeMatch = this.RegExpService.Match(html, @$"<th>{match.Groups[1].Value}<\/th>(.*)");
            //if (judgeMatch != null)
            //{
            //var athleteModel = this.OlympediaService.FindAthlete(judgeMatch.Groups[1].Value);
            var athleteModel = this.OlympediaService.FindAthlete(match.Groups[0].Value);
            //var nocCode = this.OlympediaService.FindNOCCode(judgeMatch.Groups[1].Value);
            var nocCode = this.OlympediaService.FindNOCCode(match.Groups[0].Value);
            var nocCodeCache = this.DataCacheService.NOCs.FirstOrDefault(x => x.Code == nocCode);
            if (athleteModel != null && nocCodeCache != null)
            {
                var athlete = await this.AthleteRepository.GetAsync(x => x.Code == athleteModel.Code);

                var judge = new Judge
                {
                    Id = athlete == null ? Guid.Empty : athlete.Id,
                    Code = athleteModel.Code,
                    Name = athleteModel.Name,
                    NOC = nocCode,
                    Title = $"{match.Groups[1].Value}",
                    Info = info
                };

                judges.Add(judge);
                //}
            }
        }

        judges.RemoveAll(x => x == null);
        return judges;
    }

    protected int? GetInt(Dictionary<string, int> indexes, string name, List<HtmlNode> nodes)
    {
        return indexes.TryGetValue(name, out var value) ? this.RegExpService.MatchInt(nodes[value].InnerText) : null;
    }

    protected double? GetDouble(Dictionary<string, int> indexes, string name, List<HtmlNode> nodes)
    {
        return indexes.TryGetValue(name, out var value) ? this.RegExpService.MatchDouble(nodes[value].InnerText) : null;
    }

    protected string GetString(Dictionary<string, int> indexes, string name, List<HtmlNode> nodes)
    {
        var data = indexes.TryGetValue(name, out var value) ? nodes[value].InnerText : null;
        if (string.IsNullOrEmpty(data))
        {
            return null;
        }

        return data;
    }

    protected TimeSpan? GetTime(Dictionary<string, int> indexes, string name, List<HtmlNode> nodes)
    {
        return indexes.TryGetValue(name, out var value) ? this.DateService.ParseTime(nodes[value].InnerText) : null;
    }

    protected async Task ProcessJsonAsync<TModel>(List<Round<TModel>> rounds, Options options)
    {
        var ranking = await this.ProcessRankingAsync(options);

        var resultModel = new Result<TModel>
        {
            EventData = new EventData
            {
                Id = options.Event.Id,
                Gender = options.Event.Gender,
                IsTeamEvent = options.Event.IsTeamEvent,
                Name = options.Event.Name,
                NormalizedName = options.Event.NormalizedName,
                OriginalName = options.Event.OriginalName
            },
            DisciplineData = new DisciplineData
            {
                Id = options.Discipline.Id,
                Name = options.Discipline.Name
            },
            GameData = new GameData
            {
                Id = options.Game.Id,
                Type = options.Game.Type,
                Year = options.Game.Year
            },
            Ranking = ranking,
            Rounds = rounds,
        };

        var json = JsonSerializer.Serialize(resultModel, new JsonSerializerOptions { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull });
        var result = new Result
        {
            EventId = options.Event.Id,
            UniqueNumber = $"{resultModel.GameData.Id}_{resultModel.DisciplineData.Id}_{resultModel.EventData.Id}",
            Json = json,
            //GameId = options.Game.Id,
        };

        //var dbResult = await this.resultRepository.GetAsync(x => x.UniqueNumber == $"{resultModel.GameData.Id}_{resultModel.DisciplineData.Id}_{resultModel.EventData.Id}");
        //if (dbResult != null)
        //{
        //    var equals = result.Equals(dbResult);
        //    if (!equals)
        //    {
        //        this.resultRepository.Update(result);
        //        await this.resultRepository.SaveChangesAsync();
        //    }
        //}
        //else
        //{
        //    await this.resultRepository.AddAsync(result);
        //    await this.resultRepository.SaveChangesAsync();
        //    dbResult = result;
        //}

        //await this.MapResultToParticipationsAndTeams(dbResult, rounds);
    }

    private async Task<Ranking> ProcessRankingAsync(Options options)
    {
        var ranking = new Ranking();
        var roundData = options.Rounds.FirstOrDefault();
        if (roundData == null)
        {
            return ranking;
        }

        foreach (var row in roundData.Rows.Skip(1))
        {
            var noc = this.OlympediaService.FindNOCCode(row.OuterHtml);
            var data = row.Elements("td").ToList();
            var athleteModels = this.OlympediaService.FindAthletes(row.OuterHtml);

            if (noc != null && athleteModels.Count == 0)
            {
                var nocCache = this.DataCacheService.NOCs.FirstOrDefault(x => x.Code == noc);
                if (nocCache != null)
                {
                    var teamName = this.GetString(roundData.Indexes, ConverterConstants.Name, data);
                    var team = await this.TeamRepository.GetAsync(x => x.Name == teamName && x.NOCId == nocCache.Id && x.EventId == options.Event.Id);
                    team ??= await this.TeamRepository.GetAsync(x => x.NOCId == nocCache.Id && x.EventId == options.Event.Id);

                    if (team != null)
                    {
                        ranking.Teams.Add(new TeamRanking
                        {
                            Id = team.Id,
                            Name = team.Name,
                            NOC = noc,
                            Medal = this.OlympediaService.FindMedal(row.OuterHtml)
                        });
                    }
                }
            }
            else if (noc != null && athleteModels.Count == 1)
            {
                var athleteModel = athleteModels.Single();
                var participation = await this.ParticipationRepository.GetAsync(x => x.Code == athleteModel.Code && x.EventId == options.Event.Id);
                ranking.Athletes.Add(new AthleteRanking
                {
                    Id = participation.Id,
                    Name = athleteModel.Name,
                    Code = athleteModel.Code,
                    NOC = noc,
                    Medal = this.OlympediaService.FindMedal(row.OuterHtml)
                });
            }
            else if (noc == null && athleteModels.Count == 1)
            {
                var athleteModel = athleteModels.Single();
                var participation = await this.ParticipationRepository.GetAsync(x => x.Code == athleteModel.Code && x.EventId == options.Event.Id);
                if (participation != null)
                {
                    var team = ranking.Teams.Last();

                    team.Athletes.Add(new AthleteRanking
                    {
                        Id = participation.Id,
                        Name = athleteModel.Name,
                        Code = athleteModel.Code,
                        NOC = team.NOC,
                        Medal = this.OlympediaService.FindMedal(row.OuterHtml)
                    });
                }
            }
            else if (noc == null && athleteModels.Count > 1)
            {
                var team = ranking.Teams.Last();
                foreach (var athleteModel in athleteModels)
                {
                    var participation = await this.ParticipationRepository.GetAsync(x => x.Code == athleteModel.Code && x.EventId == options.Event.Id);
                    team.Athletes.Add(new AthleteRanking
                    {
                        Id = participation.Id,
                        Name = athleteModel.Name,
                        Code = athleteModel.Code,
                        NOC = team.NOC,
                        Medal = this.OlympediaService.FindMedal(row.OuterHtml)
                    });
                }
            }
            else if (noc != null && athleteModels.Count == 2)
            {
                var nocCache = this.DataCacheService.NOCs.FirstOrDefault(x => x.Code == noc);
                var teamName = this.GetString(roundData.Indexes, ConverterConstants.Name, data);
                var dbTeam = await this.TeamRepository.GetAsync(x => x.Name == teamName && x.NOCId == nocCache.Id && x.EventId == options.Event.Id);
                dbTeam ??= await this.TeamRepository.GetAsync(x => x.NOCId == nocCache.Id && x.EventId == options.Event.Id);

                ranking.Teams.Add(new TeamRanking
                {
                    Id = dbTeam.Id,
                    Name = dbTeam.Name,
                    NOC = noc,
                    Medal = this.OlympediaService.FindMedal(row.OuterHtml)
                });

                var team = ranking.Teams.Last();
                foreach (var athleteModel in athleteModels)
                {
                    var participation = await this.ParticipationRepository.GetAsync(x => x.Code == athleteModel.Code && x.EventId == options.Event.Id);
                    team.Athletes.Add(new AthleteRanking
                    {
                        Id = participation.Id,
                        Name = athleteModel.Name,
                        Code = athleteModel.Code,
                        NOC = team.NOC,
                        Medal = this.OlympediaService.FindMedal(row.OuterHtml)
                    });
                }
            }
            else if (noc == null && athleteModels.Count == 0)
            {
                continue;
            }
            else
            {
                ;
            }
        }

        return ranking;
    }

    private async Task MapResultToParticipationsAndTeams<TModel>(Result result, List<Round<TModel>> rounds)
    {
        foreach (var round in rounds)
        {
            var athletes = new List<Guid>();
            var teams = new List<Guid>();

            if (round.Athletes.Count > 0)
            {
                athletes.AddRange(round.Athletes.Select(x => (x as BaseModel).Id));
            }

            if (round.Teams.Count > 0)
            {
                teams.AddRange(round.Teams.Select(x => (x as BaseModel).Id));
            }

            switch (typeof(TModel).Name)
            {
                case nameof(Basketball):
                    (round.TeamMatches as List<TeamMatch<Basketball>>).ForEach(x =>
                    {
                        teams.Add(x.Team1.Id);
                        teams.Add(x.Team2.Id);
                        athletes.AddRange(x.Team1.Athletes.Select(x => x.Id));
                        athletes.AddRange(x.Team2.Athletes.Select(x => x.Id));
                    });
                    break;
                case nameof(AlpineSkiing):
                    (round.TeamMatches as List<TeamMatch<AlpineSkiing>>).ForEach(x =>
                    {
                        teams.Add(x.Team1.Id);
                        teams.Add(x.Team2.Id);
                        athletes.AddRange(x.Team1.Athletes.Select(x => x.Id));
                        athletes.AddRange(x.Team2.Athletes.Select(x => x.Id));
                    });
                    break;
            }

            foreach (var athleteId in athletes)
            {
                var participation = await this.ParticipationRepository.GetAsync(x => x.Id == athleteId);
                //participation.ResultId = result.Id;
                this.ParticipationRepository.Update(participation);
                await this.ParticipationRepository.SaveChangesAsync();
            }

            foreach (var teamId in teams)
            {
                var team = await this.TeamRepository.GetAsync(x => x.Id == teamId);
                //team.ResultId = result.Id;
                this.TeamRepository.Update(team);
                await this.TeamRepository.SaveChangesAsync();
            }
        }
    }
}