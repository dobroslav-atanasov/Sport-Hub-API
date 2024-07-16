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

public class AthleticsConverter : BaseSportConverter
{
    public AthleticsConverter(IOlympediaService olympediaService, IDateService dateService, IDataCacheService dataCacheService, IRegExpService regExpService, IMapper mapper,
        INormalizeService normalizeService, OlympicGamesRepository<Team> teamRepository, OlympicGamesRepository<Participation> participationRepository,
        OlympicGamesRepository<Athlete> athleteRepository, OlympicGamesRepository<Result> resultRepository)
        : base(olympediaService, dateService, dataCacheService, regExpService, mapper, normalizeService, teamRepository, participationRepository, athleteRepository, resultRepository)
    {
    }

    public override async Task ProcessAsync(Options options)
    {
        var eventType = this.MapAthleticsEventType(options.Event.Name);
        var rounds = new List<Round<Athletics>>();

        if (eventType == AthleticsEventType.TrackEvents)
        {
            foreach (var roundData in options.Rounds)
            {
                var wind = this.ExtractWind(roundData.Html);
                var round = this.CreateRound<Athletics>(roundData, options.Event.Name, null);
                await this.SetTrackAndRoadEventsAsync(round, roundData, options.Event.Id, options.Event.IsTeamEvent, wind);
                rounds.Add(round);
            }
        }
        else if (eventType == AthleticsEventType.RoadEvents)
        {
            var roundData = options.Rounds.FirstOrDefault();
            var round = this.CreateRound<Athletics>(roundData, options.Event.Name, null);
            await this.SetTrackAndRoadEventsAsync(round, roundData, options.Event.Id, options.Event.IsTeamEvent, null);
            rounds.Add(round);
        }
        else if (eventType == AthleticsEventType.FieldEvents)
        {
            foreach (var roundData in options.Rounds)
            {
                var round = this.CreateRound<Athletics>(roundData, options.Event.Name, null);
                await this.SetFieldEventsAsync(round, roundData, options.Event.Id);
                rounds.Add(round);
            }
        }
        else if (eventType == AthleticsEventType.CombinedEvents)
        {
            var roundData = options.Rounds.FirstOrDefault();
            var round = this.CreateRound<Athletics>(roundData, options.Event.Name, null);
            await this.SetCombinedEventsAsync(round, roundData, options.Event.Id);

            foreach (var document in options.Documents.Where(x => !x.Title.Contains("Standings", StringComparison.CurrentCultureIgnoreCase)))
            {
                var date = this.DateService.ParseDate(this.RegExpService.MatchFirstGroup(document.Html, @"<th>\s*Date\s*<\/th>\s*<td>(.*?)<\/td>")).From;
                this.ProcessCombinedDocument(round, document.Rounds.FirstOrDefault(), date, document.Title);
            }

            rounds.Add(round);
        }
        else if (eventType == AthleticsEventType.CrossCountryEvent)
        {
            var roundData = options.Rounds.FirstOrDefault();
            var round = this.CreateRound<Athletics>(roundData, options.Event.Name, null);
            await this.SetTrackAndRoadEventsAsync(round, roundData, options.Event.Id, options.Event.IsTeamEvent, null);
            rounds.Add(round);
        }

        await this.ProcessJsonAsync(rounds, options);
    }

