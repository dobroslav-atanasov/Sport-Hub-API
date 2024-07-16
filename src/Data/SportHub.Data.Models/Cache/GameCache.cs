namespace SportHub.Data.Models.Cache;

using SportHub.Data.Models.DbEntities.OlympicGames;
using SportHub.Data.Models.Enumerations.OlympicGames;
using SportHub.Services.Mapper.Interfaces;

public class GameCache : IMapFrom<Game>
{
    public int Id { get; set; }

    public int Year { get; set; }

    public GameTypeEnum Type { get; set; }

    public DateTime? OpeningDate { get; set; }

    public DateTime? StartCompetitionDate { get; set; }
}