namespace SportHub.Data.Models.Converters.OlympicGames;

using SportHub.Data.Models.Converters.OlympicGames.Olympedia.Base;
using SportHub.Data.Models.Converters.OlympicGames.Paris2024.Base;

public class ConverterModel
{
    public Guid Identifier { get; set; }

    public string Name { get; set; }

    public int CrawlerId { get; set; }

    public Dictionary<int, OlympediaDocumentModel> OlympediaDocuments { get; set; } = [];

    public Dictionary<int, Paris2024ConverterModel> Paris2024Documents { get; set; } = [];
}