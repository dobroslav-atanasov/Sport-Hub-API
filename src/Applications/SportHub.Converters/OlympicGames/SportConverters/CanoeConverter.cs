namespace SportHub.Converters.OlympicGames.SportConverters;

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

public class CanoeConverter : BaseSportConverter
{
    public CanoeConverter(IOlympediaService olympediaService, IDateService dateService, IDataCacheService dataCacheService, IRegExpService regExpService, IMapper mapper,
        INormalizeService normalizeService, OlympicGamesRepository<Team> teamRepository, OlympicGamesRepository<Participation> participationRepository,
        OlympicGamesRepository<Athlete> athleteRepository, OlympicGamesRepository<Result> resultRepository)
        : base(olympediaService, dateService, dataCacheService, regExpService, mapper, normalizeService, teamRepository, participationRepository, athleteRepository, resultRepository)
    {
    }

    public override async Task ProcessAsync(Options options)
    {
        switch (options.Discipline.Name)
        {
            case DisciplineConstants.CANOE_SLALOM:
                await this.ProcessCanoeSlalomAsync(options);
                break;
            case DisciplineConstants.CANOE_SPRINT:
                await this.ProcessCanoeSprintAsync(options);
                break;
        }
    }

    private async Task ProcessCanoeSprintAsync(Options options)
    {
        var rounds = new List<Round<CanoeSprint>>();

        var allRounds = options.Rounds;
        if (options.Rounds.Count != 1)
        {
            allRounds = options.Rounds.Skip(1).ToList();
        }

        foreach (var roundData in allRounds)
        {
            var round = this.CreateRound<CanoeSprint>(roundData, options.Event.Name, null);
            await this.SetCanoeSprintAthletesAsync(round, roundData, options);
            rounds.Add(round);
        }

        await this.ProcessJsonAsync(rounds, options);
    }

    private async Task SetCanoeSprintAthletesAsync(Round<CanoeSprint> round, RoundDataModel roundData, Options options)
    {
        foreach (var row in roundData.Rows.Skip(1))
        {
            var noc = this.OlympediaService.FindNOCCode(row.OuterHtml);
            var data = row.Elements("td").ToList();

            CanoeSprint canoeSprint = null;
            if (options.Event.IsTeamEvent)
            {
                var specialCase = false;
                var athleteModels = this.OlympediaService.FindAthletes(row.OuterHtml);
                if (noc == null && athleteModels.Count > 1)
                {
                    continue;
                }

                if (noc == null && athleteModels.Count == 1)
                {
                    specialCase = true;
                }

                if (athleteModels.Count == 0)
                {
                    athleteModels = this.OlympediaService.FindAthletes(row.OuterHtml);
                }

                if (specialCase)
                {
                    var lastTeam = round.Teams.Last();
                    var athleteModel = athleteModels.FirstOrDefault();
                    var participant = await this.ParticipationRepository.GetAsync(x => x.Code == athleteModel.Code && x.EventId == options.Event.Id);
                    lastTeam.Athletes.Add(new CanoeSprint
                    {
                        Id = participant.Id,
                        Name = athleteModel.Name,
                        NOC = noc,
                        Code = athleteModel.Code,
                        Time = this.GetTime(roundData.Indexes, ConverterConstants.Time, data)
                    });
                    continue;
                }

                var nocCache = this.DataCacheService.NationalOlympicCommittees.FirstOrDefault(x => x.Code == noc);
                var teamName = this.GetString(roundData.Indexes, ConverterConstants.Name, data);
                var dbTeam = await this.TeamRepository.GetAsync(x => x.NationalOlympicCommitteeId == nocCache.Id && x.EventId == options.Event.Id);

                canoeSprint = new CanoeSprint
                {
                    Id = dbTeam.Id,
                    Name = dbTeam.Name,
                };

                foreach (var athleteModel in athleteModels)
                {
                    var participant = await this.ParticipationRepository.GetAsync(x => x.Code == athleteModel.Code && x.EventId == options.Event.Id);
                    canoeSprint.Athletes.Add(new CanoeSprint
                    {
                        Id = participant.Id,
                        Name = athleteModel.Name,
                        NOC = noc,
                        Code = athleteModel.Code,
                    });
                }

                round.Teams.Add(canoeSprint);
            }
            else
            {
                var athleteModel = this.OlympediaService.FindAthlete(data[roundData.Indexes[ConverterConstants.Name]].OuterHtml);
                if (athleteModel != null)
                {
                    var participant = await this.ParticipationRepository.GetAsync(x => x.Code == athleteModel.Code && x.EventId == options.Event.Id);

                    canoeSprint = new CanoeSprint
                    {
                        Id = participant.Id,
                        Name = athleteModel.Name,
                        Code = athleteModel.Code,
                    };

                    round.Athletes.Add(canoeSprint);
                }
            }

            if (canoeSprint != null)
            {
                canoeSprint.NOC = noc;
                canoeSprint.FinishStatus = this.OlympediaService.FindFinishStatus(row.OuterHtml);
                canoeSprint.IsQualified = this.OlympediaService.IsQualified(row.OuterHtml);
                canoeSprint.Number = this.GetInt(roundData.Indexes, ConverterConstants.Number, data);
                canoeSprint.Lane = this.GetInt(roundData.Indexes, ConverterConstants.Lane, data);
                canoeSprint.Time = this.GetTime(roundData.Indexes, ConverterConstants.Time, data);
                canoeSprint.Time ??= this.DateService.ParseTimeFromSeconds($"{this.GetDouble(roundData.Indexes, ConverterConstants.Time, data)}");

                if (roundData.Indexes.ContainsKey(ConverterConstants.M250))
                {
                    canoeSprint.Intermediates.Add(new() { Meters = 250, Time = this.DateService.ParseTimeFromSeconds($"{this.GetDouble(roundData.Indexes, ConverterConstants.M250, data)}") });
                }

                if (roundData.Indexes.ContainsKey(ConverterConstants.M500))
                {
                    canoeSprint.Intermediates.Add(new() { Meters = 500, Time = this.GetTime(roundData.Indexes, ConverterConstants.M500, data) });
                }

                if (roundData.Indexes.ContainsKey(ConverterConstants.M750))
                {
                    canoeSprint.Intermediates.Add(new() { Meters = 750, Time = this.GetTime(roundData.Indexes, ConverterConstants.M750, data) });
                }
            }
        }
    }

