namespace SportHub.Data.Models.Crawlers.Paris2024.Events;

using System.Text.Json.Serialization;

public class EventUnitPhase
{
    [JsonPropertyName("type")]
    public string Type { get; set; }
}