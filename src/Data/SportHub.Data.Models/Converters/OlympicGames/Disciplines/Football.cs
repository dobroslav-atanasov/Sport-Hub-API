namespace SportHub.Data.Models.Converters.OlympicGames.Disciplines;

using SportHub.Data.Models.Converters.OlympicGames.Base;

public class Football : BaseModel
{
    public int? Goals { get; set; }

    public bool AfterExtraTime { get; set; }

    public List<Football> Athletes { get; set; } = [];
}