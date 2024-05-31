namespace SportData.Services;

using SportData.Common.Constants;
using SportData.Data.Models.Converters;
using SportData.Data.Models.Converters.OlympicGames;
using SportData.Data.Models.Converters.OlympicGames.Base;
using SportData.Data.Models.Entities.Enumerations;
using SportData.Data.Models.Entities.OlympicGames.Enumerations;
using SportData.Services.Interfaces;

public class OlympediaService : IOlympediaService
{
    private readonly IRegExpService regExpService;
    private readonly IDateService dateService;

    public OlympediaService(IRegExpService regExpService, IDateService dateService)
    {
        this.regExpService = regExpService;
        this.dateService = dateService;
    }

    public List<int> FindClubs(string text)
    {
        if (string.IsNullOrEmpty(text))
        {
            return [];
        }

        var clubs = this.regExpService
            .Matches(text, @"<a href=""\/affiliations\/(\d+)"">")
            .Select(x => int.Parse(x.Groups[1].Value))?
            .ToList();

        return clubs;
    }

    public AthleteModel FindAthlete(string text)
    {
        if (!string.IsNullOrEmpty(text))
        {
            var match = this.regExpService.Match(text, @"<a href=""\/athletes\/(\d+)"">(.*?)<\/a>");
            if (match != null)
            {
                return new AthleteModel
                {
                    Code = int.Parse(match.Groups[1].Value),
                    Name = match.Groups[2].Value.Trim()
                };
            }
        }

        return null;
    }

    public List<AthleteModel> FindAthletes(string text)
    {
        if (string.IsNullOrEmpty(text))
        {
            return [];
        }

        var numbers = this.regExpService
            .Matches(text, @"<a href=""\/athletes\/(\d+)"">(.*?)<\/a>")
            .Select(x => new AthleteModel { Code = int.Parse(x.Groups[1].Value.Trim()), Name = x.Groups[2].Value.Trim() })?
            .ToList();

        return numbers;
    }

    public string FindNOCCode(string text)
    {
        if (string.IsNullOrEmpty(text))
        {
            return null;
        }

        var match = this.regExpService.Match(text, @"<a href=""\/countries\/(.*?)"">");
        if (match != null)
        {
            var code = match.Groups[1].Value.Trim();
            code = code.Replace("CHI%20", "CHI");
            return code;
        }

        return null;
    }

    public IList<string> FindNOCCodes(string text)
    {
        if (string.IsNullOrEmpty(text))
        {
            return new List<string>();
        }

        var codes = this.regExpService
            .Matches(text, @"<a href=""\/countries\/(.*?)"">")
            .Select(x => x.Groups[1].Value)?
            .Where(x => x != "UNK")
            .ToList();

        return codes;
    }

    public MedalTypeEnum FindMedal(string text)
    {
        var match = this.regExpService.Match(text, @"<span class=""(?:Gold|Silver|Bronze)"">(Gold|Silver|Bronze)<\/span>");
        if (match != null)
        {
            var medalType = match.Groups[1].Value.ToLower();
            switch (medalType)
            {
                case "gold": return MedalTypeEnum.Gold;
                case "silver": return MedalTypeEnum.Silver;
                case "bronze": return MedalTypeEnum.Bronze;
            }
        }

        return MedalTypeEnum.None;
    }

    public MedalTypeEnum FindMedal(string text, RoundTypeEnum roundType)
    {
        var medalType = MedalTypeEnum.None;
        if (roundType == RoundTypeEnum.FinalRound)
        {
            if (text.Contains("1/2") || text.Contains("1-2"))
            {
                medalType = MedalTypeEnum.Gold;
            }
            else if (text.Contains("3/4") || text.Contains("3-4"))
            {
                medalType = MedalTypeEnum.Bronze;
            }
        }

        return medalType;
    }

    public FinishTypeEnum FindStatus(string text)
    {
        if (string.IsNullOrEmpty(text))
        {
            return FinishTypeEnum.None;
        }

        var acMatch = this.regExpService.Match(text, @"<abbrev title=""Also Competed"">AC</abbrev>");
        if (acMatch != null)
        {
            return FinishTypeEnum.AlsoCompeted;
        }

        var dnsMatch = this.regExpService.Match(text, @"<abbrev title=""Did Not Start"">DNS</abbrev>");
        if (dnsMatch != null)
        {
            return FinishTypeEnum.DidNotStart;
        }

        var dnfMatch = this.regExpService.Match(text, @"<abbrev title=""Did Not Finish"">DNF</abbrev>");
        if (dnfMatch != null)
        {
            return FinishTypeEnum.DidNotFinish;
        }

        var dqMatch = this.regExpService.Match(text, @"<abbrev title=""Disqualified"">DQ</abbrev>");
        if (dqMatch != null)
        {
            return FinishTypeEnum.Disqualified;
        }

        var tnkMatch = this.regExpService.Match(text, @"<abbrev title=""Time Not Known"">TNK</abbrev>");
        if (tnkMatch != null)
        {
            return FinishTypeEnum.TimeNotKnow;
        }

        return FinishTypeEnum.Finish;
    }

    public List<int> FindVenues(string text)
    {
        if (string.IsNullOrEmpty(text))
        {
            return [];
        }

        var venues = this.regExpService
            .Matches(text, @"\/venues\/(\d+)")
            .Select(x => int.Parse(x.Groups[1].Value))?
            .Distinct()
            .ToList();

        return venues;
    }

    public int FindMatchNumber(string text)
    {
        if (string.IsNullOrEmpty(text))
        {
            return 0;
        }

        var match = this.regExpService.Match(text, @"(?:Match|Game|Race)\s*#(\d+)");
        if (match != null)
        {
            var matchNumber = match.Groups[1].Value;
            return int.Parse(matchNumber);
        }

        return 0;
    }