    private void ProcessCombinedDocument(Round<Athletics> round, RoundDataModel roundData, DateTime? date, string info)
    {
        foreach (var row in roundData.Rows.Skip(1))
        {
            var noc = this.OlympediaService.FindNOCCode(row.OuterHtml);
            var data = row.Elements("td").ToList();

            var athleteModel = this.OlympediaService.FindAthlete(row.OuterHtml);
            if (athleteModel != null)
            {
                var athlete = round.Athletes.FirstOrDefault(x => x.Code == athleteModel.Code);
                var measurement = this.GetDouble(roundData.Indexes, ConverterConstants.Distance, data);
                measurement ??= this.GetDouble(roundData.Indexes, ConverterConstants.BestHeight, data);
                var combined = new AthleticsCombined
                {
                    EventName = info,
                    Date = date,
                    Points = this.GetDouble(roundData.Indexes, ConverterConstants.Points, data),
                    BestMeasurement = measurement,
                    Time = this.GetTime(roundData.Indexes, ConverterConstants.Time, data),
                    TimeAutomatic = this.GetTime(roundData.Indexes, ConverterConstants.TimeAutomatic, data),
                    TimeHand = this.GetTime(roundData.Indexes, ConverterConstants.TimeHand, data),
                    TieBreakingTime = this.GetTime(roundData.Indexes, ConverterConstants.TieBreakingTime, data),
                    SecondBestMeasurement = this.GetDouble(roundData.Indexes, ConverterConstants.SecondBestDistance, data),
                    MissesAtBeast = this.GetInt(roundData.Indexes, ConverterConstants.MissesAtBest, data),
                };

                athlete.Combined.Add(combined);
            }
        }

        //// TODO: Results from every sub event.
        //foreach (var table in document.Tables.Skip(1))
        //{
        //}
    }

    private async Task SetCombinedEventsAsync(Round<Athletics> round, RoundDataModel roundData, Guid eventId)
    {
        foreach (var row in roundData.Rows.Skip(1))
        {
            var noc = this.OlympediaService.FindNOCCode(row.OuterHtml);
            var data = row.Elements("td").ToList();

            var athleteModel = this.OlympediaService.FindAthlete(row.OuterHtml);
            if (athleteModel != null)
            {
                var participant = await this.ParticipationRepository.GetAsync(x => x.Code == athleteModel.Code && x.EventId == eventId);

                var athlete = new Athletics
                {
                    Id = participant != null ? participant.Id : Guid.Empty,
                    Name = athleteModel.Name,
                    NOC = noc,
                    Code = athleteModel.Code,
                    FinishStatus = this.OlympediaService.FindFinishStatus(row.OuterHtml),
                    IsQualified = this.OlympediaService.IsQualified(row.OuterHtml),
                    Record = this.OlympediaService.FindRecord(row.OuterHtml),
                    Number = this.GetInt(roundData.Indexes, ConverterConstants.Number, data),
                    Points = this.GetDouble(roundData.Indexes, ConverterConstants.Points, data)
                };

                round.Athletes.Add(athlete);
            }
        }
    }

    private async Task SetFieldEventsAsync(Round<Athletics> round, RoundDataModel roundData, Guid eventId)
    {
        if (roundData.Rows == null)
        {
            return;
        }

        foreach (var row in roundData.Rows.Skip(1))
        {
            var noc = this.OlympediaService.FindNOCCode(row.OuterHtml);
            var data = row.Elements("td").ToList();

            var athleteModel = this.OlympediaService.FindAthlete(row.OuterHtml);
            if (athleteModel != null)
            {
                var participant = await this.ParticipationRepository.GetAsync(x => x.Code == athleteModel.Code && x.EventId == eventId);

                var bestMeasurement = this.GetDouble(roundData.Indexes, ConverterConstants.Distance, data);
                bestMeasurement ??= this.GetDouble(roundData.Indexes, ConverterConstants.Height, data);
                bestMeasurement ??= this.GetDouble(roundData.Indexes, ConverterConstants.BestHeight, data);

                var athlete = new Athletics
                {
                    Id = participant != null ? participant.Id : Guid.Empty,
                    Name = athleteModel.Name,
                    NOC = noc,
                    Code = athleteModel.Code,
                    FinishStatus = this.OlympediaService.FindFinishStatus(row.OuterHtml),
                    IsQualified = this.OlympediaService.IsQualified(row.OuterHtml),
                    Record = this.OlympediaService.FindRecord(row.OuterHtml),
                    Number = this.GetInt(roundData.Indexes, ConverterConstants.Number, data),
                    BestMeasurement = bestMeasurement,
                    SecondBestMeasurement = this.GetDouble(roundData.Indexes, ConverterConstants.SecondBestDistance, data),
                    Misses = this.GetInt(roundData.Indexes, ConverterConstants.Misses, data),
                    TotalAttempts = this.GetInt(roundData.Indexes, ConverterConstants.TotalAttempts, data),
                    ThrowOff = this.GetDouble(roundData.Indexes, ConverterConstants.ThrowOff, data),
                    MissesAtBeast = this.GetInt(roundData.Indexes, ConverterConstants.MissesAtBest, data),
                    Attempts = this.SetAttempts(roundData.Indexes, roundData.Headers, data),
                };

                round.Athletes.Add(athlete);
            }
        }
    }

