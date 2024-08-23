namespace SportHub.Data.Models.Converters.OlympicGames.Olympedia;

using HtmlAgilityPack;

using SportHub.Data.Entities.Enumerations.OlympicGames;

public class PhaseData
{
    public Guid EventId { get; set; }

    public string EventCode { get; set; }

    public string Code { get; set; }

    public string Name { get; set; }

    public DateTime? From { get; set; }

    public DateTime? To { get; set; }

    public int Order { get; set; }

    public string Html { get; set; }

    public string Format { get; set; }

    public HtmlNodeCollection Rows { get; set; }

    public List<string> Headers { get; set; } = [];

    public Dictionary<string, int> Indexes { get; set; } = [];

    public HtmlDocument HtmlDocument
    {
        get
        {
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(this.Html);

            return htmlDocument;
        }
    }




    public string NameHtml { get; set; }

    public RoundEnum Type { get; set; }

    public RoundEnum SubType { get; set; }

    public int Number { get; set; }

    public string Info { get; set; }

    public string SplitsHtml { get; set; }
}