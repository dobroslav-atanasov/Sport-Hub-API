namespace SportHub.Data.Models.Crawlers.Paris2024.Events;

using System.Text.Json.Serialization;

public class EventList
{
    [JsonPropertyName("events")]
    public List<EventInfo> Events { get; set; }
}