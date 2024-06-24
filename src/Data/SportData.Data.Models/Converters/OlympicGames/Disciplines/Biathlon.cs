namespace SportData.Data.Models.Converters.OlympicGames.Disciplines;

using SportData.Data.Models.Converters.OlympicGames.Base;

public class Biathlon : BaseModel
{
    public TimeSpan? Time { get; set; }

    public int? Misses { get; set; }

    public TimeSpan? Skiing { get; set; }

    public TimeSpan? StartBehind { get; set; }

    public int? Penalties { get; set; }

    public TimeSpan? Exchange { get; set; }

    public int? ExtraShots { get; set; }

    public string Position { get; set; }

    public List<Shooting> Shootings { get; set; } = [];

    public List<Biathlon> Athletes { get; set; } = [];
}

public class Shooting
{
    public int Number { get; set; }

    public TimeSpan? Time { get; set; }

    public int? Misses { get; set; }

    public int? Penalties { get; set; }

    public int? ExtraShots { get; set; }
}