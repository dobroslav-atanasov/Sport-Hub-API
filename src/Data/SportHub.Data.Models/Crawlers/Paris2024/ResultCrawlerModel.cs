namespace SportHub.Data.Models.Crawlers.Paris2024;

using System.Text.Json.Serialization;

public class ResultCrawlerModel
{
    [JsonPropertyName("results")]
    public ResultInfo Result { get; set; }
}

public class EventUnit
{
    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("shortDescription")]
    public string ShortDescription { get; set; }

    [JsonPropertyName("longDescription")]
    public string LongDescription { get; set; }

    [JsonPropertyName("phase")]
    public Phase Phase { get; set; }
}

public class ExtendedInfo
{
    [JsonPropertyName("extended_info_code")]
    public string ExtendedInfoCode { get; set; }

    [JsonPropertyName("ei_code")]
    public string EiCode { get; set; }

    [JsonPropertyName("ei_type")]
    public string EiType { get; set; }

    [JsonPropertyName("ei_value")]
    public string EiValue { get; set; }

    [JsonPropertyName("ei_pos")]
    public string EiPos { get; set; }

    [JsonPropertyName("extensions")]
    public List<Extension> Extensions { get; set; }
}

public class Extension
{
    [JsonPropertyName("ei_extension_code")]
    public string EiExtensionCode { get; set; }

    [JsonPropertyName("eie_code")]
    public string EieCode { get; set; }

    [JsonPropertyName("eie_value")]
    public string EieValue { get; set; }
}

public class Location
{
    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("longDescription")]
    public string LongDescription { get; set; }

    [JsonPropertyName("shortDescription")]
    public string ShortDescription { get; set; }
}

public class Phase
{
    [JsonPropertyName("order")]
    public string Order { get; set; }
}

public class ResultInfo
{
    [JsonPropertyName("date")]
    public DateTime Date { get; set; }

    [JsonPropertyName("eventUnitCode")]
    public string EventUnitCode { get; set; }

    [JsonPropertyName("eventUnit")]
    public EventUnit EventUnit { get; set; }

    [JsonPropertyName("status")]
    public Status Status { get; set; }

    [JsonPropertyName("extendedInfos")]
    public List<ExtendedInfo> ExtendedInfos { get; set; }

    [JsonPropertyName("schedule")]
    public Schedule Schedule { get; set; }
}

public class Schedule
{
    [JsonPropertyName("startDate")]
    public DateTime StartDate { get; set; }

    [JsonPropertyName("endDate")]
    public DateTime EndDate { get; set; }

    [JsonPropertyName("status")]
    public Status Status { get; set; }

    [JsonPropertyName("venue")]
    public Venue Venue { get; set; }

    [JsonPropertyName("location")]
    public Location Location { get; set; }
}

public class Status
{
    [JsonPropertyName("code")]
    public string Code { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }
}

public class Venue
{
    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("longDescription")]
    public string LongDescription { get; set; }
}