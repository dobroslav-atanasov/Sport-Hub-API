namespace SportData.Data.Models.Converters.OlympicGames.Base;

using SportData.Data.Models.Entities.OlympicGames.Enumerations;

public abstract class BaseMatch
{
    public int Number { get; set; }

    public string Location { get; set; }

    public int? Attendance { get; set; }

    public DateTime? Date { get; set; }

    public MedalTypeEnum Medal { get; set; }

    public string Info { get; set; }

    public int ResultId { get; set; }

    public DecisionType Decision { get; set; }

    public TimeSpan? MatchTime { get; set; }

    public List<Judge> Judges { get; set; } = [];
}