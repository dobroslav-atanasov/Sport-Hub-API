namespace SportData.Data.Models.Converters.OlympicGames.Disciplines;

using SportData.Data.Models.Converters.OlympicGames.Base;

public class Football : BaseModel
{
    public int? Goals { get; set; }

    public bool AfterExtraTime { get; set; }

    public List<Football> Athletes { get; set; } = [];
}