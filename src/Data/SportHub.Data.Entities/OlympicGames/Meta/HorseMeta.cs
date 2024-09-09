namespace SportHub.Data.Entities.OlympicGames.Meta;

public class HorseMeta
{
    public List<HorseDataMeta> Data { get; set; } = [];
}

public class HorseDataMeta
{
    public string Key { get; set; }

    public string Value { get; set; }

    public string Type { get; set; }

    public string Position { get; set; }
}