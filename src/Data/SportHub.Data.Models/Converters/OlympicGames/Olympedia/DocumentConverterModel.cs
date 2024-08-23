namespace SportHub.Data.Models.Converters.OlympicGames.Olympedia;

using HtmlAgilityPack;

using SportHub.Data.Models.Cache;

public class DocumentConverterModel
{
    public HtmlDocument HtmlDocument { get; set; }

    public int PageId { get; set; }

    public string Title { get; set; }

    public Header Header { get; set; } = new Header();

    public GameCache Game { get; set; }

    public DisciplineCache Discipline { get; set; }

    public EventCache Event { get; set; }

    public EventInfo EventInfo { get; set; } = new EventInfo();

    public bool IsValidEvent { get; set; } = true;

    public List<PhaseData> Phases { get; set; } = [];
}