    private async Task ProcessCanoeSlalomAsync(Options options)
    {
        var rounds = new List<Round<CanoeSlalom>>();

        var allRounds = options.Rounds;
        if (options.Rounds.Count != 1)
        {
            allRounds = options.Rounds.Skip(1).ToList();
        }

        foreach (var roundData in allRounds)
        {
            var track = this.ExtractTrack(roundData.Html);
            var round = this.CreateRound<CanoeSlalom>(roundData, options.Event.Name, track);

            await this.SetCanoeSlalomAthletesAsync(round, roundData, options);
            var documentNumbers = this.OlympediaService.FindResults(roundData.Html);
            if (allRounds.Count == 1)
            {
                documentNumbers = options.Documents.Select(x => x.Id).ToList();
            }

            foreach (var number in documentNumbers)
            {
                var document = options.Documents.FirstOrDefault(x => x.Id == number);
                this.SetCanoeSlalomDocumentsAsync(round, document.Rounds.FirstOrDefault(), options, document.Title);
            }

            rounds.Add(round);
        }

        await this.ProcessJsonAsync(rounds, options);
    }

    private void SetCanoeSlalomDocumentsAsync(Round<CanoeSlalom> round, RoundDataModel roundData, Options options, string title)
    {
        foreach (var row in roundData.Rows.Skip(1))
        {
            var noc = this.OlympediaService.FindNOCCode(row.OuterHtml);
            var data = row.Elements("td").ToList();

            CanoeSlalom canoeSlalom = null;
            if (options.Event.IsTeamEvent)
            {
                var teamName = this.GetString(roundData.Indexes, ConverterConstants.Name, data);
                canoeSlalom = round.Teams.FirstOrDefault(x => x.Name == teamName && x.NOC == noc);
            }
            else
            {
                var athleteModel = this.OlympediaService.FindAthlete(data[roundData.Indexes[ConverterConstants.Name]].OuterHtml);
                if (athleteModel != null)
                {
                    canoeSlalom = round.Athletes.FirstOrDefault(x => x.Code == athleteModel.Code);
                }
            }

            if (canoeSlalom != null)
            {
                switch (title)
                {
                    case "Run #1":
                        canoeSlalom.Run1 ??= this.GetTime(roundData.Indexes, ConverterConstants.Time, data);
                        canoeSlalom.Run1 ??= this.DateService.ParseTimeFromSeconds($"{this.GetDouble(roundData.Indexes, ConverterConstants.Time, data)}");
                        canoeSlalom.Run1TotalTime = this.GetTime(roundData.Indexes, ConverterConstants.TotalTime, data);
                        canoeSlalom.Run1TotalTime ??= this.DateService.ParseTimeFromSeconds($"{this.GetString(roundData.Indexes, ConverterConstants.TotalTime, data)}");
                        canoeSlalom.Run1PenaltySeconds = this.GetInt(roundData.Indexes, ConverterConstants.PenaltyTime, data);
                        break;
                    case "Run #2":
                        canoeSlalom.Run2 ??= this.GetTime(roundData.Indexes, ConverterConstants.Time, data);
                        canoeSlalom.Run2 ??= this.DateService.ParseTimeFromSeconds($"{this.GetDouble(roundData.Indexes, ConverterConstants.Time, data)}");
                        canoeSlalom.Run2TotalTime = this.GetTime(roundData.Indexes, ConverterConstants.TotalTime, data);
                        canoeSlalom.Run2TotalTime ??= this.DateService.ParseTimeFromSeconds($"{this.GetString(roundData.Indexes, ConverterConstants.TotalTime, data)}");
                        canoeSlalom.Run2PenaltySeconds = this.GetInt(roundData.Indexes, ConverterConstants.PenaltyTime, data);
                        break;
                }
            }
        }
    }

