﻿namespace SportHub.Data.Models.Crawlers.Paris2024;

using System.Text.Json.Serialization;

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