    private List<AthleticsAttempt> SetAttempts(Dictionary<string, int> indexes, List<string> headers, List<HtmlNode> data)
    {
        var index = 0;
        var number = 0;
        var attempts = new List<AthleticsAttempt>();

        foreach (var kvp in indexes)
        {
            var @try = Tuple.Create<double?, AthleticsTry>(null, AthleticsTry.None);
            var order = 0;
            switch (kvp.Key)
            {
                case ConverterConstants.Round1:
                    @try = this.ExtractDistanceTry(indexes, ConverterConstants.Round1, data);
                    order = 1;
                    break;
                case ConverterConstants.Round2:
                    @try = this.ExtractDistanceTry(indexes, ConverterConstants.Round2, data);
                    order = 2;
                    break;
                case ConverterConstants.Round3:
                    @try = this.ExtractDistanceTry(indexes, ConverterConstants.Round3, data);
                    order = 3;
                    break;
                case ConverterConstants.Round4:
                    @try = this.ExtractDistanceTry(indexes, ConverterConstants.Round4, data);
                    order = 4;
                    break;
                case ConverterConstants.Round5:
                    @try = this.ExtractDistanceTry(indexes, ConverterConstants.Round5, data);
                    order = 5;
                    break;
                case ConverterConstants.Round6:
                    @try = this.ExtractDistanceTry(indexes, ConverterConstants.Round6, data);
                    order = 6;
                    break;
            }

            if (order != 0)
            {
                attempts.Add(new AthleticsAttempt
                {
                    Measurement = @try.Item1,
                    Number = order,
                    Try1 = @try.Item2,
                });
            }
        }

        foreach (var header in headers)
        {
            var match = this.RegExpService.Match(header, @"([\d\.\,]+)\s*m");
            if (match != null)
            {
                number++;
                var isJumpOff = false;
                if (header.Contains("jump", StringComparison.CurrentCultureIgnoreCase))
                {
                    isJumpOff = true;
                }

                var tries = this.ExtractAttemptTries(data[index].InnerText);

                var attempt = new AthleticsAttempt
                {
                    Number = number,
                    IsJumpOff = isJumpOff,
                    Measurement = double.Parse(match.Groups[1].Value.Replace(".", ",")),
                    Try1 = tries.Item1,
                    Try2 = tries.Item2,
                    Try3 = tries.Item3,
                };

                attempts.Add(attempt);
            }
            index++;
        }

        return attempts;
    }

    private Tuple<double?, AthleticsTry> ExtractDistanceTry(Dictionary<string, int> indexes, string round, List<HtmlNode> data)
    {
        var result = Tuple.Create<double?, AthleticsTry>(null, AthleticsTry.None);
        var text = this.GetString(indexes, round, data);
        if (text == null)
        {
            return result;
        }

        if (this.RegExpService.Match(text, @"x") != null)
        {
            result = Tuple.Create<double?, AthleticsTry>(null, AthleticsTry.Fail);
        }
        else if (this.RegExpService.Match(text, @"–") != null || this.RegExpService.Match(text, @"p") != null)
        {
            result = Tuple.Create<double?, AthleticsTry>(null, AthleticsTry.Skip);
        }
        else
        {
            var measurement = this.GetDouble(indexes, round, data);
            result = Tuple.Create(measurement, AthleticsTry.Success);
        }

        return result;
    }

    private Tuple<AthleticsTry, AthleticsTry, AthleticsTry> ExtractAttemptTries(string text)
    {
        var firstTry = this.GetAthleticsTry(text.ElementAtOrDefault(0));
        var secondTry = this.GetAthleticsTry(text.ElementAtOrDefault(1));
        var thirdTry = this.GetAthleticsTry(text.ElementAtOrDefault(2));
        var result = Tuple.Create(firstTry, secondTry, thirdTry);

        return result;
    }

