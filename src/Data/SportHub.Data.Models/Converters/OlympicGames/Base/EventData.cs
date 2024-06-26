namespace SportHub.Data.Models.Converters.OlympicGames.Base;

using SportHub.Data.Models.Entities.OlympicGames.Enumerations;

public class EventData
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public string OriginalName { get; set; }

    public string NormalizedName { get; set; }

    public bool IsTeamEvent { get; set; }

    public EventGenderTypeEnum EventGenderType { get; set; }
}