namespace SportHub.Data.Models.Converters.OlympicGames.Disciplines;

using SportHub.Data.Models.Converters.OlympicGames.Base;

public class BeachVolleyball : BaseModel
{
    public int Points { get; set; }

    public int? Set1 { get; set; }

    public int? Set2 { get; set; }

    public int? Set3 { get; set; }

    public string Position { get; set; }

    public int? ServiceAttempts { get; set; }

    public int? ServiceFaults { get; set; }

    public int? ServiceAces { get; set; }

    public int? FastestServe { get; set; }

    public int? AttackAttempts { get; set; }

    public int? AttackSuccesses { get; set; }

    public int? BlockSuccesses { get; set; }

    public int? DigSuccesses { get; set; }

    public int? OpponentErrors { get; set; }

    public int? TotalPoints { get; set; }

    public List<BeachVolleyball> Athletes { get; set; } = [];
}