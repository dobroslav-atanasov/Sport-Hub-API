namespace SportHub.Data.Models.Crawlers.Paris2024.NOC;

using System.Text.Json.Serialization;

public class NOCInfo
{
    [JsonPropertyName("nocBio")]
    public NOCBio NocBio { get; set; }
}

public class Interest
{
    [JsonPropertyName("ocFlagBearer")]
    public List<string> OcFlagBearer { get; set; }

    [JsonPropertyName("highlights")]
    public string Highlights { get; set; }

    [JsonPropertyName("addInformation")]
    public string AddInformation { get; set; }
}

public class Membership
{
    [JsonPropertyName("officialNocName")]
    public string OfficialNocName { get; set; }

    [JsonPropertyName("foundingDate")]
    public string FoundingDate { get; set; }

    [JsonPropertyName("dateIOCRecognition")]
    public string DateIOCRecognition { get; set; }
}

public class NOCBio
{
    [JsonPropertyName("organisationId")]
    public string OrganisationId { get; set; }

    [JsonPropertyName("isHistoric")]
    public bool IsHistoric { get; set; }

    [JsonPropertyName("membership")]
    public Membership Membership { get; set; }

    [JsonPropertyName("interest")]
    public Interest Interest { get; set; }

    [JsonPropertyName("officials")]
    public Officials Officials { get; set; }

    [JsonPropertyName("participation")]
    public Participation Participation { get; set; }
}

public class Officials
{
    [JsonPropertyName("nocPresident")]
    public string NocPresident { get; set; }

    [JsonPropertyName("nocGenSecretary")]
    public string NocGenSecretary { get; set; }
}

public class Participation
{
    [JsonPropertyName("firstOGAppearance")]
    public string FirstOGAppearance { get; set; }

    [JsonPropertyName("numOGAppearance")]
    public string NumOGAppearance { get; set; }

    [JsonPropertyName("summary")]
    public string Summary { get; set; }
}