    private async Task SetCanoeSlalomAthletesAsync(Round<CanoeSlalom> round, RoundDataModel roundData, Options options)
    {
        foreach (var row in roundData.Rows.Skip(1))
        {
            var noc = this.OlympediaService.FindNOCCode(row.OuterHtml);
            var data = row.Elements("td").ToList();

            CanoeSlalom canoeSlalom = null;
            if (options.Event.IsTeamEvent)
            {
                var nocCache = this.DataCacheService.NationalOlympicCommittees.FirstOrDefault(x => x.Code == noc);
                var teamName = this.GetString(roundData.Indexes, ConverterConstants.Name, data);
                var dbTeam = await this.TeamRepository.GetAsync(x => x.Name == teamName && x.NationalOlympicCommitteeId == nocCache.Id && x.EventId == options.Event.Id);

                canoeSlalom = new CanoeSlalom
                {
                    Id = dbTeam.Id,
                    Name = dbTeam.Name,
                };

                var athleteModels = this.OlympediaService.FindAthletes(row.OuterHtml);
                foreach (var athleteModel in athleteModels)
                {
                    var participant = await this.ParticipationRepository.GetAsync(x => x.Code == athleteModel.Code && x.EventId == options.Event.Id);
                    canoeSlalom.Athletes.Add(new CanoeSlalom
                    {
                        Id = participant.Id,
                        Name = athleteModel.Name,
                        NOC = noc,
                        Code = athleteModel.Code,
                    });
                }

                round.Teams.Add(canoeSlalom);
            }
            else
            {
                var athleteModel = this.OlympediaService.FindAthlete(data[roundData.Indexes[ConverterConstants.Name]].OuterHtml);
                if (athleteModel != null)
                {
                    var participant = await this.ParticipationRepository.GetAsync(x => x.Code == athleteModel.Code && x.EventId == options.Event.Id);

                    canoeSlalom = new CanoeSlalom
                    {
                        Id = participant.Id,
                        Name = athleteModel.Name,
                        Code = athleteModel.Code,
                    };

                    round.Athletes.Add(canoeSlalom);
                }
            }

            if (canoeSlalom != null)
            {
                canoeSlalom.NOC = noc;
                canoeSlalom.FinishStatus = this.OlympediaService.FindFinishStatus(row.OuterHtml);
                canoeSlalom.IsQualified = this.OlympediaService.IsQualified(row.OuterHtml);
                canoeSlalom.Number = this.GetInt(roundData.Indexes, ConverterConstants.Number, data);
                canoeSlalom.Order = this.GetInt(roundData.Indexes, ConverterConstants.Order, data);
                canoeSlalom.Time = this.GetTime(roundData.Indexes, ConverterConstants.Time, data);
                canoeSlalom.Time ??= this.DateService.ParseTimeFromSeconds($"{this.GetDouble(roundData.Indexes, ConverterConstants.Time, data)}");
                canoeSlalom.TotalTime = this.GetTime(roundData.Indexes, ConverterConstants.TotalTime, data);
                canoeSlalom.TotalTime ??= this.DateService.ParseTimeFromSeconds($"{this.GetString(roundData.Indexes, ConverterConstants.TotalTime, data)}");
                canoeSlalom.PenaltySeconds = this.GetInt(roundData.Indexes, ConverterConstants.PenaltyTime, data);
                canoeSlalom.Run1 = this.GetTime(roundData.Indexes, ConverterConstants.Run1, data);
                canoeSlalom.Run1 ??= this.DateService.ParseTimeFromSeconds($"{this.GetString(roundData.Indexes, ConverterConstants.Run1, data)}");
                canoeSlalom.Run2 = this.GetTime(roundData.Indexes, ConverterConstants.Run2, data);
                canoeSlalom.Run2 ??= this.DateService.ParseTimeFromSeconds($"{this.GetString(roundData.Indexes, ConverterConstants.Run2, data)}");
            }
        }
    }

