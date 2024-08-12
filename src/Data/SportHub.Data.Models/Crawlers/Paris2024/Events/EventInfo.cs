namespace SportHub.Data.Models.Crawlers.Paris2024.Events;

using System.Text.Json.Serialization;

public class EventInfo
{
    [JsonPropertyName("order")]
    public int Order { get; set; }

    [JsonPropertyName("code")]
    public string Code { get; set; }

    [JsonPropertyName("isTeam")]
    public bool IsTeam { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("longDescription")]
    public string LongDescription { get; set; }

    [JsonPropertyName("seodescription")]
    public string Seodescription { get; set; }

    [JsonPropertyName("phases")]
    public List<EventPhase> Phases { get; set; }
}