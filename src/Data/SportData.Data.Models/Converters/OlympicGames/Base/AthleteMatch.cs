namespace SportData.Data.Models.Converters.OlympicGames.Base;

public class AthleteMatch<TModel> : BaseMatch
{
    public TModel Athlete1 { get; set; }

    public TModel Athlete2 { get; set; }
}