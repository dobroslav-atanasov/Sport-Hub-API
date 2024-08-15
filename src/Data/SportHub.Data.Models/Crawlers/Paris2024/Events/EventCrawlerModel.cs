namespace SportHub.Data.Models.Crawlers.Paris2024.Events;

using System.Text.Json.Serialization;

public class EventCrawlerModel
{
    [JsonPropertyName("event")]
    public EventModel Event { get; set; }
}

public class ScheduleEventModel
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
    public ResultEventModel Result { get; set; }
}

public class StatusEventModel
{
    [JsonPropertyName("code")]
    public string Code { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }
}

public class UnitEventModel
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
    public ScheduleEventModel Schedule { get; set; }
}

public class EventModel
{
    [JsonPropertyName("code")]
    public string Code { get; set; }

    [JsonPropertyName("gender")]
    public GenderEventModel Gender { get; set; }

    [JsonPropertyName("isTeam")]
    public bool IsTeam { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("longDescription")]
    public string LongDescription { get; set; }

    [JsonPropertyName("phases")]
    public List<PhaseEventModel> Phases { get; set; }
}

public class GenderEventModel
{
    [JsonPropertyName("code")]
    public string Code { get; set; }
}

public class PhaseEventModel
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
    public List<UnitEventModel> Units { get; set; }
}

public class ResultEventModel
{
    [JsonPropertyName("status")]
    public StatusEventModel Status { get; set; }
}