namespace SportHub.Converters.OlympicGames.SportConverters;

using AutoMapper;

using SportHub.Common.Constants;
using SportHub.Data.Models.Converters.OlympicGames;
using SportHub.Data.Models.Converters.OlympicGames.Base;
using SportHub.Data.Models.Converters.OlympicGames.Disciplines;
using SportHub.Data.Models.Entities.OlympicGames;
using SportHub.Data.Repositories;
using SportHub.Services.Data.OlympicGamesDb.Interfaces;
using SportHub.Services.Interfaces;

public class SkiingConverter : BaseSportConverter
{
    public SkiingConverter(IOlympediaService olympediaService, IDateService dateService, IDataCacheService dataCacheService, IRegExpService regExpService, IMapper mapper,
        INormalizeService normalizeService, OlympicGamesRepository<Team> teamRepository, OlympicGamesRepository<Participation> participationRepository,
        OlympicGamesRepository<Athlete> athleteRepository, OlympicGamesRepository<Result> resultRepository)
        : base(olympediaService, dateService, dataCacheService, regExpService, mapper, normalizeService, teamRepository, participationRepository, athleteRepository, resultRepository)
    {
    }

    public override async Task ProcessAsync(Options options)
    {
        switch (options.Discipline.Name)
        {
            case DisciplineConstants.ALPINE_SKIING:
                await this.ProcessAlpineSkiingAsync(options);
                break;
            case DisciplineConstants.CROSS_COUNTRY_SKIING:
                await this.ProcessCrossCountrySkiingAsync(options);
                break;
        }
    }

    private async Task ProcessCrossCountrySkiingAsync(Options options)
    {
        var rounds = new List<Round<CrossCountrySkiing>>();
        await Console.Out.WriteLineAsync($"{options.Game.Year} - {options.Event.Name}");

        var allRounds = options.Rounds;
        if (options.Rounds.Count != 1)
        {
            allRounds = options.Rounds.Skip(1).ToList();
        }

        var track = this.SetCrossCountrySkiingTrack(options.HtmlDocument.ParsedText);
        foreach (var roundData in allRounds)
        {
            var round = this.CreateRound<CrossCountrySkiing>(roundData, options.Event.Name, track);
            await this.SetCrossCountrySkiingAthletesAsync(round, roundData, options);
            rounds.Add(round);
        }

        foreach (var document in options.Documents)
        {
            this.ProcessCrossCountrySkiingDocuments(rounds.FirstOrDefault(), document.Rounds.FirstOrDefault(), options, document.Title);
        }

        await this.ProcessJsonAsync(rounds, options);
    }

    private void ProcessCrossCountrySkiingDocuments(Round<CrossCountrySkiing> round, RoundDataModel roundData, Options options, string title)
    {
        foreach (var row in roundData.Rows.Skip(1))
        {
            var data = row.Elements("td").ToList();
            var athleteModel = this.OlympediaService.FindAthlete(row.OuterHtml);
            var athlete = round.Athletes.FirstOrDefault(x => x.Code == athleteModel.Code);

            if (title.Contains("Freestyle"))
            {
                athlete.Freestyle = this.GetTime(roundData.Indexes, ConverterConstants.Time, data);
                athlete.StartBehind = this.GetTime(roundData.Indexes, ConverterConstants.StartBehind, data);
                athlete.Race = this.GetTime(roundData.Indexes, ConverterConstants.Race, data);
            }
            else if (title.Contains("Classical"))
            {
                athlete.Classical = this.GetTime(roundData.Indexes, ConverterConstants.Time, data);
            }
        }
    }

