namespace SportHub.Data.Models.Cache;

using SportHub.Data.Models.Entities.OlympicGames;
using SportHub.Services.Mapper.Interfaces;

public class DisciplineCache : IMapFrom<Discipline>
{
    public int Id { get; set; }

    public string Name { get; set; }
}