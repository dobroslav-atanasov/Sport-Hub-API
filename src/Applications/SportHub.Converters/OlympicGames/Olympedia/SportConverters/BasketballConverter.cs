namespace SportHub.Converters.OlympicGames.Olympedia.SportConverters;

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

public class BasketballConverter : BaseSportConverter
{
    public BasketballConverter(IOlympediaService olympediaService, IDateService dateService, IDataCacheService dataCacheService, IRegExpService regExpService, IMapper mapper,
        INormalizeService normalizeService, OlympicGamesRepository<Team> teamRepository, OlympicGamesRepository<Participation> participationRepository,
        OlympicGamesRepository<Athlete> athleteRepository, OlympicGamesRepository<Result> resultRepository)
        : base(olympediaService, dateService, dataCacheService, regExpService, mapper, normalizeService, teamRepository, participationRepository, athleteRepository, resultRepository)
    {
    }

    public override async Task ProcessAsync(Options options)
    {
        switch (options.Discipline.Name)
        {
            case DisciplineConstants.BASKETBALL_3X3:
                await this.ProcessBasketball3x3Async(options);
                break;
            case DisciplineConstants.BASKETBALL:
                await this.ProcessBasketballAsync(options);
                break;
        }
    }

    private async Task ProcessBasketballAsync(Options options)
    {
        var rounds = new List<Round<Basketball>>();

        foreach (var roundData in options.Rounds.Skip(1))
        {
            var round = this.CreateRound<Basketball>(roundData, options.Event.Name, null);

            foreach (var row in roundData.Rows.Where(x => this.OlympediaService.FindResultNumber(x.OuterHtml) != 0))
            {
                var data = row.Elements("td").ToList();
                var matchInputModel = new MatchInputModel
                {
                    Row = row.OuterHtml,
                    Number = data[0].OuterHtml,
                    Date = data[1].InnerText,
                    Year = options.Game.Year,
                    EventId = options.Event.Id,
                    IsTeam = true,
                    HomeName = options.Game.Year >= 2020 ? data[2].OuterHtml : null,
                    HomeNOC = options.Game.Year >= 2020 ? data[3].OuterHtml : data[2].OuterHtml,
                    Result = options.Game.Year >= 2020 ? data[4].InnerHtml : data[3].InnerHtml,
                    AwayName = options.Game.Year >= 2020 ? data[5].OuterHtml : null,
                    AwayNOC = options.Game.Year >= 2020 ? data[6].OuterHtml : data[4].OuterHtml,
                    AnyParts = false,
                    RoundType = roundData.Type,
                    RoundSubType = roundData.SubType,
                    Location = null
                };
                var matchModel = await this.GetMatchAsync(matchInputModel);
                var match = this.Mapper.Map<TeamMatch<Basketball>>(matchModel);

                var document = options.Documents.FirstOrDefault(x => x.Id == match.ResultId);
                if (document != null)
                {
                    match.Judges = await this.GetJudgesAsync(document.Html);
                    match.Location = this.OlympediaService.FindLocation(document.Html);
                    match.Attendance = this.RegExpService.MatchInt(this.RegExpService.MatchFirstGroup(document.HtmlDocument.ParsedText, @"<th>Attendance<\/th>\s*<td(?:.*?)>(.*?)<\/td>"));

                    await this.SetAthletesAsync(match.Team1, options.Game.Year <= 2016 ? document.Rounds.FirstOrDefault() : document.Rounds[1], options.Event.Id, options.Game.Year);
                    await this.SetAthletesAsync(match.Team2, options.Game.Year <= 2016 ? document.Rounds.LastOrDefault() : document.Rounds[2], options.Event.Id, options.Game.Year);

                    if (options.Game.Year <= 2016)
                    {
                        this.SetHalfPoints(match, document.HtmlDocument);
                    }

                    this.SetStatistics(match.Team1);
                    this.SetStatistics(match.Team2);
                }

                round.TeamMatches.Add(match);
            }

            rounds.Add(round);
        }

        await this.ProcessJsonAsync(rounds, options);
    }

