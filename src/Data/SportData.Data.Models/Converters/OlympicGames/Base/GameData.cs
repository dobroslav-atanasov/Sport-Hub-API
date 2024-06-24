namespace SportData.Data.Models.Converters.OlympicGames.Base;

using SportData.Data.Models.Entities.OlympicGames.Enumerations;

public class GameData
{
    public int Id { get; set; }

    public int Year { get; set; }

    public OlympicGameTypeEnum OlympicGameType { get; set; }
}