    private AthleticsTry GetAthleticsTry(char symbol)
    {
        var @try = AthleticsTry.None;
        switch (symbol)
        {
            case '×':
            case 'x':
                @try = AthleticsTry.Fail;
                break;
            case '–':
                @try = AthleticsTry.Skip;
                break;
            case 'o':
                @try = AthleticsTry.Success;
                break;
        }

        return @try;
    }

    private double? ExtractWind(string html)
    {
        var match = this.RegExpService.Match(html, @"<th>Wind<\/th>\s*<td>(.*?)<\/td>");
        if (match != null)
        {
            return this.RegExpService.MatchDouble(match.Groups[1].Value);
        }

        return null;
    }

    private async Task SetTrackAndRoadEventsAsync(Round<Athletics> round, RoundDataModel roundData, Guid eventId, bool isTeamEvent, double? wind)
    {
        Athletics team = null;
        foreach (var row in roundData.Rows.Skip(1))
        {
            var noc = this.OlympediaService.FindNOCCode(row.OuterHtml);
            var data = row.Elements("td").ToList();

            if (isTeamEvent)
            {
                if (noc != null)
                {
                    var teamName = data[roundData.Indexes[ConverterConstants.Name]].InnerText;
                    var nocCache = this.DataCacheService.NationalOlympicCommittees.FirstOrDefault(x => x.Code == noc);
                    var dbTeam = await this.TeamRepository.GetAsync(x => x.Name == teamName && x.NationalOlympicCommitteeId == nocCache.Id && x.EventId == eventId);
                    dbTeam ??= await this.TeamRepository.GetAsync(x => x.NationalOlympicCommitteeId == nocCache.Id && x.EventId == eventId);

                    team = new Athletics
                    {
                        Id = dbTeam.Id,
                        Name = dbTeam.Name,
                        NOC = noc,
                        FinishStatus = this.OlympediaService.FindFinishStatus(row.OuterHtml),
                        IsQualified = this.OlympediaService.IsQualified(row.OuterHtml),
                        Record = this.OlympediaService.FindRecord(row.OuterHtml),
                        Number = this.GetInt(roundData.Indexes, ConverterConstants.Number, data),
                        Order = this.GetInt(roundData.Indexes, ConverterConstants.Order, data),
                        Lane = this.GetInt(roundData.Indexes, ConverterConstants.Lane, data),
                        ReactionTime = this.GetDouble(roundData.Indexes, ConverterConstants.ReactionTime, data),
                        Time = this.GetTime(roundData.Indexes, ConverterConstants.Time, data),
                        TimeAutomatic = this.GetTime(roundData.Indexes, ConverterConstants.TimeAutomatic, data),
                        TimeHand = this.GetTime(roundData.Indexes, ConverterConstants.TimeHand, data),
                        TieBreakingTime = this.GetTime(roundData.Indexes, ConverterConstants.TieBreakingTime, data),
                        Points = this.GetInt(roundData.Indexes, ConverterConstants.Points, data),
                        Wind = wind
                    };

                    team.Points ??= this.GetInt(roundData.Indexes, ConverterConstants.AdjustedPoints, data);

                    round.Teams.Add(team);
                }
                else
                {
                    var athleteModels = this.OlympediaService.FindAthletes(row.OuterHtml);
                    foreach (var athleteModel in athleteModels)
                    {
                        var participant = await this.ParticipationRepository.GetAsync(x => x.Code == athleteModel.Code && x.EventId == eventId);
                        var athlete = new Athletics
                        {
                            Id = participant != null ? participant.Id : Guid.Empty,
                            Name = athleteModel.Name,
                            NOC = team.NOC,
                            Code = athleteModel.Code,
                            FinishStatus = this.OlympediaService.FindFinishStatus(row.OuterHtml),
                            IsQualified = this.OlympediaService.IsQualified(row.OuterHtml),
                            Record = this.OlympediaService.FindRecord(row.OuterHtml),
                            Wind = wind
                        };

                        if (athleteModels.Count == 1)
                        {
                            athlete.Number = this.GetInt(roundData.Indexes, ConverterConstants.Number, data);
                            athlete.Order = this.GetInt(roundData.Indexes, ConverterConstants.Order, data);
                            athlete.Lane = this.GetInt(roundData.Indexes, ConverterConstants.Lane, data);
                            athlete.Position = this.GetString(roundData.Indexes, ConverterConstants.Lane, data);
                            athlete.ReactionTime = this.GetDouble(roundData.Indexes, ConverterConstants.ReactionTime, data);
                            athlete.Time = this.GetTime(roundData.Indexes, ConverterConstants.Time, data);
                            athlete.TimeAutomatic = this.GetTime(roundData.Indexes, ConverterConstants.TimeAutomatic, data);
                            athlete.TimeHand = this.GetTime(roundData.Indexes, ConverterConstants.TimeHand, data);
                            athlete.TieBreakingTime = this.GetTime(roundData.Indexes, ConverterConstants.TieBreakingTime, data);
                            athlete.Points = this.GetInt(roundData.Indexes, ConverterConstants.Points, data);
                        }

                        team.Athletes.Add(athlete);
                    }
                }
            }
            else
            {
                var athleteModel = this.OlympediaService.FindAthlete(row.OuterHtml);
                if (athleteModel != null)
                {
                    var participant = await this.ParticipationRepository.GetAsync(x => x.Code == athleteModel.Code && x.EventId == eventId);
                    var athlete = new Athletics
                    {
                        Id = participant != null ? participant.Id : Guid.Empty,
                        Name = athleteModel.Name,
                        NOC = noc,
                        Code = athleteModel.Code,
                        FinishStatus = this.OlympediaService.FindFinishStatus(row.OuterHtml),
                        IsQualified = this.OlympediaService.IsQualified(row.OuterHtml),
                        Record = this.OlympediaService.FindRecord(row.OuterHtml),
                        Number = this.GetInt(roundData.Indexes, ConverterConstants.Number, data),
                        Order = this.GetInt(roundData.Indexes, ConverterConstants.Order, data),
                        Lane = this.GetInt(roundData.Indexes, ConverterConstants.Lane, data),
                        ReactionTime = this.GetDouble(roundData.Indexes, ConverterConstants.ReactionTime, data),
                        Time = this.GetTime(roundData.Indexes, ConverterConstants.Time, data),
                        TimeAutomatic = this.GetTime(roundData.Indexes, ConverterConstants.TimeAutomatic, data),
                        TimeHand = this.GetTime(roundData.Indexes, ConverterConstants.TimeHand, data),
                        TieBreakingTime = this.GetTime(roundData.Indexes, ConverterConstants.TieBreakingTime, data),
                        Points = this.GetInt(roundData.Indexes, ConverterConstants.Points, data),
                        BentKneeWarnings = this.GetInt(roundData.Indexes, ConverterConstants.BentKnee, data),
                        LostOfContactWarnings = this.GetInt(roundData.Indexes, ConverterConstants.LostOfContact, data),
                        Warnings = this.GetInt(roundData.Indexes, ConverterConstants.Warnings, data),
                        Wind = wind
                    };

                    var splits = this.SetAthleticsSplits(roundData, data);
                    athlete.Splits = splits;

                    round.Athletes.Add(athlete);
                }
            }
        }
    }

