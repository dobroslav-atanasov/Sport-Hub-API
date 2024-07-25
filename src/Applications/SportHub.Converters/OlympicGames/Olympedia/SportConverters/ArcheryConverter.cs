namespace SportHub.Converters.OlympicGames.Olympedia.SportConverters;

using AutoMapper;

using SportHub.Common.Constants;
using SportHub.Data.Models.Converters.OlympicGames;
using SportHub.Data.Models.Converters.OlympicGames.Base;
using SportHub.Data.Models.Converters.OlympicGames.Disciplines;
using SportHub.Data.Models.DbEntities.OlympicGames;
using SportHub.Data.Models.Enumerations.OlympicGames;
using SportHub.Data.Repositories;
using SportHub.Services.Data.OlympicGamesDb.Interfaces;
using SportHub.Services.Interfaces;

public class ArcheryConverter : BaseSportConverter
{
    public ArcheryConverter(IOlympediaService olympediaService, IDateService dateService, IDataCacheService dataCacheService, IRegExpService regExpService, IMapper mapper,
        INormalizeService normalizeService, OlympicGamesRepository<Team> teamRepository, OlympicGamesRepository<Participation> participationRepository,
        OlympicGamesRepository<Athlete> athleteRepository, OlympicGamesRepository<Result> resultRepository)
        : base(olympediaService, dateService, dataCacheService, regExpService, mapper, normalizeService, teamRepository, participationRepository, athleteRepository, resultRepository)
    {
    }

    public override async Task ProcessAsync(Options options)
    {
        var rounds = new List<Round<Archery>>();

        if (options.Event.IsTeamEvent)
        {
            foreach (var roundData in options.Rounds)
            {
                if (options.Game.Year >= 1988 && roundData.Order == 1)
                {
                    continue;
                }

                var round = this.CreateRound<Archery>(roundData, options.Event.Name, null);
                if (options.Game.Year <= 1988 || round.Type == RoundEnum.RankingRound)
                {
                    await this.SetTeamsAsync(round, roundData, options.Event.Id, null);
                    var documentNumbers = this.OlympediaService.FindResults(roundData.Html);
                    foreach (var number in documentNumbers)
                    {
                        var document = options.Documents.FirstOrDefault(x => x.Id == number);
                        await this.SetTeamsAsync(round, document.Rounds.FirstOrDefault(), options.Event.Id, document.Title);
                    }
                }
                else if (options.Game.Year >= 1992)
                {
                    await this.SetTeamMatchesAsync(round, roundData, options);
                }

                rounds.Add(round);
            }
        }
        else
        {
            foreach (var roundData in options.Rounds)
            {
                if (options.Game.Year >= 1988 && roundData.Order == 1)
                {
                    continue;
                }

                var round = this.CreateRound<Archery>(roundData, options.Event.Name, null);
                if (options.Game.Year <= 1984)
                {
                    await this.SetAthletesAsync(round, roundData, options.Event.Id, null);
                }
                if (options.Game.Year == 1988 || (options.Game.Year >= 1992 && round.Type == RoundEnum.RankingRound))
                {
                    await this.SetAthletesAsync(round, roundData, options.Event.Id, null);
                    var documentNumbers = this.OlympediaService.FindResults(roundData.Html);
                    foreach (var number in documentNumbers)
                    {
                        var document = options.Documents.FirstOrDefault(x => x.Id == number);
                        await this.SetAthletesAsync(round, document.Rounds.FirstOrDefault(), options.Event.Id, document.Title);
                    }
                }
                else if (options.Game.Year >= 1992)
                {
                    await this.SetAthleteMatchesAsync(round, roundData, options);
                }

                rounds.Add(round);
            }
        }

        await this.ProcessJsonAsync(rounds, options);
    }

