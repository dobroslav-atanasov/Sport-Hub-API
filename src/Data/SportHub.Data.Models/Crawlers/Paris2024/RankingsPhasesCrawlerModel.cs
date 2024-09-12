namespace SportHub.Data.Models.Crawlers.Paris2024.RankingsPhases;

using System.Text.Json.Serialization;

public class RankingsPhasesCrawlerModel
{
    [JsonPropertyName("event")]
    public EventInfo Event { get; set; }
}

public class Athlete
{
    [JsonPropertyName("code")]
    public string Code { get; set; }

    [JsonPropertyName("personGender")]
    public PersonGender PersonGender { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("shortName")]
    public string ShortName { get; set; }

    [JsonPropertyName("givenName")]
    public string GivenName { get; set; }

    [JsonPropertyName("familyName")]
    public string FamilyName { get; set; }

    [JsonPropertyName("__typename")]
    public string Typename { get; set; }

    [JsonPropertyName("mainFunction")]
    public MainFunction MainFunction { get; set; }

    [JsonPropertyName("organisation")]
    public Organisation Organisation { get; set; }
}

public class Away
{
    [JsonPropertyName("score")]
    public string Score { get; set; }

    [JsonPropertyName("periodScore")]
    public string PeriodScore { get; set; }

    [JsonPropertyName("participant")]
    public ParticipantInfo Participant { get; set; }
}

public class Cumulative
{
    [JsonPropertyName("documentCode")]
    public string DocumentCode { get; set; }

    [JsonPropertyName("results")]
    public List<ResultInfo> Results { get; set; }
}

public class EventInfo
{
    [JsonPropertyName("phases")]
    public List<PhaseInfo> Phases { get; set; }
}

public class EventUnitEntry
{
    [JsonPropertyName("eue_value")]
    public string EueValue { get; set; }

    [JsonPropertyName("eue_code")]
    public string EueCode { get; set; }

    [JsonPropertyName("eue_type")]
    public string EueType { get; set; }
}

public class ExtendedInfo
{
    [JsonPropertyName("ei_code")]
    public string EiCode { get; set; }

    [JsonPropertyName("ei_value")]
    public string EiValue { get; set; }
}

public class ExtendedResult
{
    [JsonPropertyName("er_code")]
    public string ErCode { get; set; }

    [JsonPropertyName("er_type")]
    public string ErType { get; set; }

    [JsonPropertyName("er_value")]
    public string ErValue { get; set; }

    [JsonPropertyName("er_pos")]
    public string ErPos { get; set; }
}

public class Home
{
    [JsonPropertyName("score")]
    public string Score { get; set; }

    [JsonPropertyName("periodScore")]
    public string PeriodScore { get; set; }

    [JsonPropertyName("participant")]
    public ParticipantInfo Participant { get; set; }
}

public class Item
{
    [JsonPropertyName("sortOrder")]
    public int SortOrder { get; set; }

    [JsonPropertyName("startSortOrder")]
    public int StartSortOrder { get; set; }

    [JsonPropertyName("startOrder")]
    public string StartOrder { get; set; }

    [JsonPropertyName("itemType")]
    public string ItemType { get; set; }

    [JsonPropertyName("resultType")]
    public string ResultType { get; set; }

    [JsonPropertyName("resultData")]
    public string ResultData { get; set; }

    [JsonPropertyName("resultDataText")]
    public string ResultDataText { get; set; }

    [JsonPropertyName("resultWLT")]
    public string ResultWLT { get; set; }

    [JsonPropertyName("medalType")]
    public string MedalType { get; set; }

    [JsonPropertyName("teamCode")]
    public string TeamCode { get; set; }

    [JsonPropertyName("participant")]
    public ParticipantInfo Participant { get; set; }

    [JsonPropertyName("teamAthletes")]
    public List<TeamAthlete> TeamAthletes { get; set; }

    //[JsonPropertyName("resultRank")]
    //public string ResultRank { get; set; }

    [JsonPropertyName("athleteCode")]
    public string AthleteCode { get; set; }

    [JsonPropertyName("extendedResults")]
    public List<ExtendedResult> ExtendedResults { get; set; }

    [JsonPropertyName("diff")]
    public string Diff { get; set; }

    [JsonPropertyName("qualificationMark")]
    public string QualificationMark { get; set; }

    [JsonPropertyName("bib")]
    public string Bib { get; set; }

    [JsonPropertyName("recordIndicators")]
    public List<RecordIndicator> RecordIndicators { get; set; }

    [JsonPropertyName("eventUnitCode")]
    public string EventUnitCode { get; set; }
}

public class MainFunction
{
    [JsonPropertyName("category")]
    public string Category { get; set; }
}

