﻿namespace SportHub.Data.Models.Crawlers.Paris2024.EventUnit;

using System.Text.Json.Serialization;

public class EventUnitCrawlerModel
{
    [JsonPropertyName("eventUnits")]
    public List<EventUnit> EventUnits { get; set; }
}

public class EventUnit
{
    [JsonPropertyName("code")]
    public string Code { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("shortDescription")]
    public string ShortDescription { get; set; }

    [JsonPropertyName("longDescription")]
    public string LongDescription { get; set; }

    [JsonPropertyName("seodescription")]
    public string Seodescription { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("scheduled")]
    public string Scheduled { get; set; }

    [JsonPropertyName("phase")]
    public Phase Phase { get; set; }
}

public class Phase
{
    [JsonPropertyName("type")]
    public string Type { get; set; }
}