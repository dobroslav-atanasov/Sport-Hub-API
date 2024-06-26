namespace SportHub.Data.Models.Converters.OlympicGames.Disciplines;

using SportHub.Data.Models.Converters.OlympicGames.Base;

public class CanoeSlalom : BaseModel
{
    public TimeSpan? Time { get; set; }

    public TimeSpan? TotalTime { get; set; }

    public int? PenaltySeconds { get; set; }

    public TimeSpan? Run1 { get; set; }

    public TimeSpan? Run1TotalTime { get; set; }

    public int? Run1PenaltySeconds { get; set; }

    public TimeSpan? Run2 { get; set; }

    public TimeSpan? Run2TotalTime { get; set; }

    public int? Run2PenaltySeconds { get; set; }

    public List<CanoeSlalom> Athletes { get; set; } = [];
}