namespace SportData.Data.Models.Converters.OlympicGames.Base;

using SportData.Data.Models.Entities.OlympicGames.Enumerations;

public class EventData
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public string OriginalName { get; set; }

    public string NormalizedName { get; set; }

    public bool IsTeamEvent { get; set; }

    public EventGenderTypeEnum EventGenderType { get; set; }
}