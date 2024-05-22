namespace SportData.Data.Models.Cache;

using SportData.Data.Models.Entities.OlympicGames;
using SportData.Services.Mapper.Interfaces;

public class GameCache : IMapFrom<Game>
{
    public int Id { get; set; }

    public int Year { get; set; }

    public int OlympicGameTypeId { get; set; }

    public DateTime? OpeningDate { get; set; }

    public DateTime? StartCompetitionDate { get; set; }
}