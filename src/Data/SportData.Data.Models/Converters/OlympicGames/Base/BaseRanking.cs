namespace SportData.Data.Models.Converters.OlympicGames.Base;

using SportData.Data.Models.Entities.OlympicGames.Enumerations;

public abstract class BaseRanking
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public string NOC { get; set; }

    public MedalTypeEnum Medal { get; set; }
}