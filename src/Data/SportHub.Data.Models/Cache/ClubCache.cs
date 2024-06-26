namespace SportHub.Data.Models.Cache;

using SportHub.Data.Models.Entities.OlympicGames;
using SportHub.Services.Mapper.Interfaces;

public class ClubCache : IMapFrom<Club>
{
    public int Id { get; set; }

    public int Code { get; set; }
}