    private List<AthleticsSplit> SetAthleticsSplits(RoundDataModel roundData, List<HtmlNode> data)
    {
        var list = new List<string>
        {
            ConverterConstants.Km1,
            ConverterConstants.Km2,
            ConverterConstants.Km3,
            ConverterConstants.Km4,
            ConverterConstants.Km5,
            ConverterConstants.Km6,
            ConverterConstants.Km7,
            ConverterConstants.Km8,
            ConverterConstants.Km9,
            ConverterConstants.Km10,
            ConverterConstants.Km11,
            ConverterConstants.Km12,
            ConverterConstants.Km13,
            ConverterConstants.Km14,
            ConverterConstants.Km15,
            ConverterConstants.Km16,
            ConverterConstants.Km17,
            ConverterConstants.Km18,
            ConverterConstants.Km19,
            ConverterConstants.Km20,
            ConverterConstants.Km25,
            ConverterConstants.Km26,
            ConverterConstants.Km28,
            ConverterConstants.Km30,
            ConverterConstants.Km31,
            ConverterConstants.Km35,
            ConverterConstants.Km36,
            ConverterConstants.Km37,
            ConverterConstants.Km38,
            ConverterConstants.Km40,
            ConverterConstants.Km45,
            ConverterConstants.Km46,
            ConverterConstants.HalfMarathon,
        };

        var splits = new List<AthleticsSplit>();
        foreach (var item in list)
        {
            var time = this.GetTime(roundData.Indexes, item, data);
            if (time != null)
            {
                splits.Add(new AthleticsSplit
                {
                    Distance = item,
                    Time = time
                });
            }
        }

        return splits;
    }

