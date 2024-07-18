namespace SportHub.Data.Models.Cache;

using SportHub.Data.Models.DbEntities.OlympicGames;
using SportHub.Services.Mapper.Interfaces;

public class NOCCache : IMapFrom<NOC>
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Code { get; set; }
}