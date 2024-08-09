namespace SportHub.Data.Models.Crawlers.Paris2024;

using System.Text.Json.Serialization;

public class GroupInfo
{
    [JsonPropertyName("isLive")]
    public bool IsLive { get; set; }

    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("startDate")]
    public DateTime StartDate { get; set; }

    [JsonPropertyName("hasMedals")]
    public bool HasMedals { get; set; }

    [JsonPropertyName("hasWarnings")]
    public bool HasWarnings { get; set; }

    [JsonPropertyName("title")]
    public string Title { get; set; }

    [JsonPropertyName("subTitle")]
    public string SubTitle { get; set; }

    [JsonPropertyName("unitsCount")]
    public int UnitsCount { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; }
}