    private async Task SetCrossCountrySkiingAthletesAsync(Round<CrossCountrySkiing> round, RoundDataModel roundData, Options options)
    {
        foreach (var row in roundData.Rows.Skip(1))
        {
            var noc = this.OlympediaService.FindNOCCode(row.OuterHtml);
            var data = row.Elements("td").ToList();

            CrossCountrySkiing crossCountrySkiing = null;
            if (options.Event.IsTeamEvent)
            {
                var athleteModels = this.OlympediaService.FindAthletes(row.OuterHtml);

                if (athleteModels.Count == 0)
                {
                    var nocCache = this.DataCacheService.NOCs.FirstOrDefault(x => x.Code == noc);
                    var teamName = this.GetString(roundData.Indexes, ConverterConstants.Name, data);
                    var dbTeam = await this.TeamRepository.GetAsync(x => x.NOCId == nocCache.Id && x.EventId == options.Event.Id);

                    crossCountrySkiing = new CrossCountrySkiing
                    {
                        Id = dbTeam.Id,
                        Name = dbTeam.Name,
                    };

                    round.Teams.Add(crossCountrySkiing);

                }
                else
                {
                    var team = round.Teams.Last();
                    var athleteModel = this.OlympediaService.FindAthlete(data[roundData.Indexes[ConverterConstants.Name]].OuterHtml);
                    if (athleteModel != null)
                    {
                        var participant = await this.ParticipationRepository.GetAsync(x => x.Code == athleteModel.Code && x.EventId == options.Event.Id);

                        crossCountrySkiing = new CrossCountrySkiing
                        {
                            Id = participant.Id,
                            Name = athleteModel.Name,
                            Code = athleteModel.Code,
                        };

                        team.Athletes.Add(crossCountrySkiing);
                    }
                }
            }
            else
            {
                var athleteModel = this.OlympediaService.FindAthlete(data[roundData.Indexes[ConverterConstants.Name]].OuterHtml);
                if (athleteModel != null)
                {
                    var participant = await this.ParticipationRepository.GetAsync(x => x.Code == athleteModel.Code && x.EventId == options.Event.Id);

                    crossCountrySkiing = new CrossCountrySkiing
                    {
                        Id = participant.Id,
                        Name = athleteModel.Name,
                        Code = athleteModel.Code,
                    };

                    round.Athletes.Add(crossCountrySkiing);
                }
            }

            if (crossCountrySkiing != null)
            {
                crossCountrySkiing.NOC = noc;
                crossCountrySkiing.FinishStatus = this.OlympediaService.FindStatus(row.OuterHtml);
                crossCountrySkiing.Qualification = this.OlympediaService.FindQualification(row.OuterHtml);
                crossCountrySkiing.Number = this.GetInt(roundData.Indexes, ConverterConstants.Number, data);
                crossCountrySkiing.Time = this.GetTime(roundData.Indexes, ConverterConstants.Time, data);
                crossCountrySkiing.Race = this.GetTime(roundData.Indexes, ConverterConstants.Race, data);
                crossCountrySkiing.StartBehind = this.GetTime(roundData.Indexes, ConverterConstants.StartBehind, data);
                crossCountrySkiing.Classical = this.GetTime(roundData.Indexes, ConverterConstants.Classical, data);
                crossCountrySkiing.Freestyle = this.GetTime(roundData.Indexes, ConverterConstants.Freestyle, data);
                crossCountrySkiing.PitStop = this.DateService.ParseTimeFromSeconds($"{this.GetDouble(roundData.Indexes, ConverterConstants.PitStop, data)}");
                crossCountrySkiing.Leg1 = this.GetTime(roundData.Indexes, ConverterConstants.Leg1, data);
                crossCountrySkiing.Leg2 = this.GetTime(roundData.Indexes, ConverterConstants.Leg2, data);
                crossCountrySkiing.Leg3 = this.GetTime(roundData.Indexes, ConverterConstants.Leg3, data);

                if (roundData.Indexes.ContainsKey(ConverterConstants.Intermediate1))
                {
                    crossCountrySkiing.Intermediates.Add(new() { Number = 1, Time = this.GetTime(roundData.Indexes, ConverterConstants.Intermediate1, data) });
                }

                if (roundData.Indexes.ContainsKey(ConverterConstants.Intermediate2))
                {
                    crossCountrySkiing.Intermediates.Add(new() { Number = 2, Time = this.GetTime(roundData.Indexes, ConverterConstants.Intermediate2, data) });
                }

                if (roundData.Indexes.ContainsKey(ConverterConstants.Intermediate3))
                {
                    crossCountrySkiing.Intermediates.Add(new() { Number = 3, Time = this.GetTime(roundData.Indexes, ConverterConstants.Intermediate3, data) });
                }

                if (roundData.Indexes.ContainsKey(ConverterConstants.Intermediate4))
                {
                    crossCountrySkiing.Intermediates.Add(new() { Number = 4, Time = this.GetTime(roundData.Indexes, ConverterConstants.Intermediate5, data) });
                }

                if (roundData.Indexes.ContainsKey(ConverterConstants.Intermediate5))
                {
                    crossCountrySkiing.Intermediates.Add(new() { Number = 5, Time = this.GetTime(roundData.Indexes, ConverterConstants.Intermediate5, data) });
                }

                if (roundData.Indexes.ContainsKey(ConverterConstants.Intermediate6))
                {
                    crossCountrySkiing.Intermediates.Add(new() { Number = 6, Time = this.GetTime(roundData.Indexes, ConverterConstants.Intermediate6, data) });
                }

                if (roundData.Indexes.ContainsKey(ConverterConstants.Intermediate7))
                {
                    crossCountrySkiing.Intermediates.Add(new() { Number = 7, Time = this.GetTime(roundData.Indexes, ConverterConstants.Intermediate7, data) });
                }

                if (roundData.Indexes.ContainsKey(ConverterConstants.Intermediate8))
                {
                    crossCountrySkiing.Intermediates.Add(new() { Number = 8, Time = this.GetTime(roundData.Indexes, ConverterConstants.Intermediate8, data) });
                }

                if (roundData.Indexes.ContainsKey(ConverterConstants.Intermediate9))
                {
                    crossCountrySkiing.Intermediates.Add(new() { Number = 9, Time = this.GetTime(roundData.Indexes, ConverterConstants.Intermediate9, data) });
                }
            }
        }
    }

