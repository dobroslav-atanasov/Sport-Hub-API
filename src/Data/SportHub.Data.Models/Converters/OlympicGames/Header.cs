namespace SportHub.Data.Models.Converters.OlympicGames;

using SportHub.Data.Models.Enumerations.OlympicGames;

public class Header
{
    public GameTypeEnum GameType { get; set; }

    public int GameYear { get; set; }

    public string Discipline { get; set; }

    public string Event { get; set; }
}