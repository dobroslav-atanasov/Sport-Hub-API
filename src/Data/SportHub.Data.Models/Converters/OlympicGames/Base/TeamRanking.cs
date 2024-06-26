namespace SportHub.Data.Models.Converters.OlympicGames.Base;

public class TeamRanking : BaseRanking
{
    public List<AthleteRanking> Athletes { get; set; } = [];
}