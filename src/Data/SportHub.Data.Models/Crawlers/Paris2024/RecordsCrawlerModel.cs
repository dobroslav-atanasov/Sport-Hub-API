namespace SportHub.Data.Models.Crawlers.Paris2024.Records;

using System.Text.Json.Serialization;

public class RecordsCrawlerModel
{
    [JsonPropertyName("records")]
    public List<RecordInfo> Records { get; set; }
}

public class Athlete
{
    [JsonPropertyName("order")]
    public int Order { get; set; }

    [JsonPropertyName("code")]
    public string Code { get; set; }

    [JsonPropertyName("birthDate")]
    public string BirthDate { get; set; }

    [JsonPropertyName("givenName")]
    public string GivenName { get; set; }

    [JsonPropertyName("familyName")]
    public string FamilyName { get; set; }

    [JsonPropertyName("gender")]
    public string Gender { get; set; }

    [JsonPropertyName("organisation")]
    public string Organisation { get; set; }

    [JsonPropertyName("participant")]
    public Participant Participant { get; set; }
}

public class Competitor
{
    [JsonPropertyName("code")]
    public string Code { get; set; }

    [JsonPropertyName("organisation")]
    public Organisation Organisation { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("participant")]
    public Participant Participant { get; set; }

    [JsonPropertyName("athletes")]
    public List<Athlete> Athletes { get; set; }
}

public class EventUnit
{
    [JsonPropertyName("code")]
    public string Code { get; set; }

    [JsonPropertyName("longDescription")]
    public string LongDescription { get; set; }

    [JsonPropertyName("shortDescription")]
    public string ShortDescription { get; set; }

    [JsonPropertyName("schedule")]
    public Schedule Schedule { get; set; }
}

public class MainFunction
{
    [JsonPropertyName("functionCode")]
    public string FunctionCode { get; set; }

    [JsonPropertyName("category")]
    public string Category { get; set; }
}

public class Organisation
{
    [JsonPropertyName("code")]
    public string Code { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("longDescription")]
    public string LongDescription { get; set; }
}

public class Participant
{
    [JsonPropertyName("code")]
    public string Code { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("current")]
    public bool Current { get; set; }

    [JsonPropertyName("__typename")]
    public string Typename { get; set; }

    [JsonPropertyName("shortName")]
    public string ShortName { get; set; }

    [JsonPropertyName("organisation")]
    public Organisation Organisation { get; set; }

    [JsonPropertyName("givenName")]
    public string GivenName { get; set; }

    [JsonPropertyName("familyName")]
    public string FamilyName { get; set; }

    [JsonPropertyName("mainFunction")]
    public MainFunction MainFunction { get; set; }
}

public class RecordInfo
{
    [JsonPropertyName("recordId")]
    public string RecordId { get; set; }

    [JsonPropertyName("notEstablished")]
    public bool NotEstablished { get; set; }

    [JsonPropertyName("recordCode")]
    public string RecordCode { get; set; }

    [JsonPropertyName("order")]
    public int Order { get; set; }

    [JsonPropertyName("record")]
    public Record2 Record { get; set; }

    [JsonPropertyName("recordType")]
    public RecordType RecordType { get; set; }

    [JsonPropertyName("recordData")]
    public List<RecordDatum> RecordData { get; set; }
}

public class Record2
{
    [JsonPropertyName("order")]
    public int Order { get; set; }

    [JsonPropertyName("recordCode")]
    public string RecordCode { get; set; }

    [JsonPropertyName("event")]
    public string Event { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }
}

public class RecordDatum
{
    [JsonPropertyName("order")]
    public int Order { get; set; }

    [JsonPropertyName("current")]
    public bool Current { get; set; }

    [JsonPropertyName("historic")]
    public bool Historic { get; set; }

    [JsonPropertyName("recordDate")]
    public string RecordDate { get; set; }

    [JsonPropertyName("resultType")]
    public string ResultType { get; set; }

    [JsonPropertyName("result")]
    public string Result { get; set; }

    [JsonPropertyName("country")]
    public string Country { get; set; }

    [JsonPropertyName("place")]
    public string Place { get; set; }

    [JsonPropertyName("competition")]
    public string Competition { get; set; }

    [JsonPropertyName("competitor")]
    public Competitor Competitor { get; set; }

    [JsonPropertyName("eventUnit")]
    public EventUnit EventUnit { get; set; }

    [JsonPropertyName("recordExtensions")]
    public List<RecordExtension> RecordExtensions { get; set; }
}

public class RecordExtension
{
    [JsonPropertyName("code")]
    public string Code { get; set; }

    [JsonPropertyName("value")]
    public string Value { get; set; }
}

public class RecordType
{
    [JsonPropertyName("code")]
    public string Code { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }
}

public class Schedule
{
    [JsonPropertyName("startDate")]
    public DateTime StartDate { get; set; }
}
