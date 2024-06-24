namespace SportData.Data.Models.Converters.OlympicGames.Disciplines;

using SportData.Data.Models.Converters.OlympicGames.Base;

public class Bobsleigh : BaseModel
{
    public TimeSpan? Time { get; set; }

    public List<BobsleighRun> Runs { get; set; } = [];

    public List<Bobsleigh> Athletes { get; set; } = [];
}

public class BobsleighRun
{
    public int Number { get; set; }

    public TimeSpan? Time { get; set; }

    public TimeSpan? Intermediate1 { get; set; }

    public TimeSpan? Intermediate2 { get; set; }

    public TimeSpan? Intermediate3 { get; set; }

    public TimeSpan? Intermediate4 { get; set; }

    public TimeSpan? Intermediate5 { get; set; }
}