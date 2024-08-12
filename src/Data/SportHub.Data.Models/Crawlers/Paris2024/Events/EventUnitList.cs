namespace SportHub.Data.Models.Crawlers.Paris2024.Events;

using System.Text.Json.Serialization;

using SportHub.Data.Models.Crawlers.Paris2024.PDFs;

public class EventUnitList
{
    [JsonPropertyName("eventUnits")]
    public List<EventUnit> EventUnits { get; set; }
}