namespace SportHub.Data.Models.Converters.OlympicGames;

public class EventInfo
{
    public string OriginalName { get; set; }

    public string Name { get; set; }

    public string NormalizedName { get; set; }

    public string AdditionalInfo { get; set; }

    public bool IsForbidden { get; set; } = false;
}