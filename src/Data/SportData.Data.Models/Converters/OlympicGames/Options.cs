namespace SportData.Data.Models.Converters.OlympicGames;

using HtmlAgilityPack;

using SportData.Data.Models.Cache;

public class Options
{
    public HtmlDocument HtmlDocument { get; set; }

    public GameCache Game { get; set; }

    public DisciplineCache Discipline { get; set; }

    public EventCache Event { get; set; }

    public List<RoundDataModel> Rounds { get; set; }

    public List<DocumentDataModel> Documents { get; set; }
}