    public int FindResultNumber(string text)
    {
        var match = this.regExpService.Match(text, @"<a href=""\/results\/(\d+)"">");
        if (match != null)
        {
            return int.Parse(match.Groups[1].Value);
        }

        return 0;
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

    public string FindLocation(string html)
    {
        if (string.IsNullOrEmpty(html))
        {
            return null;
        }

        var match = this.regExpService.Match(html, @"<a href=""\/venues\/(\d+)"">(.*?)<\/a>");
        if (match != null)
        {
            return match.Groups[2].Value.Trim();
        }

        return null;
    }

    //public MatchResultOld GetMatchResult(string text, MatchResultType type)
    //{
    //    if (string.IsNullOrEmpty(text))
    //    {
    //        return null;
    //    }

    //    text = text.Replace("[", string.Empty).Replace("]", string.Empty);

    //    if (type == MatchResultType.Games)
    //    {
    //        var result = new MatchResultOld();
    //        var match = this.regExpService.Match(text, @"(\d+)\s*-\s*(\d+)\s*,\s*(\d+)\s*-\s*(\d+)\s*,\s*(\d+)\s*-\s*(\d+)");
    //        if (match != null)
    //        {
    //            result.Games1 = new List<int?> { int.Parse(match.Groups[1].Value), int.Parse(match.Groups[3].Value), int.Parse(match.Groups[5].Value) };
    //            result.Games2 = new List<int?> { int.Parse(match.Groups[2].Value), int.Parse(match.Groups[4].Value), int.Parse(match.Groups[6].Value) };

    //            var points = result.Games1[0].Value > result.Games2[0].Value ? result.Points1++ : result.Points2++;
    //            points = result.Games1[1].Value > result.Games2[1].Value ? result.Points1++ : result.Points2++;
    //            points = result.Games1[2].Value > result.Games2[2].Value ? result.Points1++ : result.Points2++;

    //            this.SetWinAndLose(result);
    //            return result;
    //        }
    //        match = this.regExpService.Match(text, @"(\d+)\s*-\s*(\d+)\s*,\s*(\d+)\s*-\s*(\d+)");
    //        if (match != null)
    //        {
    //            result.Games1 = new List<int?> { int.Parse(match.Groups[1].Value), int.Parse(match.Groups[3].Value) };
    //            result.Games2 = new List<int?> { int.Parse(match.Groups[2].Value), int.Parse(match.Groups[4].Value) };

    //            var points = result.Games1[0].Value > result.Games2[0].Value ? result.Points1++ : result.Points2++;
    //            points = result.Games1[1].Value > result.Games2[1].Value ? result.Points1++ : result.Points2++;

    //            this.SetWinAndLose(result);
    //            return result;
    //        }
    //        match = this.regExpService.Match(text, @"(\d+)\s*-\s*(\d+)");
    //        if (match != null)
    //        {
    //            result.Games1 = new List<int?> { int.Parse(match.Groups[1].Value) };
    //            result.Games2 = new List<int?> { int.Parse(match.Groups[2].Value) };

    //            var points = result.Games1[0].Value > result.Games2[0].Value ? result.Points1++ : result.Points2++;

    //            result.Result1 = ResultType.Win;
    //            result.Result2 = ResultType.Lose;

    //            return result;
    //        }

    //        return result;
    //    }
    //    else
    //    {
    //        var result = new MatchResultOld();
    //        var match = this.regExpService.Match(text, @"(\d+)\s*(?:-|–|—)\s*(\d+)");
    //        if (match != null)
    //        {
    //            result.Points1 = int.Parse(match.Groups[1].Value.Trim());
    //            result.Points2 = int.Parse(match.Groups[2].Value.Trim());

    //            this.SetWinAndLose(result);
    //        }
    //        match = this.regExpService.Match(text, @"(\d+)\.(\d+)\s*(?:-|–|—)\s*(\d+)\.(\d+)");
    //        if (match != null)
    //        {
    //            result.Time1 = this.dateService.ParseTime($"{match.Groups[1].Value}.{match.Groups[2].Value}");
    //            result.Time2 = this.dateService.ParseTime($"{match.Groups[3].Value}.{match.Groups[4].Value}");

    //            if (result.Time1 < result.Time2)
    //            {
    //                result.Result1 = ResultType.Win;
    //                result.Result2 = ResultType.Lose;
    //            }
    //            else if (result.Time1 > result.Time2)
    //            {
    //                result.Result1 = ResultType.Lose;
    //                result.Result2 = ResultType.Win;
    //            }
    //        }
    //        match = this.regExpService.Match(text, @"(\d+)\.(\d+)\s*(?:-|–|—)\s*DNF");
    //        if (match != null)
    //        {
    //            result.Time1 = this.dateService.ParseTime($"{match.Groups[1].Value}.{match.Groups[2].Value}");
    //            result.Time2 = null;

    //            result.Result1 = ResultType.Win;
    //            result.Result2 = ResultType.Lose;
    //        }

    //        return result;
    //    }

    //    return null;
    //}

    public string FindMatchInfo(string text)
    {
        if (string.IsNullOrEmpty(text))
        {
            return null;
        }

        var match = this.regExpService.Match(text, @"(?:Match|Game)\s*(\d+)(?:\/|-)(\d+)");
        if (match != null)
        {
            return $"{match.Groups[1].Value}-{match.Groups[2].Value}";
        }

        return null;
    }

    public RecordType FindRecord(string text)
    {
        var record = RecordType.None;
        if (string.IsNullOrEmpty(text))
        {
            return record;
        }

        var match = this.regExpService.Match(text, @"World\s*Record");
        if (match != null)
        {
            record = RecordType.World;
        }

        match = this.regExpService.Match(text, @"Olympic\s*Record");
        if (match != null)
        {
            record = RecordType.Olympic;
        }

        return record;
    }

    public QualificationType FindQualification(string text)
    {
        var type = QualificationType.None;
        if (string.IsNullOrEmpty(text))
        {
            return type;
        }

        var match = this.regExpService.Match(text, @"Qualified");
        if (match != null)
        {
            type = QualificationType.Qualified;
        }

        return type;
    }

    public IList<int> FindResults(string text)
    {
        if (string.IsNullOrEmpty(text))
        {
            return new List<int>();
        }

        var results = this.regExpService
            .Matches(text, @"<option value=""(.*?)"">")
            .Where(x => !string.IsNullOrEmpty(x.Groups[1].Value))
            .Select(x => int.Parse(x.Groups[1].Value))?
            .ToList();

        return results;
    }

    public DecisionType FindDecision(string text)
    {
        var decision = DecisionType.None;

        if (string.IsNullOrEmpty(text))
        {
            return decision;
        }

        var match = this.regExpService.Match(text, @">bye<");
        if (match != null)
        {
            decision = DecisionType.Buy;
        }

        match = this.regExpService.Match(text, @">walkover<");
        if (match != null)
        {
            decision = DecisionType.Walkover;
        }

        return decision;
    }

    public int FindSeedNumber(string text)
    {
        if (string.IsNullOrEmpty(text))
        {
            return 0;
        }

        var match = this.regExpService.Match(text, @"\((\d+)\)");
        if (match != null)
        {
            return int.Parse(match.Groups[1].Value);
        }

        return 0;
    }

    public Horse FindHorse(string text)
    {
        if (!string.IsNullOrEmpty(text))
        {
            var match = this.regExpService.Match(text, @"<a href=""/horses/(\d+)"">(.*?)</a>");
            if (match != null)
            {
                return new Horse
                {
                    Code = int.Parse(match.Groups[1].Value),
                    Name = match.Groups[2].Value.Trim()
                };
            }
        }

        return null;
    }

    public Dictionary<string, int> GetIndexes(List<string> headers)
    {
        var indexes = new Dictionary<string, int>();

        for (var i = 0; i < headers.Count; i++)
        {
            var key = string.Empty;

            switch (headers[i])
            {
                case "10s":
                case "Center 10s":
                    key = ConverterConstants.S10;
                    break;
                case "10s/9s":
                    key = ConverterConstants.S10S9;
                    break;
                case "8s":
                    key = ConverterConstants.S8;
                    break;
                case "9s":
                    key = ConverterConstants.S9;
                    break;
                case "Actual Time":
                    key = ConverterConstants.ActualTime;
                    break;
                case "Adjusted Points":
                case "Adj. Points":
                case "AIP":
                    key = ConverterConstants.AdjustedPoints;
                    break;
                case "Adjusted Time":
                case "AT":
                case "Adjusted":
                    key = ConverterConstants.AdjustedTime;
                    break;
                case "AI":
                    key = ConverterConstants.AI;
                    break;
                case "Air &amp; Form Points":
                    key = ConverterConstants.AirFormPoints;
                    break;
                case "Air Points":
                case "Air Score":
                    key = ConverterConstants.AirPoints;
                    break;
                case "AP":
                case "Apparatus":
                case "HAP":
                case "PAP":
                    key = ConverterConstants.ApparatusPoints;
                    break;
                case "Arrow 1":
                    key = ConverterConstants.Arrow1;
                    break;
                case "Arrow 10":
                    key = ConverterConstants.Arrow10;
                    break;
                case "Arrow 2":
                    key = ConverterConstants.Arrow2;
                    break;
                case "Arrow 3":
                    key = ConverterConstants.Arrow3;
                    break;
                case "Arrow 4":
                    key = ConverterConstants.Arrow4;
                    break;
                case "Arrow 5":
                    key = ConverterConstants.Arrow5;
                    break;
                case "Arrow 6":
                    key = ConverterConstants.Arrow6;
                    break;
                case "Arrow 7":
                    key = ConverterConstants.Arrow7;
                    break;
                case "Arrow 8":
                    key = ConverterConstants.Arrow8;
                    break;
                case "Arrow 9":
                    key = ConverterConstants.Arrow9;
                    break;
                case "Artistic Impression":
                case "Artistic Impression Points":
                    key = ConverterConstants.ArtisticImpression;
                    break;
                case "Artistic Impression Choreography Points":
                    key = ConverterConstants.ArtisticImpressionChoreographyPoints;
                    break;
                case "Artistic Impression Judge 1 Points":
                    key = ConverterConstants.ArtisticImpressionJudge1Points;
                    break;
                case "Artistic Impression Judge 2 Points":
                    key = ConverterConstants.ArtisticImpressionJudge2Points;
                    break;
                case "Artistic Impression Judge 3 Points":
                    key = ConverterConstants.ArtisticImpressionJudge3Points;
                    break;
                case "Artistic Impression Judge 4 Points":
                    key = ConverterConstants.ArtisticImpressionJudge4Points;
                    break;
                case "Artistic Impression Judge 5 Points":
                    key = ConverterConstants.ArtisticImpressionJudge5Points;
                    break;
                case "Artistic Impression Manner of Presentation Points":
                    key = ConverterConstants.ArtisticImpressionMannerofPresentationPoints;
                    break;
                case "Artistic Impression Music Interpretation Points":
                    key = ConverterConstants.ArtisticImpressionMusicInterpretationPoints;
                    break;
                case "Artistic Judge 1 Score":
                    key = ConverterConstants.ArtisticJudge1;
                    break;
                case "Artistic Judge 2 Score":
                    key = ConverterConstants.ArtisticJudge2;
                    break;
                case "Artistic Judge 3 Score":
                    key = ConverterConstants.ArtisticJudge3;
                    break;
                case "Artistic Judge 4 Score":
                    key = ConverterConstants.ArtisticJudge4;
                    break;
                case "Artistic Penalty Score":
                    key = ConverterConstants.ArtisticPenalty;
                    break;
                case "Artistic Points":
                    key = ConverterConstants.ArtisticPoints;
                    break;
                case "Artistic Reference Score":
                case "Artistic Score":
                    key = ConverterConstants.ArtisticScore;
                    break;
                case "Assists":
                case "AST":
                case "ASS":
                    key = ConverterConstants.Assits;
                    break;
                case "1st Inning/At Bats":
                case "Inning 1/At Bats":
                    key = ConverterConstants.AtBats;
                    break;
                case "Athletic Factor":
                    key = ConverterConstants.AthleticFactor;
                    break;
                case "Attack Attempts":
                    key = ConverterConstants.AttackAttempts;
                    break;
                case "Attack Successes":
                    key = ConverterConstants.AttackSuccesses;
                    break;
                case "Attempts":
                case "Attempts to Zone":
                    key = ConverterConstants.Attempts;
                    break;
                case "Attempts / Shots":
                    key = ConverterConstants.AttemptsShots;
                    break;
                case "Attempts / Shots on Goal":
                    key = ConverterConstants.AttemptsShotsOnGoal;
                    break;
                case "AVG":
                    key = ConverterConstants.Average;
                    break;
                case "Bad Points":
                    key = ConverterConstants.BadPoints;
                    break;
                case "Balance Beam":
                case "BB":
                    key = ConverterConstants.BalanceBeam;
                    break;
                case "Ball":
                case "Ball/Ribbon":
                case "Balls":
                    key = ConverterConstants.Ball;
                    break;
                case "BBL":
                    key = ConverterConstants.BarrageBoutsLoss;
                    break;
                case "BBW":
                    key = ConverterConstants.BarrageBoutsWon;
                    break;
                case "Bases-on-Balls":
                    key = ConverterConstants.BaseOnBalls;
                    break;
                case "Batting Average":
                    key = ConverterConstants.BattingAverage;
                    break;
                case "Bent Knee":
                case "Bent Knee Warnings":
                    key = ConverterConstants.BentKnee;
                    break;
                case "Best Mark Distance":
                    key = ConverterConstants.BestDistance;
                    break;
                case "Best Height Cleared":
                case "BHC":
                    //case "BHC(I)":
                    key = ConverterConstants.BestHeight;
                    break;
                case "Best Time":
                    key = ConverterConstants.BestTime;
                    break;
                case "Best Wave":
                    key = ConverterConstants.BestWave;
                    break;
                case "B Final":
                    key = ConverterConstants.Bfinal;
                    break;
                case "BLK":
                case "Blocks":
                case "Block Points":
                case "Block Points / Side-Outs":
                    key = ConverterConstants.Blocks;
                    break;
                case "Block Successes":
                    key = ConverterConstants.BlockSuccesses;
                    break;
                case "Bodyweight":
                    key = ConverterConstants.Bodyweight;
                    break;
                case "Bonus Points":
                    key = ConverterConstants.BonusPoints;
                    break;
                case "Boulder 1":
                    key = ConverterConstants.Boulder1;
                    break;
                case "Boulder 2":
                    key = ConverterConstants.Boulder2;
                    break;
                case "Boulder 3":
                    key = ConverterConstants.Boulder3;
                    break;
                case "Boulder 4":
                    key = ConverterConstants.Boulder4;
                    break;
                case "Bouldering":
                    key = ConverterConstants.Bouldering;
                    break;
                case "Breakthrough Shots / Attempts - Saves / Shots":
                    key = ConverterConstants.BreakthroughtShots;
                    break;
                case "Card":
                    key = ConverterConstants.Card;
                    break;
                case "Center Hits":
                case "Centrals":
                    key = ConverterConstants.Centrals;
                    break;
                case "Centre Goals/Attempts":
                    key = ConverterConstants.CentreGoal;
                    break;
                case "Classical":
                    key = ConverterConstants.Classical;
                    break;
                case "Class. Pts":
                case "Classification Points":
                    key = ConverterConstants.ClassificationPoints;
                    break;
                case "Classification Round":
                    key = ConverterConstants.ClassificationRound;
                    break;
                case "C&J":
                case "Clean & Jerk":
                    key = ConverterConstants.CleanJerk;
                    break;
                case "Clubs":
                case "Clubs Score":
                    key = ConverterConstants.Clubs;
                    break;
                case "Code":
                    key = ConverterConstants.Code;
                    break;
                case "Color":
                case "Colour":
                    key = ConverterConstants.Color;
                    break;
                case "C1EP":
                case "CEP":
                case "CP":
                case "TCP":
                case "1EP":
                case "Compulsory Exercise Points":
                case "Comp. Points":
                    key = ConverterConstants.CompulsaryPoints;
                    break;
                case "Compulsory":
                case "Compulsory Dance":
                    key = ConverterConstants.Compulsory;
                    break;
                case "CD#1FP":
                    key = ConverterConstants.CompulsoryDance1FactoredPlacements;
                    break;
                case "CD#2FP":
                    key = ConverterConstants.CompulsoryDance2FactoredPlacements;
                    break;
                case "Conversions":
                    key = ConverterConstants.Conversions;
                    break;
                case "Corner Kicks":
                    key = ConverterConstants.CornerKicks;
                    break;
                case "Count Back":
                case "Countback":
                    key = ConverterConstants.Countback;
                    break;
                case "Counter Attack Goals/Attempts":
                case "Counter-Attack Goals/Attempts":
                    key = ConverterConstants.CounterAttackGoals;
                    break;
                case "Cross-Country":
                    key = ConverterConstants.CrossCountry;
                    break;
                case "Cross-Country (20 km)":
                    key = ConverterConstants.CrossCountry20km;
                    break;
                case "Cross-Country (50 km)":
                    key = ConverterConstants.CrossCountry50km;
                    break;
                case "Cross-Country (5 km)":
                    key = ConverterConstants.CrossCountry5km;
                    break;
                case "Cross-Country Penalty Points":
                    key = ConverterConstants.CrossCountryPenaltyPoints;
                    break;
                case "Cross Country Skiing, 10 km":
                    key = ConverterConstants.CrossCountrySkiing10km;
                    break;
                case "Cross Country Skiing, 15 km":
                    key = ConverterConstants.CrossCountrySkiing15km;
                    break;
                case "Cross Country Skiing, 18 km":
                    key = ConverterConstants.CrossCountrySkiing18km;
                    break;
                case "Cross Country Skiing, 3 × 10 km Relay":
                    key = ConverterConstants.CrossCountrySkiing3x10kmRelay;
                    break;
                case "Cross Country Skiing, 4 × 5 km Relay":
                    key = ConverterConstants.CrossCountrySkiing4x10kmRelay;
                    break;
                case "Cross Country Skiing, 7.5 km":
                    key = ConverterConstants.CrossCountrySkiing75km;
                    break;
                case "Cumulative Time":
                    key = ConverterConstants.CumulativeTime;
                    break;
                case "40 km Cycling":
                case "7.4 km Cycling":
                    key = ConverterConstants.Cycling;
                    break;
                case "Dance 1":
                    key = ConverterConstants.Dance1;
                    break;
                case "Dance 2":
                    key = ConverterConstants.Dance2;
                    break;
                case "Dance 3":
                    key = ConverterConstants.Dance3;
                    break;
                case "Date/Time":
                    key = ConverterConstants.DateTime;
                    break;
                case "Deductions":
                    key = ConverterConstants.Deductions;
                    break;
                case "DRB":
                    key = ConverterConstants.DefensiveRebounds;
                    break;
                case "Deuk-Jeom":
                    key = ConverterConstants.DeukJeom;
                    break;
                case "Difficulty Points":
                case "DD":
                case "DoD":
                case "Degree of Difficulty":
                case "Difficulty Score":
                case "Difficulty":
                    key = ConverterConstants.Difficulty;
                    break;
                case "Difficulty Judge 1 Points":
                case "Difficulty Judge 1 Score":
                    key = ConverterConstants.DifficultyJudge1;
                    break;
                case "Difficulty Judge 2 Points":
                case "Difficulty Judge 1-1 Score":
                    key = ConverterConstants.DifficultyJudge2;
                    break;
                case "Difficulty Judge 3 Points":
                case "Difficulty Judge 1-2 Score":
                    key = ConverterConstants.DifficultyJudge3;
                    break;
                case "Difficulty Judge 4 Points":
                case "Difficulty Judge 2 Score":
                    key = ConverterConstants.DifficultyJudge4;
                    break;
                case "Difficulty Judge 5 Points":
                case "Difficulty Judge 2-1 Score":
                    key = ConverterConstants.DifficultyJudge5;
                    break;
                case "Difficulty Judge 2-2 Score":
                    key = ConverterConstants.DifficultyJudge6;
                    break;
                case "Difficulty Penalty Score":
                    key = ConverterConstants.DifficultyPenalty;
                    break;
                case "Digs":
                    key = ConverterConstants.Digs;
                    break;
                case "Dig Successes":
                    key = ConverterConstants.DigSuccesses;
                    break;
                case "Discus Throw":
                    key = ConverterConstants.DiscusThrow;
                    break;
                case "Disqualifications":
                case "DQ":
                    key = ConverterConstants.Disqualifcations;
                    break;
                case "Disqualifications with Report":
                    key = ConverterConstants.DisqualificationsReport;
                    break;
                case "D()":
                case "Distance":
                //case "D(I)":
                //case "Distance (Imp.)":
                //case "Distance (Imperial)":
                case "Distance Points":
                    key = ConverterConstants.Distance;
                    break;
                case "Dive":
                case "DC#":
                    key = ConverterConstants.Dive;
                    break;
                case "Dive #1":
                    key = ConverterConstants.Dive1;
                    break;
                case "Dive #2":
                    key = ConverterConstants.Dive2;
                    break;
                case "Dive #3":
                    key = ConverterConstants.Dive3;
                    break;
                case "Dive #4":
                    key = ConverterConstants.Dive4;
                    break;
                case "Dive #5":
                    key = ConverterConstants.Dive5;
                    break;
                case "Dive #6":
                    key = ConverterConstants.Dive6;
                    break;
                case "Double Exclusion":
                    key = ConverterConstants.DoubleExclusion;
                    break;
                case "Inning 4/Doubles":
                case "4th Inning/Doubles":
                    key = ConverterConstants.Doubles;
                    break;
                case "Downhill":
                    key = ConverterConstants.Downhill;
                    break;
                case "Bouts Tied":
                case "BWT":
                case "MT":
                case "Ties":
                case "T":
                    key = ConverterConstants.Draw;
                    break;
                case "Draws":
                    key = ConverterConstants.Draws;
                    break;
                case "Dressage":
                    key = ConverterConstants.Dressage;
                    break;
                case "Dressage Penalty Points":
                    key = ConverterConstants.DressagePenaltyPoints;
                    break;
                case "TDP":
                    key = ConverterConstants.DrillPoints;
                    break;
                case "Driving Goals/Attempts":
                    key = ConverterConstants.DrivingGoals;
                    break;
                case "Drop Goals":
                    key = ConverterConstants.DropGoals;
                    break;
                case "D Score":
                    key = ConverterConstants.Dscore;
                    break;
                case "Earned Run Average":
                    key = ConverterConstants.EarnedRunAverage;
                    break;
                case "Earned Runs Allowed":
                case "Team Runs/Earned Runs Allowed":
                    key = ConverterConstants.EarnedRunsAllowed;
                    break;
                case "Elimination Race":
                case "Elimination Race Points":
                    key = ConverterConstants.EliminationRace;
                    break;
                case "Elimination Round":
                case "Elimination Round Points":
                    key = ConverterConstants.EliminationRound;
                    break;
                case "Empty Goal Shots / Attempts":
                    key = ConverterConstants.EmptyGoalShots;
                    break;
                case "Errors":
                    key = ConverterConstants.Erros;
                    break;
                case "E Score":
                    key = ConverterConstants.Escore;
                    break;
                case "Event Points":
                    key = ConverterConstants.EventPoints;
                    break;
                case "Exchange":
                case "Exchange (Pos)":
                    key = ConverterConstants.Exchange;
                    break;
                case "Exclusion":
                    key = ConverterConstants.Exclusion;
                    break;
                case "Exclusion (20 seconds)":
                    key = ConverterConstants.Exclusion20Seconds;
                    break;
                case "Exclusion (20 seconds) (Centre/Field)":
                    key = ConverterConstants.Exclusion20SecondsCentre;
                    break;
                case "Exclusions (35 seconds)":
                    key = ConverterConstants.Exclusion35Seconds;
                    break;
                case "Exclusions":
                    key = ConverterConstants.Exclusions;
                    break;
                case "Exclusion with Sub":
                    key = ConverterConstants.ExclusionSub;
                    break;
                case "Exclusion (w/wo Sub)":
                    key = ConverterConstants.ExclusionWSub;
                    break;
                case "Execution":
                case "Execution Score":
                case "Execution Reference Score":
                    key = ConverterConstants.Execution;
                    break;
                case "Execution Judge 1 Points":
                case "J1":
                case "J1S":
                case "E1":
                case "EJ1S":
                case "Execution Judge 1 Score":
                    key = ConverterConstants.ExecutionJudge1;
                    break;
                case "Execution Judge 2 Points":
                case "J2":
                case "J2S":
                case "E2":
                case "EJ2S":
                case "Execution Judge 2 Score":
                    key = ConverterConstants.ExecutionJudge2;
                    break;
                case "Execution Judge 3 Points":
                case "J3":
                case "J3S":
                case "E3":
                case "EJ3S":
                case "Execution Judge 3 Score":
                    key = ConverterConstants.ExecutionJudge3;
                    break;
                case "Execution Judge 4 Points":
                case "J4":
                case "J4S":
                case "E4":
                case "Execution Judge 4 Score":
                    key = ConverterConstants.ExecutionJudge4;
                    break;
                case "Execution Judge 5 Points":
                case "J5":
                case "J5S":
                case "E5":
                    key = ConverterConstants.ExecutionJudge5;
                    break;
                case "Execution Judge 6 Points":
                case "J6":
                case "J6S":
                case "E6":
                    key = ConverterConstants.ExecutionJudge6;
                    break;
                case "Execution Judge 7 Points":
                case "J7":
                case "J7S":
                    key = ConverterConstants.ExecutionJudge7;
                    break;
                case "Execution Judge Average":
                    key = ConverterConstants.ExecutionJudgeAverage;
                    break;
                case "Execution Judge Points":
                    key = ConverterConstants.ExecutionJudgePoints;
                    break;
                case "Execution Penalty":
                    key = ConverterConstants.ExecutionPenalty;
                    break;
                case "Execution Points":
                    key = ConverterConstants.ExecutionPoints;
                    break;
                case "Execution Reference Points":
                    key = ConverterConstants.ExecutionReferencePoints;
                    break;
                case "Execution Reference 1 Points":
                    key = ConverterConstants.ExecutionReferencePoints1;
                    break;
                case "Execution Reference 2 Points":
                    key = ConverterConstants.ExecutionReferencePoints2;
                    break;
                case "Extraman Goals":
                case "Extraman Goals/Attempts":
                    key = ConverterConstants.ExtramanGoals;
                    break;
                case "Extra Shots":
                    key = ConverterConstants.ExtraShots;
                    break;
                case "After Extra Time":
                    key = ConverterConstants.ExtraTime;
                    break;
                case "Extra-time 1 Score":
                    key = ConverterConstants.ExtraTime1;
                    break;
                case "Extra-time 2 Score":
                    key = ConverterConstants.ExtraTime2;
                    break;
                case "Extra-Time 3 Score":
                    key = ConverterConstants.ExtraTime3;
                    break;
                case "Extra-Time 2 Score / Penalty Shoot-Out":
                case "Extra-time 3/4/5/6 Score":
                    key = ConverterConstants.ExtraTimeScore;
                    break;
                case "Fastbreak Shots / Attempts - Saves / Shots":
                    key = ConverterConstants.FastbreakShots;
                    break;
                case "Fastest Serve":
                    key = ConverterConstants.FastestServe;
                    break;
                case "Fast Run":
                case "Fast Run Points":
                    key = ConverterConstants.FastRun;
                    break;
                case "Faults":
                    key = ConverterConstants.Faults;
                    break;
                case "Fencing":
                    key = ConverterConstants.Fencing;
                    break;
                case "Field Exclusion":
                    key = ConverterConstants.FieldExclusion;
                    break;
                case "Field Goals":
                    key = ConverterConstants.FieldGoal;
                    break;
                case "Field Goal Attempts":
                    key = ConverterConstants.FieldGoalAttempts;
                    break;
                case "FG/FGA":
                    key = ConverterConstants.FieldGoalFieldGoalAttempts;
                    break;
                case "FG":
                    key = ConverterConstants.FieldGoals;
                    break;
                case "Fielding Average":
                    key = ConverterConstants.FieldingAverage;
                    break;
                case "Figure 1":
                    key = ConverterConstants.Figure1;
                    break;
                case "Figure 2":
                    key = ConverterConstants.Figure2;
                    break;
                case "Figure 3":
                    key = ConverterConstants.Figure3;
                    break;
                case "Figure Points":
                case "Figures":
                    key = ConverterConstants.Figures;
                    break;
                case "Final":
                case "FP":
                case "FRP":
                case "Jumping Final":
                case "Final Round":
                case "Final Points":
                case "Final Round Points":
                    key = ConverterConstants.Final;
                    break;
                case "Final Round 1":
                    key = ConverterConstants.FinalRound1;
                    break;
                case "Final Round 2":
                    key = ConverterConstants.FinalRound2;
                    break;
                case "Final Round 3":
                    key = ConverterConstants.FinalRound3;
                    break;
                case "First Final":
                    key = ConverterConstants.FirstFinal;
                    break;
                case "1TT":
                    key = ConverterConstants.FirstTimeTrial;
                    break;
                case "Floor Exercise":
                case "FE":
                    key = ConverterConstants.FloorExercise;
                    break;
                case "250 m Flying Start":
                    key = ConverterConstants.FlyingStart250;
                    break;
                case "Form Score":
                    key = ConverterConstants.FormScore;
                    break;
                case "Fouls Committed":
                    key = ConverterConstants.FoulsCommitted;
                    break;
                case "Free":
                    key = ConverterConstants.Free;
                    break;
                case "Free Dance":
                    key = ConverterConstants.FreeDance;
                    break;
                case "FDFP":
                    key = ConverterConstants.FreeDanceFactoredPlacements;
                    break;
                case "Free Kicks":
                    key = ConverterConstants.FreeKicks;
                    break;
                case "Free Routine":
                case "Free Routine Points":
                    key = ConverterConstants.FreeRoutine;
                    break;
                case "Free Skating":
                case "Women's Free Skating":
                case "Pairs Free Skating":
                case "Men's Free Skating":
                    key = ConverterConstants.FreeSkating;
                    break;
                case "Freestyle":
                    key = ConverterConstants.Freestyle;
                    break;
                case "FT":
                    key = ConverterConstants.FreeThrows;
                    break;
                case "Game 1":
                    key = ConverterConstants.Game1;
                    break;
                case "Game 2":
                    key = ConverterConstants.Game2;
                    break;
                case "Game 3":
                    key = ConverterConstants.Game3;
                    break;
                case "Game 4":
                    key = ConverterConstants.Game4;
                    break;
                case "Game 5":
                    key = ConverterConstants.Game5;
                    break;
                case "Game 6":
                    key = ConverterConstants.Game6;
                    break;
                case "Game 7":
                    key = ConverterConstants.Game7;
                    break;
                case "GP":
                    key = ConverterConstants.GamePlayed;
                    break;
                case "Games Won":
                case "Games":
                    key = ConverterConstants.Games;
                    break;
                case "Gate":
                    key = ConverterConstants.Gate;
                    break;
                case "Gate Points":
                    key = ConverterConstants.GatePoints;
                    break;
                case "Goal from Mark":
                    key = ConverterConstants.GoalFromMark;
                    break;
                case "GK In":
                    key = ConverterConstants.GoalkeeperIn;
                    break;
                case "GK Out":
                    key = ConverterConstants.GoalkeeperOut;
                    break;
                case "Goals":
                case "GLS":
                case "Goals/Saves":
                    key = ConverterConstants.Goals;
                    break;
                case "5-metre Goals/Attempts":
                    key = ConverterConstants.Goals5M;
                    break;
                case "6-metre Goals/Attempts":
                    key = ConverterConstants.Goals6M;
                    break;
                case "7-metre Goals/Attempts":
                    key = ConverterConstants.Goals7M;
                    break;
                case "GAA":
                    key = ConverterConstants.GoalsAgainst;
                    break;
                case "Goals / Saves":
                    key = ConverterConstants.GoalsSaves;
                    break;
                case "Goals/Shots":
                    key = ConverterConstants.GoalsShots;
                    break;
                case "Golds":
                    key = ConverterConstants.Golds;
                    break;
                case "Grand Prix":
                    key = ConverterConstants.GrandPrix;
                    break;
                case "Grand Prix Freestyle":
                    key = ConverterConstants.GrandPrixFreestyle;
                    break;
                case "Grand Prix Freestyle Artistic Points":
                    key = ConverterConstants.GrandPrixFreestyleArtisticPoints;
                    break;
                case "Grand Prix Freestyle Points":
                    key = ConverterConstants.GrandPrixFreestylePoints;
                    break;
                case "Grand Prix Freestyle Technical Points":
                    key = ConverterConstants.GrandPrixFreestyleTechnicalPoints;
                    break;
                case "Grand Prix Special":
                    key = ConverterConstants.GrandPrixSpecial;
                    break;
                case "Grand Prix Special Points":
                    key = ConverterConstants.GrandPrixSpecialPoints;
                    break;
                case "Green":
                case "Green Cards":
                    key = ConverterConstants.GreenCards;
                    break;
                case "Group":
                    key = ConverterConstants.Group;
                    break;
                case "Group Exercise Points":
                case "GEP":
                    key = ConverterConstants.GroupExercise;
                    break;
                case "1st-Half Points":
                    key = ConverterConstants.Half1;
                    break;
                case "2nd-Half Points":
                    key = ConverterConstants.Half2;
                    break;
                case "Half (Pos)":
                case "Half-Marathon":
                    key = ConverterConstants.HalfMarathon;
                    break;
                case "50% Points":
                    key = ConverterConstants.HalfPoints;
                    break;
                case "QP(50%)":
                    key = ConverterConstants.HalfQualificationPoints;
                    break;
                case "ATP(50%)":
                    key = ConverterConstants.HalfTeamPoints;
                    break;
                case "Hammer Throw":
                    key = ConverterConstants.HammerThrow;
                    break;
                case "Handicap":
                    key = ConverterConstants.Handicap;
                    break;
                case "Hand Used":
                    key = ConverterConstants.HandUsed;
                    break;
                case "Height":
                    key = ConverterConstants.Height;
                    break;
                case "High Jump":
                    key = ConverterConstants.HighJump;
                    break;
                case "3rd Inning/Hits":
                case "Inning 3/Hits":
                case "Hits":
                    key = ConverterConstants.Hits;
                    break;
                case "Hits Allowed":
                case "Team Errors/Hits Allowed":
                    key = ConverterConstants.HitsAllowed;
                    break;
                case "Hold Reached":
                    key = ConverterConstants.HoldReached;
                    break;
                case "Holes Up/To Play":
                    key = ConverterConstants.Holes;
                    break;
                case "Home Runs":
                case "Inning 6/Home Runs":
                case "6th Inning/Home Runs":
                    key = ConverterConstants.HomeRuns;
                    break;
                case "Home Runs Allowed":
                    key = ConverterConstants.HomeRunsAllowed;
                    break;
                case "Hoop":
                case "Hoop Score":
                case "Hoop/Ball":
                case "Hoop/Ribbon":
                case "Hoops/Clubs":
                case "Hoops/Clubs Score":
                    key = ConverterConstants.Hoop;
                    break;
                case "Horizontal Bar":
                case "HB":
                    key = ConverterConstants.HorizontalBar;
                    break;
                case "Horizontal Displacement Points":
                    key = ConverterConstants.HorizontalDisplacementPoints;
                    break;
                case "Horse":
                    key = ConverterConstants.Horse;
                    break;
                case "Horse Vault":
                case "HV":
                    key = ConverterConstants.HorseVault;
                    break;
                case "Ice Dance Free Dance":
                    key = ConverterConstants.IceDanceFreeDance;
                    break;
                case "Ice Dance Rhythm Dance":
                    key = ConverterConstants.IceDanceRhythmDance;
                    break;
                case "Ice Dance Short Dance":
                    key = ConverterConstants.IceDanceShortDance;
                    break;
                case "Impr.":
                    key = ConverterConstants.Improvisation;
                    break;
                case "IO":
                case "AIO":
                    key = ConverterConstants.IndividualOrdinalsPoints;
                    break;
                case "Individual Penalty Points":
                    key = ConverterConstants.IndividualPenaltyPoints;
                    break;
                case "Individual Points":
                case "IP":
                case "SIP":
                    key = ConverterConstants.IndividualPoints;
                    break;
                case "3,000 m Individual Pursuit":
                    key = ConverterConstants.IndividualPursuit3000;
                    break;
                case "4,000 m Individual Pursuit":
                    key = ConverterConstants.IndividualPursuit4000;
                    break;
                case "Inner 10s":
                    key = ConverterConstants.Inner10s;
                    break;
                case "Result First Innings":
                    key = ConverterConstants.Inning1;
                    break;
                case "Result Second Innings":
                    key = ConverterConstants.Inning2;
                    break;
                case "Innings Pitched":
                case "15th Inning/Innings Pitched":
                case "Home-Visiting Team/Innings Pitched":
                    key = ConverterConstants.InningsPitched;
                    break;
                case "In/Out":
                    key = ConverterConstants.InOut;
                    break;
                case "Intermediate":
                    key = ConverterConstants.Intermediate;
                    break;
                case "Intermediate 1":
                case "Intermediate Time 1":
                    key = ConverterConstants.Intermediate1;
                    break;
                case "Intermediate 10":
                    key = ConverterConstants.Intermediate10;
                    break;
                case "Intermediate 2":
                case "Intermediate Time 2":
                    key = ConverterConstants.Intermediate2;
                    break;
                case "Intermediate 3":
                case "Intermediate Time 3":
                    key = ConverterConstants.Intermediate3;
                    break;
                case "Intermediate 4":
                case "Intermediate Time 4":
                    key = ConverterConstants.Intermediate4;
                    break;
                case "Intermediate 5":
                case "Intermediate Time 5":
                    key = ConverterConstants.Intermediate5;
                    break;
                case "Intermediate 6":
                    key = ConverterConstants.Intermediate6;
                    break;
                case "Intermediate 7":
                    key = ConverterConstants.Intermediate7;
                    break;
                case "Intermediate 8":
                    key = ConverterConstants.Intermediate8;
                    break;
                case "Intermediate 9":
                    key = ConverterConstants.Intermediate9;
                    break;
                case "Ippon":
                    key = ConverterConstants.Ippon;
                    break;
                case "Scored?":
                    key = ConverterConstants.IsScored;
                    break;
                case "Javelin Throw":
                    key = ConverterConstants.JavelinThrow;
                    break;
                case "Judge #1 Score":
                case "Original Judge #1 Score":
                case "Judge #1":
                case "Athletic Points Judge 1":
                case "Judge 1 Points":
                    key = ConverterConstants.Judge1;
                    break;
                case "Judge #10":
                    key = ConverterConstants.Judge10;
                    break;
                case "Judge #11":
                    key = ConverterConstants.Judge11;
                    break;
                case "Judge 1(Air &amp; Form) Score":
                case "Judge 1 Air &amp; Form Score":
                    key = ConverterConstants.Judge1AirFormScore;
                    break;
                case "Judge 1 Air Score":
                    key = ConverterConstants.Judge1AirScore;
                    break;
                case "Judge 1 (Turns) Deductions":
                    key = ConverterConstants.Judge1Deductions;
                    break;
                case "Judge 1 Form Score":
                    key = ConverterConstants.Judge1FormScore;
                    break;
                case "Judge 1 Landing Score":
                    key = ConverterConstants.Judge1LandingScore;
                    break;
                case "Judge 1 (Turns) Score":
                case "Judge 1 Score":
                case "Judge #1 Score (Standard Airs)":
                    key = ConverterConstants.Judge1Score;
                    break;
                case "Technical Points Judge 1":
                    key = ConverterConstants.Judge1TechnicalPoints;
                    break;
                case "Judge #2 Score":
                case "Original Judge #2 Score":
                case "Judge #2":
                case "Athletic Points Judge 2":
                case "Judge 2 Points":
                    key = ConverterConstants.Judge2;
                    break;
                case "Judge 2(Air &amp; Form) Score":
                case "Judge 2 Air &amp;  Form Score":
                    key = ConverterConstants.Judge2AirFormScore;
                    break;
                case "Judge 2 Air Score":
                    key = ConverterConstants.Judge2AirScore;
                    break;
                case "Judge 2 (Turns) Deductions":
                    key = ConverterConstants.Judge2Deductions;
                    break;
                case "Judge 2 Form Score":
                    key = ConverterConstants.Judge2FormScore;
                    break;
                case "Judge 2 Landing Score":
                    key = ConverterConstants.Judge2LandingScore;
                    break;
                case "Judge 2 (Turns) Score":
                case "Judge 2 Score":
                case "Judge #2 Score (Rotations)":
                    key = ConverterConstants.Judge2Score;
                    break;
                case "Technical Points Judge 2":
                    key = ConverterConstants.Judge2TechnicalPoints;
                    break;
                case "Judge #3 Score":
                case "Original Judge #3 Score":
                case "Judge #3":
                case "Athletic Points Judge 3":
                case "Judge 3 Points":
                    key = ConverterConstants.Judge3;
                    break;
                case "Judge 3(Air &amp; Form) Score":
                case "Judge 3 Air &amp; Form Score":
                    key = ConverterConstants.Judge3AirFormScore;
                    break;
                case "Judge 3 Air Score":
                case "Judge 3 Score":
                    key = ConverterConstants.Judge3AirScore;
                    break;
                case "Judge 3 (Turns) Deductions":
                    key = ConverterConstants.Judge3Deductions;
                    break;
                case "Judge 3 Form Score":
                    key = ConverterConstants.Judge3FormScore;
                    break;
                case "Judge 3 Landing Score":
                    key = ConverterConstants.Judge3LandingScore;
                    break;
                case "Judge 3 (Turns) Score":
                case "Judge #3 Score (Amplitude)":
                    key = ConverterConstants.Judge3Score;
                    break;
                case "Technical Points Judge 3":
                    key = ConverterConstants.Judge3TechnicalPoints;
                    break;
                case "Judge #4 Score":
                case "Original Judge #4 Score":
                case "Judge #4":
                case "Athletic Points Judge 4":
                case "Judge 4 Points":
                    key = ConverterConstants.Judge4;
                    break;
                case "Judge 4(Air &amp; Form) Score":
                case "Judge 4 Air &amp; Form Score":
                    key = ConverterConstants.Judge4AirFormScore;
                    break;
                case "Judge 4 Air Score":
                    key = ConverterConstants.Judge4AirScore;
                    break;
                case "Judge 4 (Turns) Deductions":
                    key = ConverterConstants.Judge4Deductions;
                    break;
                case "Judge 4 Form Score":
                    key = ConverterConstants.Judge4FormScore;
                    break;
                case "Judge 4 Landing Score":
                    key = ConverterConstants.Judge4LandingScore;
                    break;
                case "Judge 4 (Turns) Score":
                case "Judge 4 Score":
                case "Judge #4 Score (Overall #1)":
                case "Judge #4 Score (Landings)":
                    key = ConverterConstants.Judge4Score;
                    break;
                case "Technical Points Judge 4":
                    key = ConverterConstants.Judge4TechnicalPoints;
                    break;
                case "Judge #5 Score":
                case "Original Judge #5 Score":
                case "Judge #5":
                case "Athletic Points Judge 5":
                case "Judge 5 Points":
                    key = ConverterConstants.Judge5;
                    break;
                case "Judge 7 (Air #1) Score":
                    key = ConverterConstants.Judge7Air1Score;
                    break;
                case "Judge 7 (Air #2) Score":
                    key = ConverterConstants.Judge7Air2Score;
                    break;
                case "Judge 5(Air &amp; Form) Score":
                case "Judge 5 Air &amp; Form Score":
                    key = ConverterConstants.Judge5AirFormScore;
                    break;
                case "Judge 5 Air Score":
                    key = ConverterConstants.Judge5AirScore;
                    break;
                case "Judge 7 (Air) Score":
                    key = ConverterConstants.Judge7AirScore;
                    break;
                case "Judge 5 (Turns) Deductions":
                    key = ConverterConstants.Judge5Deductions;
                    break;
                case "Judge 5 Form Score":
                    key = ConverterConstants.Judge5FormScore;
                    break;
                case "Judge 5 Landing Score":
                    key = ConverterConstants.Judge5LandingScore;
                    break;
                case "Judge 7 (Landing) Score":
                    key = ConverterConstants.Judge7LandingScore;
                    break;
                case "Judge 5 (Turns) Score":
                case "Judge 5 Score":
                case "Judge #5 Score (Technical Merit)":
                case "Judge #5 Score (Overall #2)":
                    key = ConverterConstants.Judge5Score;
                    break;
                case "Technical Points Judge 5":
                    key = ConverterConstants.Judge5TechnicalPoints;
                    break;
                case "Judge #6":
                case "Athletic Points Judge 6":
                    key = ConverterConstants.Judge6;
                    break;
                case "Judge 6 (Air #1) Score":
                    key = ConverterConstants.Judge6Air1Score;
                    break;
                case "Judge 6 (Air #2) Score":
                    key = ConverterConstants.Judge6Air2Score;
                    break;
                case "Judge 6 (Air) Score":
                    key = ConverterConstants.Judge6AirScore;
                    break;
                case "Judge 6 (Landing) Score":
                    key = ConverterConstants.Judge6LandingScore;
                    break;
                case "Judge 6 Score":
                case "Judge #6 Score":
                    key = ConverterConstants.Judge6Score;
                    break;
                case "Technical Points Judge 6":
                    key = ConverterConstants.Judge6TechnicalPoints;
                    break;
                case "Judge #7":
                case "Athletic Points Judge 7":
                    key = ConverterConstants.Judge7;
                    break;
                case "Technical Points Judge 7":
                    key = ConverterConstants.Judge7TechnicalPoints;
                    break;
                case "Judge #8":
                    key = ConverterConstants.Judge8;
                    break;
                case "Judge #9":
                    key = ConverterConstants.Judge9;
                    break;
                case "Judge B":
                    key = ConverterConstants.JudgeB;
                    break;
                case "Judge C":
                case "Judge C Points":
                    key = ConverterConstants.JudgeC;
                    break;
                case "Judge E":
                case "Judge E Points":
                    key = ConverterConstants.JudgeE;
                    break;
                case "Judge F":
                    key = ConverterConstants.JudgeF;
                    break;
                case "Judge H":
                    key = ConverterConstants.JudgeH;
                    break;
                case "Judge K":
                    key = ConverterConstants.JudgeK;
                    break;
                case "Judge M":
                case "Judge M Points":
                    key = ConverterConstants.JudgeM;
                    break;
                case "Total Judges' Points":
                case "Original Total Judges' Points":
                    key = ConverterConstants.Judges;
                    break;
                case "Judges Favoring":
                case "Original Judges Favoring":
                    key = ConverterConstants.JudgesFavoring;
                    break;
                case "Jump #1":
                case "Jump 1 Points":
                    key = ConverterConstants.Jump1;
                    break;
                case "Jump #2":
                case "Jump 2 Points":
                    key = ConverterConstants.Jump2;
                    break;
                case "Jump #1 Code":
                    key = ConverterConstants.Jump1Code;
                    break;
                case "Jump #2 Code":
                    key = ConverterConstants.Jump2Code;
                    break;
                case "Jump #1 Degree of Difficulty":
                    key = ConverterConstants.Jump1Difficulty;
                    break;
                case "Jump #2 Degree of Difficulty":
                    key = ConverterConstants.Jump2Difficulty;
                    break;
                case "Jump #3":
                    key = ConverterConstants.Jump3;
                    break;
                case "Jump Code":
                case "Jump ID":
                    key = ConverterConstants.JumpCode;
                    break;
                case "Jumping":
                    key = ConverterConstants.Jumping;
                    break;
                case "Jump-Off":
                    key = ConverterConstants.JumpOff;
                    break;
                case "Jump-Off Faults":
                    key = ConverterConstants.JumpOffFaults;
                    break;
                case "Jump-Off Points/Time":
                    key = ConverterConstants.JumpOffPointsTime;
                    break;
                case "Jump-Off Time":
                    key = ConverterConstants.JumpOffTime;
                    break;
                case "Jump One Penalty Points":
                    key = ConverterConstants.JumpOnePenaltyPoints;
                    break;
                case "Jump Penalties":
                    key = ConverterConstants.JumpPenalties;
                    break;
                case "Jump Two Penalty Points":
                    key = ConverterConstants.JumpTwoPenaltyPoints;
                    break;
                case "Kata":
                    key = ConverterConstants.Kata;
                    break;
                case "Kata #1":
                    key = ConverterConstants.Kata1;
                    break;
                case "Kata #2":
                    key = ConverterConstants.Kata2;
                    break;
                case "Kilograms":
                case "K":
                    key = ConverterConstants.Kilograms;
                    break;
                case "1 km (Pos)":
                case "1 km split (1 km rank)":
                    key = ConverterConstants.Km1;
                    break;
                case "10 km":
                case "10 km (Pos)":
                case "10 km split (10 km rank)":
                    key = ConverterConstants.Km10;
                    break;
                case "11 km split (11 km rank)":
                    key = ConverterConstants.Km11;
                    break;
                case "12 km (Pos)":
                case "12 km split (12 km rank)":
                    key = ConverterConstants.Km12;
                    break;
                case "13 km split (13 km rank)":
                    key = ConverterConstants.Km13;
                    break;
                case "14 km (Pos)":
                case "14 km split (14 km rank)":
                    key = ConverterConstants.Km14;
                    break;
                case "15 km":
                case "15 km (Pos)":
                case "15 km split (15 km rank)":
                    key = ConverterConstants.Km15;
                    break;
                case "16 km (Pos)":
                case "16 km split (16 km rank)":
                    key = ConverterConstants.Km16;
                    break;
                case "17 km split (17 km rank)":
                    key = ConverterConstants.Km17;
                    break;
                case "18 km (Pos)":
                case "18 km split (18 km rank)":
                    key = ConverterConstants.Km18;
                    break;
                case "19 km split (19 km rank)":
                    key = ConverterConstants.Km19;
                    break;
                case "2 km (Pos)":
                case "2 km split (2 km rank)":
                    key = ConverterConstants.Km2;
                    break;
                case "20 km":
                case "20 km (Pos)":
                    key = ConverterConstants.Km20;
                    break;
                case "25 km":
                case "25 km (Pos)":
                    key = ConverterConstants.Km25;
                    break;
                case "26 km (Pos)":
                    key = ConverterConstants.Km26;
                    break;
                case "28 km (Pos)":
                    key = ConverterConstants.Km28;
                    break;
                case "3 km (Pos)":
                case "3 km split (3 km rank)":
                    key = ConverterConstants.Km3;
                    break;
                case "30 km":
                case "30 km (Pos)":
                    key = ConverterConstants.Km30;
                    break;
                case "31 km (Pos)":
                    key = ConverterConstants.Km31;
                    break;
                case "35 km":
                case "35 km (Pos)":
                    key = ConverterConstants.Km35;
                    break;
                case "36 km (Pos)":
                    key = ConverterConstants.Km36;
                    break;
                case "37 km (Pos)":
                    key = ConverterConstants.Km37;
                    break;
                case "38 km (Pos)":
                    key = ConverterConstants.Km38;
                    break;
                case "4 km (Pos)":
                case "4 km split (4 km rank)":
                    key = ConverterConstants.Km4;
                    break;
                case "40 km":
                case "40 km (Pos)":
                    key = ConverterConstants.Km40;
                    break;
                case "45 km":
                case "45 km (Pos)":
                    key = ConverterConstants.Km45;
                    break;
                case "46 km (Pos)":
                    key = ConverterConstants.Km46;
                    break;
                case "5 km":
                case "5 km (Pos)":
                case "5 km split (5 km rank)":
                    key = ConverterConstants.Km5;
                    break;
                case "6 km (Pos)":
                case "6 km split (6 km rank)":
                    key = ConverterConstants.Km6;
                    break;
                case "7 km (Pos)":
                case "7 km split (7 km rank)":
                    key = ConverterConstants.Km7;
                    break;
                case "8 km (Pos)":
                case "8 km split (8 km rank)":
                    key = ConverterConstants.Km8;
                    break;
                case "9 km (Pos)":
                case "9 km split (9 km rank)":
                    key = ConverterConstants.Km9;
                    break;
                case "Kneeling":
                case "Kneeling Points":
                case "Kneeling/Sitting Points":
                    key = ConverterConstants.Kneeling;
                    break;
                case "Landing Points":
                case "Landing Score":
                    key = ConverterConstants.LandingPoints;
                    break;
                case "Lane":
                    key = ConverterConstants.Lane;
                    break;
                case "Lane A Time":
                    key = ConverterConstants.Lane1;
                    break;
                case "Lane B Time":
                    key = ConverterConstants.Lane2;
                    break;
                case "Lap 1":
                case "Lap 1 Time":
                    key = ConverterConstants.Lap1;
                    break;
                case "Lap 2":
                case "Lap 2 Time":
                    key = ConverterConstants.Lap2;
                    break;
                case "Lap 3":
                case "Lap 3 Time":
                    key = ConverterConstants.Lap3;
                    break;
                case "Lap 4":
                case "Lap 4 Time":
                    key = ConverterConstants.Lap4;
                    break;
                case "Lap 5":
                case "Lap 5 Time":
                    key = ConverterConstants.Lap5;
                    break;
                case "Lap 6":
                case "Lap 6 Time":
                    key = ConverterConstants.Lap6;
                    break;
                case "Lap 7":
                case "Lap 7 Time":
                    key = ConverterConstants.Lap7;
                    break;
                case "Lap 8 Time":
                    key = ConverterConstants.Lap8;
                    break;
                case "Lap 9 Time":
                    key = ConverterConstants.Lap9;
                    break;
                case "Lap Points":
                    key = ConverterConstants.LapPoints;
                    break;
                case "Laps":
                    key = ConverterConstants.Laps;
                    break;
                case "Last Race":
                    key = ConverterConstants.LastRace;
                    break;
                case "Last Run Time":
                    key = ConverterConstants.LastRunTime;
                    break;
                case "Lead":
                    key = ConverterConstants.Lead;
                    break;
                case "Leg 1":
                    key = ConverterConstants.Leg1;
                    break;
                case "Leg 2":
                    key = ConverterConstants.Leg2;
                    break;
                case "Leg 3":
                    key = ConverterConstants.Leg3;
                    break;
                case "Leg Rank":
                    key = ConverterConstants.LegRank;
                    break;
                case "LG":
                    key = ConverterConstants.LG;
                    break;
                case "Lift 1":
                    key = ConverterConstants.Lift1;
                    break;
                case "Lift 2":
                    key = ConverterConstants.Lift2;
                    break;
                case "Lift 3":
                    key = ConverterConstants.Lift3;
                    break;
                case "Line Penalty":
                    key = ConverterConstants.LinePenalty;
                    break;
                case "Location":
                    key = ConverterConstants.Location;
                    break;
                case "LJP":
                case "Long Jump":
                    key = ConverterConstants.LongJump;
                    break;
                case "Losses":
                case "L":
                case "Bouts Lost":
                case "ML":
                    key = ConverterConstants.Losses;
                    break;
                case "Losses/Runs":
                    key = ConverterConstants.LossesRuns;
                    break;
                case "Loss of Contact":
                case "Loss of Contact Warnings":
                    key = ConverterConstants.LostOfContact;
                    break;
                case "100 metres":
                case "100 m":
                case "100 metre split":
                case "100 m Time":
                    key = ConverterConstants.M100;
                    break;
                case "1,000 m":
                case "1000 m":
                    key = ConverterConstants.M1000;
                    break;
                case "1,000-1,500 m":
                    key = ConverterConstants.M1000_1500;
                    break;
                case "1,000-2,000 m":
                    key = ConverterConstants.M1000_2000;
                    break;
                case "10,000 m":
                    key = ConverterConstants.M10000;
                    break;
                case "100 metres Hurdles":
                    key = ConverterConstants.M100Hurdles;
                    break;
                case "1100 m":
                case "1,100 m":
                    key = ConverterConstants.M1100;
                    break;
                case "110 metres Hurdles":
                    key = ConverterConstants.M110Hurdles;
                    break;
                case "1200 m":
                case "1,200 m":
                    key = ConverterConstants.M1200;
                    break;
                case "1,300 m":
                    key = ConverterConstants.M1300;
                    break;
                case "1,303 m":
                    key = ConverterConstants.M1303;
                    break;
                case "1400 m":
                    key = ConverterConstants.M1400;
                    break;
                case "150 m":
                case "150 metre split":
                case "150 m Time":
                    key = ConverterConstants.M150;
                    break;
                case "1,500 metres":
                case "1,500 m":
                    key = ConverterConstants.M1500;
                    break;
                case "1,500-2,000 m":
                    key = ConverterConstants.M1500_2000;
                    break;
                case "1600 m":
                    key = ConverterConstants.M1600;
                    break;
                case "1800 m":
                    key = ConverterConstants.M1800;
                    break;
                case "200 metres":
                case "200 m":
                case "200 m Time":
                    key = ConverterConstants.M200;
                    break;
                case "2000 m":
                case "2,000 m":
                    key = ConverterConstants.M2000;
                    break;
                case "2200 m":
                    key = ConverterConstants.M2200;
                    break;
                case "2400 m":
                    key = ConverterConstants.M2400;
                    break;
                case "250 m":
                case "250 m Time":
                    key = ConverterConstants.M250;
                    break;
                case "250-500 m":
                case "250-500 m Time":
                    key = ConverterConstants.M250_500;
                    break;
                case "2600 m":
                    key = ConverterConstants.M2600;
                    break;
                case "2800 m":
                    key = ConverterConstants.M2800;
                    break;
                case "30 m":
                    key = ConverterConstants.M30;
                    break;
                case "300 m":
                case "300 m Time":
                    key = ConverterConstants.M300;
                    break;
                case "3000 m":
                case "3,000 m":
                    key = ConverterConstants.M3000;
                    break;
                case "3200 m":
                    key = ConverterConstants.M3200;
                    break;
                case "3400 m":
                    key = ConverterConstants.M3400;
                    break;
                case "350 m":
                case "350 m Time":
                    key = ConverterConstants.M350;
                    break;
                case "3600 m":
                    key = ConverterConstants.M3600;
                    break;
                case "3800 m":
                    key = ConverterConstants.M3800;
                    break;
                case "400 metres":
                case "400 m":
                    key = ConverterConstants.M400;
                    break;
                case "4000 m":
                    key = ConverterConstants.M4000;
                    break;
                case "4200 m":
                    key = ConverterConstants.M4200;
                    break;
                case "4400 m":
                    key = ConverterConstants.M4400;
                    break;
                case "4600 m":
                    key = ConverterConstants.M4600;
                    break;
                case "4800 m":
                    key = ConverterConstants.M4800;
                    break;
                case "50 m":
                case "50 metre split":
                case "50 m Time":
                    key = ConverterConstants.M50;
                    break;
                case "500 m":
                    key = ConverterConstants.M500;
                    break;
                case "500-1,000 m":
                    key = ConverterConstants.M500_1000;
                    break;
                case "500-750 m":
                    key = ConverterConstants.M500_750;
                    break;
                case "5,000 m":
                    key = ConverterConstants.M5000;
                    break;
                case "5200 m":
                    key = ConverterConstants.M5200;
                    break;
                case "5600 m":
                    key = ConverterConstants.M5600;
                    break;
                case "6000 m":
                    key = ConverterConstants.M5800;
                    break;
                case "60 m":
                    key = ConverterConstants.M60;
                    break;
                case "600 m":
                    key = ConverterConstants.M600;
                    break;
                case "6400 m":
                    key = ConverterConstants.M6000;
                    break;
                case "6800 m":
                    key = ConverterConstants.M6800;
                    break;
                case "70 m":
                    key = ConverterConstants.M70;
                    break;
                case "700 m":
                    key = ConverterConstants.M700;
                    break;
                case "7200 m":
                    key = ConverterConstants.M7200;
                    break;
                case "750 m":
                    key = ConverterConstants.M750;
                    break;
                case "750-1,000 m":
                    key = ConverterConstants.M750_1000;
                    break;
                case "7600 m":
                    key = ConverterConstants.M7600;
                    break;
                case "800 metres":
                case "800 m":
                    key = ConverterConstants.M800;
                    break;
                case "8000 m":
                    key = ConverterConstants.M8000;
                    break;
                case "80 metres Hurdles":
                    key = ConverterConstants.M80Hurdles;
                    break;
                case "8400 m":
                    key = ConverterConstants.M8400;
                    break;
                case "8800 m":
                    key = ConverterConstants.M8800;
                    break;
                case "90 m":
                    key = ConverterConstants.M90;
                    break;
                case "900 m":
                    key = ConverterConstants.M900;
                    break;
                case "9200 m":
                    key = ConverterConstants.M9200;
                    break;
                case "9600 m":
                    key = ConverterConstants.M9600;
                    break;
                case "Maj. Ordinals":
                case "MO":
                    key = ConverterConstants.MajorityOrdinals;
                    break;
                case "Maj. Placements":
                case "MP":
                    key = ConverterConstants.MajorityPlacements;
                    break;
                case "Margin":
                case "Lap Margin":
                    key = ConverterConstants.Margin;
                    break;
                case "Mark 1":
                    key = ConverterConstants.Mark1;
                    break;
                case "Mark 2":
                    key = ConverterConstants.Mark2;
                    break;
                case "Mark 3":
                    key = ConverterConstants.Mark3;
                    break;
                case "Mark 4":
                    key = ConverterConstants.Mark4;
                    break;
                case "Mark 5":
                    key = ConverterConstants.Mark5;
                    break;
                case "Mark 6":
                    key = ConverterConstants.Mark6;
                    break;
                case "Mark 7":
                    key = ConverterConstants.Mark7;
                    break;
                case "Mark 8":
                    key = ConverterConstants.Mark8;
                    break;
                case "Mark 9":
                    key = ConverterConstants.Mark9;
                    break;
                case "Match":
                    key = ConverterConstants.Match;
                    break;
                case "Matches":
                    key = ConverterConstants.Matches;
                    break;
                case "Matches Played":
                    key = ConverterConstants.MatchesPlayed;
                    break;
                case "Match Result":
                    key = ConverterConstants.MatchResult;
                    break;
                case "Medal Race":
                case "MRP":
                    key = ConverterConstants.MedalRace;
                    break;
                case "Merit":
                    key = ConverterConstants.Merit;
                    break;
                case "1 mile":
                    key = ConverterConstants.Mile1;
                    break;
                case "Mins":
                    key = ConverterConstants.Minutes;
                    break;
                case "Total Misses":
                case "Total Misses thru Best Height Cleared":
                case "Misses":
                    key = ConverterConstants.Misses;
                    break;
                case "Total Misses at Best Height Cleared":
                case "Misses at Best Height Cleared":
                    key = ConverterConstants.MissesAtBest;
                    break;
                case "Musical Routine Points":
                    key = ConverterConstants.MusicalRoutinePoints;
                    break;
                case "Team":
                case "Competitors":
                case "Competitor":
                case "Competitor(s)":
                case "Gymnast":
                case "Pair":
                case "Competitor (Seed)":
                case "Pair (Seed)":
                case "Cyclist":
                case "Diver":
                case "Divers":
                case "Competitors (Seed)":
                case "Athlete":
                case "Walker":
                case "Player":
                case "Athlete(s)":
                case "Diver(s)":
                case "Skater":
                case "Skater(s)":
                case "Judoka":
                case "Pentathlete":
                case "Boat":
                case "Swimmer":
                case "Lifter":
                case "Wrestler":
                    key = ConverterConstants.Name;
                    break;
                case "Natural Action Goals":
                case "Natural Action Goals/Attempts":
                    key = ConverterConstants.NaturalActionGoals;
                    break;
                case "Net Points":
                    key = ConverterConstants.NetPoints;
                    break;
                case "NOC":
                case "Nat":
                    key = ConverterConstants.NOC;
                    break;
                case "Notes":
                    key = ConverterConstants.Notes;
                    break;
                case "Number":
                case "Nr":
                case "Bib":
                case "Start Nr.":
                case "Helmet Nr":
                    key = ConverterConstants.Number;
                    break;
                case "Obstacle Penalties":
                    key = ConverterConstants.ObstaclePenalties;
                    break;
                case "Offensive Faults":
                    key = ConverterConstants.OffensiveFaults;
                    break;
                case "ORB":
                    key = ConverterConstants.OffensiveRebounds;
                    break;
                case "Offsides":
                    key = ConverterConstants.Offsides;
                    break;
                case "1P":
                    key = ConverterConstants.OnePoint;
                    break;
                case "Opponent Errors":
                    key = ConverterConstants.OpponentErrors;
                    break;
                case "OEP":
                case "OP":
                case "TOP":
                case "2EP":
                case "O1EP":
                case "Optional Exercise Points":
                    key = ConverterConstants.OptionalPoints;
                    break;
                case "Ord":
                    key = ConverterConstants.Order;
                    break;
                case "Ordinals":
                case "J'O":
                    key = ConverterConstants.Ordinals;
                    break;
                case "Original":
                    key = ConverterConstants.Original;
                    break;
                case "OSDFP":
                    key = ConverterConstants.OriginalSetDanceFactoredPlacements;
                    break;
                case "Original Set Pattern Dance":
                    key = ConverterConstants.OriginalSetPatternDance;
                    break;
                case "Other Penalties":
                    key = ConverterConstants.OtherPenalties;
                    break;
                case "Other Penalty":
                    key = ConverterConstants.OtherPenalty;
                    break;
                case "Overall Impression":
                    key = ConverterConstants.OverallImpression;
                    break;
                case "Overall Impression Judge 1 Points":
                    key = ConverterConstants.OverallImpressionJudge1;
                    break;
                case "Overall Impression Judge 2 Points":
                    key = ConverterConstants.OverallImpressionJudge2;
                    break;
                case "Overall Impression Judge 3 Points":
                    key = ConverterConstants.OverallImpressionJudge3;
                    break;
                case "Overall Impression Judge 4 Points":
                    key = ConverterConstants.OverallImpressionJudge4;
                    break;
                case "Overall Impression Judge 5 Points":
                    key = ConverterConstants.OverallImpressionJudge5;
                    break;
                case "Overall Impression Judge 6 Points":
                    key = ConverterConstants.OverallImpressionJudge6;
                    break;
                case "Overall Impression Judge 7 Points":
                    key = ConverterConstants.OverallImpressionJudge7;
                    break;
                case "Overall Impression Points":
                    key = ConverterConstants.OverallImpressionPoints;
                    break;
                case "Overall Points":
                    key = ConverterConstants.OverallPoints;
                    break;
                case "Overall Score (40%)":
                    key = ConverterConstants.OverallScore40;
                    break;
                case "OTL":
                    key = ConverterConstants.OvertimeLose;
                    break;
                case "OTW":
                    key = ConverterConstants.OvertimeWin;
                    break;
                case "Parallel Bars":
                case "PB":
                    key = ConverterConstants.ParallelBars;
                    break;
                case "Part #1":
                    key = ConverterConstants.Part1;
                    break;
                case "Part #2":
                    key = ConverterConstants.Part2;
                    break;
                case "Part #3":
                    key = ConverterConstants.Part3;
                    break;
                case "Penalties":
                    key = ConverterConstants.Penalties;
                    break;
                case "Penalty":
                    key = ConverterConstants.Penalty;
                    break;
                case "Penalty Corner Attempts":
                    key = ConverterConstants.PenaltyCornerAttmepts;
                    break;
                case "Penalty Corner Goals":
                    key = ConverterConstants.PenaltyCornerGoals;
                    break;
                case "PC/PCA":
                    key = ConverterConstants.PenaltyCornerPenaltyCornerAttempts;
                    break;
                case "Penalty Fouls":
                    key = ConverterConstants.PenaltyFouls;
                    break;
                case "Penalty Goals":
                    key = ConverterConstants.PenaltyGoals;
                    break;
                case "PIM":
                    key = ConverterConstants.PenaltyInfractionMinutes;
                    break;
                case "Penalty Kicks":
                    key = ConverterConstants.PenaltyKicks;
                    break;
                case "Penalty Points":
                case "PP":
                case "Penalty Points Earned":
                    key = ConverterConstants.PenaltyPoints;
                    break;
                case "Penalty Shoot-Out":
                    key = ConverterConstants.PenaltyShootOut;
                    break;
                case "Penalty Shot Goals":
                case "Penalty Shot Goals/Attempts":
                case "Penalty Shots":
                case "Penalty Shots/Attempts":
                    key = ConverterConstants.PenaltyShotGoals;
                    break;
                case "Penalty Stroke Attempts":
                    key = ConverterConstants.PenaltyStrokeAttmepts;
                    break;
                case "Penalty Stroke Goals":
                    key = ConverterConstants.PenaltyStrokeGoals;
                    break;
                case "PS/PSA":
                    key = ConverterConstants.PenaltyStrokePenaltyStrokeAttempts;
                    break;
                case "Penalty Time":
                case "Penalty Seconds":
                    key = ConverterConstants.PenaltyTime;
                    break;
                case "%":
                    key = ConverterConstants.Percent;
                    break;
                case "Period 1":
                case "Period 1 Score":
                    key = ConverterConstants.Period1;
                    break;
                case "Period 2":
                case "Period 2 Score":
                    key = ConverterConstants.Period2;
                    break;
                case "Period 3":
                case "Period 3 Score":
                    key = ConverterConstants.Period3;
                    break;
                case "Period 4 Score":
                    key = ConverterConstants.Period4;
                    break;
                case "Period 4":
                    key = ConverterConstants.Perion4;
                    break;
                case "Personal Fouls":
                case "PF":
                    key = ConverterConstants.PersonalFouls;
                    break;
                case "Pit Stop":
                    key = ConverterConstants.PitStop;
                    break;
                case "P":
                case "Placement":
                    key = ConverterConstants.Placement;
                    break;
                case "+/-":
                    key = ConverterConstants.PlusMinus;
                    break;
                case "Points Race Points":
                    key = ConverterConstants.PointRacePoints;
                    break;
                case "Points":
                case "Pts":
                case "DP":
                case "J'P":
                case "Athletic Points":
                case "PS":
                    key = ConverterConstants.Points;
                    break;
                case "Points/Hits":
                    key = ConverterConstants.PointsHits;
                    break;
                case "PPG":
                case "Scoring Average":
                    key = ConverterConstants.PointsPerGame;
                    break;
                case "20 km Points Race":
                    key = ConverterConstants.PointsRace20km;
                    break;
                case "30 km Points Race":
                    key = ConverterConstants.PointsRace30km;
                    break;
                case "Points Ratio":
                    key = ConverterConstants.PointsRatio;
                    break;
                case "Pole Vault":
                    key = ConverterConstants.PoleVault;
                    break;
                case "Pommelled Horse":
                case "PH":
                    key = ConverterConstants.PommellHorse;
                    break;
                case "Pos":
                case "Positions":
                    key = ConverterConstants.Position;
                    break;
                case "Pounds (lbs)":
                    key = ConverterConstants.Pounds;
                    break;
                case "Precision":
                case "Precision Points":
                    key = ConverterConstants.Precision;
                    break;
                case "TPP":
                    key = ConverterConstants.PrecisionPoints;
                    break;
                case "Preliminary Round":
                case "Preliminary Points":
                case "Preliminary Round Points":
                    key = ConverterConstants.PreliminaryRound;
                    break;
                case "Presentation":
                    key = ConverterConstants.Presentation;
                    break;
                case "Progr. Components":
                    key = ConverterConstants.ProgramComponents;
                    break;
                case "Prone":
                case "Prone Points":
                    key = ConverterConstants.Prone;
                    break;
                case "Pulls Won":
                    key = ConverterConstants.PullsWon;
                    break;
                case "Putouts":
                    key = ConverterConstants.Putots;
                    break;
                case "Qualification":
                case "QOP":
                case "Qualifying":
                case "QF":
                case "QRP":
                case "Jumping Qualifying":
                case "Qualifying Round":
                case "Qualifying Points":
                case "Qualification Points":
                    key = ConverterConstants.Qualification;
                    break;
                case "Qualifying Round 1":
                    key = ConverterConstants.Qualification1;
                    break;
                case "Qualifying Round 2":
                    key = ConverterConstants.Qualification2;
                    break;
                case "Qualifying Round One":
                    key = ConverterConstants.QualifyingRoundOne;
                    break;
                case "Qualifying Round Two":
                    key = ConverterConstants.QualifyingRoundTwo;
                    break;
                case "After Quarter 1":
                    key = ConverterConstants.Quarter1;
                    break;
                case "After Quarter 2":
                    key = ConverterConstants.Quarter2;
                    break;
                case "After Quarter 3":
                    key = ConverterConstants.Quarter3;
                    break;
                case "After Quarter 4":
                    key = ConverterConstants.Quarter4;
                    break;
                case "Race":
                    key = ConverterConstants.Race;
                    break;
                case "Race #1":
                case "Race 1":
                    key = ConverterConstants.Race1;
                    break;
                case "Race 10":
                case "Race #10":
                    key = ConverterConstants.Race10;
                    break;
                case "Race 11":
                case "Race #11":
                    key = ConverterConstants.Race11;
                    break;
                case "Race 12":
                case "Race #12":
                    key = ConverterConstants.Race12;
                    break;
                case "Race 13":
                    key = ConverterConstants.Race13;
                    break;
                case "Race 14":
                    key = ConverterConstants.Race14;
                    break;
                case "Race 15":
                    key = ConverterConstants.Race15;
                    break;
                case "Race #2":
                case "Race 2":
                    key = ConverterConstants.Race2;
                    break;
                case "Race #3":
                case "Race 3":
                    key = ConverterConstants.Race3;
                    break;
                case "Race #4":
                case "Race 4":
                    key = ConverterConstants.Race4;
                    break;
                case "Race #5":
                case "Race 5":
                    key = ConverterConstants.Race5;
                    break;
                case "Race #6":
                case "Race 6":
                    key = ConverterConstants.Race6;
                    break;
                case "Race #7":
                case "Race 7":
                    key = ConverterConstants.Race7;
                    break;
                case "Race #8":
                case "Race 8":
                    key = ConverterConstants.Race8;
                    break;
                case "Race #9":
                case "Race 9":
                    key = ConverterConstants.Race9;
                    break;
                case "Rank":
                    key = ConverterConstants.Rank;
                    break;
                case "Ranking Points":
                    key = ConverterConstants.RankingPoints;
                    break;
                case "Rapid":
                case "Rapid Points":
                    key = ConverterConstants.Rapid;
                    break;
                case "Final Points (Raw Points)":
                case "Raw Points":
                case "Raw Score":
                case "Raw Athletic Points":
                    key = ConverterConstants.RawPoints;
                    break;
                case "Raw Technical Points":
                    key = ConverterConstants.RawTechnicalPoints;
                    break;
                case "RBIs":
                case "Inning 7/RBIs":
                case "7th Inning/RBIs":
                    key = ConverterConstants.RBIs;
                    break;
                case "Reaction Time":
                case "Reaction":
                case "RT":
                    key = ConverterConstants.ReactionTime;
                    break;
                case "Reason":
                    key = ConverterConstants.Reason;
                    break;
                case "Receptions":
                    key = ConverterConstants.Receptions;
                    break;
                case "Red Cards":
                case "Red":
                    key = ConverterConstants.RedCard;
                    break;
                case "RE":
                case "Reduced":
                    key = ConverterConstants.Reduced;
                    break;
                case "Reduced Points":
                    key = ConverterConstants.ReducedPoints;
                    break;
                case "Required Element Penalty":
                    key = ConverterConstants.RequiredElementPenalty;
                    break;
                case "Result":
                    key = ConverterConstants.Result;
                    break;
                case "After Extra Time Half 1":
                    key = ConverterConstants.ResultAfterExtraTimeHalf1;
                    break;
                case "After Extra Time Half 2":
                    key = ConverterConstants.ResultAfterExtraTimeHalf2;
                    break;
                case "After Extra Time Half 3":
                    key = ConverterConstants.ResultAfterExtraTimeHalf3;
                    break;
                case "After Half 1":
                    key = ConverterConstants.ResultAfterHalf1;
                    break;
                case "After Half 2":
                    key = ConverterConstants.ResultAfterHalf2;
                    break;
                case "Rhythm Dance":
                    key = ConverterConstants.RhythmDance;
                    break;
                case "Ribbon":
                case "Ribbon Score":
                case "Ribbons":
                    key = ConverterConstants.Ribbon;
                    break;
                case "Riding":
                    key = ConverterConstants.Riding;
                    break;
                case "Rings":
                    key = ConverterConstants.Rings;
                    break;
                case "Rope":
                case "Rope Score":
                case "Ropes":
                case "Ropes Score":
                    key = ConverterConstants.Rope;
                    break;
                case "Round":
                    key = ConverterConstants.Round;
                    break;
                case "Round One Points":
                case "Group A Round #1":
                case "Group A Round 1":
                case "Group A Round One":
                case "Group B Round #1":
                case "Group B Round 1":
                case "Group B Round One":
                case "Group C Round One":
                case "Group D Round One":
                case "Group E Round One":
                case "R1":
                case "Round #1":
                case "Round 1 Score":
                case "Round One Score":
                case "ROP":
                case "Round 1 Points":
                case "Round 1 Deuk-Jeom":
                case "1":
                    key = ConverterConstants.Round1;
                    break;
                case "Round Two Points":
                case "Group A Round #2":
                case "Group A Round 2":
                case "Group A Round Two":
                case "Group B Round #2":
                case "Group B Round 2":
                case "Group B Round Two":
                case "Group C Round Two":
                case "Group D Round Two":
                case "Group E Round Two":
                case "R2":
                case "Round #2":
                case "Round 2 Score":
                case "Round Two Score":
                case "Round 2 Points":
                case "Round 2 Deuk-Jeom":
                case "2":
                    key = ConverterConstants.Round2;
                    break;
                case "Group A Round #3":
                case "Group A Round 3":
                case "Group A Round Three":
                case "Group B Round #3":
                case "Group B Round 3":
                case "Group B Round Three":
                case "Group C Round Three":
                case "Group D Round Three":
                case "Group E Round Three":
                case "R3":
                case "Round #3":
                case "Round 3 Score":
                case "Round 3 Points":
                case "Round 3 Deuk-Jeom":
                case "3":
                    key = ConverterConstants.Round3;
                    break;
                case "R4":
                case "Round #4":
                case "Round 4 Score":
                case "Round 4 Points":
                case "Round 4 Deuk-Jeom":
                    key = ConverterConstants.Round4;
                    break;
                case "R5":
                case "Round #5":
                case "Round 5 Points":
                    key = ConverterConstants.Round5;
                    break;
                case "R6":
                case "Round #6":
                case "Round 6 Points":
                    key = ConverterConstants.Round6;
                    break;
                case "Round #7":
                    key = ConverterConstants.Round7;
                    break;
                case "Round One Penalties":
                    key = ConverterConstants.RoundOnePenalties;
                    break;
                case "Round Three Penalties":
                    key = ConverterConstants.RoundThreePenalties;
                    break;
                case "Round Two Penalties":
                    key = ConverterConstants.RoundTwoPenalties;
                    break;
                case "Routine 1 Points":
                case "Routine #1":
                    key = ConverterConstants.Routine1;
                    break;
                case "Routine 1 Degree Of Difficulty":
                    key = ConverterConstants.Routine1DegreeOfDifficulty;
                    break;
                case "Routine 2 Points":
                case "Routine #2":
                    key = ConverterConstants.Routine2;
                    break;
                case "Routine 2 Degree Of Difficulty":
                    key = ConverterConstants.Routine2DegreeOfDifficulty;
                    break;
                case "Routine 3 Points":
                    key = ConverterConstants.Routine3;
                    break;
                case "Routine 3 Degree Of Difficulty":
                    key = ConverterConstants.Routine3DegreeOfDifficulty;
                    break;
                case "Routine 4 Points":
                    key = ConverterConstants.Routine4;
                    break;
                case "Routine 4 Degree Of Difficulty":
                    key = ConverterConstants.Routine4DegreeOfDifficulty;
                    break;
                case "Routine 5 Points":
                    key = ConverterConstants.Routine5;
                    break;
                case "Routine 5 Degree Of Difficulty":
                    key = ConverterConstants.Routine5DegreeOfDifficulty;
                    break;
                case "Run #1":
                case "Run 1 Points":
                case "Run 1":
                    key = ConverterConstants.Run1;
                    break;
                case "Run #2":
                case "Run 2 Points":
                case "Run 2":
                    key = ConverterConstants.Run2;
                    break;
                case "Run #3":
                case "Run 3 Points":
                case "Run 3":
                    key = ConverterConstants.Run3;
                    break;
                case "Run #4":
                    key = ConverterConstants.Run4;
                    break;
                case "Run #5":
                    key = ConverterConstants.Run5;
                    break;
                case "Run #6":
                    key = ConverterConstants.Run6;
                    break;
                case "10 km Running":
                case "2 km Running":
                    key = ConverterConstants.Running;
                    break;
                case "Running score":
                    key = ConverterConstants.RunningScore;
                    break;
                case "Running & Shooting":
                    key = ConverterConstants.RunningShooting;
                    break;
                case "Running Time":
                    key = ConverterConstants.RunningTime;
                    break;
                case "Runs":
                case "2nd Inning/Runs":
                case "Inning 2/Runs":
                    key = ConverterConstants.Runs;
                    break;
                case "Runs Against/Triples":
                    key = ConverterConstants.RunsAgainstTriples;
                    break;
                case "Runs Allowed":
                case "Team Hits/Runs Allowed":
                case "Team LOB/Home Runs Allowed":
                    key = ConverterConstants.RunsAllowed;
                    break;
                case "Runs For/Doubles":
                    key = ConverterConstants.RunsForDoubles;
                    break;
                case "Sacrifice Hits/Sacrifice Flies":
                case "12th Inning/Sacrifice Hits":
                case "13th Inning/Sacrifice Flies":
                case "Team Errors/Sacrifice Flies":
                case "Team Hits/Sacrifice Hits":
                    key = ConverterConstants.SacrificeHitsSacrificeFlies;
                    break;
                case "SVS":
                case "Saves":
                    key = ConverterConstants.Saves;
                    break;
                case "Score":
                    key = ConverterConstants.Score;
                    break;
                case "Scorer":
                case "Scorer (assists)":
                    key = ConverterConstants.Scorer;
                    break;
                case "10 km Scratch":
                    key = ConverterConstants.Scratch10km;
                    break;
                case "15 km Scratch":
                    key = ConverterConstants.Scratch15km;
                    break;
                case "Scratch Race Points":
                    key = ConverterConstants.ScratchRacePoints;
                    break;
                case "Second Best Mark Distance":
                case "2nd Best":
                    key = ConverterConstants.SecondBestDistance;
                    break;
                case "Second-Best Wave":
                    key = ConverterConstants.SecondBestWave;
                    break;
                case "Round 5 (2 seconds) Points":
                    key = ConverterConstants.Seconds2;
                    break;
                case "Round 4 (3 seconds) Points":
                    key = ConverterConstants.Seconds3;
                    break;
                case "4 Seconds Points":
                case "Round 3 (4 seconds) Points":
                    key = ConverterConstants.Seconds4;
                    break;
                    key = ConverterConstants.Seconds5;
                    break;
                case "6 Seconds Points":
                case "Round 2 (6 seconds) Points":
                    key = ConverterConstants.Seconds6;
                    break;
                case "8 Seconds Points":
                case "Round 1 (8 seconds) Points":
                    key = ConverterConstants.Seconds8;
                    break;
                case "2TT":
                    key = ConverterConstants.SecondTimeTrial;
                    break;
                case "Section 1 Points":
                    key = ConverterConstants.Section1;
                    break;
                case "Section 2 Points":
                    key = ConverterConstants.Section2;
                    break;
                case "Section 3 Points":
                    key = ConverterConstants.Section3;
                    break;
                case "Section 4 Points":
                    key = ConverterConstants.Section4;
                    break;
                case "Section 5 Points":
                    key = ConverterConstants.Section5;
                    break;
                case "Section 6 Points":
                    key = ConverterConstants.Section6;
                    break;
                case "Sections Points":
                    key = ConverterConstants.SectionPoints;
                    break;
                case "Sections Score (60%)":
                    key = ConverterConstants.SectionsScore60;
                    break;
                case "Seed":
                    key = ConverterConstants.Seed;
                    break;
                case "Seeding Round":
                    key = ConverterConstants.SeedingRound;
                    break;
                case "SF":
                case "S-FP":
                case "S-FRP":
                case "Semi-Final":
                case "Semi-Final Points":
                case "Semi-Final Round Points":
                    key = ConverterConstants.Semifinals;
                    break;
                case "Series 1 Points":
                case "String #1":
                case "String 1 Points":
                case "Stage #1":
                case "Stage 1 Points":
                case "Stage One Points":
                    key = ConverterConstants.Series1;
                    break;
                case "Series 10 Points":
                    key = ConverterConstants.Series10;
                    break;
                case "Series 2 Points":
                case "String #2":
                case "String 2 Points":
                case "Stage #2":
                case "Stage 2 Points":
                case "Stage Two Points":
                    key = ConverterConstants.Series2;
                    break;
                case "Series 3 Points":
                case "String #3":
                case "String 3 Points":
                case "Stage Three Points":
                    key = ConverterConstants.Series3;
                    break;
                case "Series 4 Points":
                case "String #4":
                    key = ConverterConstants.Series4;
                    break;
                case "Series 5 Points":
                case "String #5":
                    key = ConverterConstants.Series5;
                    break;
                case "Series 6 Points":
                case "String #6":
                    key = ConverterConstants.Series6;
                    break;
                case "Series 7 Points":
                    key = ConverterConstants.Series7;
                    break;
                case "Series 8 Points":
                    key = ConverterConstants.Series8;
                    break;
                case "Series 9 Points":
                    key = ConverterConstants.Series9;
                    break;
                case "Serves":
                    key = ConverterConstants.Serves;
                    break;
                case "Service Aces":
                    key = ConverterConstants.ServiceAces;
                    break;
                case "Service Attempts":
                    key = ConverterConstants.ServiceAttempts;
                    break;
                case "Service Faults":
                    key = ConverterConstants.ServiceFaults;
                    break;
                case "Service Points":
                    key = ConverterConstants.ServicePoints;
                    break;
                case "Set 1 Points":
                case "Set 1":
                case "Line-up Set 1":
                    key = ConverterConstants.Set1;
                    break;
                case "Set 2 Points":
                case "Set 2":
                case "Line-up Set 2":
                    key = ConverterConstants.Set2;
                    break;
                case "Set 3 Points":
                case "Set 3":
                case "Line-up Set 3":
                    key = ConverterConstants.Set3;
                    break;
                case "Set 4 Points":
                case "Set 4":
                case "Line-up Set 4":
                    key = ConverterConstants.Set4;
                    break;
                case "Set 5 Points":
                case "Set 5":
                case "Line-up Set 5":
                    key = ConverterConstants.Set5;
                    break;
                case "Set 6":
                    key = ConverterConstants.Set6;
                    break;
                case "Set Ratio":
                    key = ConverterConstants.SetRatio;
                    break;
                case "Set Points":
                case "Sets":
                case "Sets Won":
                    key = ConverterConstants.Sets;
                    break;
                case "Shift":
                    key = ConverterConstants.Shift;
                    break;
                case "Shooting 1 Extra Shots":
                    key = ConverterConstants.Shooting1ExtraShots;
                    break;
                case "Shooting 1 Misses":
                    key = ConverterConstants.Shooting1Misses;
                    break;
                case "Shooting 1 Penalties":
                    key = ConverterConstants.Shooting1Penalties;
                    break;
                case "Shooting 2 Extra Shots":
                    key = ConverterConstants.Shooting2ExtraShots;
                    break;
                case "Shooting 2 Misses":
                    key = ConverterConstants.Shooting2Misses;
                    break;
                case "Shooting 2 Penalties":
                    key = ConverterConstants.Shooting2Penalties;
                    break;
                case "Shooting 3 Misses":
                    key = ConverterConstants.Shooting3Misses;
                    break;
                case "Shooting 3 Penalties":
                    key = ConverterConstants.Shooting3Penalties;
                    break;
                case "Shooting 4 Misses":
                    key = ConverterConstants.Shooting4Misses;
                    break;
                case "Shooting 4 Penalties":
                    key = ConverterConstants.Shooting4Penalties;
                    break;
                case "SE":
                    key = ConverterConstants.ShootingEfficiency;
                    break;
                case "Shooting Time":
                case "Total Shooting Time":
                    key = ConverterConstants.ShootingTime;
                    break;
                case "Shoot-off":
                case "Shoot-Off Points":
                case "Shoot-Off for 1st place":
                case "Shoot-Off for 3rd place":
                case "Shoot-Off 1/2":
                    key = ConverterConstants.ShootOff;
                    break;
                case "Shoot-Off #1":
                case "Shoot-off 1":
                case "Shoot-off 1 Points":
                    key = ConverterConstants.ShootOff1;
                    break;
                case "Shoot-Off #2":
                case "Shoot-off 2":
                case "Shoot-off 2 Points":
                    key = ConverterConstants.ShootOff2;
                    break;
                case "Shoot-Off #3":
                case "Shoot-off 3 Points":
                    key = ConverterConstants.ShootOff3;
                    break;
                case "Shoot-Off Arrow":
                    key = ConverterConstants.ShootOffArrow;
                    break;
                case "Shoot-out Goals":
                    key = ConverterConstants.ShootOutGoals;
                    break;
                case "Shooting 1":
                    key = ConverterConstants.Shooting1;
                    break;
                case "Shooting 2":
                    key = ConverterConstants.Shooting2;
                    break;
                case "Shooting 3":
                    key = ConverterConstants.Shooting3;
                    break;
                case "Shooting 4":
                    key = ConverterConstants.Shooting4;
                    break;
                case "Short Dance":
                    key = ConverterConstants.ShortDance;
                    break;
                case "Short-handed Goals":
                    key = ConverterConstants.ShortHandedGoals;
                    break;
                case "Short Program":
                case "Men's Short Program":
                case "Women's Short Program":
                case "Pairs Short Program":
                case "Short":
                    key = ConverterConstants.ShortProgram;
                    break;
                case "Shot":
                case "Shot #":
                    key = ConverterConstants.Shot;
                    break;
                case "Shot 1 Points":
                    key = ConverterConstants.Shot1;
                    break;
                case "Shot 10 Points":
                    key = ConverterConstants.Shot10;
                    break;
                case "Shot 11 Points":
                    key = ConverterConstants.Shot11;
                    break;
                case "Shot 12 Points":
                    key = ConverterConstants.Shot12;
                    break;
                case "Shot 13 Points":
                    key = ConverterConstants.Shot13;
                    break;
                case "Shot 14 Points":
                    key = ConverterConstants.Shot14;
                    break;
                case "Shot 15 Points":
                    key = ConverterConstants.Shot15;
                    break;
                case "Shot 16 Points":
                    key = ConverterConstants.Shot16;
                    break;
                case "Shot 17 Points":
                    key = ConverterConstants.Shot17;
                    break;
                case "Shot 18 Points":
                    key = ConverterConstants.Shot18;
                    break;
                case "Shot 19 Points":
                    key = ConverterConstants.Shot19;
                    break;
                case "Shot 2 Points":
                    key = ConverterConstants.Shot2;
                    break;
                case "Shot 20 Points":
                    key = ConverterConstants.Shot20;
                    break;
                case "Shot 21 Points":
                    key = ConverterConstants.Shot21;
                    break;
                case "Shot 22 Points":
                    key = ConverterConstants.Shot22;
                    break;
                case "Shot 23 Points":
                    key = ConverterConstants.Shot23;
                    break;
                case "Shot 24 Points":
                    key = ConverterConstants.Shot24;
                    break;
                case "Shot 3 Points":
                    key = ConverterConstants.Shot3;
                    break;
                case "Shot 4 Points":
                    key = ConverterConstants.Shot4;
                    break;
                case "Shot 5 Points":
                    key = ConverterConstants.Shot5;
                    break;
                case "Shot 6 Points":
                    key = ConverterConstants.Shot6;
                    break;
                case "Shot 7 Points":
                    key = ConverterConstants.Shot7;
                    break;
                case "Shot 8 Points":
                    key = ConverterConstants.Shot8;
                    break;
                case "Shot 9 Points":
                    key = ConverterConstants.Shot9;
                    break;
                case "SOG":
                    key = ConverterConstants.ShotOnGoal;
                    break;
                case "SPP":
                case "Shot Put":
                    key = ConverterConstants.ShotPut;
                    break;
                case "Shots":
                    key = ConverterConstants.Shots;
                    break;
                case "Shots 1":
                case "Valid Shots at Station #1":
                    key = ConverterConstants.Shots1;
                    break;
                case "Shots 2":
                case "Valid Shots at Station #2":
                    key = ConverterConstants.Shots2;
                    break;
                case "Shots 3":
                case "Valid Shots at Station #3":
                    key = ConverterConstants.Shots3;
                    break;
                case "Shots 4":
                case "Valid Shots at Station #4":
                    key = ConverterConstants.Shots4;
                    break;
                case "6 metre Shots / Attempts - Saves / Shots":
                    key = ConverterConstants.Shots6M;
                    break;
                case "9 metre Shots / Attempts - Saves / Shots":
                    key = ConverterConstants.Shots9M;
                    break;
                case "7 metre Shots / Attempts":
                case "7 metre Shots / Attempts - Saves / Shots":
                    key = ConverterConstants.Shots7M;
                    break;
                case "Shots on Goal":
                    key = ConverterConstants.ShotsOnGoal;
                    break;
                case "Side Outs":
                    key = ConverterConstants.SideOuts;
                    break;
                case "Situation":
                    key = ConverterConstants.Situation;
                    break;
                case "Skiing":
                    key = ConverterConstants.Skiing;
                    break;
                case "Ski Jumping, Large Hill":
                    key = ConverterConstants.SkiJumpingLargeHill;
                    break;
                case "Ski Jumping, Normal Hill":
                    key = ConverterConstants.SkiJumpingNormalHill;
                    break;
                case "Slalom":
                    key = ConverterConstants.Slalom;
                    break;
                case "Slow Run":
                case "Slow Run Points":
                    key = ConverterConstants.SlowRun;
                    break;
                case "Snatch":
                    key = ConverterConstants.Snatch;
                    break;
                case "Speed":
                    key = ConverterConstants.Speed;
                    break;
                case "Spike Points":
                case "Spike Points / Side-Outs":
                    key = ConverterConstants.SpikePoints;
                    break;
                case "Spikes":
                    key = ConverterConstants.Spikes;
                    break;
                case "Split (Pos)":
                    key = ConverterConstants.Split;
                    break;
                case "Split 1":
                case "Split time lap 1":
                    key = ConverterConstants.Split1;
                    break;
                case "Split time lap 10":
                    key = ConverterConstants.Split10;
                    break;
                case "Split time lap 11":
                    key = ConverterConstants.Split11;
                    break;
                case "Split time lap 12":
                    key = ConverterConstants.Split12;
                    break;
                case "Split time lap 13":
                    key = ConverterConstants.Split13;
                    break;
                case "Split time lap 14":
                    key = ConverterConstants.Split14;
                    break;
                case "Split time lap 15":
                    key = ConverterConstants.Split15;
                    break;
                case "Split 2":
                case "Split time lap 2":
                    key = ConverterConstants.Split2;
                    break;
                case "Split 3":
                case "Split time lap 3":
                    key = ConverterConstants.Split3;
                    break;
                case "Split 4":
                case "Split time lap 4":
                    key = ConverterConstants.Split4;
                    break;
                case "Split 5":
                case "Split time lap 5":
                    key = ConverterConstants.Split5;
                    break;
                case "Split time lap 6":
                    key = ConverterConstants.Split6;
                    break;
                case "Split time lap 7":
                    key = ConverterConstants.Split7;
                    break;
                case "Split time lap 8":
                    key = ConverterConstants.Split8;
                    break;
                case "Split time lap 9":
                    key = ConverterConstants.Split9;
                    break;
                case "SP":
                    key = ConverterConstants.Sprint;
                    break;
                case "Sprint 1 Points":
                case "Split Time 1":
                    key = ConverterConstants.Sprint1;
                    break;
                case "Sprint 10 Points":
                    key = ConverterConstants.Sprint10;
                    break;
                case "Sprint 11 Points":
                    key = ConverterConstants.Sprint11;
                    break;
                case "Sprint 12 Points":
                    key = ConverterConstants.Sprint12;
                    break;
                case "Sprint 13 Points":
                    key = ConverterConstants.Sprint13;
                    break;
                case "Sprint 14 Points":
                    key = ConverterConstants.Sprint14;
                    break;
                case "Sprint 15 Points":
                    key = ConverterConstants.Sprint15;
                    break;
                case "Sprint 16 Points":
                    key = ConverterConstants.Sprint16;
                    break;
                case "Sprint 17 Points":
                    key = ConverterConstants.Sprint17;
                    break;
                case "Sprint 18 Points":
                    key = ConverterConstants.Sprint18;
                    break;
                case "Sprint 19 Points":
                    key = ConverterConstants.Sprint19;
                    break;
                case "Sprint 2 Points":
                case "Split Time 2":
                    key = ConverterConstants.Sprint2;
                    break;
                case "Sprint 20 Points":
                    key = ConverterConstants.Sprint20;
                    break;
                case "Sprint 3 Points":
                case "Split Time 3":
                    key = ConverterConstants.Sprint3;
                    break;
                case "Sprint 4 Points":
                case "Split Time 4":
                    key = ConverterConstants.Sprint4;
                    break;
                case "Sprint 5 Points":
                case "Split Time 5":
                    key = ConverterConstants.Sprint5;
                    break;
                case "Sprint 6 Points":
                    key = ConverterConstants.Sprint6;
                    break;
                case "Sprint 7 Points":
                    key = ConverterConstants.Sprint7;
                    break;
                case "Sprint 8 Points":
                    key = ConverterConstants.Sprint8;
                    break;
                case "Sprint 9 Points":
                    key = ConverterConstants.Sprint9;
                    break;
                case "Sprints Won/Contested":
                    key = ConverterConstants.SprintsWon;
                    break;
                case "200ST":
                    key = ConverterConstants.ST200;
                    break;
                case "Standing":
                case "Standing Points":
                    key = ConverterConstants.Standing;
                    break;
                case "Start Behind":
                    key = ConverterConstants.StartBehind;
                    break;
                case "Start Loop":
                    key = ConverterConstants.StartLoop;
                    break;
                case "Steals":
                case "STL":
                    key = ConverterConstants.Steals;
                    break;
                case "Steeplechase":
                    key = ConverterConstants.Steeplechase;
                    break;
                case "Stolen Bases":
                case "Inning 10/Stolen Bases":
                case "Stolen Bases/Caught Stealing":
                case "10th Inning/Stolen Bases":
                    key = ConverterConstants.StolenBasesCaughtStealing;
                    break;
                case "St. Order":
                    key = ConverterConstants.StOrder;
                    break;
                case "Strikeouts":
                case "9th Inning/Strikeouts":
                case "Inning 9/Strikeouts":
                case "Home-Visiting Team/Strikeouts":
                    key = ConverterConstants.Strieouts;
                    break;
                case "Strokes":
                    key = ConverterConstants.Strokes;
                    break;
                case "Strokes Hole 1":
                    key = ConverterConstants.Strokes1;
                    break;
                case "Strokes Hole 2":
                    key = ConverterConstants.Strokes2;
                    break;
                case "Strokes Hole 3":
                    key = ConverterConstants.Strokes3;
                    break;
                case "Strokes Hole 4":
                    key = ConverterConstants.Strokes4;
                    break;
                case "Style Points":
                    key = ConverterConstants.StylePoints;
                    break;
                case "Suspensions":
                    key = ConverterConstants.Suspensions;
                    break;
                case "2-minute Suspensions":
                    key = ConverterConstants.Suspensions2Minutes;
                    break;
                case "1.5 km Swimming":
                case "300 m Swimming":
                case "Swimming":
                    key = ConverterConstants.Swimming;
                    break;
                case "Swim-Off":
                    key = ConverterConstants.SwimOff;
                    break;
                case "S1":
                case "SJ1S":
                    key = ConverterConstants.Synchronization1;
                    break;
                case "S2":
                case "SJ2S":
                    key = ConverterConstants.Synchronization2;
                    break;
                case "S3":
                case "SJ3S":
                    key = ConverterConstants.Synchronization3;
                    break;
                case "S4":
                case "SJ4S":
                    key = ConverterConstants.Synchronization4;
                    break;
                case "S5":
                case "SJ5S":
                    key = ConverterConstants.Synchronization5;
                    break;
                case "Target":
                    key = ConverterConstants.Target;
                    break;
                case "Round 1 Targets Hit":
                case "Round 1 Targets Hits":
                    key = ConverterConstants.TargetHits1;
                    break;
                case "Round 2 Targets Hit":
                    key = ConverterConstants.TargetHits2;
                    break;
                case "Targets":
                    key = ConverterConstants.Targets;
                    break;
                case "Targets Hit":
                case "TH":
                    key = ConverterConstants.TargetsHit;
                    break;
                case "Team Penalty Points":
                    key = ConverterConstants.TeamPenaltyPoints;
                    break;
                case "Team Points":
                case "TP":
                    key = ConverterConstants.TeamPoints;
                    break;
                case "Technical Factor":
                    key = ConverterConstants.TechnicalFactor;
                    break;
                case "Technical Faults":
                    key = ConverterConstants.TechnicalFaults;
                    break;
                case "Technical Merit":
                case "Technical Merit Points":
                    key = ConverterConstants.TechnicalMerit;
                    break;
                case "Technical Merit Difficulty Points":
                    key = ConverterConstants.TechnicalMeritDifficultyPoints;
                    break;
                case "Technical Merit Execution Points":
                    key = ConverterConstants.TechnicalMeritExecutionPoints;
                    break;
                case "Technical Merit Synchronization Points":
                    key = ConverterConstants.TechnicalMeritSynchronizationPoints;
                    break;
                case "Tech. Points":
                case "Technical Points":
                    key = ConverterConstants.TechnicalPoints;
                    break;
                case "Technical Routine":
                case "Technical Routine Points":
                    key = ConverterConstants.TechnicalRoutine;
                    break;
                case "Tech.":
                case "Tech. Elements":
                case "Technique":
                case "T/E":
                    key = ConverterConstants.Technique;
                    break;
                case "Tempo Race Points":
                    key = ConverterConstants.TempoRacePoints;
                    break;
                case "3TT":
                    key = ConverterConstants.ThirdTimeTrial;
                    break;
                case "3P":
                    key = ConverterConstants.ThreePoints;
                    break;
                case "3-point Field Goals Attempts":
                    key = ConverterConstants.ThreePointsAttempts;
                    break;
                case "3-point Field Goals Made":
                    key = ConverterConstants.ThreePointsMade;
                    break;
                case "Throw-Off":
                    //case "Throw-Off (Imperial)":
                    key = ConverterConstants.ThrowOff;
                    break;
                case "56 lb Weight Throw":
                    key = ConverterConstants.ThrowWeightLb56;
                    break;
                case "Tie-Break":
                case "Tie-breaker":
                case "Tie-breaking Score":
                case "TS-OP":
                case "Tiebreak Points":
                case "Tie-Break Score":
                case "S-OP":
                case "Match tie-break":
                    key = ConverterConstants.TieBreak;
                    break;
                case "Tiebreak 1":
                case "Tie-break 1":
                    key = ConverterConstants.TieBreak1;
                    break;
                case "Tiebreak 2":
                case "Tie-break 2":
                    key = ConverterConstants.TieBreak2;
                    break;
                case "Tie-break 3":
                    key = ConverterConstants.TieBreak3;
                    break;
                case "Tie-break 4":
                    key = ConverterConstants.TieBreak4;
                    break;
                case "Tie-Breaking Time":
                    key = ConverterConstants.TieBreakingTime;
                    break;
                case "Time":
                case "Time Adjustment":
                    key = ConverterConstants.Time;
                    break;
                case "Time (A)":
                case "Time (Automatic)":
                case "T(A)":
                    key = ConverterConstants.TimeAutomatic;
                    break;
                case "Tim./Exec.":
                    key = ConverterConstants.TimeExec;
                    break;
                case "Time Fault/Other Faults":
                    key = ConverterConstants.TimeFaultOtherFaults;
                    break;
                case "Time of Flight Points":
                    key = ConverterConstants.TimeFlightPoints;
                    break;
                case "Time (H)":
                case "Time (Hand)":
                case "T(H)":
                    key = ConverterConstants.TimeHand;
                    break;
                case "Time/Margin":
                case "Time Margin":
                case "T/M":
                    key = ConverterConstants.TimeMargin;
                    break;
                case "Time Penalties":
                    key = ConverterConstants.TimePenalties;
                    break;
                case "Time Penalty":
                    key = ConverterConstants.TimePenalty;
                    break;
                    //case "TP":
                    //    key = ConverterConstants.TimePlayed;
                    break;
                case "Time Points":
                    key = ConverterConstants.TimePoints;
                    break;
                case "Time at Station #1":
                case "Shoot Time 1":
                    key = ConverterConstants.TimeStation1;
                    break;
                case "Time at Station #2":
                case "Shoot Time 2":
                    key = ConverterConstants.TimeStation2;
                    break;
                case "Time at Station #3":
                case "Shoot Time 3":
                    key = ConverterConstants.TimeStation3;
                    break;
                case "Time at Station #4":
                case "Shoot Time 4":
                    key = ConverterConstants.TimeStation4;
                    break;
                case "TT":
                    key = ConverterConstants.TimeTrial;
                    break;
                case "1,000 m Time Trial":
                    key = ConverterConstants.TimeTrial1000;
                    break;
                case "500 m Time Trial":
                    key = ConverterConstants.TimeTrial500;
                    break;
                case "To Par":
                    key = ConverterConstants.ToPar;
                    break;
                case "Total":
                    key = ConverterConstants.Total;
                    break;
                case "Total Attempts":
                case "Total Attempts thru Best Height Cleared":
                    key = ConverterConstants.TotalAttempts;
                    break;
                case "TFP":
                    key = ConverterConstants.TotalFactoredPlacements;
                    break;
                case "TO":
                case "TOM":
                case "TOM/TO":
                    key = ConverterConstants.TotalOridianls;
                    break;
                case "Total Penalty Points":
                    key = ConverterConstants.TotalPenaltyPoints;
                    break;
                case "Total Points":
                case "Total Score":
                    key = ConverterConstants.TotalPoints;
                    break;
                case "TPF":
                case "In Favor":
                    key = ConverterConstants.TotalPointsInFavor;
                    break;
                case "TPG":
                    key = ConverterConstants.TotalPointsPerGame;
                    break;
                case "TRB":
                case "Total Rebounds":
                    key = ConverterConstants.TotalRebounds;
                    break;
                case "Total Shots at Station #1":
                    key = ConverterConstants.TotalShots1;
                    break;
                case "Total Shots at Station #2":
                    key = ConverterConstants.TotalShots2;
                    break;
                case "Total Shots at Station #3":
                    key = ConverterConstants.TotalShots3;
                    break;
                case "Total Shots at Station #4":
                    key = ConverterConstants.TotalShots4;
                    break;
                case "Total Time":
                    key = ConverterConstants.TotalTime;
                    break;
                case "TD":
                case "Touches Delivered":
                case "Team Touches Delivered":
                case "Individual Touches Delivered":
                    key = ConverterConstants.TouchesDelivered;
                    break;
                case "TR":
                    key = ConverterConstants.TouchesReceived;
                    break;
                case "Transition 1":
                case "Transition 1 (swim-to-cycle)":
                    key = ConverterConstants.Transition1;
                    break;
                case "Transition 2":
                case "Transition 2 (cycle-to-run)":
                    key = ConverterConstants.Transition2;
                    break;
                case "Trick 1":
                    key = ConverterConstants.Trick1;
                    break;
                case "Trick 2":
                    key = ConverterConstants.Trick2;
                    break;
                case "Trick 3":
                    key = ConverterConstants.Trick3;
                    break;
                case "Trick 4":
                    key = ConverterConstants.Trick4;
                    break;
                case "Trick 5":
                    key = ConverterConstants.Trick5;
                    break;
                case "Trick 6":
                    key = ConverterConstants.Trick6;
                    break;
                case "Trick ID":
                    key = ConverterConstants.TrickId;
                    break;
                case "Tries":
                    key = ConverterConstants.Tries;
                    break;
                case "Inning 5/Triples":
                case "5th Inning/Triples":
                    key = ConverterConstants.Triples;
                    break;
                case "Trunks":
                    key = ConverterConstants.Trunks;
                    break;
                case "Turnover Fouls":
                    key = ConverterConstants.TurnoverFouls;
                    break;
                case "TOV":
                    key = ConverterConstants.Turnovers;
                    break;
                case "Turns Points":
                    key = ConverterConstants.TurnsPoints;
                    break;
                case "2P":
                    key = ConverterConstants.TwoPoints;
                    break;
                case "2-point Field Goals Attempts":
                    key = ConverterConstants.TwoPointsAttempts;
                    break;
                case "2-point Field Goals Made":
                    key = ConverterConstants.TwoPointsMade;
                    break;
                case "Uneven Bars":
                    key = ConverterConstants.UnevenBars;
                    break;
                case "VAL":
                    key = ConverterConstants.Value;
                    break;
                case "Vault 1":
                case "Jump 1":
                case "1JP":
                case "FJ#1P":
                case "J#1P":
                    key = ConverterConstants.Vault1;
                    break;
                case "Vault 2":
                case "Jump 2":
                case "2JP":
                case "FJ#2P":
                case "J#2P":
                case "C2EP":
                case "O2EP":
                    key = ConverterConstants.Vault2;
                    break;
                case "J-O1JP":
                case "J-OP":
                    key = ConverterConstants.VaultOff1;
                    break;
                case "J-O2JP":
                    key = ConverterConstants.VaultOff2;
                    break;
                case "Victories in Tie Group":
                    key = ConverterConstants.VictoriesInTieGroup;
                    break;
                case "Walks":
                case "Inning 8/Walks":
                case "8th Inning/Walks":
                    key = ConverterConstants.Walks;
                    break;
                case "Warm-Up Penalties":
                case "W-UP":
                    key = ConverterConstants.WarmUpPenalties;
                    break;
                case "Warnings":
                case "Total Warnings":
                    key = ConverterConstants.Warnings;
                    break;
                case "Wave 1":
                    key = ConverterConstants.Wave1;
                    break;
                case "Wave 10":
                    key = ConverterConstants.Wave10;
                    break;
                case "Wave 11":
                    key = ConverterConstants.Wave11;
                    break;
                case "Wave 12":
                    key = ConverterConstants.Wave12;
                    break;
                case "Wave 13":
                    key = ConverterConstants.Wave13;
                    break;
                case "Wave 14":
                    key = ConverterConstants.Wave14;
                    break;
                case "Wave 15":
                    key = ConverterConstants.Wave15;
                    break;
                case "Wave 16":
                    key = ConverterConstants.Wave16;
                    break;
                case "Wave 2":
                    key = ConverterConstants.Wave2;
                    break;
                case "Wave 3":
                    key = ConverterConstants.Wave3;
                    break;
                case "Wave 4":
                    key = ConverterConstants.Wave4;
                    break;
                case "Wave 5":
                    key = ConverterConstants.Wave5;
                    break;
                case "Wave 6":
                    key = ConverterConstants.Wave6;
                    break;
                case "Wave 7":
                    key = ConverterConstants.Wave7;
                    break;
                case "Wave 8":
                    key = ConverterConstants.Wave8;
                    break;
                case "Wave 9":
                    key = ConverterConstants.Wave9;
                    break;
                case "Waza-ari":
                    key = ConverterConstants.Wazaari;
                    break;
                case "Weight":
                    key = ConverterConstants.Weight;
                    break;
                case "Wild Pitches":
                    key = ConverterConstants.WildPitches;
                    break;
                case "Wind":
                case "Wind Speed":
                    key = ConverterConstants.Wind;
                    break;
                case "Wind Compensation Points":
                    key = ConverterConstants.WindPoints;
                    break;
                case "Wing Shots / Attempts - Saves / Shots":
                    key = ConverterConstants.WingShots;
                    break;
                case "Wins":
                case "W":
                case "Races Won":
                case "Bouts Won":
                case "MW":
                case "Matches Won":
                    key = ConverterConstants.Wins;
                    break;
                case "Wins/At Bats":
                    key = ConverterConstants.WinsAtBats;
                    break;
                case "Won/Lost/Saves":
                case "14th Inning/Win/Lost/Save":
                case "Team LOB/Win/Lost/Save":
                    key = ConverterConstants.WonLostSaves;
                    break;
                case "Xs":
                    key = ConverterConstants.Xs;
                    break;
                case "100 y":
                case "100P":
                case "100 yard Points":
                case "100 yards":
                    key = ConverterConstants.Y100;
                    break;
                case "1,000 y":
                    key = ConverterConstants.Y1000;
                    break;
                case "110 y Time (Rank)":
                    key = ConverterConstants.Y110;
                    break;
                case "1,100 y Time (Rank)":
                    key = ConverterConstants.Y1100;
                    break;
                case "120 yards hurdles":
                    key = ConverterConstants.Y120hurdles;
                    break;
                case "1,210 y Time (Rank)":
                    key = ConverterConstants.Y1210;
                    break;
                case "1,320 y Time (Rank)":
                    key = ConverterConstants.Y1320;
                    break;
                case "1,430 y Time (Rank)":
                    key = ConverterConstants.Y1430;
                    break;
                case "1,540 y Time (Rank)":
                    key = ConverterConstants.Y1540;
                    break;
                case "1,650 y Time (Rank)":
                    key = ConverterConstants.Y1650;
                    break;
                case "200 y":
                    key = ConverterConstants.Y200;
                    break;
                case "220 y Time (Rank)":
                    key = ConverterConstants.Y220;
                    break;
                case "30 y":
                    key = ConverterConstants.Y30;
                    break;
                case "330 y Time (Rank)":
                    key = ConverterConstants.Y330;
                    break;
                case "40 y":
                    key = ConverterConstants.Y40;
                    break;
                case "440 y Time (Rank)":
                    key = ConverterConstants.Y440;
                    break;
                case "50 y":
                case "50 yard Points":
                    key = ConverterConstants.Y50;
                    break;
                case "500 y":
                    key = ConverterConstants.Y500;
                    break;
                case "550 y Time (Rank)":
                    key = ConverterConstants.Y550;
                    break;
                case "60 y":
                    key = ConverterConstants.Y60;
                    break;
                case "600 y":
                    key = ConverterConstants.Y600;
                    break;
                case "660 y Time (Rank)":
                    key = ConverterConstants.Y660;
                    break;
                case "770 y Time (Rank)":
                    key = ConverterConstants.Y770;
                    break;
                case "80 y":
                    key = ConverterConstants.Y80;
                    break;
                case "800 y":
                    key = ConverterConstants.Y800;
                    break;
                case "880 y Time (Rank)":
                    key = ConverterConstants.Y880;
                    break;
                case "880 yards Walk":
                    key = ConverterConstants.Y880Walk;
                    break;
                case "900 y":
                case "990 y Time (Rank)":
                    key = ConverterConstants.Y900;
                    break;
                case "Yellow Cards":
                case "Yellow":
                    key = ConverterConstants.YellowCard;
                    break;
                case "Yellow Card Warnings":
                    key = ConverterConstants.YellowCardWarnings;
                    break;
                case "Yuko":
                    key = ConverterConstants.Yuko;
                    break;
            }

            if (!string.IsNullOrEmpty(key) && !indexes.ContainsKey(key))
            {
                indexes[key] = i;
            }
        }

        return indexes;
    }

    public Dictionary<string, int> FindIndexes(List<string> headers)
    {
        var indexes = new Dictionary<string, int>();

        for (var i = 0; i < headers.Count; i++)
        {
            var header = headers[i].ToLower();
            switch (header)
            {
                case "archer":
                case "athlete":
                case "biathlete":
                case "boarder":
                case "boat":
                case "bobsleigh":
                case "boxer":
                case "climber":
                case "competitor":
                case "competitor (seed)":
                case "competitor(s)":
                case "competitors":
                case "cyclist":
                case "cyclists":
                case "diver":
                case "divers":
                case "fencer":
                case "fencers":
                case "fighter":
                case "gymnast":
                case "gymnasts":
                case "judoka":
                case "jumper":
                case "karateka":
                case "lifter":
                case "pair":
                case "pair (seed)":
                case "pentathlete":
                case "player":
                case "rider":
                case "shooter":
                case "skater":
                case "skier":
                case "slider":
                case "surfer":
                case "swimmer":
                case "team":
                case "triathlete":
                case "walker":
                case "wrestler":
                    indexes[ConverterConstants.INDEX_NAME] = i;
                    break;
            }
        }

        return indexes;
    }
}