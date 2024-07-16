namespace SportHub.Data.Models.Cache;

using SportHub.Data.Models.DbEntities.OlympicGames;
using SportHub.Services.Mapper.Interfaces;

public class DisciplineCache : IMapFrom<Discipline>
{
    public int Id { get; set; }

    public string Name { get; set; }

    public int SportId { get; set; }
}