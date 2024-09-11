namespace SportHub.Data.Models.Converters.OlympicGames.Base;

using SportHub.Data.Entities.Enumerations.OlympicGames;

public class GameData
{
    public int Id { get; set; }

    public int Year { get; set; }

    public GameTypeEnum Type { get; set; }
}