    private Track ExtractTrack(string html)
    {
        var total = this.RegExpService.MatchFirstGroup(html, @"<th>Total gates</th>\s*<td>(.*?)</td>");
        var downstream = this.RegExpService.MatchFirstGroup(html, @"<th>Downstream gates</th>\s*<td>(.*?)</td>");
        var upstream = this.RegExpService.MatchFirstGroup(html, @"<th>Upstream gates</th>\s*<td>(.*?)</td>");

        return new Track
        {
            Turns = this.RegExpService.MatchInt(total),
            Downstream = this.RegExpService.MatchInt(downstream),
            Upstream = this.RegExpService.MatchInt(upstream),
        };
    }
}

//#region CANOE
//private async Task ProcessCanoeSprintAsync(ConvertOptions options)
//{
//    var eventRound = this.CreateEventRound<CSPRound>(options.HtmlDocument, options.Event.Name);

//    if (options.Tables.Any())
//    {
//        foreach (var table in options.Tables)
//        {
//            var dateString = this.RegExpService.MatchFirstGroup(table.HtmlDocument.DocumentNode.OuterHtml, @"<th>\s*Date\s*<\/th>\s*<td>(.*?)<\/td>");
//            var dateModel = this.dateService.ParseDate(dateString);
//            var format = this.RegExpService.MatchFirstGroup(table.HtmlDocument.DocumentNode.OuterHtml, @"<th>Format<\/th>\s*<td(?:.*?)>(.*?)<\/td>");

//            var round = this.CreateRound<CSPRound>(dateModel.From, format, table.Round, eventRound.EventName);
//            var heats = this.SplitHeats(table);

//            if (heats.Any())
//            {
//                foreach (var heat in heats)
//                {
//                    var heatType = this.NormalizeService.MapHeats(heat.Title);
//                    if (options.Event.IsTeamEvent)
//                    {
//                        await this.SetCSPTeamsAsync(round, heat.HtmlDocument, options.Event, heatType);
//                    }
//                    else
//                    {
//                        await this.SetCSPCanoiestsAsync(round, heat.HtmlDocument, options.Event, heatType);
//                    }
//                }
//            }
//            else
//            {
//                if (options.Event.IsTeamEvent)
//                {
//                    await this.SetCSPTeamsAsync(round, table.HtmlDocument, options.Event, HeatType.None);
//                }
//                else
//                {
//                    await this.SetCSPCanoiestsAsync(round, table.HtmlDocument, options.Event, HeatType.None);
//                }
//            }

//            eventRound.Rounds.Add(round);
//        }
//    }
//    else
//    {
//        var round = this.CreateRound<CSPRound>(eventRound.Dates.From, eventRound.Format, RoundType.FinalRound, eventRound.EventName);
//        if (options.Event.IsTeamEvent)
//        {
//            await this.SetCSPTeamsAsync(round, options.StandingTable.HtmlDocument, options.Event, HeatType.None);
//        }
//        else
//        {
//            await this.SetCSPCanoiestsAsync(round, options.StandingTable.HtmlDocument, options.Event, HeatType.None);
//        }

//        eventRound.Rounds.Add(round);
//    }

//    var numbers = this.GetCSPCanoeistsNumbers(options.StandingTable, options.Event.IsTeamEvent);

//    eventRound.Rounds.ForEach(r =>
//    {
//        r.Canoeists.ForEach(x =>
//        {
//            if (numbers.ContainsKey(x.Code))
//            {
//                x.Number = numbers[x.Code];
//            }
//        });

//        r.Teams.ForEach(t =>
//        {
//            t.Canoeists.ForEach(c =>
//            {
//                if (numbers.ContainsKey(c.Code))
//                {
//                    c.Number = numbers[c.Code];
//                }
//            });
//        });
//    });

