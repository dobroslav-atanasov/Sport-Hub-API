namespace SportHub.Data.Models.Crawlers.Paris2024.Teams;

using System.Text.Json.Serialization;

public class TeamsList
{
    [JsonPropertyName("teams")]
    public List<Team> Teams { get; set; }
}

public class Athlete
{
    [JsonPropertyName("order")]
    public int Order { get; set; }

    [JsonPropertyName("person")]
    public Person Person { get; set; }
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
    [JsonPropertyName("order")]
    public int Order { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("longDescription")]
    public string LongDescription { get; set; }
}

public class EventEntry
{
    [JsonPropertyName("ee_code")]
    public string EeCode { get; set; }

    [JsonPropertyName("ee_type")]
    public string EeType { get; set; }

    [JsonPropertyName("ee_value")]
    public string EeValue { get; set; }
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

public class MainFunction
{
    [JsonPropertyName("functionCode")]
    public string FunctionCode { get; set; }

    [JsonPropertyName("category")]
    public string Category { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }
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
    [JsonPropertyName("type")]
    public string Type { get; set; }

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

public class Person
{
    [JsonPropertyName("nationality")]
    public Nationality Nationality { get; set; }

    [JsonPropertyName("organisation")]
    public Organisation Organisation { get; set; }

    [JsonPropertyName("code")]
    public string Code { get; set; }

    [JsonPropertyName("current")]
    public bool Current { get; set; }

    [JsonPropertyName("TVName")]
    public string TVName { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("shortName")]
    public string ShortName { get; set; }

    [JsonPropertyName("birthDate")]
    public string BirthDate { get; set; }

    [JsonPropertyName("height")]
    public int Height { get; set; }

    [JsonPropertyName("__typename")]
    public string Typename { get; set; }

    [JsonPropertyName("image")]
    public Image Image { get; set; }

    [JsonPropertyName("mainFunction")]
    public MainFunction MainFunction { get; set; }

    [JsonPropertyName("personGender")]
    public PersonGender PersonGender { get; set; }

    [JsonPropertyName("registeredEvents")]
    public List<RegisteredEvent> RegisteredEvents { get; set; }
}

public class PersonGender
{
    [JsonPropertyName("code")]
    public string Code { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }
}

public class RegisteredEvent
{
    [JsonPropertyName("code")]
    public string Code { get; set; }

    [JsonPropertyName("event")]
    public Event Event { get; set; }

    [JsonPropertyName("ifid")]
    public string Ifid { get; set; }

    [JsonPropertyName("eventEntries")]
    public List<EventEntry> EventEntries { get; set; }
}

public class Team
{
    [JsonPropertyName("code")]
    public string Code { get; set; }

    [JsonPropertyName("current")]
    public bool Current { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("shortName")]
    public string ShortName { get; set; }

    [JsonPropertyName("TVName")]
    public string TVName { get; set; }

    [JsonPropertyName("teamGender")]
    public string TeamGender { get; set; }

    [JsonPropertyName("teamType")]
    public string TeamType { get; set; }

    [JsonPropertyName("__typename")]
    public string Typename { get; set; }

    [JsonPropertyName("discipline")]
    public Discipline Discipline { get; set; }

    [JsonPropertyName("organisation")]
    public Organisation Organisation { get; set; }

    [JsonPropertyName("registeredEvents")]
    public List<RegisteredEvent> RegisteredEvents { get; set; }

    [JsonPropertyName("athletes")]
    public List<Athlete> Athletes { get; set; }
}