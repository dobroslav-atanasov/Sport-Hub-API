namespace SportHub.Data.Models.Converters.OlympicGames.Base;

using SportHub.Data.Models.Entities.OlympicGames.Enumerations;

public abstract class BaseRanking
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public string NOC { get; set; }

    public MedalTypeEnum Medal { get; set; }
}