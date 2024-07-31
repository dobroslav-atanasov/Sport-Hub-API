namespace SportHub.Data.Models.Converters.OlympicGames.Base;

using SportHub.Data.Models.Enumerations.OlympicGames;

public abstract class BaseRanking
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public string NOC { get; set; }

    public MedalEnum Medal { get; set; }
}