    private async Task SetAthletesAsync(Round<Archery> round, RoundDataModel roundData, Guid eventId, string info)
    {
        foreach (var row in roundData.Rows.Skip(1))
        {
            var data = row.Elements("td").ToList();
            var athleteModel = this.OlympediaService.FindAthlete(data[roundData.Indexes[ConverterConstants.Name]].OuterHtml);

            if (string.IsNullOrEmpty(info))
            {
                var participant = await this.ParticipationRepository.GetAsync(x => x.Code == athleteModel.Code && x.EventId == eventId);
                var noc = this.OlympediaService.FindNOCCode(data[roundData.Indexes[ConverterConstants.NOC]].OuterHtml);
                var athlete = new Archery
                {
                    Id = participant.Id,
                    Name = athleteModel.Name,
                    NOC = noc,
                    Code = athleteModel.Code,
                    FinishStatus = this.OlympediaService.FindFinishStatus(row.OuterHtml),
                    IsQualified = this.OlympediaService.IsQualified(row.OuterHtml),
                    Number = this.GetInt(roundData.Indexes, ConverterConstants.Number, data),
                    TargetsHit = this.GetInt(roundData.Indexes, ConverterConstants.TargetsHit, data),
                    Golds = this.GetInt(roundData.Indexes, ConverterConstants.Golds, data),
                    Points = this.GetDouble(roundData.Indexes, ConverterConstants.Points, data),
                    Record = this.OlympediaService.FindRecord(row.OuterHtml),
                    Score10s = this.GetInt(roundData.Indexes, ConverterConstants.S10, data),
                    Score9s = this.GetInt(roundData.Indexes, ConverterConstants.S9, data),
                    ScoreXs = this.GetInt(roundData.Indexes, ConverterConstants.Xs, data),
                    Target = this.GetString(roundData.Indexes, ConverterConstants.Target, data),
                    Score = this.GetInt(roundData.Indexes, ConverterConstants.Score, data),
                    ShootOff = this.GetInt(roundData.Indexes, ConverterConstants.ShootOff, data),
                    Meters30 = this.GetInt(roundData.Indexes, ConverterConstants.M30, data),
                    Meters50 = this.GetInt(roundData.Indexes, ConverterConstants.M50, data),
                    Meters60 = this.GetInt(roundData.Indexes, ConverterConstants.M60, data),
                    Meters70 = this.GetInt(roundData.Indexes, ConverterConstants.M70, data),
                    Meters90 = this.GetInt(roundData.Indexes, ConverterConstants.M90, data),
                    Part1 = this.GetInt(roundData.Indexes, ConverterConstants.Part1, data),
                    Part2 = this.GetInt(roundData.Indexes, ConverterConstants.Part2, data),
                    Yards30 = this.GetInt(roundData.Indexes, ConverterConstants.Y30, data),
                    Yards40 = this.GetInt(roundData.Indexes, ConverterConstants.Y40, data),
                    Yards50 = this.GetInt(roundData.Indexes, ConverterConstants.Y50, data),
                    Yards60 = this.GetInt(roundData.Indexes, ConverterConstants.Y60, data),
                    Yards80 = this.GetInt(roundData.Indexes, ConverterConstants.Y80, data),
                    Yards100 = this.GetInt(roundData.Indexes, ConverterConstants.Y100, data),
                };

                round.Athletes.Add(athlete);
            }
            else
            {
                var athlete = round.Athletes.FirstOrDefault(x => x.Code == athleteModel.Code);
                this.SetDocumentData(athlete, info, roundData, data);
            }
        }
    }

    private void SetDocumentData(Archery archery, string info, RoundDataModel roundData, List<HtmlAgilityPack.HtmlNode> data)
    {
        if (info == "30 m")
        {
            archery.Meters30 = this.GetInt(roundData.Indexes, ConverterConstants.Points, data);
        }
        else if (info == "50 m")
        {
            archery.Meters50 = this.GetInt(roundData.Indexes, ConverterConstants.Points, data);
        }
        else if (info == "60 m")
        {
            archery.Meters60 = this.GetInt(roundData.Indexes, ConverterConstants.Points, data);
        }
        else if (info == "70 m")
        {
            archery.Meters70 = this.GetInt(roundData.Indexes, ConverterConstants.Points, data);
        }
        else if (info == "90 m")
        {
            archery.Meters90 = this.GetInt(roundData.Indexes, ConverterConstants.Points, data);
        }
        else if (info is "Part #1" or "Ranking Round, Part #1")
        {
            archery.Part1 = this.GetInt(roundData.Indexes, ConverterConstants.Points, data);
        }
        else if (info is "Part #2" or "Ranking Round, Part #2")
        {
            archery.Part2 = this.GetInt(roundData.Indexes, ConverterConstants.Points, data);
        }
    }