//    await this.ProcessJsonAsync(eventRound, options);
//}

//private Dictionary<int, int?> GetCSPCanoeistsNumbers(TableModel table, bool isTeamEvent)
//{
//    var rows = table.HtmlDocument.DocumentNode.SelectNodes("//table[@class='table table-striped']//tr");
//    var headers = rows.First().Elements("th").Select(x => x.InnerText).ToList();
//    var indexes = this.OlympediaService.FindIndexes(headers);

//    var dicionary = new Dictionary<int, int?>();
//    foreach (var row in rows.Skip(1))
//    {
//        var data = row.Elements("td").ToList();
//        if (isTeamEvent)
//        {
//            var athleteModels = this.OlympediaService.FindAthletes(row.OuterHtml);
//            var numbers = indexes.TryGetValue(ConverterConstants.INDEX_NR, out int value1) ? data[value1].InnerText.Replace("–", string.Empty) : null;
//            if (!string.IsNullOrEmpty(numbers))
//            {
//                var parts = numbers.Split("/").Where(x => int.TryParse(x, out int r)).Select(int.Parse).ToList();
//                for (int i = 0; i < athleteModels.Count; i++)
//                {
//                    if (parts.ElementAtOrDefault(i) != null)
//                    {
//                        dicionary[athleteModels[i].Code] = parts.ElementAtOrDefault(i);
//                    }
//                }
//            }
//        }
//        else
//        {
//            var number = indexes.TryGetValue(ConverterConstants.INDEX_NR, out int value1) ? this.RegExpService.MatchInt(data[value1].InnerText) : null;
//            var athleteModel = this.OlympediaService.FindAthlete(row.OuterHtml);
//            if (number != null)
//            {
//                dicionary[athleteModel.Code] = number;
//            }
//        }
//    }

//    return dicionary;
//}

//private async Task SetCSPTeamsAsync(CSPRound round, HtmlDocument htmlDocument, EventCacheModel eventCache, HeatType heat)
//{
//    var rows = htmlDocument.DocumentNode.SelectNodes("//table[@class='table table-striped']//tr");
//    var headers = rows.First().Elements("th").Select(x => x.InnerText).ToList();
//    var indexes = this.OlympediaService.FindIndexes(headers);

//    CSPTeam cspTeam = null;
//    foreach (var row in rows.Skip(1))
//    {
//        var data = row.Elements("td").ToList();
//        var name = data[indexes[ConverterConstants.INDEX_NAME]].InnerText.Trim();
//        var nocCode = this.OlympediaService.FindNOCCode(row.OuterHtml);
//        if (nocCode != null)
//        {
//            var nocCacheModel = this.DataCacheService.NOCCacheModels.FirstOrDefault(x => x.Code == nocCode);
//            var team = await this.teamsService.GetAsync(nocCacheModel.Id, eventCache.Id);

//            cspTeam = new CSPTeam
//            {
//                Id = team.Id,
//                Name = name,
//                NOC = nocCode,
//                FinishStatus = this.OlympediaService.FindStatus(row.OuterHtml),
//                Qualification = this.OlympediaService.FindQualification(row.OuterHtml),
//                Heat = heat,
//                Lane = indexes.TryGetValue(ConverterConstants.INDEX_LANE, out int value1) ? this.RegExpService.MatchInt(data[value1].InnerText) : null,
//                Time = indexes.TryGetValue(ConverterConstants.INDEX_TIME, out int value2) ? this.dateService.ParseTime(data[value2].InnerText) : null,
//                Split250 = indexes.TryGetValue(ConverterConstants.INDEX_SPLIT_250, out int value4) ? this.dateService.ParseTime(data[value4].InnerText) : null,
//                Split500 = indexes.TryGetValue(ConverterConstants.INDEX_SPLIT_500, out int value5) ? this.dateService.ParseTime(data[value5].InnerText) : null,
//                Split750 = indexes.TryGetValue(ConverterConstants.INDEX_SPLIT_750, out int value6) ? this.dateService.ParseTime(data[value6].InnerText) : null,
//                Split250To500 = indexes.TryGetValue(ConverterConstants.INDEX_SPLIT_250_500, out int value7) ? this.dateService.ParseTime(data[value7].InnerText) : null,
//                Split500To1000 = indexes.TryGetValue(ConverterConstants.INDEX_SPLIT_500_1000, out int value8) ? this.dateService.ParseTime(data[value8].InnerText) : null,
//                Split500To750 = indexes.TryGetValue(ConverterConstants.INDEX_SPLIT_500_750, out int value9) ? this.dateService.ParseTime(data[value9].InnerText) : null,
//                Split750To1000 = indexes.TryGetValue(ConverterConstants.INDEX_SPLIT_750_1000, out int value10) ? this.dateService.ParseTime(data[value10].InnerText) : null,
//            };