    private void SetHalfPoints(TeamMatch<Basketball> match, HtmlDocument htmlDocument)
    {
        var scoreMatch = this.RegExpService.Match(htmlDocument.ParsedText, @"<h2 class=""match_part_title"">Score<\/h2>(.*?)<h2");
        if (scoreMatch != null)
        {
            var document = new HtmlDocument();
            document.LoadHtml(scoreMatch.Groups[1].Value);

            var rows = document.DocumentNode.SelectNodes("//tr").Skip(1).ToList();
            match.Team1.FirstHalfPoints = this.RegExpService.MatchInt(rows[0].Elements("td").ToList().ElementAtOrDefault(2)?.InnerText);
            match.Team1.SecondHalfPoints = this.RegExpService.MatchInt(rows[0].Elements("td").ElementAtOrDefault(3)?.InnerText);
            match.Team2.FirstHalfPoints = this.RegExpService.MatchInt(rows[1].Elements("td").ElementAtOrDefault(2)?.InnerText);
            match.Team2.SecondHalfPoints = this.RegExpService.MatchInt(rows[1].Elements("td").ElementAtOrDefault(3)?.InnerText);
        }
    }

    private async Task SetAthletesAsync(Basketball team, RoundDataModel roundData, Guid eventId, int year)
    {
        foreach (var row in roundData.Rows.Skip(1))
        {
            var data = row.Elements("td").ToList();
            var athleteModel = this.OlympediaService.FindAthlete(data[roundData.Indexes[ConverterConstants.Name]].OuterHtml);
            if (athleteModel != null)
            {
                var participant = await this.ParticipationRepository.GetAsync(x => x.Code == athleteModel.Code && x.EventId == eventId);
                if (participant != null)
                {
                    var freeThrowPointMatch = this.RegExpService.Match(this.GetString(roundData.Indexes, ConverterConstants.FreeThrows, data), @"(\d+)\/(\d+)");

                    var athlete = new Basketball
                    {
                        Id = participant.Id,
                        Name = athleteModel.Name,
                        NOC = team.NOC,
                        Code = athleteModel.Code,
                        Number = this.GetInt(roundData.Indexes, ConverterConstants.Number, data),
                        Position = this.GetString(roundData.Indexes, ConverterConstants.Position, data),
                        Points = this.GetInt(roundData.Indexes, ConverterConstants.Points, data),
                        TimePlayed = this.GetTime(roundData.Indexes, ConverterConstants.TimePlayed, data),
                        FreeThrowsGoals = this.RegExpService.MatchInt(freeThrowPointMatch?.Groups[1].Value),
                        FreeThrowsAttempts = this.RegExpService.MatchInt(freeThrowPointMatch?.Groups[2].Value),
                        OffensiveRebounds = this.GetInt(roundData.Indexes, ConverterConstants.OffensiveRebounds, data),
                        DefensiveRebounds = this.GetInt(roundData.Indexes, ConverterConstants.DefensiveRebounds, data),
                        Assists = this.GetInt(roundData.Indexes, ConverterConstants.Assits, data),
                        Steals = this.GetInt(roundData.Indexes, ConverterConstants.Steals, data),
                        Blocks = this.GetInt(roundData.Indexes, ConverterConstants.Blocks, data),
                        Turnovers = this.GetInt(roundData.Indexes, ConverterConstants.Turnovers, data),
                        PersonalFouls = this.GetInt(roundData.Indexes, ConverterConstants.PersonalFouls, data),
                        DisqualifyingFouls = this.GetInt(roundData.Indexes, ConverterConstants.Disqualifcations, data),
                    };

                    if (year <= 2016)
                    {
                        var twoPointMatch = this.RegExpService.Match(this.GetString(roundData.Indexes, ConverterConstants.TwoPoints, data), @"(\d+)\/(\d+)");
                        var threePointMatch = this.RegExpService.Match(this.GetString(roundData.Indexes, ConverterConstants.ThreePoints, data), @"(\d+)\/(\d+)");

                        athlete.TwoPointsGoals = this.RegExpService.MatchInt(twoPointMatch?.Groups[1].Value);
                        athlete.TwoPointsAttempts = this.RegExpService.MatchInt(twoPointMatch?.Groups[2].Value);
                        athlete.ThreePointsGoals = this.RegExpService.MatchInt(threePointMatch?.Groups[1].Value);
                        athlete.ThreePointsAttempts = this.RegExpService.MatchInt(threePointMatch?.Groups[2].Value);
                    }
                    else
                    {
                        athlete.TwoPointsGoals = this.GetInt(roundData.Indexes, ConverterConstants.TwoPointsMade, data);
                        athlete.TwoPointsAttempts = this.GetInt(roundData.Indexes, ConverterConstants.TwoPointsAttempts, data);
                        athlete.ThreePointsGoals = this.GetInt(roundData.Indexes, ConverterConstants.ThreePointsMade, data);
                        athlete.ThreePointsAttempts = this.GetInt(roundData.Indexes, ConverterConstants.ThreePointsAttempts, data);
                    }

                    athlete.TotalFieldGoals = athlete.TwoPointsGoals + athlete.ThreePointsGoals;
                    athlete.TotalFieldGoalsAttempts = athlete.TwoPointsAttempts + athlete.ThreePointsAttempts;
                    athlete.TotalRebounds = athlete.OffensiveRebounds + athlete.DefensiveRebounds;

                    team.Athletes.Add(athlete);
                }
            }
            else
            {
                if (data[0].InnerText == "Team Totals")
                {
                    team.FirstHalfPoints = this.RegExpService.MatchInt(data[2].InnerText);
                    team.SecondHalfPoints = this.RegExpService.MatchInt(data[3].InnerText);
                }
            }
        }
    }

