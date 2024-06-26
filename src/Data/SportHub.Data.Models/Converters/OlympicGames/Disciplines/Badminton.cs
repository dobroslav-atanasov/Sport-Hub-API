namespace SportHub.Data.Models.Converters.OlympicGames.Disciplines;
using SportHub.Data.Models.Converters.OlympicGames.Base;

public class Badminton : BaseModel
{
    public int Points { get; set; }

    public int? Game1 { get; set; }

    public int? Game2 { get; set; }

    public int? Game3 { get; set; }

    public List<Badminton> Athletes { get; set; } = [];
}