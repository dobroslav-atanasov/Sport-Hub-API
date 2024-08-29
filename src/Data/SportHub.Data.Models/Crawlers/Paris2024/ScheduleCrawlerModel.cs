namespace SportHub.Data.Models.Crawlers.Paris2024.Schedule;

using System.Text.Json.Serialization;

public class ScheduleCrawlerModel
{
    [JsonPropertyName("schedules")]
    public List<ScheduleInfo> Schedules { get; set; }
}

public class Location
{
    [JsonPropertyName("locationOrder")]
    public int LocationOrder { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("longDescription")]
    public string LongDescription { get; set; }

    [JsonPropertyName("shortDescription")]
    public string ShortDescription { get; set; }
}

public class Organisation
{
    [JsonPropertyName("code")]
    public string Code { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("longDescription")]
    public string LongDescription { get; set; }
}

public class Participant
{
    [JsonPropertyName("__typename")]
    public string Typename { get; set; }

    [JsonPropertyName("code")]
    public string Code { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("shortName")]
    public string ShortName { get; set; }

    [JsonPropertyName("organisation")]
    public Organisation Organisation { get; set; }
}

public class Result
{
    [JsonPropertyName("status")]
    public Status Status { get; set; }
}

public class ScheduleInfo
{
    [JsonPropertyName("code")]
    public string Code { get; set; }

    [JsonPropertyName("order")]
    public int Order { get; set; }

    [JsonPropertyName("unitNum")]
    public string UnitNum { get; set; }

    [JsonPropertyName("startDate")]
    public DateTime StartDate { get; set; }

    [JsonPropertyName("endDate")]
    public DateTime EndDate { get; set; }

    [JsonPropertyName("hideStartDate")]
    public bool HideStartDate { get; set; }

    [JsonPropertyName("hideEndDate")]
    public bool HideEndDate { get; set; }

    [JsonPropertyName("startText")]
    public string StartText { get; set; }

    [JsonPropertyName("start")]
    public List<Start> Start { get; set; }

    [JsonPropertyName("status")]
    public Status Status { get; set; }

    [JsonPropertyName("venue")]
    public Venue Venue { get; set; }

    [JsonPropertyName("location")]
    public Location Location { get; set; }

    [JsonPropertyName("result")]
    public Result Result { get; set; }
}

public class Start
{
    [JsonPropertyName("sortOrder")]
    public int SortOrder { get; set; }

    [JsonPropertyName("startOrder")]
    public int StartOrder { get; set; }

    [JsonPropertyName("athleteCode")]
    public string AthleteCode { get; set; }

    [JsonPropertyName("participant")]
    public Participant Participant { get; set; }
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
    [JsonPropertyName("isCompetition")]
    public bool IsCompetition { get; set; }

    [JsonPropertyName("inOutDoor")]
    public string InOutDoor { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("longDescription")]
    public string LongDescription { get; set; }
}