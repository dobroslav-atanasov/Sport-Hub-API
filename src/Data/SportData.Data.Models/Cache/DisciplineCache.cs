namespace SportData.Data.Models.Cache;

using SportData.Data.Models.Entities.OlympicGames;
using SportData.Services.Mapper.Interfaces;

public class DisciplineCache : IMapFrom<Discipline>
{
    public int Id { get; set; }

    public string Name { get; set; }
}