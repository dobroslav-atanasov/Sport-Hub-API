namespace SportHub.Data.Models.Converters.OlympicGames.Disciplines;

using SportHub.Data.Models.Converters.OlympicGames.Base;

public class Equestrian : BaseModel
{
    public Horse Horse { get; set; }

    public double? Points { get; set; }

    public double? TechnicalPoints { get; set; }

    public double? ArtisticPoints { get; set; }

    public double? SpecialPoints { get; set; }

    public double? FreestylePoints { get; set; }

    public double? RawPoints { get; set; }

    public double? AdjustedPoints { get; set; }

    public double? Ordinals { get; set; }

    //public double? PercentagePoints { get; set; }

    //public double? CrossCountry { get; set; }

    //public double? Dressage { get; set; }

    //public double? Jumping { get; set; }

    //public double? JumpingQualification { get; set; }

    //public double? CrossCountry5km { get; set; }

    //public double? CrossCountry20km { get; set; }

    //public double? CrossCountry50km { get; set; }

    //public double? Steeplechase { get; set; }

    //public double? PenaltyPoints { get; set; }

    //public TimeSpan? Time { get; set; }

    //public int? TimePenaltyPoints { get; set; }

    //public double? Distance { get; set; }

    //public int? JumpPenaltyPoints { get; set; }

    //public double? JumpOff1PenaltyPoints { get; set; }

    //public double? JumpOff2PenaltyPoints { get; set; }

    //public double? JumpOffTime { get; set; }

    public List<EquestrianPart> Parts { get; set; } = [];

    public List<EquestrianScore> Scores { get; set; } = [];

    public List<Equestrian> Athletes { get; set; } = [];
}

public class EquestrianPart
{
    public double? PenaltyPoints { get; set; }

    public int? TimePenaltyPoints { get; set; }

    public int? JumpPenaltyPoints { get; set; }
}

public class EquestrianScore
{
    public string Judge { get; set; }

    public double? Points { get; set; }

    public double? TechnicalPoints { get; set; }

    public double? ArtisticPoints { get; set; }

    public double? RawPoints { get; set; }

    public double? PercentagePoints { get; set; }
}