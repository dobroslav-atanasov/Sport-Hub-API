namespace SportHub.Data.Models.Crawlers.Paris2024.Individuals.Person;

using System.Text.Json.Serialization;

using SportHub.Data.Models.Crawlers.Paris2024.Schedules;

public class PersonCrawlerModel
{
    [JsonPropertyName("person")]
    public PersonInfo Person { get; set; }
}

public class Athlete
{
    [JsonPropertyName("person")]
    public PersonInfo Person { get; set; }
}

public class AudioFile
{
    [JsonPropertyName("audioType")]
    public string AudioType { get; set; }
}
public class Competitor
{
    [JsonPropertyName("code")]
    public string Code { get; set; }
}

public class CountryofBirth
{
    [JsonPropertyName("code")]
    public string Code { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }
}

public class CountryofResidence
{
    [JsonPropertyName("code")]
    public string Code { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }
}

public class Discipline
{
    [JsonPropertyName("code")]
    public string Code { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("descriptionOrder")]
    public int DescriptionOrder { get; set; }
}

public class Event
{
    [JsonPropertyName("code")]
    public string Code { get; set; }

    [JsonPropertyName("longDescription")]
    public string LongDescription { get; set; }

    [JsonPropertyName("order")]
    public int Order { get; set; }

    [JsonPropertyName("rankings")]
    public List<Ranking> Rankings { get; set; }
}

public class EventUnit
{
    [JsonPropertyName("code")]
    public string Code { get; set; }

    [JsonPropertyName("resultRSC")]
    public string ResultRSC { get; set; }

    [JsonPropertyName("longDescription")]
    public string LongDescription { get; set; }

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

    [JsonPropertyName("phase")]
    public Phase Phase { get; set; }
}

public class ExtendedBio
{
    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("code")]
    public string Code { get; set; }

    [JsonPropertyName("value")]
    public string Value { get; set; }
}

public class Highlight
{
    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("value")]
    public string Value { get; set; }
}

public class Image
{
    [JsonPropertyName("imageType")]
    public string ImageType { get; set; }

    [JsonPropertyName("imageExtension")]
    public string ImageExtension { get; set; }

    [JsonPropertyName("imageVersion")]
    public string ImageVersion { get; set; }
}

public class Interest
{
    [JsonPropertyName("nickname")]
    public string Nickname { get; set; }

    [JsonPropertyName("hobbies")]
    public string Hobbies { get; set; }

    [JsonPropertyName("occupation")]
    public string Occupation { get; set; }

    [JsonPropertyName("education")]
    public string Education { get; set; }

    [JsonPropertyName("family")]
    public string Family { get; set; }

    [JsonPropertyName("langSpoken")]
    public string LangSpoken { get; set; }

    [JsonPropertyName("coach")]
    public string Coach { get; set; }

    [JsonPropertyName("debut")]
    public string Debut { get; set; }

    [JsonPropertyName("start")]
    public string Start { get; set; }

    [JsonPropertyName("reason")]
    public string Reason { get; set; }

    [JsonPropertyName("ambition")]
    public string Ambition { get; set; }

    [JsonPropertyName("injuries")]
    public string Injuries { get; set; }

    [JsonPropertyName("natLeague")]
    public string NatLeague { get; set; }

    [JsonPropertyName("milestones")]
    public string Milestones { get; set; }

    [JsonPropertyName("training")]
    public string Training { get; set; }

    [JsonPropertyName("hero")]
    public string Hero { get; set; }

    [JsonPropertyName("influence")]
    public string Influence { get; set; }

    [JsonPropertyName("philosophy")]
    public string Philosophy { get; set; }

    [JsonPropertyName("award")]
    public string Award { get; set; }

    [JsonPropertyName("addInformation")]
    public string AddInformation { get; set; }

    [JsonPropertyName("clubName")]
    public string ClubName { get; set; }

    [JsonPropertyName("positionStyle")]
    public string PositionStyle { get; set; }

    [JsonPropertyName("hand")]
    public string Hand { get; set; }

    [JsonPropertyName("sportingRelatives")]
    public string SportingRelatives { get; set; }

    [JsonPropertyName("otherSports")]
    public string OtherSports { get; set; }

