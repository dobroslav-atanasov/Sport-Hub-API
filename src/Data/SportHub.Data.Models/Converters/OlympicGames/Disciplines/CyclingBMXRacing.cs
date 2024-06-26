namespace SportHub.Data.Models.Converters.OlympicGames.Disciplines;

using SportHub.Data.Models.Converters.OlympicGames.Base;

public class CyclingBMXRacing : BaseModel
{
    public int? Points { get; set; }

    public TimeSpan? Time { get; set; }

    public int? Run1Points { get; set; }

    public int? Run2Points { get; set; }

    public int? Run3Points { get; set; }

    public TimeSpan? Race1 { get; set; }

    public TimeSpan? Race2 { get; set; }

    public TimeSpan? Race3 { get; set; }

    public TimeSpan? Race4 { get; set; }

    public TimeSpan? Race5 { get; set; }
}