    private async Task ProcessBasketball3x3Async(Options options)
    {
        var rounds = new List<Round<Basketball>>();

        foreach (var roundData in options.Rounds.Skip(1))
        {
            var round = this.CreateRound<Basketball>(roundData, options.Event.Name, null);

            foreach (var row in roundData.Rows.Where(x => this.OlympediaService.FindResultNumber(x.OuterHtml) != 0))
            {
                var data = row.Elements("td").ToList();
                var matchInputModel = new MatchInputModel
                {
                    Row = row.OuterHtml,
                    Number = data[0].OuterHtml,
                    Date = data[1].InnerText,
                    Year = options.Game.Year,
                    EventId = options.Event.Id,
                    IsTeam = true,
                    HomeName = data[2].OuterHtml,
                    HomeNOC = data[3].OuterHtml,
                    Result = data[4].InnerHtml,
                    AwayName = data[5].OuterHtml,
                    AwayNOC = data[6].OuterHtml,
                    AnyParts = false,
                    RoundType = roundData.Type,
                    RoundSubType = roundData.SubType,
                    Location = null
                };
                var matchModel = await this.GetMatchAsync(matchInputModel);
                var match = this.Mapper.Map<TeamMatch<Basketball>>(matchModel);

                var document = options.Documents.FirstOrDefault(x => x.Id == match.ResultId);
                if (document != null)
                {
                    match.Judges = await this.GetJudgesAsync(document.Html);
                    match.Location = this.OlympediaService.FindLocation(document.Html);
                    match.Attendance = this.RegExpService.MatchInt(this.RegExpService.MatchFirstGroup(document.HtmlDocument.ParsedText, @"<th>Attendance<\/th>\s*<td(?:.*?)>(.*?)<\/td>"));

                    await this.SetAthletes3x3Async(match.Team1, document.Rounds[1], options.Event.Id);
                    await this.SetAthletes3x3Async(match.Team2, document.Rounds[2], options.Event.Id);

                    this.SetStatistics(match.Team1);
                    this.SetStatistics(match.Team2);
                }

                round.TeamMatches.Add(match);
            }

            rounds.Add(round);
        }

        await this.ProcessJsonAsync(rounds, options);
    }

