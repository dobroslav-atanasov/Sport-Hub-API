namespace SportData.Data.Models.Converters.OlympicGames.Disciplines;

using SportData.Data.Models.Converters.OlympicGames.Base;

public class Curling : BaseModel
{
    public int? Points { get; set; }

    public string Position { get; set; }

    public int? Percent { get; set; }

    public List<End> Ends { get; set; } = [];

    public List<Curling> Athletes { get; set; } = [];
}

public class End
{
    public int Number { get; set; }

    public int? Points { get; set; }
}