    private Track SetCrossCountrySkiingTrack(string html)
    {
        var length = this.RegExpService.MatchFirstGroup(html, @"Course Length:(.*?)<br>");
        var height = this.RegExpService.MatchFirstGroup(html, @"Height Differential:(.*?)<br>");
        var climb = this.RegExpService.MatchFirstGroup(html, @"Maximum Climb:(.*?)<br>");
        var inter1 = this.RegExpService.MatchFirstGroup(html, @"Intermediate 1:(.*?)<br>");
        var inter2 = this.RegExpService.MatchFirstGroup(html, @"Intermediate 2:(.*?)<br>");
        var inter3 = this.RegExpService.MatchFirstGroup(html, @"Intermediate 3:(.*?)<br>");
        var inter4 = this.RegExpService.MatchFirstGroup(html, @"Intermediate 4:(.*?)<br>");
        var inter5 = this.RegExpService.MatchFirstGroup(html, @"Intermediate 5:(.*?)<br>");
        var inter6 = this.RegExpService.MatchFirstGroup(html, @"Intermediate 6:(.*?)<br>");
        var inter7 = this.RegExpService.MatchFirstGroup(html, @"Intermediate 7:(.*?)<br>");
        var inter8 = this.RegExpService.MatchFirstGroup(html, @"Intermediate 8:(.*?)<br>");
        var inter9 = this.RegExpService.MatchFirstGroup(html, @"Intermediate 9:(.*?)<br>");
        var totalClimbing = this.RegExpService.MatchFirstGroup(html, @"Total Climbing:(.*?)<\/td>");

        var track = new Track
        {
            HeightDifference = this.RegExpService.MatchInt(height),
            Length = this.RegExpService.MatchInt(length),
            MaximumClimb = this.RegExpService.MatchInt(climb),
            TotalClimb = this.RegExpService.MatchInt(totalClimbing),
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

        if (inter8 != null)
        {
            track.Intermediates.Add(new Intermediate { Number = 8, Kilometers = this.RegExpService.MatchDouble(inter8) });
        }

        if (inter9 != null)
        {
            track.Intermediates.Add(new Intermediate { Number = 9, Kilometers = this.RegExpService.MatchDouble(inter9) });
        }

        return track;
    }

    private async Task ProcessAlpineSkiingAsync(Options options)
    {
        var rounds = new List<Round<AlpineSkiing>>();

        if (options.Event.IsTeamEvent)
        {
            var track = await this.SetAlpineSkiingTrackAsync(options.HtmlDocument.DocumentNode.OuterHtml);
            foreach (var roundData in options.Rounds.Skip(1))
            {
                var round = this.CreateRound<AlpineSkiing>(roundData, options.Event.Name, track);
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
                        HomeName = data[3].OuterHtml,
                        HomeNOC = data[4].OuterHtml,
                        Result = data[5].InnerHtml,
                        AwayName = data[6].OuterHtml,
                        AwayNOC = data[7].OuterHtml,
                        RoundType = roundData.Type,
                        RoundSubType = roundData.SubType,
                        Location = data[2].InnerText
                    };
                    var matchModel = await this.GetMatchAsync(matchInputModel);
                    var match = this.Mapper.Map<TeamMatch<AlpineSkiing>>(matchModel);

                    var documentModel = options.Documents.FirstOrDefault(x => x.Id == match.ResultId);
                    if (documentModel != null)
                    {
                        await this.SetMatchesAsync(match, documentModel.Rounds.Last(), options.Event.Id, options.Game.Year);
                    }

                    round.TeamMatches.Add(match);
                }

                rounds.Add(round);
            }
        }
        else
        {
            foreach (var roundData in options.Rounds)
            {
                var track = await this.SetAlpineSkiingTrackAsync(options.HtmlDocument.DocumentNode.OuterHtml);
                var round = this.CreateRound<AlpineSkiing>(roundData, options.Event.Name, track);
                await this.SetAlpineSkiingAthletesAsync(round, roundData, options.Event.Id, null);
                rounds.Add(round);
            }

            if (options.Documents.Count == 2 && rounds.Count == 1)
            {
                foreach (var document in options.Documents)
                {
                    await this.SetAlpineSkiingAthletesAsync(rounds.FirstOrDefault(), document.Rounds.FirstOrDefault(), options.Event.Id, document.Title);
                }
            }
        }

