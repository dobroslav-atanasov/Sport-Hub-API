namespace SportData.Data.Models.Converters.OlympicGames.Disciplines;

using SportData.Data.Models.Converters.OlympicGames.Base;

public class Baseball : BaseModel
{
    public string Position { get; set; }

    public int? AtBats { get; set; }

    public int? Runs { get; set; }

    public int? Hits { get; set; }

    public int? Doubles { get; set; }

    public int? Triples { get; set; }

    public int? HomeRuns { get; set; }

    public int? RBIs { get; set; }

    public int? Walks { get; set; }

    public int? BattingStrikeouts { get; set; }

    public int? StolenBases { get; set; }

    public int? CaughtStealing { get; set; }

    public int? SacrificeHits { get; set; }

    public int? SacrificeFlies { get; set; }

    public string WinLostSave { get; set; }

    public decimal? InningsPitched { get; set; }

    public int? EarnedRunsAllowed { get; set; }

    public int? RunsAllowed { get; set; }

    public int? HitsAllowed { get; set; }

    public int? HomeRunsAllowed { get; set; }

    public int? Strikeouts { get; set; }

    public int? BasesOnBalls { get; set; }

    public int? WildPitches { get; set; }

    public int? Putouts { get; set; }

    public int? Assists { get; set; }

    public int? Errors { get; set; }

    public int? LOB { get; set; }

    public List<Inning> Innings { get; set; } = [];

    public List<Baseball> Athletes { get; set; } = [];
}

public class Inning
{
    public int Number { get; set; }

    public int? Points { get; set; }
}