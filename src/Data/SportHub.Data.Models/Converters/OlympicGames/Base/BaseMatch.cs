namespace SportHub.Data.Models.Converters.OlympicGames.Base;

using SportHub.Data.Models.Enumerations.OlympicGames;

public abstract class BaseMatch
{
    public int Number { get; set; }

    public string Location { get; set; }

    public int? Attendance { get; set; }

    public DateTime? Date { get; set; }

    public MedalEnum Medal { get; set; }

    public string Info { get; set; }

    public int ResultId { get; set; }

    public DecisionEnum Decision { get; set; }

    public TimeSpan? MatchTime { get; set; }

    public List<Judge> Judges { get; set; } = [];
}