        await this.ProcessJsonAsync(rounds, options);
    }

    private async Task SetMatchesAsync(TeamMatch<AlpineSkiing> match, RoundDataModel roundData, Guid eventId, int year)
    {
        foreach (var row in roundData.Rows.Skip(1))
        {
            var data = row.Elements("td").ToList();
            var matchInputModel = new MatchInputModel
            {
                Row = row.OuterHtml,
                Number = data[0].OuterHtml,
                Date = data[1].InnerText,
                Year = year,
                EventId = eventId,
                IsTeam = false,
                HomeName = data[3].OuterHtml,
                HomeNOC = data[4].OuterHtml,
                Result = data[5].InnerHtml,
                AwayName = data[6].OuterHtml,
                AwayNOC = data[7].OuterHtml,
                AnyParts = false,
                RoundType = roundData.Type,
                RoundSubType = roundData.SubType,
                Location = data[2].InnerText
            };

            var athleteMatch = await this.GetMatchAsync(matchInputModel);

            match.Team1.Athletes.Add(new AlpineSkiing
            {
                Id = athleteMatch.Team1.Id,
                Name = athleteMatch.Team1.Name,
                Code = athleteMatch.Team1.Code,
                NOC = athleteMatch.Team1.NOC,
                Time = athleteMatch.Team1.Time,
                MatchResult = athleteMatch.Team1.MatchResult,
                Race = athleteMatch.Number,
            });

            match.Team2.Athletes.Add(new AlpineSkiing
            {
                Id = athleteMatch.Team2.Id,
                Name = athleteMatch.Team2.Name,
                Code = athleteMatch.Team2.Code,
                NOC = athleteMatch.Team2.NOC,
                Time = athleteMatch.Team2.Time,
                MatchResult = athleteMatch.Team2.MatchResult,
                Race = athleteMatch.Number,
            });
        }
    }

    private async Task<Track> SetAlpineSkiingTrackAsync(string html)
    {
        var courseSetterMatch = this.RegExpService.Match(html, @"<th>\s*Course Setter\s*<\/th>\s*<td(?:.*?)>(.*?)<\/td>");
        var gatesMatch = this.RegExpService.MatchFirstGroup(html, @"Gates:(.*?)<br>");
        var lengthMatch = this.RegExpService.MatchFirstGroup(html, @"Length:(.*?)<br>");
        var startAltitudeMatch = this.RegExpService.MatchFirstGroup(html, @"Start Altitude:(.*?)<br>");
        var verticalDropMatch = this.RegExpService.MatchFirstGroup(html, @"Vertical Drop:(.*?)<\/td>");
        var athleteModel = courseSetterMatch != null ? this.OlympediaService.FindAthlete(courseSetterMatch.Groups[1].Value) : null;
        var courseSetter = athleteModel != null ? await this.AthleteRepository.GetAsync(x => x.Code == athleteModel.Code) : null;

        var gates = this.RegExpService.MatchInt(gatesMatch);
        var length = this.RegExpService.MatchInt(lengthMatch);
        var startAltitude = this.RegExpService.MatchInt(startAltitudeMatch);
        var verticalDrop = this.RegExpService.MatchInt(verticalDropMatch);

        return new Track
        {
            Turns = gates,
            Length = length,
            StartAltitude = startAltitude,
            HeightDifference = verticalDrop,
            PersonId = courseSetter != null ? courseSetter.Id : Guid.Empty,
            PersonName = athleteModel?.Name
        };
    }

    private async Task SetAlpineSkiingAthletesAsync(Round<AlpineSkiing> round, RoundDataModel roundData, Guid eventId, string info)
    {
        foreach (var row in roundData.Rows.Skip(1))
        {
            var data = row.Elements("td").ToList();
            var athleteModel = this.OlympediaService.FindAthlete(data[roundData.Indexes[ConverterConstants.Name]].OuterHtml);

            if (string.IsNullOrEmpty(info))
            {
                var participant = await this.ParticipationRepository.GetAsync(x => x.Code == athleteModel.Code && x.EventId == eventId);
                var noc = this.OlympediaService.FindNOCCode(data[roundData.Indexes[ConverterConstants.NOC]].OuterHtml);
                var athlete = new AlpineSkiing
                {
                    Id = participant.Id,
                    Name = athleteModel.Name,
                    NOC = noc,
                    Code = athleteModel.Code,
                    FinishStatus = this.OlympediaService.FindStatus(row.OuterHtml),
                    Qualification = this.OlympediaService.FindQualification(row.OuterHtml),
                    Number = this.GetInt(roundData.Indexes, ConverterConstants.Number, data),
                    Downhill = this.GetTime(roundData.Indexes, ConverterConstants.Downhill, data),
                    PenaltyTime = this.GetTime(roundData.Indexes, ConverterConstants.PenaltyTime, data),
                    Points = this.GetDouble(roundData.Indexes, ConverterConstants.Points, data),
                    Run1 = this.GetTime(roundData.Indexes, ConverterConstants.Run1, data),
                    Run2 = this.GetTime(roundData.Indexes, ConverterConstants.Run2, data),
                    Slalom = this.GetTime(roundData.Indexes, ConverterConstants.Slalom, data),
                    Time = this.GetTime(roundData.Indexes, ConverterConstants.Time, data),
                    GroupNumber = roundData.Number,
                };

                round.Athletes.Add(athlete);
            }
            else
            {
                var athlete = round.Athletes.FirstOrDefault(x => x.Code == athleteModel.Code);
                if (info is "Downhill" or "Downhill1")
                {
                    athlete.Downhill = this.GetTime(roundData.Indexes, ConverterConstants.Time, data);
                    athlete.DownhillPoints = this.GetDouble(roundData.Indexes, ConverterConstants.Points, data);
                }
                else if (info is "Slalom" or "Slalom1")
                {
                    athlete.Slalom = this.GetTime(roundData.Indexes, ConverterConstants.Time, data);
                    athlete.Run1 = this.GetTime(roundData.Indexes, ConverterConstants.Run1, data);
                    athlete.Run2 = this.GetTime(roundData.Indexes, ConverterConstants.Run2, data);
                    athlete.SlalomPoints = this.GetDouble(roundData.Indexes, ConverterConstants.Points, data);
                }
                else if (info is "Run #1" or "Run #11")
                {
                    athlete.Run1 = this.GetTime(roundData.Indexes, ConverterConstants.Time, data);
                }
                else if (info is "Run #2" or "Run #21")
                {
                    athlete.Run2 = this.GetTime(roundData.Indexes, ConverterConstants.Time, data);
                }

                athlete.PenaltyTime = this.GetTime(roundData.Indexes, ConverterConstants.PenaltyTime, data);
            }
        }
    }
}

