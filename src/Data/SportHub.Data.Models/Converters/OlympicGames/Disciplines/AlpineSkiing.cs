namespace SportHub.Data.Models.Converters.OlympicGames.Disciplines;
using SportHub.Data.Models.Converters.OlympicGames.Base;

public class AlpineSkiing : BaseModel
{
    public TimeSpan? Time { get; set; }

    public double? Points { get; set; }

    public TimeSpan? Downhill { get; set; }

    public double? DownhillPoints { get; set; }

    public TimeSpan? Slalom { get; set; }

    public double? SlalomPoints { get; set; }

    public TimeSpan? PenaltyTime { get; set; }

    public TimeSpan? Run1 { get; set; }

    public TimeSpan? Run2 { get; set; }

    public int Race { get; set; }

    public List<AlpineSkiing> Athletes { get; set; } = [];
}