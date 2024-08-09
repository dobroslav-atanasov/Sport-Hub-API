namespace SportHub.Data.Models.Crawlers.Paris2024;

using System.Text.Json.Serialization;

public class UnitGroupList
{
    [JsonPropertyName("units")]
    public List<UnitInfo> Units { get; set; }

    [JsonPropertyName("groups")]
    public List<GroupInfo> Groups { get; set; }
}