namespace SportHub.Data.Models.Crawlers.Paris2024.Disciplines;

using System.Text.Json.Serialization;

public class EventPhase
{
    [JsonPropertyName("code")]
    public string Code { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("shortDescription")]
    public string ShortDescription { get; set; }

    [JsonPropertyName("longDescription")]
    public string LongDescription { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("order")]
    public string Order { get; set; }
}