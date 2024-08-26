namespace SportHub.Data.Models.Cache;

using SportHub.Data.Entities.OlympicGames;
using SportHub.Services.Mapper.Interfaces;

public class GameCache : IMapFrom<Game>
{
    public int Id { get; set; }

    public int Year { get; set; }

    public string Type { get; set; }

    public DateTime? OpeningCeremony { get; set; }

    public DateTime? StartCompetitionDate { get; set; }
}