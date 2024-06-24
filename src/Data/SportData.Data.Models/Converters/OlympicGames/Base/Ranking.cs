namespace SportData.Data.Models.Converters.OlympicGames.Base;

public class Ranking
{
    public List<AthleteRanking> Athletes { get; set; } = [];

    public List<TeamRanking> Teams { get; set; } = [];
}