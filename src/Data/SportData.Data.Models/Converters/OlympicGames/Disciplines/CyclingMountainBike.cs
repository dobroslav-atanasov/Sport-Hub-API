namespace SportData.Data.Models.Converters.OlympicGames.Disciplines;

using SportData.Data.Models.Converters.OlympicGames.Base;

public class CyclingMountainBike : BaseModel
{
    public TimeSpan? Time { get; set; }

    public List<CyclingMountainBIkeIntermediate> Intermediates { get; set; } = [];
}

public class CyclingMountainBIkeIntermediate
{
    public int Number { get; set; }

    public TimeSpan? Time { get; set; }
}