//#region CROSS COUNTRY SKIING
//private async Task ProcessCrossCountrySkiing(ConvertOptions options)
//{
//    var eventRound = this.CreateEventRound<CCSRound>(options.HtmlDocument, options.Event.Name);

//    if (options.Event.IsTeamEvent)
//    {
//        if (options.Tables.Any())
//        {
//            foreach (var table in options.Tables)
//            {
//                var dateString = this.RegExpService.MatchFirstGroup(table.HtmlDocument.DocumentNode.OuterHtml, @"<th>\s*Date\s*<\/th>\s*<td>(.*?)<\/td>");
//                var dateModel = this.dateService.ParseDate(dateString);
//                var format = this.RegExpService.MatchFirstGroup(table.HtmlDocument.DocumentNode.OuterHtml, @"<th>Format<\/th>\s*<td(?:.*?)>(.*?)<\/td>");
//                var round = this.CreateRound<CCSRound>(dateModel.From, format, table.Round, eventRound.EventName);
//                var heats = this.SplitHeats(table);

//                if (heats.Any())
//                {
//                    foreach (var heat in heats)
//                    {
//                        var heatType = this.NormalizeService.MapHeats(heat.Title);
//                        await this.SetCCSTeamsAsync(round, heat.HtmlDocument, options.Event, heatType);
//                    }
//                }
//                else
//                {
//                    await this.SetCCSTeamsAsync(round, table.HtmlDocument, options.Event, HeatType.None);
//                }

//                eventRound.Rounds.Add(round);
//            }
//        }
//        else
//        {
//            var round = this.CreateRound<CCSRound>(eventRound.Dates.From, eventRound.Format, RoundType.Final, eventRound.EventName);
//            round.Track = this.SetCCSTracks(options.HtmlDocument);
//            await this.SetCCSTeamsAsync(round, options.StandingTable.HtmlDocument, options.Event, HeatType.None);
//            eventRound.Rounds.Add(round);
//        }
//    }
//    else
//    {
//        if (options.Documents.Any())
//        {
//            var round = this.CreateRound<CCSRound>(eventRound.Dates.From, eventRound.Format, RoundType.Final, eventRound.EventName);

//            var firstHtmlDocument = this.CreateHtmlDocument(options.Documents.First());
//            await this.SetCCSSkierAsync(round, firstHtmlDocument, options.Event, HeatType.None);