//            round.Teams.Add(cspTeam);
//        }
//        else
//        {
//            var athleteModels = this.OlympediaService.FindAthletes(row.OuterHtml);
//            var nocCacheModel = this.DataCacheService.NOCCacheModels.FirstOrDefault(x => x.Code == cspTeam.NOC);

//            foreach (var athleteModel in athleteModels)
//            {
//                var participant = await this.participantsService.GetAsync(athleteModel.Code, eventCache.Id, nocCacheModel.Id);
//                var canoeist = new CSPCanoeist
//                {
//                    Id = participant.Id,
//                    Code = athleteModel.Code,
//                    Name = athleteModel.Name,
//                    NOC = cspTeam.NOC,
//                };

//                cspTeam.Canoeists.Add(canoeist);
//            }
//        }
//    }
//}

//private async Task SetCSPCanoiestsAsync(CSPRound round, HtmlDocument htmlDocument, EventCacheModel eventCache, HeatType heat)
//{
//    var rows = htmlDocument.DocumentNode.SelectNodes("//table[@class='table table-striped']//tr");
//    var headers = rows.First().Elements("th").Select(x => x.InnerText).ToList();
//    var indexes = this.OlympediaService.FindIndexes(headers);

//    foreach (var row in rows.Skip(1))
//    {
//        var data = row.Elements("td").ToList();
//        var nocCode = this.OlympediaService.FindNOCCode(row.OuterHtml);
//        var nocCacheModel = this.DataCacheService.NOCCacheModels.FirstOrDefault(x => x.Code == nocCode);
//        var athleteModel = this.OlympediaService.FindAthlete(row.OuterHtml);
//        var participant = await this.participantsService.GetAsync(athleteModel.Code, eventCache.Id, nocCacheModel.Id);

//        var canoeist = new CSPCanoeist
//        {
//            Id = participant.Id,
//            Code = athleteModel.Code,
//            Name = athleteModel.Name,
//            NOC = nocCode,
//            FinishStatus = this.OlympediaService.FindStatus(data[0].OuterHtml),
//            Qualification = this.OlympediaService.FindQualification(row.OuterHtml),
//            Heat = heat,
//            Lane = indexes.TryGetValue(ConverterConstants.INDEX_LANE, out int value1) ? this.RegExpService.MatchInt(data[value1].InnerText) : null,
//            Time = indexes.TryGetValue(ConverterConstants.INDEX_TIME, out int value2) ? this.dateService.ParseTime(data[value2].InnerText) : null,
//            Exchange = indexes.TryGetValue(ConverterConstants.INDEX_EXCHANGE_TIME, out int value3) ? this.dateService.ParseTime(data[value3].InnerText) : null,
//            Split250 = indexes.TryGetValue(ConverterConstants.INDEX_SPLIT_250, out int value4) ? this.dateService.ParseTime(data[value4].InnerText) : null,
//            Split500 = indexes.TryGetValue(ConverterConstants.INDEX_SPLIT_500, out int value5) ? this.dateService.ParseTime(data[value5].InnerText) : null,
//            Split750 = indexes.TryGetValue(ConverterConstants.INDEX_SPLIT_750, out int value6) ? this.dateService.ParseTime(data[value6].InnerText) : null,
//            Split250To500 = indexes.TryGetValue(ConverterConstants.INDEX_SPLIT_250_500, out int value7) ? this.dateService.ParseTime(data[value7].InnerText) : null,
//            Split500To1000 = indexes.TryGetValue(ConverterConstants.INDEX_SPLIT_500_1000, out int value8) ? this.dateService.ParseTime(data[value8].InnerText) : null,
//            Split500To750 = indexes.TryGetValue(ConverterConstants.INDEX_SPLIT_500_750, out int value9) ? this.dateService.ParseTime(data[value9].InnerText) : null,
//            Split750To1000 = indexes.TryGetValue(ConverterConstants.INDEX_SPLIT_750_1000, out int value10) ? this.dateService.ParseTime(data[value10].InnerText) : null,
//        };

//        round.Canoeists.Add(canoeist);
//    }
//}
