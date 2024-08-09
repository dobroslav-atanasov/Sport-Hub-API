namespace SportHub.Data.Models.Crawlers.Paris2024;

using System.Text.Json.Serialization;

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