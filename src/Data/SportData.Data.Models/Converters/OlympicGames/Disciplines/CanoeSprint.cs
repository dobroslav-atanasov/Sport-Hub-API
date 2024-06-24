namespace SportData.Data.Models.Converters.OlympicGames.Disciplines;

using SportData.Data.Models.Converters.OlympicGames.Base;

public class CanoeSprint : BaseModel
{
    public int? Lane { get; set; }

    public TimeSpan? Time { get; set; }

    //public TimeSpan? Exchange { get; set; }

    public List<CanoeSprintIntermediate> Intermediates { get; set; } = [];

    public List<CanoeSprint> Athletes { get; set; } = [];
}

public class CanoeSprintIntermediate
{
    public int Meters { get; set; }

    public TimeSpan? Time { get; set; }
}