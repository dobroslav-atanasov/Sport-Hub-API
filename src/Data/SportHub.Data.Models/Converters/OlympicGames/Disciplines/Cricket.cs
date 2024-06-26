namespace SportHub.Data.Models.Converters.OlympicGames.Disciplines;

using SportHub.Data.Models.Converters.OlympicGames.Base;

public class Cricket : BaseModel
{
    public List<Cricket> Athletes { get; set; } = [];
}