namespace SportHub.Data.Models.Crawlers.Paris2024;

using System.Text.Json.Serialization;

public class ExtraData
{
    [JsonPropertyName("detailUrl")]
    public string DetailUrl { get; set; }
}