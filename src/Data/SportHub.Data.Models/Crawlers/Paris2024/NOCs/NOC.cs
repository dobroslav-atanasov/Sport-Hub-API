namespace SportHub.Data.Models.Crawlers.Paris2024.NOCs;

using System.Text.Json.Serialization;

public class NOC
{
    [JsonPropertyName("code")]
    public string Code { get; set; }

    [JsonPropertyName("note")]
    public string Note { get; set; }

    [JsonPropertyName("medal")]
    public string Medal { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("longDescription")]
    public string LongDescription { get; set; }

    [JsonPropertyName("seodescription")]
    public string Seodescription { get; set; }

    [JsonPropertyName("protocolOrder")]
    public int ProtocolOrder { get; set; }

    [JsonPropertyName("descriptionOrder")]
    public int DescriptionOrder { get; set; }

    [JsonPropertyName("longDescriptionOrder")]
    public int LongDescriptionOrder { get; set; }
}