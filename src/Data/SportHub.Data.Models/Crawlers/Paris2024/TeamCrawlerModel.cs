namespace SportHub.Data.Models.Crawlers.Paris2024.Team;

using System.Text.Json.Serialization;

public class TeamCrawlerModel
{
    [JsonPropertyName("team")]
    public TeamInfo Team { get; set; }
}

public class AthleteInfo
{
    [JsonPropertyName("order")]
    public int Order { get; set; }

    [JsonPropertyName("person")]
    public PersonInfo Person { get; set; }
}

public class AudioFile
{
    [JsonPropertyName("audioType")]
    public string AudioType { get; set; }
}

public class CoachInfo
{
    [JsonPropertyName("code")]
    public string Code { get; set; }

    [JsonPropertyName("current")]
    public bool Current { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("birthDate")]
    public string BirthDate { get; set; }

    [JsonPropertyName("height")]
    public int Height { get; set; }

    [JsonPropertyName("nationality")]
    public Nationality Nationality { get; set; }

    [JsonPropertyName("organisation")]
    public Organisation Organisation { get; set; }

    [JsonPropertyName("image")]
    public Image Image { get; set; }

    [JsonPropertyName("registeredEvents")]
    public List<RegisteredEvent> RegisteredEvents { get; set; }
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

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("longDescription")]
    public string LongDescription { get; set; }

    [JsonPropertyName("rankings")]
    public List<Ranking> Rankings { get; set; }
}

public class EventUnit
{
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
}

public class Function
{
    [JsonPropertyName("functionCode")]
    public string FunctionCode { get; set; }

    [JsonPropertyName("category")]
    public string Category { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("fOrder")]
    public int FOrder { get; set; }
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
    [JsonPropertyName("addInformation")]
    public string AddInformation { get; set; }
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
}

public class Medal
{
    [JsonPropertyName("medal_type")]
    public string MedalType { get; set; }

    [JsonPropertyName("event_code")]
    public string EventCode { get; set; }
}

public class Nationality
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

    [JsonPropertyName("protocolOrder")]
    public int ProtocolOrder { get; set; }

    [JsonPropertyName("descriptionOrder")]
    public int DescriptionOrder { get; set; }

    [JsonPropertyName("longDescriptionOrder")]
    public int LongDescriptionOrder { get; set; }

    [JsonPropertyName("longDescription")]
    public string LongDescription { get; set; }
}

public class PersonInfo
{
    [JsonPropertyName("current")]
    public bool Current { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("code")]
    public string Code { get; set; }

    [JsonPropertyName("birthDate")]
    public string BirthDate { get; set; }

    [JsonPropertyName("height")]
    public int Height { get; set; }

    [JsonPropertyName("image")]
    public Image Image { get; set; }

    [JsonPropertyName("mainFunction")]
    public MainFunction MainFunction { get; set; }

    [JsonPropertyName("audioFile")]
    public AudioFile AudioFile { get; set; }

    [JsonPropertyName("registeredEvents")]
    public List<RegisteredEvent> RegisteredEvents { get; set; }
}

public class Ranking
{
    [JsonPropertyName("event_code")]
    public string EventCode { get; set; }

    [JsonPropertyName("rk_Rank")]
    public string RkRank { get; set; }

    [JsonPropertyName("rk_Result")]
    public string RkResult { get; set; }

    [JsonPropertyName("team_code")]
    public string TeamCode { get; set; }
}

public class RegisteredEvent
{
    [JsonPropertyName("event")]
    public Event Event { get; set; }
}

public class ScheduleInfo
{
    [JsonPropertyName("code")]
    public string Code { get; set; }

    [JsonPropertyName("nocs")]
    public List<string> Nocs { get; set; }

    [JsonPropertyName("startDate")]
    public DateTime StartDate { get; set; }

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
}

public class Status
{
    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("code")]
    public string Code { get; set; }
}

public class TeamInfo
{
    [JsonPropertyName("code")]
    public string Code { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("discipline")]
    public Discipline Discipline { get; set; }

    [JsonPropertyName("organisation")]
    public Organisation Organisation { get; set; }

    [JsonPropertyName("athletes")]
    public List<AthleteInfo> Athletes { get; set; }

    [JsonPropertyName("teamCoaches")]
    public List<TeamCoach> TeamCoaches { get; set; }

    [JsonPropertyName("registeredEvents")]
    public List<RegisteredEvent> RegisteredEvents { get; set; }

    [JsonPropertyName("medals")]
    public List<Medal> Medals { get; set; }

    [JsonPropertyName("schedules")]
    public List<ScheduleInfo> Schedules { get; set; }

    [JsonPropertyName("teamBio")]
    public TeamBio TeamBio { get; set; }
}

public class TeamBio
{
    [JsonPropertyName("organisationId")]
    public string OrganisationId { get; set; }

    [JsonPropertyName("isHistoric")]
    public bool IsHistoric { get; set; }

    [JsonPropertyName("number")]
    public string Number { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("highlights")]
    public List<Highlight> Highlights { get; set; }

    [JsonPropertyName("interest")]
    public Interest Interest { get; set; }
}

public class TeamCoach
{
    [JsonPropertyName("order")]
    public int Order { get; set; }

    [JsonPropertyName("function")]
    public Function Function { get; set; }

    [JsonPropertyName("coach")]
    public CoachInfo Coach { get; set; }
}

public class Venue
{
    [JsonPropertyName("description")]
    public string Description { get; set; }
}