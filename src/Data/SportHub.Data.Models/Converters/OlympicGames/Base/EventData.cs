namespace SportHub.Data.Models.Converters.OlympicGames.Base;

public class EventData
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public string OriginalName { get; set; }

    public string NormalizedName { get; set; }

    public bool IsTeamEvent { get; set; }

    public int Gender { get; set; }
}