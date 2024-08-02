namespace SportHub.Data.Models.Converters.OlympicGames.Olympedia.Base;

using HtmlAgilityPack;

using SportHub.Data.Models.Cache;

public class OlympediaDocumentModel
{
    public HtmlDocument HtmlDocument { get; set; }

    public int PageId { get; set; }

    public string Title { get; set; }

    public Header Header { get; set; } = new Header();

    public GameCache GameCache { get; set; }

    public DisciplineCache DisciplineCache { get; set; }

    public EventCache EventCache { get; set; }

    public EventInfo EventInfo { get; set; } = new EventInfo();

    public bool IsValidEvent { get; set; } = true;
}