    [JsonPropertyName("extendedBios")]
    public List<ExtendedBio> ExtendedBios { get; set; }
}

public class Item
{
    [JsonPropertyName("resultDataText")]
    public string ResultDataText { get; set; }

    [JsonPropertyName("participant")]
    public ParticipantInfo Participant { get; set; }
}

public class Location
{
    [JsonPropertyName("shortDescription")]
    public string ShortDescription { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("locationOrder")]
    public int LocationOrder { get; set; }
}

public class MainFunction
{
    [JsonPropertyName("category")]
    public string Category { get; set; }

    [JsonPropertyName("functionCode")]
    public string FunctionCode { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }
}

public class Medal
{
    [JsonPropertyName("medal_type")]
    public string MedalType { get; set; }

    [JsonPropertyName("event_code")]
    public string EventCode { get; set; }
}

public class NationalityPerson
{
    [JsonPropertyName("code")]
    public string Code { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("longDescription")]
    public string LongDescription { get; set; }
}

public class Organisation
{
    [JsonPropertyName("code")]
    public string Code { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("longDescription")]
    public string LongDescription { get; set; }

    [JsonPropertyName("protocolOrder")]
    public int ProtocolOrder { get; set; }

    [JsonPropertyName("descriptionOrder")]
    public int DescriptionOrder { get; set; }

    [JsonPropertyName("longDescriptionOrder")]
    public int LongDescriptionOrder { get; set; }
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

    [JsonPropertyName("mainFunction")]
    public MainFunction MainFunction { get; set; }

    [JsonPropertyName("TVName")]
    public string TVName { get; set; }

    [JsonPropertyName("shortTVName")]
    public string ShortTVName { get; set; }
}

public class ParticipantBio
{
    [JsonPropertyName("organisationId")]
    public string OrganisationId { get; set; }

    [JsonPropertyName("gender")]
    public string Gender { get; set; }

    [JsonPropertyName("isHistoric")]
    public bool IsHistoric { get; set; }

    [JsonPropertyName("placeofBirth")]
    public string PlaceofBirth { get; set; }

    [JsonPropertyName("countryofBirth")]
    public CountryofBirth CountryofBirth { get; set; }

    [JsonPropertyName("placeofResidence")]
    public string PlaceofResidence { get; set; }

    [JsonPropertyName("countryofResidence")]
    public CountryofResidence CountryofResidence { get; set; }

    [JsonPropertyName("highlights")]
    public List<Highlight> Highlights { get; set; }

    [JsonPropertyName("interest")]
    public Interest Interest { get; set; }
}

public class PersonInfo
{
    [JsonPropertyName("code")]
    public string Code { get; set; }

    [JsonPropertyName("audioFile")]
    public AudioFile AudioFile { get; set; }

    [JsonPropertyName("image")]
    public Image Image { get; set; }

    [JsonPropertyName("organisation")]
    public Organisation Organisation { get; set; }

    [JsonPropertyName("nationality")]
    public NationalityPerson Nationality { get; set; }

    [JsonPropertyName("personGender")]
    public PersonGender PersonGender { get; set; }

    [JsonPropertyName("disciplines")]
    public List<Discipline> Disciplines { get; set; }

    [JsonPropertyName("birthDate")]
    public string BirthDate { get; set; }

    [JsonPropertyName("height")]
    public int Height { get; set; }

    [JsonPropertyName("weight")]
    public int Weight { get; set; }

    [JsonPropertyName("givenName")]
    public string GivenName { get; set; }

    [JsonPropertyName("familyName")]
    public string FamilyName { get; set; }

    [JsonPropertyName("olympicSolidarity")]
    public bool OlympicSolidarity { get; set; }

    [JsonPropertyName("status")]
    public Status Status { get; set; }

    [JsonPropertyName("registeredEvents")]
    public List<RegisteredEvent> RegisteredEvents { get; set; }

    [JsonPropertyName("medals")]
    public List<Medal> Medals { get; set; }

    [JsonPropertyName("schedules")]
    public List<Schedule> Schedules { get; set; }

    [JsonPropertyName("participantBio")]
    public ParticipantBio ParticipantBio { get; set; }

    [JsonPropertyName("mainFunction")]
    public MainFunction MainFunction { get; set; }

