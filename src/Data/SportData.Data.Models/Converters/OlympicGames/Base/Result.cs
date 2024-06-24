namespace SportData.Data.Models.Converters.OlympicGames.Base;

public class Result<TModel>
{
    public EventData EventData { get; set; }

    public DisciplineData DisciplineData { get; set; }

    public GameData GameData { get; set; }

    public Ranking Ranking { get; set; }

    public List<Round<TModel>> Rounds { get; set; } = [];
}