//            var lastHtmlDocument = this.CreateHtmlDocument(options.Documents.Last());
//            this.ConvertCCSSkier(round, lastHtmlDocument);
//            eventRound.Rounds.Add(round);
//        }
//        else if (options.Tables.Any())
//        {
//            foreach (var table in options.Tables)
//            {
//                var dateString = this.RegExpService.MatchFirstGroup(table.HtmlDocument.DocumentNode.OuterHtml, @"<th>\s*Date\s*<\/th>\s*<td>(.*?)<\/td>");
//                var dateModel = this.dateService.ParseDate(dateString);
//                var format = this.RegExpService.MatchFirstGroup(table.HtmlDocument.DocumentNode.OuterHtml, @"<th>Format<\/th>\s*<td(?:.*?)>(.*?)<\/td>");
//                var round = this.CreateRound<CCSRound>(dateModel.From, format, table.Round, eventRound.EventName);
//                var heats = this.SplitHeats(table);

//                if (heats.Any())
//                {
//                    foreach (var heat in heats)
//                    {
//                        var heatType = this.NormalizeService.MapHeats(heat.Title);
//                        await this.SetCCSSkierAsync(round, heat.HtmlDocument, options.Event, heatType);
//                    }
//                }
//                else
//                {
//                    await this.SetCCSSkierAsync(round, table.HtmlDocument, options.Event, HeatType.None);
//                }

//                eventRound.Rounds.Add(round);
//            }
//        }
//        else
//        {
//            var round = this.CreateRound<CCSRound>(eventRound.Dates.From, eventRound.Format, RoundType.Final, eventRound.EventName);
//            round.Track = this.SetCCSTracks(options.HtmlDocument);
//            await this.SetCCSSkierAsync(round, options.StandingTable.HtmlDocument, options.Event, HeatType.None);
//            eventRound.Rounds.Add(round);
//        }
//    }

//    await this.ProcessJsonAsync(eventRound, options);
//}

//private async Task SetCCSTeamsAsync(CCSRound round, HtmlDocument htmlDocument, EventCacheModel eventCache, HeatType heat)
//{
//    var rows = htmlDocument.DocumentNode.SelectNodes("//table[@class='table table-striped']//tr");
//    var headers = rows.First().Elements("th").Select(x => x.InnerText).ToList();
//    var indexes = this.OlympediaService.FindIndexes(headers);

//    CCSTeam ccsTeam = null;
//    var number = 1;
//    foreach (var row in rows.Skip(1))
//    {
//        var data = row.Elements("td").ToList();
//        var name = data[indexes[ConverterConstants.INDEX_NAME]].InnerText.Trim();
//        var nocCode = this.OlympediaService.FindNOCCode(row.OuterHtml);
//        if (nocCode != null)
//        {
//            var nocCacheModel = this.DataCacheService.NOCCacheModels.FirstOrDefault(x => x.Code == nocCode);
//            var team = await this.teamsService.GetAsync(nocCacheModel.Id, eventCache.Id);
//            number = 1;

//            ccsTeam = new CCSTeam
//            {
//                Id = team.Id,
//                Name = name,
//                NOC = nocCode,
//                FinishStatus = this.OlympediaService.FindStatus(row.OuterHtml),
//                Qualification = this.OlympediaService.FindQualification(row.OuterHtml),
//                Heat = heat,
//                Number = indexes.TryGetValue(ConverterConstants.INDEX_NR, out int value1) ? this.RegExpService.MatchInt(data[value1].InnerText) : null,
//                Time = indexes.TryGetValue(ConverterConstants.INDEX_TIME, out int value2) ? this.dateService.ParseTime(data[value2].InnerText) : null
//            };

//            round.Teams.Add(ccsTeam);
//        }
//        else
//        {
//            var athleteModel = this.OlympediaService.FindAthlete(row.OuterHtml);
//            var nocCacheModel = this.DataCacheService.NOCCacheModels.FirstOrDefault(x => x.Code == ccsTeam.NOC);
//            var participant = await this.participantsService.GetAsync(athleteModel.Code, eventCache.Id, nocCacheModel.Id);

//            var skier = new CCSSkier
//            {
//                Id = participant.Id,
//                Code = athleteModel.Code,
//                Name = athleteModel.Name,
//                NOC = nocCode,
//                FinishStatus = this.OlympediaService.FindStatus(row.OuterHtml),
//                Heat = heat,
//                Number = number,
//                Time = indexes.TryGetValue(ConverterConstants.INDEX_TIME, out int value2) ? this.dateService.ParseTime(data[value2].InnerText) : null,
//                Exchange = indexes.TryGetValue(ConverterConstants.INDEX_EXCHANGE_TIME, out int value6) ? this.dateService.ParseTime(data[value6].InnerText) : null,
//                Leg1 = indexes.TryGetValue(ConverterConstants.INDEX_LEG_1, out int value9) ? this.dateService.ParseTime(data[value9].InnerText) : null,
//                Leg2 = indexes.TryGetValue(ConverterConstants.INDEX_LEG_2, out int value10) ? this.dateService.ParseTime(data[value10].InnerText) : null,
//                Leg3 = indexes.TryGetValue(ConverterConstants.INDEX_LEG_3, out int value11) ? this.dateService.ParseTime(data[value11].InnerText) : null,
//            };

