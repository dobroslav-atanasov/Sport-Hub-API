namespace SportHub.Data.Models.Crawlers.Paris2024;

using System.Text.Json.Serialization;

public class Competitor
{
    [JsonPropertyName("code")]
    public string Code { get; set; }

    [JsonPropertyName("noc")]
    public string Noc { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("order")]
    public int Order { get; set; }

    [JsonPropertyName("results")]
    public Results Results { get; set; }
}