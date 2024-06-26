namespace SportHub.Data.Models.Converters.OlympicGames.Disciplines;

using SportHub.Data.Models.Converters.OlympicGames.Base;

public class CyclingTrack : BaseModel
{
    public TimeSpan? Time { get; set; }

    public TimeSpan? SprintTime { get; set; }

    public string LapMargin { get; set; }

    public string Margin { get; set; }

    public decimal? Points { get; set; }

    public int? Wins { get; set; }

    public TimeSpan? Race1 { get; set; }

    public TimeSpan? Race2 { get; set; }

    public TimeSpan? Race3 { get; set; }

    public int? ScratchPoints { get; set; }

    public int? PointRacePoints { get; set; }

    public int? FlyingStartPoints { get; set; }

    public int? TimeTrialPoints { get; set; }

    public int? IndividualPursuitPoints { get; set; }

    public int? EliminationRacePoints { get; set; }

    public List<CyclingTrack> Athletes { get; set; } = [];
}