namespace SportHub.Data.Models.Crawlers.Paris2024.Horses;

using System.Text.Json.Serialization;

public class HorsesCrawlerModel
{
    [JsonPropertyName("horses")]
    public List<HorseInfo> Horses { get; set; }
}

public class Discipline
{
    [JsonPropertyName("code")]
    public string Code { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("descriptionOrder")]
    public int DescriptionOrder { get; set; }
}

public class Entry
{
    [JsonPropertyName("code")]
    public string Code { get; set; }

    [JsonPropertyName("value")]
    public string Value { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("pos")]
    public string Pos { get; set; }
}

public class HorseInfo
{
    [JsonPropertyName("__typename")]
    public string Typename { get; set; }

    [JsonPropertyName("discipline")]
    public Discipline Discipline { get; set; }

    [JsonPropertyName("code")]
    public string Code { get; set; }

    [JsonPropertyName("current")]
    public bool Current { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("shortName")]
    public string ShortName { get; set; }

    [JsonPropertyName("yearBirth")]
    public string YearBirth { get; set; }

    [JsonPropertyName("owner")]
    public string Owner { get; set; }

    [JsonPropertyName("entries")]
    public List<Entry> Entries { get; set; }

    [JsonPropertyName("organisation")]
    public Organisation Organisation { get; set; }

    [JsonPropertyName("passport")]
    public string Passport { get; set; }

    [JsonPropertyName("sire")]
    public string Sire { get; set; }

    [JsonPropertyName("groom")]
    public string Groom { get; set; }

    [JsonPropertyName("secondOwner")]
    public string SecondOwner { get; set; }
}

public class Organisation
{
    [JsonPropertyName("code")]
    public string Code { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("longDescription")]
    public string LongDescription { get; set; }

    [JsonPropertyName("protocolOrder")]
    public int ProtocolOrder { get; set; }

    [JsonPropertyName("descriptionOrder")]
    public int DescriptionOrder { get; set; }

    [JsonPropertyName("longDescriptionOrder")]
    public int LongDescriptionOrder { get; set; }
}