//            number++;
//            ccsTeam.Skiers.Add(skier);
//        }
//    }
//}

//private void ConvertCCSSkier(CCSRound round, HtmlDocument htmlDocument)
//{
//    var rows = htmlDocument.DocumentNode.SelectNodes("//table[@class='table table-striped']//tr");
//    var headers = rows.First().Elements("th").Select(x => x.InnerText).ToList();
//    var indexes = this.OlympediaService.FindIndexes(headers);

//    foreach (var row in rows.Skip(1))
//    {
//        var data = row.Elements("td").ToList();
//        var athleteModel = this.OlympediaService.FindAthlete(row.OuterHtml);

//        var skier = round.Skiers.FirstOrDefault(x => x.Code == athleteModel.Code);
//        skier.Freestyle = indexes.TryGetValue(ConverterConstants.INDEX_TIME, out int value4) ? this.dateService.ParseTime(data[value4].InnerText) : null;
//        skier.Race = indexes.TryGetValue(ConverterConstants.INDEX_RACE, out int value7) ? this.dateService.ParseTime(data[value7].InnerText) : null;
//        skier.StartBehind = indexes.TryGetValue(ConverterConstants.INDEX_START_BEHIND, out int value8) ? this.dateService.ParseTime(data[value8].InnerText) : null;
//    }
//}

//private CCSTrack SetCCSTracks(HtmlDocument htmlDocument)
//{
//    var length = this.RegExpService.MatchFirstGroup(htmlDocument.DocumentNode.OuterHtml, @"Course Length:(.*?)<br>");
//    var height = this.RegExpService.MatchFirstGroup(htmlDocument.DocumentNode.OuterHtml, @"Height Differential:(.*?)<br>");
//    var climb = this.RegExpService.MatchFirstGroup(htmlDocument.DocumentNode.OuterHtml, @"Maximum Climb:(.*?)<br>");
//    var inter1 = this.RegExpService.MatchFirstGroup(htmlDocument.DocumentNode.OuterHtml, @"Intermediate 1:(.*?)<br>");
//    var inter2 = this.RegExpService.MatchFirstGroup(htmlDocument.DocumentNode.OuterHtml, @"Intermediate 2:(.*?)<br>");
//    var inter3 = this.RegExpService.MatchFirstGroup(htmlDocument.DocumentNode.OuterHtml, @"Intermediate 3:(.*?)<br>");
//    var inter4 = this.RegExpService.MatchFirstGroup(htmlDocument.DocumentNode.OuterHtml, @"Intermediate 4:(.*?)<br>");
//    var inter5 = this.RegExpService.MatchFirstGroup(htmlDocument.DocumentNode.OuterHtml, @"Intermediate 5:(.*?)<br>");
//    var inter6 = this.RegExpService.MatchFirstGroup(htmlDocument.DocumentNode.OuterHtml, @"Intermediate 6:(.*?)<br>");
//    var inter7 = this.RegExpService.MatchFirstGroup(htmlDocument.DocumentNode.OuterHtml, @"Intermediate 7:(.*?)<br>");
//    var inter8 = this.RegExpService.MatchFirstGroup(htmlDocument.DocumentNode.OuterHtml, @"Intermediate 8:(.*?)<br>");
//    var inter9 = this.RegExpService.MatchFirstGroup(htmlDocument.DocumentNode.OuterHtml, @"Intermediate 9:(.*?)<br>");
//    var totalClimbing = this.RegExpService.MatchFirstGroup(htmlDocument.DocumentNode.OuterHtml, @"Total Climbing:(.*?)<\/td>");

//    return new CCSTrack
//    {
//        HeightDifferential = this.RegExpService.MatchInt(height),
//        Length = this.RegExpService.MatchInt(length),
//        MaximumClimb = this.RegExpService.MatchInt(climb),
//        TotalClimbing = this.RegExpService.MatchInt(totalClimbing),
//        Intermediate1 = this.RegExpService.MatchDecimal(inter1),
//        Intermediate2 = this.RegExpService.MatchDecimal(inter2),
//        Intermediate3 = this.RegExpService.MatchDecimal(inter3),
//        Intermediate4 = this.RegExpService.MatchDecimal(inter4),
//        Intermediate5 = this.RegExpService.MatchDecimal(inter5),
//        Intermediate6 = this.RegExpService.MatchDecimal(inter6),
//        Intermediate7 = this.RegExpService.MatchDecimal(inter7),
//        Intermediate8 = this.RegExpService.MatchDecimal(inter8),
//        Intermediate9 = this.RegExpService.MatchDecimal(inter9),
//    };
//}

