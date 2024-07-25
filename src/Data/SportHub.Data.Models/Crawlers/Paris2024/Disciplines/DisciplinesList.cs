namespace SportHub.Data.Models.Crawlers.Paris2024.Disciplines;

using System.Text.Json.Serialization;

public class DisciplinesList
{
    [JsonPropertyName("disciplines")]
    public List<Discipline> Disciplines { get; set; }
}

public class Discipline
{
    [JsonPropertyName("isSport")]
    public bool IsSport { get; set; }

    [JsonPropertyName("scheduled")]
    public bool Scheduled { get; set; }

    [JsonPropertyName("code")]
    public string Code { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("seodescription")]
    public string Seodescription { get; set; }
}