    private void SetStatistics(Basketball team)
    {
        team.OnePointsGoals = team.Athletes.Sum(x => x.OnePointsGoals);
        team.OnePointsAttempts = team.Athletes.Sum(x => x.OnePointsAttempts);
        team.TwoPointsGoals = team.Athletes.Sum(x => x.TwoPointsGoals);
        team.TwoPointsAttempts = team.Athletes.Sum(x => x.TwoPointsAttempts);
        team.ThreePointsGoals = team.Athletes.Sum(x => x.ThreePointsGoals);
        team.ThreePointsAttempts = team.Athletes.Sum(x => x.ThreePointsAttempts);
        team.TotalFieldGoals = team.Athletes.Sum(x => x.TotalFieldGoals);
        team.TotalFieldGoalsAttempts = team.Athletes.Sum(x => x.TotalFieldGoalsAttempts);
        team.FreeThrowsGoals = team.Athletes.Sum(x => x.FreeThrowsGoals);
        team.FreeThrowsAttempts = team.Athletes.Sum(x => x.FreeThrowsAttempts);
        team.OffensiveRebounds = team.Athletes.Sum(x => x.OffensiveRebounds);
        team.DefensiveRebounds = team.Athletes.Sum(x => x.DefensiveRebounds);
        team.TotalRebounds = team.OffensiveRebounds + team.DefensiveRebounds;
        team.Assists = team.Athletes.Sum(x => x.Assists);
        team.Steals = team.Athletes.Sum(x => x.Steals);
        team.Blocks = team.Athletes.Sum(x => x.Blocks);
        team.Turnovers = team.Athletes.Sum(x => x.Turnovers);
        team.PersonalFouls = team.Athletes.Sum(x => x.PersonalFouls);
        team.DisqualifyingFouls = team.Athletes.Sum(x => x.DisqualifyingFouls);
    }

    private async Task SetAthletes3x3Async(Basketball team, RoundDataModel roundData, Guid eventId)
    {
        foreach (var row in roundData.Rows.Skip(1).Take(roundData.Rows.Count - 2))
        {
            var data = row.Elements("td").ToList();
            var athleteModel = this.OlympediaService.FindAthlete(data[roundData.Indexes[ConverterConstants.Name]].OuterHtml);
            var participant = await this.ParticipationRepository.GetAsync(x => x.Code == athleteModel.Code && x.EventId == eventId);
            var onePointMatch = this.RegExpService.Match(data[9].InnerText, @"(\d+)\/(\d+)");
            var twoPointMatch = this.RegExpService.Match(data[11].InnerText, @"(\d+)\/(\d+)");
            var freeThrowPointMatch = this.RegExpService.Match(data[15].InnerText, @"(\d+)\/(\d+)");

            var athlete = new Basketball
            {
                Id = participant.Id,
                Name = athleteModel.Name,
                NOC = team.NOC,
                Code = athleteModel.Code,
                Number = this.GetInt(roundData.Indexes, ConverterConstants.Number, data),
                Position = this.GetString(roundData.Indexes, ConverterConstants.Position, data),
                Points = this.GetInt(roundData.Indexes, ConverterConstants.Points, data),
                TimePlayed = this.GetTime(roundData.Indexes, ConverterConstants.TimePlayed, data),
                Value = this.GetDouble(roundData.Indexes, ConverterConstants.Value, data),
                PlusMinus = this.GetInt(roundData.Indexes, ConverterConstants.PlusMinus, data),
                ShootingEfficiency = this.GetDouble(roundData.Indexes, ConverterConstants.ShootingEfficiency, data),
                OnePointsGoals = this.RegExpService.MatchInt(onePointMatch?.Groups[1].Value),
                OnePointsAttempts = this.RegExpService.MatchInt(onePointMatch?.Groups[2].Value),
                TwoPointsGoals = this.RegExpService.MatchInt(twoPointMatch?.Groups[1].Value),
                TwoPointsAttempts = this.RegExpService.MatchInt(twoPointMatch?.Groups[2].Value),
                FreeThrowsGoals = this.RegExpService.MatchInt(freeThrowPointMatch?.Groups[1].Value),
                FreeThrowsAttempts = this.RegExpService.MatchInt(freeThrowPointMatch?.Groups[2].Value),
                OffensiveRebounds = this.GetInt(roundData.Indexes, ConverterConstants.OffensiveRebounds, data),
                DefensiveRebounds = this.GetInt(roundData.Indexes, ConverterConstants.DefensiveRebounds, data),
                Blocks = this.GetInt(roundData.Indexes, ConverterConstants.Blocks, data),
                Turnovers = this.GetInt(roundData.Indexes, ConverterConstants.Turnovers, data)
            };

            athlete.TotalFieldGoals = athlete.OnePointsGoals + athlete.TwoPointsGoals;
            athlete.TotalFieldGoalsAttempts = athlete.OnePointsAttempts + athlete.TwoPointsAttempts;
            athlete.TotalRebounds = athlete.OffensiveRebounds + athlete.DefensiveRebounds;

            team.Athletes.Add(athlete);
        }
    }
}