namespace SportHub.Data.Models.Converters.OlympicGames;

using HtmlAgilityPack;

using SportHub.Data.Models.Cache;
using SportHub.Data.Models.Enumerations.OlympicGames;

public class ConverterModel
{
    public HtmlDocument HtmlDocument { get; set; }

    public int PageId { get; set; }

    public string Title { get; set; }

    public Header Header { get; set; } = new Header();

    public GameCache GameCache { get; set; }

    public DisciplineCache DisciplineCache { get; set; }

    public EventCache EventCache { get; set; }

    public EventInfo EventInfo { get; set; } = new EventInfo();
}

public class Header
{
    public GameTypeEnum GameType { get; set; }

    public int GameYear { get; set; }

    public string Discipline { get; set; }

    public string Event { get; set; }
}

public class EventInfo
{
    public string OriginalName { get; set; }

    public string Name { get; set; }

    public string NormalizedName { get; set; }

    public string AdditionalInfo { get; set; }
}