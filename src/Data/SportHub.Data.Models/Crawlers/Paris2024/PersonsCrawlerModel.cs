namespace SportHub.Data.Models.Crawlers.Paris2024;

using System.Text.Json.Serialization;

public class PersonsCrawlerModel
{
    [JsonPropertyName("persons")]
    public List<PersonInfo> Persons { get; set; }
}

public class DisciplinePerson
{
    [JsonPropertyName("code")]
    public string Code { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("descriptionOrder")]
    public int DescriptionOrder { get; set; }
}

public class EventPerson
{
    [JsonPropertyName("code")]
    public string Code { get; set; }

    [JsonPropertyName("longDescription")]
    public string LongDescription { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("order")]
    public int Order { get; set; }
}

public class EventEntryPerson
{
    [JsonPropertyName("ee_code")]
    public string EeCode { get; set; }

    [JsonPropertyName("ee_type")]
    public string EeType { get; set; }

    [JsonPropertyName("ee_value")]
    public string EeValue { get; set; }

    [JsonPropertyName("ee_pos")]
    public string EePos { get; set; }
}

public class ImagePerson
{
    [JsonPropertyName("imageType")]
    public string ImageType { get; set; }

    [JsonPropertyName("imageExtension")]
    public string ImageExtension { get; set; }

    [JsonPropertyName("imageVersion")]
    public string ImageVersion { get; set; }
}

public class MainFunctionPerson
{
    [JsonPropertyName("functionCode")]
    public string FunctionCode { get; set; }

    [JsonPropertyName("category")]
    public string Category { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }
}

public class OrganisationPerson
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

public class PersonInfo
{
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
    public ImagePerson Image { get; set; }

    [JsonPropertyName("disciplines")]
    public List<DisciplinePerson> Disciplines { get; set; }

    [JsonPropertyName("mainFunction")]
    public MainFunctionPerson MainFunction { get; set; }

    [JsonPropertyName("organisation")]
    public OrganisationPerson Organisation { get; set; }

    [JsonPropertyName("personGender")]
    public PersonGender PersonGender { get; set; }

    [JsonPropertyName("registeredEvents")]
    public List<RegisteredEventPerson> RegisteredEvents { get; set; }
}

public class PersonGender
{
    [JsonPropertyName("code")]
    public string Code { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }
}

public class RegisteredEventPerson
{
    [JsonPropertyName("code")]
    public string Code { get; set; }

    [JsonPropertyName("ifid")]
    public string Ifid { get; set; }

    [JsonPropertyName("event")]
    public EventPerson Event { get; set; }

    [JsonPropertyName("eventEntries")]
    public List<EventEntryPerson> EventEntries { get; set; }
}