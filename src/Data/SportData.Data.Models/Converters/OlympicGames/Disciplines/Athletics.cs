namespace SportData.Data.Models.Converters.OlympicGames.Disciplines;

using SportData.Data.Models.Converters.OlympicGames.Base;
using SportData.Data.Models.Entities.OlympicGames.Enumerations;

public class Athletics : BaseModel
{
    public AthleticsEventType EventType { get; set; }

    public string Position { get; set; }

    public int? Lane { get; set; }

    public double? ReactionTime { get; set; }

    public TimeSpan? Time { get; set; }

    public TimeSpan? TimeAutomatic { get; set; }

    public TimeSpan? TimeHand { get; set; }

    public double? Points { get; set; }

    public int? BentKneeWarnings { get; set; }

    public int? LostOfContactWarnings { get; set; }

    public int? Warnings { get; set; }

    public TimeSpan? TieBreakingTime { get; set; }

    public double? Wind { get; set; }

    public double? BestMeasurement { get; set; }

    public double? SecondBestMeasurement { get; set; }

    public int? TotalAttempts { get; set; }

    public int? Misses { get; set; }

    public int? MissesAtBeast { get; set; }

    public double? ThrowOff { get; set; }

    public List<AthleticsCombined> Combined { get; set; } = [];

    public List<AthleticsAttempt> Attempts { get; set; } = [];

    public List<AthleticsSplit> Splits { get; set; } = [];

    public List<Athletics> Athletes { get; set; } = [];
}

public class AthleticsCombined
{
    public double? Points { get; set; }

    public string EventName { get; set; }

    public DateTime? Date { get; set; }

    public int Group { get; set; }

    public int? Lane { get; set; }

    public TimeSpan? Time { get; set; }

    public TimeSpan? TimeAutomatic { get; set; }

    public TimeSpan? TimeHand { get; set; }

    public TimeSpan? TieBreakingTime { get; set; }

    public double? BestMeasurement { get; set; }
    public double? SecondBestMeasurement { get; set; }

    public int? MissesAtBeast { get; set; }

    public List<AthleticsAttempt> Attempts { get; set; } = [];
}

public class AthleticsAttempt
{
    public int Number { get; set; }

    public double? Measurement { get; set; }

    public RecordType Record { get; set; }

    public AthleticsTry Try1 { get; set; }

    public AthleticsTry Try2 { get; set; }

    public AthleticsTry Try3 { get; set; }

    public bool IsJumpOff { get; set; } = false;

    public double? Wind { get; set; }
}

public enum AthleticsTry
{
    None,
    Success,
    Fail,
    Skip,
}

public enum AthleticsEventType
{
    None,
    TrackEvents,
    RoadEvents,
    FieldEvents,
    CombinedEvents,
    CrossCountryEvent,
}

public class AthleticsSplit
{
    //public int Number { get; set; }

    //public int HeatNumber { get; set; }

    public string Distance { get; set; }

    public TimeSpan? Time { get; set; }
}