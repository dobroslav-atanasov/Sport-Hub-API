namespace SportData.Data.Models.Cache;

using SportData.Data.Models.Entities.OlympicGames;
using SportData.Services.Mapper.Interfaces;

public class ClubCache : IMapFrom<Club>
{
    public int Id { get; set; }

    public int Code { get; set; }
}