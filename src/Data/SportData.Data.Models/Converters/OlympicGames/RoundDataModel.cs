namespace SportData.Data.Models.Converters.OlympicGames;

using HtmlAgilityPack;

using SportData.Data.Models.Entities.OlympicGames.Enumerations;

public class RoundDataModel
{
    public string Name { get; set; }

    public string NameHtml { get; set; }

    public RoundTypeEnum Type { get; set; }

    public RoundTypeEnum SubType { get; set; }

    public int Number { get; set; }

    public string Info { get; set; }

    public int Order { get; set; }

    public Guid EventId { get; set; }

    public string Html { get; set; }

    public HtmlDocument HtmlDocument
    {
        get
        {
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(this.Html);

            return htmlDocument;
        }
    }

    public DateTime? From { get; set; }

    public DateTime? To { get; set; }

    public string Format { get; set; }

    public string SplitsHtml { get; set; }

    public HtmlNodeCollection Rows { get; set; }

    public List<string> Headers { get; set; } = [];

    public Dictionary<string, int> Indexes { get; set; } = [];
}