namespace SportHub.Data.Models.Converters.OlympicGames;

using HtmlAgilityPack;

using SportHub.Data.Models.Enumerations.OlympicGames;

public class DocumentDataModel
{
    public RoundEnum Type { get; set; }

    public RoundEnum SubType { get; set; }

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