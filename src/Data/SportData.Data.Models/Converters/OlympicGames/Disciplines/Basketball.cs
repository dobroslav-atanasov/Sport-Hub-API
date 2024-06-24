namespace SportData.Data.Models.Converters.OlympicGames.Disciplines;
using SportData.Data.Models.Converters.OlympicGames.Base;

public class Basketball : BaseModel
{
    public string Position { get; set; }

    public int? Points { get; set; }

    public TimeSpan? TimePlayed { get; set; }

    public double? Value { get; set; }

    public int? FirstHalfPoints { get; set; }

    public int? SecondHalfPoints { get; set; }

    public int? PersonalFouls { get; set; }

    public int? DisqualifyingFouls { get; set; }

    public int? PlusMinus { get; set; }

    public int? FreeThrowsGoals { get; set; }

    public int? FreeThrowsAttempts { get; set; }

    public int? OnePointsGoals { get; set; }

    public int? OnePointsAttempts { get; set; }

    public int? TwoPointsGoals { get; set; }

    public int? TwoPointsAttempts { get; set; }

    public int? ThreePointsGoals { get; set; }

    public int? ThreePointsAttempts { get; set; }

    public int? TotalFieldGoals { get; set; }

    public int? TotalFieldGoalsAttempts { get; set; }

    public double? ShootingEfficiency { get; set; }

    public int? OffensiveRebounds { get; set; }

    public int? DefensiveRebounds { get; set; }

    public int? TotalRebounds { get; set; }

    public int? Assists { get; set; }

    public int? Steals { get; set; }

    public int? Blocks { get; set; }

    public int? Turnovers { get; set; }

    public List<Basketball> Athletes { get; set; } = [];
}