namespace SportHub.Data.Models.Converters.OlympicGames.Base;

public class TeamMatch<TModel> : BaseMatch
{
    public TModel Team1 { get; set; }

    public TModel Team2 { get; set; }
}