    private async Task SetAthleteMatchesAsync(Round<Archery> round, RoundDataModel roundData, Options options)
    {
        foreach (var row in roundData.Rows.Skip(1))
        {
            var data = row.Elements("td").ToList();
            var matchInputModel = new MatchInputModel
            {
                Row = row.OuterHtml,
                Number = data[0].OuterHtml,
                Date = data[1].InnerText,
                Year = options.Game.Year,
                EventId = options.Event.Id,
                IsTeam = false,
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
            var match = this.Mapper.Map<AthleteMatch<Archery>>(matchModel);

            var document = options.Documents.FirstOrDefault(x => x.Id == match.ResultId);
            if (document != null)
            {
                var firstTable = document.Rounds.FirstOrDefault();
                if (firstTable != null)
                {
                    for (var i = 1; i < firstTable.Rows.Count; i++)
                    {
                        var firstTableData = firstTable.Rows[i].Elements("td").ToList();
                        var record = this.OlympediaService.FindRecord(firstTable.Rows[i].OuterHtml);
                        var sets = this.GetInt(firstTable.Indexes, ConverterConstants.Sets, firstTableData);
                        var set1 = this.GetInt(firstTable.Indexes, ConverterConstants.Set1, firstTableData);
                        var set2 = this.GetInt(firstTable.Indexes, ConverterConstants.Set2, firstTableData);
                        var set3 = this.GetInt(firstTable.Indexes, ConverterConstants.Set3, firstTableData);
                        var set4 = this.GetInt(firstTable.Indexes, ConverterConstants.Set4, firstTableData);
                        var set5 = this.GetInt(firstTable.Indexes, ConverterConstants.Set5, firstTableData);

                        if (i == 1)
                        {
                            match.Athlete1.Record = record;
                            match.Athlete1.Sets = sets;
                            match.Athlete1.Set1 = set1;
                            match.Athlete1.Set2 = set2;
                            match.Athlete1.Set3 = set3;
                            match.Athlete1.Set4 = set4;
                            match.Athlete1.Set5 = set5;
                        }
                        else
                        {
                            match.Athlete2.Record = record;
                            match.Athlete2.Sets = sets;
                            match.Athlete2.Set1 = set1;
                            match.Athlete2.Set2 = set2;
                            match.Athlete2.Set3 = set3;
                            match.Athlete2.Set4 = set4;
                            match.Athlete2.Set5 = set5;
                        }
                    }
                }

                var secondTable = document.Rounds.LastOrDefault();
                if (secondTable != null)
                {
                    foreach (var secondTableRows in secondTable.Rows.Skip(1))
                    {
                        var header = secondTableRows.Element("th")?.InnerText;
                        var secondTableData = secondTableRows.Elements("td").ToList();

                        if (header != null)
                        {
                            if (header.StartsWith("Arrow"))
                            {
                                var arrowNumber = int.Parse(header.Replace("Arrow", string.Empty).Trim());
                                var points1 = secondTableData[0]?.InnerText.Replace("–", string.Empty);
                                var points2 = secondTableData[1]?.InnerText.Replace("–", string.Empty);
                                if (!string.IsNullOrEmpty(points1) && !string.IsNullOrEmpty(points2))
                                {
                                    match.Athlete1.Arrows.Add(new Arrow { Number = arrowNumber, Points = !string.IsNullOrEmpty(points1) ? points1 : null });
                                    match.Athlete2.Arrows.Add(new Arrow { Number = arrowNumber, Points = !string.IsNullOrEmpty(points2) ? points2 : null });
                                }
                            }
                            else
                            {
                                switch (header.Trim())
                                {
                                    case "Points":
                                        match.Athlete1.Points = this.RegExpService.MatchInt(secondTableData[0]?.InnerText);
                                        match.Athlete2.Points = this.RegExpService.MatchInt(secondTableData[1]?.InnerText);
                                        break;
                                    case "10s":
                                        match.Athlete1.Score10s = this.RegExpService.MatchInt(secondTableData[0]?.InnerText);
                                        match.Athlete2.Score10s = this.RegExpService.MatchInt(secondTableData[1]?.InnerText);
                                        break;
                                    case "Xs":
                                        match.Athlete1.ScoreXs = this.RegExpService.MatchInt(secondTableData[0]?.InnerText);
                                        match.Athlete2.ScoreXs = this.RegExpService.MatchInt(secondTableData[1]?.InnerText);
                                        break;
                                    case "Tie-Break":
                                    case "Tiebreak 1":
                                        match.Athlete1.Tiebreak1 = this.RegExpService.MatchInt(secondTableData[0]?.InnerText);
                                        match.Athlete2.Tiebreak1 = this.RegExpService.MatchInt(secondTableData[1]?.InnerText);
                                        break;
                                    case "Tiebreak 2":
                                        match.Athlete1.Tiebreak2 = this.RegExpService.MatchInt(secondTableData[0]?.InnerText);
                                        match.Athlete2.Tiebreak2 = this.RegExpService.MatchInt(secondTableData[1]?.InnerText);
                                        break;
                                    case "Total Points":
                                        match.Athlete1.Points = this.RegExpService.MatchInt(secondTableData[0]?.InnerText);
                                        match.Athlete2.Points = this.RegExpService.MatchInt(secondTableData[1]?.InnerText);
                                        break;
                                    case "Shoot-off":
                                    case "Shoot-Off Points":
                                        match.Athlete1.ShootOff = this.RegExpService.MatchInt(secondTableData[0]?.InnerText);
                                        match.Athlete2.ShootOff = this.RegExpService.MatchInt(secondTableData[1]?.InnerText);
                                        break;
                                }
                            }
                        }
                    }
                }
            }

            round.AthleteMatches.Add(match);
        }
    }

    private async Task SetTeamsAsync(Round<Archery> round, RoundDataModel roundData, Guid eventId, string info)
    {
        Archery team = null;
        foreach (var row in roundData.Rows.Skip(1))
        {
            var noc = this.OlympediaService.FindNOCCode(row.OuterHtml);
            var data = row.Elements("td").ToList();

            if (noc != null)
            {
                var teamName = data[roundData.Indexes[ConverterConstants.Name]].InnerText;
                var nocCache = this.DataCacheService.NOCs.FirstOrDefault(x => x.Code == noc);
                if (string.IsNullOrEmpty(info))
                {
                    var dbTeam = await this.TeamRepository.GetAsync(x => x.Name == teamName && x.NOCId == nocCache.Id && x.EventId == eventId);
                    dbTeam ??= await this.TeamRepository.GetAsync(x => x.NOCId == nocCache.Id && x.EventId == eventId);

                    team = new Archery
                    {
                        Id = dbTeam.Id,
                        Name = dbTeam.Name,
                        NOC = noc,
                        FinishStatus = this.OlympediaService.FindFinishStatus(row.OuterHtml),
                        Record = this.OlympediaService.FindRecord(row.OuterHtml),
                        Points = this.GetInt(roundData.Indexes, ConverterConstants.Points, data),
                        IsQualified = this.OlympediaService.IsQualified(row.OuterHtml),
                        TargetsHit = this.GetInt(roundData.Indexes, ConverterConstants.TargetsHit, data),
                        Score10s = this.GetInt(roundData.Indexes, ConverterConstants.S10, data),
                        Score9s = this.GetInt(roundData.Indexes, ConverterConstants.S9, data),
                        ScoreXs = this.GetInt(roundData.Indexes, ConverterConstants.Xs, data),
                        ShootOff = this.GetInt(roundData.Indexes, ConverterConstants.ShootOff, data),
                        Meters30 = this.GetInt(roundData.Indexes, ConverterConstants.M30, data),
                        Meters50 = this.GetInt(roundData.Indexes, ConverterConstants.M50, data),
                        Meters70 = this.GetInt(roundData.Indexes, ConverterConstants.M70, data),
                        Meters90 = this.GetInt(roundData.Indexes, ConverterConstants.M90, data),
                    };

                    team.Points ??= this.GetInt(roundData.Indexes, ConverterConstants.TeamPoints, data);

                    round.Teams.Add(team);
                }
                else
                {
                    team = round.Teams.FirstOrDefault(x => x.NOC == nocCache.Code);
                    this.SetDocumentData(team, info, roundData, data);
                }
            }
            else
            {
                var athleteModel = this.OlympediaService.FindAthlete(row.OuterHtml);
                if (string.IsNullOrEmpty(info))
                {
                    if (athleteModel != null)
                    {
                        var participant = await this.ParticipationRepository.GetAsync(x => x.Code == athleteModel.Code && x.EventId == eventId);
                        var athlete = new Archery
                        {
                            Id = participant.Id,
                            Code = athleteModel.Code,
                            Name = athleteModel.Name,
                            NOC = team.NOC,
                            FinishStatus = this.OlympediaService.FindFinishStatus(row.OuterHtml),
                            Record = this.OlympediaService.FindRecord(row.OuterHtml),
                            Points = this.GetInt(roundData.Indexes, ConverterConstants.Points, data),
                            IsQualified = this.OlympediaService.IsQualified(row.OuterHtml),
                            TargetsHit = this.GetInt(roundData.Indexes, ConverterConstants.TargetsHit, data),
                            Score10s = this.GetInt(roundData.Indexes, ConverterConstants.S10, data),
                            Score9s = this.GetInt(roundData.Indexes, ConverterConstants.S9, data),
                            ScoreXs = this.GetInt(roundData.Indexes, ConverterConstants.Xs, data),
                            ShootOff = this.GetInt(roundData.Indexes, ConverterConstants.ShootOff, data),
                            Meters30 = this.GetInt(roundData.Indexes, ConverterConstants.M30, data),
                            Meters50 = this.GetInt(roundData.Indexes, ConverterConstants.M50, data),
                            Meters70 = this.GetInt(roundData.Indexes, ConverterConstants.M70, data),
                            Meters90 = this.GetInt(roundData.Indexes, ConverterConstants.M90, data),
                        };

                        athlete.Points ??= this.GetInt(roundData.Indexes, ConverterConstants.IndividualPoints, data);
                        athlete.Meters90 ??= this.GetInt(roundData.Indexes, ConverterConstants.M60, data);

                        team.Athletes.Add(athlete);
                    }
                }
                else
                {
                    var athlete = round.Teams.FirstOrDefault(x => x.NOC == team.NOC).Athletes.FirstOrDefault(x => x.Code == athleteModel.Code);
                    this.SetDocumentData(athlete, info, roundData, data);
                }
            }
        }
    }

    private async Task SetTeamMatchesAsync(Round<Archery> round, RoundDataModel roundData, Options options)
    {
        foreach (var row in roundData.Rows.Skip(1))
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
            var match = this.Mapper.Map<TeamMatch<Archery>>(matchModel);

            var document = options.Documents.FirstOrDefault(x => x.Id == match.ResultId);
            if (document != null)
            {
                var firstTable = document.Rounds.FirstOrDefault();
                if (firstTable != null)
                {
                    for (var i = 1; i < firstTable.Rows.Count; i++)
                    {
                        var firstTableData = firstTable.Rows[i].Elements("td").ToList();
                        var record = this.OlympediaService.FindRecord(firstTable.Rows[i].OuterHtml);
                        var sets = this.GetInt(firstTable.Indexes, ConverterConstants.Sets, firstTableData);
                        var set1 = this.GetInt(firstTable.Indexes, ConverterConstants.Set1, firstTableData);
                        var set2 = this.GetInt(firstTable.Indexes, ConverterConstants.Set2, firstTableData);
                        var set3 = this.GetInt(firstTable.Indexes, ConverterConstants.Set3, firstTableData);
                        var set4 = this.GetInt(firstTable.Indexes, ConverterConstants.Set4, firstTableData);
                        var set5 = this.GetInt(firstTable.Indexes, ConverterConstants.Set5, firstTableData);

                        if (i == 1)
                        {
                            match.Team1.Record = record;
                            match.Team1.Sets = sets;
                            match.Team1.Set1 = set1;
                            match.Team1.Set2 = set2;
                            match.Team1.Set3 = set3;
                            match.Team1.Set4 = set4;
                            match.Team1.Set5 = set5;
                        }
                        else
                        {
                            match.Team2.Record = record;
                            match.Team2.Sets = sets;
                            match.Team2.Set1 = set1;
                            match.Team2.Set2 = set2;
                            match.Team2.Set3 = set3;
                            match.Team2.Set4 = set4;
                            match.Team2.Set5 = set5;
                        }
                    }
                }

                var secondTable = document.Rounds.ElementAtOrDefault(1);
                if (secondTable != null)
                {
                    foreach (var secondTableRow in secondTable.Rows.Skip(1))
                    {
                        var secondTableData = secondTableRow.Elements("td").ToList();
                        var athleteModel = this.OlympediaService.FindAthlete(secondTableRow.OuterHtml);
                        if (athleteModel != null)
                        {
                            var participant = await this.ParticipationRepository.GetAsync(x => x.Code == athleteModel.Code && x.EventId == options.Event.Id);
                            var athlete = new Archery
                            {
                                Id = participant.Id,
                                Code = athleteModel.Code,
                                Name = athleteModel.Name,
                                NOC = match.Team1.NOC,
                                Target = this.GetString(secondTable.Indexes, ConverterConstants.Target, secondTableData),
                                ScoreXs = this.GetInt(secondTable.Indexes, ConverterConstants.Xs, secondTableData),
                                Score10s = this.GetInt(secondTable.Indexes, ConverterConstants.S10, secondTableData),
                                Points = this.GetInt(secondTable.Indexes, ConverterConstants.Points, secondTableData),
                                Tiebreak1 = this.GetInt(secondTable.Indexes, ConverterConstants.TieBreak1, secondTableData),
                                Tiebreak2 = this.GetInt(secondTable.Indexes, ConverterConstants.TieBreak2, secondTableData),
                                ShootOff = this.GetInt(secondTable.Indexes, ConverterConstants.ShootOff, secondTableData),
                            };

                            athlete.Points ??= this.GetInt(secondTable.Indexes, ConverterConstants.TotalPoints, secondTableData);
                            athlete.ShootOff ??= this.GetInt(secondTable.Indexes, ConverterConstants.ShootOffArrow, secondTableData);
                            athlete.Tiebreak1 ??= this.GetInt(secondTable.Indexes, ConverterConstants.TieBreak, secondTableData);

                            foreach (var kvp in secondTable.Indexes.Where(x => x.Key.StartsWith("Arrow")))
                            {
                                var arrowNumber = int.Parse(kvp.Key.Replace("Arrow", string.Empty).Trim());
                                var points = this.GetString(secondTable.Indexes, $"Arrow{arrowNumber}", secondTableData);
                                if (!string.IsNullOrEmpty(points))
                                {
                                    athlete.Arrows.Add(new Arrow { Number = arrowNumber, Points = !string.IsNullOrEmpty(points) ? points : null });
                                }
                            }

                            match.Team1.Athletes.Add(athlete);
                        }
                    }
                }

                var thirdTable = document.Rounds.ElementAtOrDefault(2);
                if (thirdTable != null)
                {
                    foreach (var thirdTableRow in thirdTable.Rows.Skip(1))
                    {
                        var thirdTableData = thirdTableRow.Elements("td").ToList();
                        var athleteModel = this.OlympediaService.FindAthlete(thirdTableRow.OuterHtml);
                        if (athleteModel != null)
                        {
                            var participant = await this.ParticipationRepository.GetAsync(x => x.Code == athleteModel.Code && x.EventId == options.Event.Id);
                            var athlete = new Archery
                            {
                                Id = participant.Id,
                                Code = athleteModel.Code,
                                Name = athleteModel.Name,
                                NOC = match.Team2.NOC,
                                Target = this.GetString(secondTable.Indexes, ConverterConstants.Target, thirdTableData),
                                ScoreXs = this.GetInt(secondTable.Indexes, ConverterConstants.Xs, thirdTableData),
                                Score10s = this.GetInt(secondTable.Indexes, ConverterConstants.S10, thirdTableData),
                                Points = this.GetInt(secondTable.Indexes, ConverterConstants.Points, thirdTableData),
                                Tiebreak1 = this.GetInt(secondTable.Indexes, ConverterConstants.TieBreak1, thirdTableData),
                                Tiebreak2 = this.GetInt(secondTable.Indexes, ConverterConstants.TieBreak2, thirdTableData),
                                ShootOff = this.GetInt(secondTable.Indexes, ConverterConstants.ShootOff, thirdTableData),
                            };

                            athlete.Points ??= this.GetInt(secondTable.Indexes, ConverterConstants.TotalPoints, thirdTableData);
                            athlete.ShootOff ??= this.GetInt(secondTable.Indexes, ConverterConstants.ShootOffArrow, thirdTableData);
                            athlete.Tiebreak1 ??= this.GetInt(secondTable.Indexes, ConverterConstants.TieBreak, thirdTableData);

                            foreach (var kvp in secondTable.Indexes.Where(x => x.Key.StartsWith("Arrow")))
                            {
                                var arrowNumber = int.Parse(kvp.Key.Replace("Arrow", string.Empty).Trim());
                                var points = this.GetString(secondTable.Indexes, $"Arrow{arrowNumber}", thirdTableData);
                                if (!string.IsNullOrEmpty(points))
                                {
                                    athlete.Arrows.Add(new Arrow { Number = arrowNumber, Points = !string.IsNullOrEmpty(points) ? points : null });
                                }
                            }

                            match.Team2.Athletes.Add(athlete);
                        }
                    }
                }
            }

            round.TeamMatches.Add(match);
        }
    }
}