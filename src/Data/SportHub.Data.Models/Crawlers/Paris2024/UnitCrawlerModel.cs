namespace SportHub.Data.Models.Crawlers.Paris2024.Unit;

using System.Text.Json.Serialization;

public class UnitCrawlerModel
{
    [JsonPropertyName("units")]
    public List<UnitInfo> Units { get; set; }

    [JsonPropertyName("groups")]
    public List<GroupInfo> Groups { get; set; }
}

public class UnitInfo
{
    [JsonPropertyName("disciplineName")]
    public string DisciplineName { get; set; }

    [JsonPropertyName("eventUnitName")]
    public string EventUnitName { get; set; }

    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("disciplineCode")]
    public string DisciplineCode { get; set; }

    [JsonPropertyName("genderCode")]
    public string GenderCode { get; set; }

    [JsonPropertyName("eventCode")]
    public string EventCode { get; set; }

    [JsonPropertyName("phaseCode")]
    public string PhaseCode { get; set; }

    [JsonPropertyName("eventId")]
    public string EventId { get; set; }

    [JsonPropertyName("eventName")]
    public string EventName { get; set; }

    [JsonPropertyName("phaseId")]
    public string PhaseId { get; set; }

    [JsonPropertyName("phaseName")]
    public string PhaseName { get; set; }

    [JsonPropertyName("disciplineId")]
    public string DisciplineId { get; set; }

    [JsonPropertyName("eventOrder")]
    public int EventOrder { get; set; }

    [JsonPropertyName("phaseType")]
    public string PhaseType { get; set; }

    [JsonPropertyName("eventUnitType")]
    public string EventUnitType { get; set; }

    [JsonPropertyName("olympicDay")]
    public string OlympicDay { get; set; }

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

    [JsonPropertyName("order")]
    public int Order { get; set; }

    [JsonPropertyName("venue")]
    public string Venue { get; set; }

    [JsonPropertyName("venueDescription")]
    public string VenueDescription { get; set; }

    [JsonPropertyName("location")]
    public string Location { get; set; }

    [JsonPropertyName("locationDescription")]
    public string LocationDescription { get; set; }

    [JsonPropertyName("status")]
    public string Status { get; set; }

    [JsonPropertyName("statusDescription")]
    public string StatusDescription { get; set; }

    [JsonPropertyName("medalFlag")]
    public int MedalFlag { get; set; }

    [JsonPropertyName("liveFlag")]
    public bool LiveFlag { get; set; }

    [JsonPropertyName("scheduleItemType")]
    public string ScheduleItemType { get; set; }

    [JsonPropertyName("unitNum")]
    public string UnitNum { get; set; }

    [JsonPropertyName("sessionCode")]
    public string SessionCode { get; set; }

    [JsonPropertyName("groupId")]
    public string GroupId { get; set; }

    [JsonPropertyName("resCode")]
    public string ResCode { get; set; }

    [JsonPropertyName("competitors")]
    public List<Competitor> Competitors { get; set; }

    [JsonPropertyName("extraData")]
    public ExtraData ExtraData { get; set; }
}

public class ExtraData
{
    [JsonPropertyName("detailUrl")]
    public string DetailUrl { get; set; }
}

public class Competitor
{
    [JsonPropertyName("code")]
    public string Code { get; set; }

    [JsonPropertyName("noc")]
    public string Noc { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("order")]
    public int Order { get; set; }

    [JsonPropertyName("results")]
    public Results Results { get; set; }
}

public class Results
{
    [JsonPropertyName("position")]
    public string Position { get; set; }

    [JsonPropertyName("mark")]
    public string Mark { get; set; }

    [JsonPropertyName("medalType")]
    public string MedalType { get; set; }

    [JsonPropertyName("irm")]
    public string Irm { get; set; }
}

public class GroupInfo
{
    [JsonPropertyName("isLive")]
    public bool IsLive { get; set; }

    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("startDate")]
    public DateTime StartDate { get; set; }

    [JsonPropertyName("hasMedals")]
    public bool HasMedals { get; set; }

    [JsonPropertyName("hasWarnings")]
    public bool HasWarnings { get; set; }

    [JsonPropertyName("title")]
    public string Title { get; set; }

    [JsonPropertyName("subTitle")]
    public string SubTitle { get; set; }

    [JsonPropertyName("unitsCount")]
    public int UnitsCount { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; }
}