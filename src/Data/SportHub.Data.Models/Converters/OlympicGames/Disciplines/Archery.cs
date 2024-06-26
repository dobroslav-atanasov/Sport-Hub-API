namespace SportHub.Data.Models.Converters.OlympicGames.Disciplines;
using SportHub.Data.Models.Converters.OlympicGames.Base;

public class Archery : BaseModel
{
    public double? Points { get; set; }

    public string Target { get; set; }
    public int? TargetsHit { get; set; }
    public int? Score10s { get; set; }
    public int? Score9s { get; set; }
    public int? ScoreXs { get; set; }
    public int? ShootOff { get; set; }

    public int? Meters30 { get; set; }
    public int? Meters50 { get; set; }
    public int? Meters60 { get; set; }
    public int? Meters70 { get; set; }
    public int? Meters90 { get; set; }

    public int? Part1 { get; set; }
    public int? Part2 { get; set; }
    public int? Score { get; set; }
    public int? Golds { get; set; }

    public int? Yards30 { get; set; }
    public int? Yards40 { get; set; }
    public int? Yards50 { get; set; }
    public int? Yards60 { get; set; }
    public int? Yards80 { get; set; }
    public int? Yards100 { get; set; }

    public int? Tiebreak1 { get; set; }
    public int? Tiebreak2 { get; set; }

    public int? Sets { get; set; }
    public int? Set1 { get; set; }
    public int? Set2 { get; set; }
    public int? Set3 { get; set; }
    public int? Set4 { get; set; }
    public int? Set5 { get; set; }

    public List<Arrow> Arrows { get; set; } = [];

    public List<Archery> Athletes { get; set; } = [];
}

public class Arrow
{
    public int Number { get; set; }

    public string Points { get; set; }
}