public class Organisation
{
    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("code")]
    public string Code { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("longDescription")]
    public string LongDescription { get; set; }
}

public class ParticipantInfo
{
    [JsonPropertyName("code")]
    public string Code { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("shortName")]
    public string ShortName { get; set; }

    [JsonPropertyName("__typename")]
    public string Typename { get; set; }

    [JsonPropertyName("organisation")]
    public Organisation Organisation { get; set; }

    [JsonPropertyName("givenName")]
    public string GivenName { get; set; }

    [JsonPropertyName("familyName")]
    public string FamilyName { get; set; }

    [JsonPropertyName("mainFunction")]
    public MainFunction MainFunction { get; set; }
}

public class ParticipantCode
{
    [JsonPropertyName("code")]
    public string Code { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("shortName")]
    public string ShortName { get; set; }

    [JsonPropertyName("__typename")]
    public string Typename { get; set; }

    [JsonPropertyName("mainFunction")]
    public MainFunction MainFunction { get; set; }

    [JsonPropertyName("organisation")]
    public Organisation Organisation { get; set; }
}

public class Period
{
    [JsonPropertyName("period_code")]
    public string PeriodCode { get; set; }

    [JsonPropertyName("home")]
    public Home Home { get; set; }

    [JsonPropertyName("away")]
    public Away Away { get; set; }
}

public class PersonGender
{
    [JsonPropertyName("code")]
    public string Code { get; set; }
}

public class PhaseInfo
{
    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("shortDescription")]
    public string ShortDescription { get; set; }

    [JsonPropertyName("longDescription")]
    public string LongDescription { get; set; }

    [JsonPropertyName("order")]
    public string Order { get; set; }

    [JsonPropertyName("units")]
    public List<UnitInfo> Units { get; set; }

    [JsonPropertyName("phaseResult")]
    public PhaseResult PhaseResult { get; set; }

    [JsonPropertyName("cumulative")]
    public Cumulative Cumulative { get; set; }
}

public class PhaseResult
{
    [JsonPropertyName("phaseCode")]
    public string PhaseCode { get; set; }

    [JsonPropertyName("items")]
    public List<Item> Items { get; set; }
}

public class RecordInfo
{
    [JsonPropertyName("recordCode")]
    public string RecordCode { get; set; }

    [JsonPropertyName("event")]
    public string Event { get; set; }

    [JsonPropertyName("order")]
    public int Order { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }
}

public class RecordIndicator
{
    [JsonPropertyName("ri_order")]
    public int RiOrder { get; set; }

    [JsonPropertyName("ri_code")]
    public string RiCode { get; set; }

    [JsonPropertyName("ri_recordtype")]
    public string RiRecordtype { get; set; }

    [JsonPropertyName("record")]
    public RecordInfo Record { get; set; }

    [JsonPropertyName("recordType")]
    public RecordType RecordType { get; set; }
}

public class RecordType
{
    [JsonPropertyName("code")]
    public string Code { get; set; }

    [JsonPropertyName("recordType")]
    public string RecordType1 { get; set; }

    [JsonPropertyName("discipline")]
    public string Discipline { get; set; }

    [JsonPropertyName("group")]
    public string Group { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }
}

public class ResultInfo
{
    [JsonPropertyName("schedule")]
    public Schedule Schedule { get; set; }

    [JsonPropertyName("eventUnitCode")]
    public string EventUnitCode { get; set; }

    [JsonPropertyName("date")]
    public DateTime Date { get; set; }

    [JsonPropertyName("extendedInfos")]
    public List<ExtendedInfo> ExtendedInfos { get; set; }

    [JsonPropertyName("periods")]
    public List<Period> Periods { get; set; }

    [JsonPropertyName("items")]
    public List<Item> Items { get; set; }
}

public class Schedule
{
    [JsonPropertyName("startDate")]
    public DateTime StartDate { get; set; }
}

public class TeamAthlete
{
    [JsonPropertyName("participantCode")]
    public ParticipantCode ParticipantCode { get; set; }

    [JsonPropertyName("eventUnitEntries")]
    public List<EventUnitEntry> EventUnitEntries { get; set; }

    [JsonPropertyName("athlete")]
    public Athlete Athlete { get; set; }
}

public class UnitInfo
{
    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("scheduled")]
    public string Scheduled { get; set; }

    [JsonPropertyName("order")]
    public string Order { get; set; }

    [JsonPropertyName("medal")]
    public int Medal { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("shortDescription")]
    public string ShortDescription { get; set; }

    [JsonPropertyName("longDescription")]
    public string LongDescription { get; set; }

    [JsonPropertyName("result")]
    public ResultInfo Result { get; set; }



}