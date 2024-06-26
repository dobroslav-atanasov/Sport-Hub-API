namespace SportHub.Data.Models.Converters.OlympicGames;

using HtmlAgilityPack;

using SportHub.Data.Models.Entities.OlympicGames.Enumerations;

public class DocumentDataModel
{
    public RoundTypeEnum Type { get; set; }

    public RoundTypeEnum SubType { get; set; }

    public int Number { get; set; }

    public string Info { get; set; }

    public int Id { get; set; }

    public string Title { get; set; }

    public string Format { get; set; }

    public DateTime? From { get; set; }

    public DateTime? To { get; set; }

    public string Html { get; set; }

    public HtmlDocument HtmlDocument { get; set; }

    public List<RoundDataModel> Rounds { get; set; } = [];
}