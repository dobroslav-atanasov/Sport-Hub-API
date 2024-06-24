namespace SportData.Data.Models.Converters.OlympicGames.Disciplines;

using SportData.Data.Models.Converters.OlympicGames.Base;

public class BasquePelota : BaseModel
{
    public List<BasquePelota> Athletes { get; set; } = [];
}