    [JsonPropertyName("teamCodes")]
    public List<string> TeamCodes { get; set; }

    [JsonPropertyName("records")]
    public List<Record> Records { get; set; }
}

public class PersonGender
{
    [JsonPropertyName("description")]
    public string Description { get; set; }
}

public class Phase
{
    [JsonPropertyName("code")]
    public string Code { get; set; }

    [JsonPropertyName("event")]
    public Event Event { get; set; }
}

public class Ranking
{
    [JsonPropertyName("athlete_code")]
    public string AthleteCode { get; set; }

    [JsonPropertyName("team_code")]
    public string TeamCode { get; set; }

    [JsonPropertyName("rk_Rank")]
    public string RkRank { get; set; }

    [JsonPropertyName("rk_ResultType")]
    public string RkResultType { get; set; }

    [JsonPropertyName("rk_Type")]
    public string RkType { get; set; }

    [JsonPropertyName("rk_RankEqual")]
    public string RkRankEqual { get; set; }

    [JsonPropertyName("rk_IRM")]
    public string RkIRM { get; set; }

    [JsonPropertyName("teamAthletes")]
    public List<TeamAthlete> TeamAthletes { get; set; }

    [JsonPropertyName("participant")]
    public Participant Participant { get; set; }
}

public class Record
{
    [JsonPropertyName("order")]
    public int Order { get; set; }

    [JsonPropertyName("recordCode")]
    public string RecordCode { get; set; }

    [JsonPropertyName("discipline")]
    public Discipline Discipline { get; set; }

    [JsonPropertyName("record")]
    public Record Record2 { get; set; }

    [JsonPropertyName("recordType")]
    public RecordType RecordType { get; set; }

    [JsonPropertyName("recordData")]
    public List<RecordDatum> RecordData { get; set; }
}

public class RecordDatum
{
    [JsonPropertyName("current")]
    public bool Current { get; set; }

    [JsonPropertyName("historic")]
    public bool Historic { get; set; }

    [JsonPropertyName("order")]
    public int Order { get; set; }

    [JsonPropertyName("result")]
    public string Result { get; set; }

    [JsonPropertyName("place")]
    public string Place { get; set; }

    [JsonPropertyName("country")]
    public string Country { get; set; }

    [JsonPropertyName("recordDate")]
    public string RecordDate { get; set; }

    [JsonPropertyName("competitor")]
    public Competitor Competitor { get; set; }
}

public class RecordType
{
    [JsonPropertyName("recordType")]
    public string Type { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }
}

public class RegisteredEvent
{
    [JsonPropertyName("event")]
    public Event Event { get; set; }
}

public class Result
{
    [JsonPropertyName("items")]
    public List<Item> Items { get; set; }
}

public class Schedule
{
    [JsonPropertyName("code")]
    public string Code { get; set; }

    [JsonPropertyName("nocs")]
    public List<string> Nocs { get; set; }

    [JsonPropertyName("startDate")]
    public DateTime StartDate { get; set; }

    [JsonPropertyName("unitNum")]
    public string UnitNum { get; set; }

    [JsonPropertyName("discipline")]
    public Discipline Discipline { get; set; }

    [JsonPropertyName("eventUnit")]
    public EventUnit EventUnit { get; set; }

    [JsonPropertyName("status")]
    public Status Status { get; set; }

    [JsonPropertyName("venue")]
    public Venue Venue { get; set; }

    [JsonPropertyName("location")]
    public Location Location { get; set; }

    [JsonPropertyName("result")]
    public Result Result { get; set; }

    [JsonPropertyName("start")]
    public List<Start> Start { get; set; }
}

public class Start
{
    [JsonPropertyName("sortOrder")]
    public int SortOrder { get; set; }

    [JsonPropertyName("startOrder")]
    public int StartOrder { get; set; }

    [JsonPropertyName("participant")]
    public ParticipantInfo Participant { get; set; }

    [JsonPropertyName("organisation")]
    public Organisation Organisation { get; set; }
}

public class Status
{
    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("code")]
    public string Code { get; set; }
}

public class TeamAthlete
{
    [JsonPropertyName("athlete_code")]
    public string AthleteCode { get; set; }
}

public class Venue
{
    [JsonPropertyName("description")]
    public string Description { get; set; }
}