namespace SportData.Data.Models.Converters.OlympicGames.Disciplines;

using SportData.Data.Models.Converters.OlympicGames.Base;

public class CrossCountrySkiing : BaseModel
{
    public TimeSpan? Time { get; set; }

    //public TimeSpan? Exchange { get; set; }

    public TimeSpan? Leg1 { get; set; }

    public TimeSpan? Leg2 { get; set; }

    public TimeSpan? Leg3 { get; set; }

    public TimeSpan? Classical { get; set; }

    public TimeSpan? Freestyle { get; set; }

    public TimeSpan? PitStop { get; set; }

    public TimeSpan? Race { get; set; }

    public TimeSpan? StartBehind { get; set; }

    public List<CrossCountrySkiingIntermediate> Intermediates { get; set; } = [];

    public List<CrossCountrySkiing> Athletes { get; set; } = [];
}

public class CrossCountrySkiingIntermediate
{
    public int Number { get; set; }

    public TimeSpan? Time { get; set; }
}