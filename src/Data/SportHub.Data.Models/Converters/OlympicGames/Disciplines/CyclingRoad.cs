namespace SportHub.Data.Models.Converters.OlympicGames.Disciplines;

using SportHub.Data.Models.Converters.OlympicGames.Base;

public class CyclingRoad : BaseModel
{
    public TimeSpan? Time { get; set; }

    public int? Points { get; set; }

    public List<CyclingRoadIntermediate> Intermediates { get; set; } = [];

    public List<CyclingRoad> Athletes { get; set; } = [];
}

public class CyclingRoadIntermediate
{
    public int Number { get; set; }

    public TimeSpan? Time { get; set; }
}