//private async Task SetCCSSkierAsync(CCSRound round, HtmlDocument htmlDocument, EventCacheModel eventCache, HeatType heatType)
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

//        var skier = new CCSSkier
//        {
//            Id = participant.Id,
//            Code = athleteModel.Code,
//            Name = athleteModel.Name,
//            NOC = nocCode,
//            FinishStatus = this.OlympediaService.FindStatus(data[0].OuterHtml),
//            Qualification = this.OlympediaService.FindQualification(row.OuterHtml),
//            Heat = heatType,
//            Number = indexes.TryGetValue(ConverterConstants.INDEX_NR, out int value1) ? this.RegExpService.MatchInt(data[value1].InnerText) : null,
//            Time = indexes.TryGetValue(ConverterConstants.INDEX_TIME, out int value2) ? this.dateService.ParseTime(data[value2].InnerText) : null,
//            Classical = indexes.TryGetValue(ConverterConstants.INDEX_CLASSICAL, out int value3) ? this.dateService.ParseTime(data[value3].InnerText) : null,
//            Freestyle = indexes.TryGetValue(ConverterConstants.INDEX_FREESTYLE, out int value4) ? this.dateService.ParseTime(data[value4].InnerText) : null,
//            PitStop = indexes.TryGetValue(ConverterConstants.INDEX_PIT_STOP, out int value5) ? this.dateService.ParseTime(data[value5].InnerText) : null,
//            //Exchange = indexes.TryGetValue(ConverterConstants.INDEX_EXCHANGE_TIME, out int value6) ? this.dateService.ParseTime(data[value6].InnerText) : null,
//            Race = indexes.TryGetValue(ConverterConstants.INDEX_RACE, out int value7) ? this.dateService.ParseTime(data[value7].InnerText) : null,
//            StartBehind = indexes.TryGetValue(ConverterConstants.INDEX_START_BEHIND, out int value8) ? this.dateService.ParseTime(data[value8].InnerText) : null,
//            //Leg1 = indexes.TryGetValue(ConverterConstants.INDEX_LEG_1, out int value9) ? this.dateService.ParseTime(data[value9].InnerText) : null,
//            //Leg2 = indexes.TryGetValue(ConverterConstants.INDEX_LEG_2, out int value10) ? this.dateService.ParseTime(data[value10].InnerText) : null,
//            //Leg3 = indexes.TryGetValue(ConverterConstants.INDEX_LEG_3, out int value11) ? this.dateService.ParseTime(data[value11].InnerText) : null,
//            Indermediate1 = indexes.TryGetValue(ConverterConstants.INDEX_INTERMEDIATE_1, out int value12) ? this.dateService.ParseTime(data[value12].InnerText) : null,
//            Indermediate2 = indexes.TryGetValue(ConverterConstants.INDEX_INTERMEDIATE_2, out int value13) ? this.dateService.ParseTime(data[value13].InnerText) : null,
//            Indermediate3 = indexes.TryGetValue(ConverterConstants.INDEX_INTERMEDIATE_3, out int value14) ? this.dateService.ParseTime(data[value14].InnerText) : null,
//            Indermediate4 = indexes.TryGetValue(ConverterConstants.INDEX_INTERMEDIATE_4, out int value15) ? this.dateService.ParseTime(data[value15].InnerText) : null,
//            Indermediate5 = indexes.TryGetValue(ConverterConstants.INDEX_INTERMEDIATE_5, out int value16) ? this.dateService.ParseTime(data[value16].InnerText) : null,
//            Indermediate6 = indexes.TryGetValue(ConverterConstants.INDEX_INTERMEDIATE_6, out int value17) ? this.dateService.ParseTime(data[value17].InnerText) : null,
//            Indermediate7 = indexes.TryGetValue(ConverterConstants.INDEX_INTERMEDIATE_7, out int value18) ? this.dateService.ParseTime(data[value18].InnerText) : null,
//            Indermediate8 = indexes.TryGetValue(ConverterConstants.INDEX_INTERMEDIATE_8, out int value19) ? this.dateService.ParseTime(data[value19].InnerText) : null,
//            Indermediate9 = indexes.TryGetValue(ConverterConstants.INDEX_INTERMEDIATE_9, out int value20) ? this.dateService.ParseTime(data[value20].InnerText) : null,
//        };

//        round.Skiers.Add(skier);
//    }
//}
//#endregion CROSS COUNTRY SKIING