    private AthleticsEventType MapAthleticsEventType(string name)
    {
        switch (name)
        {
            case "Men 10000m":
            case "Men 100m":
            case "Men 10km Race Walk":
            case "Men 10miles Race Walk":
            case "Men 110m Hurdles":
            case "Men 1500m":
            case "Men 1600m Medley Relay":
            case "Men 200m":
            case "Men 200m Hurdles":
            case "Men 2500m Steeplechase":
            case "Men 2590m Steeplechase":
            case "Men 3000m Race Walk":
            case "Men 3000m Steeplechase":
            case "Men 3200m Steeplechase":
            case "Men 3500m Race Walk":
            case "Men 4000m Steeplechase":
            case "Men 400m":
            case "Men 400m Hurdles":
            case "Men 4x100m Relay":
            case "Men 4x400m Relay":
            case "Men 5000m":
            case "Men 5miles":
            case "Men 60m":
            case "Men 800m":
            case "Men Team 3000m":
            case "Men Team 3miles":
            case "Men Team 4miles":
            case "Men Team 5000m":
            case "Mixed 4x400m Relay":
            case "Women 10000m":
            case "Women 100m":
            case "Women 100m Hurdles":
            case "Women 10km Race Walk":
            case "Women 1500m":
            case "Women 200m":
            case "Women 3000m":
            case "Women 3000m Steeplechase":
            case "Women 400m":
            case "Women 400m Hurdles":
            case "Women 4x100m Relay":
            case "Women 4x400m Relay":
            case "Women 5000m":
            case "Women 800m":
            case "Women 80m Hurdles":
                return AthleticsEventType.TrackEvents;
            case "Men 56-pound Weight Throw":
            case "Men Discus Throw":
            case "Men Discus Throw Both Hands":
            case "Men Discus Throw Greek Style":
            case "Men Hammer Throw":
            case "Men High Jump":
            case "Men Javelin Throw":
            case "Men Javelin Throw Both Hands":
            case "Men Javelin Throw Freestyle":
            case "Men Long Jump":
            case "Men Pole Vault":
            case "Men Shot Put":
            case "Men Shot Put Both Hands":
            case "Men Standing High Jump":
            case "Men Standing Long Jump":
            case "Men Standing Triple Jump":
            case "Men Triple Jump":
            case "Women Discus Throw":
            case "Women Hammer Throw":
            case "Women High Jump":
            case "Women Javelin Throw":
            case "Women Long Jump":
            case "Women Pole Vault":
            case "Women Shot Put":
            case "Women Triple Jump":
                return AthleticsEventType.FieldEvents;
            case "Men 20km Race Walk":
            case "Men 50km Race Walk":
            case "Men Marathon":
            case "Women 20km Race Walk":
            case "Women Marathon":
                return AthleticsEventType.RoadEvents;
            case "Men All-Around Championship":
            case "Men Decathlon":
            case "Men Pentathlon":
            case "Women Heptathlon":
            case "Women Pentathlon":
                return AthleticsEventType.CombinedEvents;
            case "Men Individual Cross-Country":
            case "Men Team Cross-Country":
                return AthleticsEventType.CrossCountryEvent;
            default:
                return AthleticsEventType.None;
        }
    }
}