namespace SportHub.Data.Models.Crawlers.Paris2024.NOCs;

using System.Text.Json.Serialization;

public class NOCList
{
    [JsonPropertyName("nocs")]
    public List<NOC> NOCs { get; set; }
}