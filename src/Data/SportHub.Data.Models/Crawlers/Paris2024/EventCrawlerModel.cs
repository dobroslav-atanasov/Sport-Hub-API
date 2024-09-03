namespace SportHub.Data.Models.Crawlers.Paris2024.Event;

using System.Text.Json.Serialization;

public class EventCrawlerModel
{
    [JsonPropertyName("event")]
    public EventInfo Event { get; set; }
}

public class EventInfo
{
    [JsonPropertyName("code")]
    public string Code { get; set; }

    [JsonPropertyName("gender")]
    public Gender Gender { get; set; }

    [JsonPropertyName("isTeam")]
    public bool IsTeam { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("longDescription")]
    public string LongDescription { get; set; }

    [JsonPropertyName("phases")]
    public List<PhaseInfo> Phases { get; set; }
}

public class Gender
{
    [JsonPropertyName("code")]
    public string Code { get; set; }
}

public class PhaseInfo
{
    [JsonPropertyName("code")]
    public string Code { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("longDescription")]
    public string LongDescription { get; set; }

    [JsonPropertyName("shortDescription")]
    public string ShortDescription { get; set; }

    [JsonPropertyName("order")]
    public string Order { get; set; }

    [JsonPropertyName("units")]
    public List<UnitInfo> Units { get; set; }
}

public class ResultInfo
{
    [JsonPropertyName("status")]
    public Status Status { get; set; }
}

public class ScheduleInfo
{
    [JsonPropertyName("startDate")]
    public DateTime StartDate { get; set; }

    [JsonPropertyName("endDate")]
    public DateTime EndDate { get; set; }

    [JsonPropertyName("medal")]
    public int Medal { get; set; }

    [JsonPropertyName("unitNum")]
    public string UnitNum { get; set; }

    [JsonPropertyName("status")]
    public Status Status { get; set; }

    [JsonPropertyName("result")]
    public ResultInfo Result { get; set; }
}

public class Status
{
    [JsonPropertyName("code")]
    public string Code { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }
}

public class UnitInfo
{
    [JsonPropertyName("code")]
    public string Code { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("scheduled")]
    public string Scheduled { get; set; }

    [JsonPropertyName("order")]
    public string Order { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("shortDescription")]
    public string ShortDescription { get; set; }

    [JsonPropertyName("longDescription")]
    public string LongDescription { get; set; }

    [JsonPropertyName("schedule")]
    public ScheduleInfo Schedule { get; set; }
}
