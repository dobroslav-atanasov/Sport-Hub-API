namespace SportData.Data.Models.Cache;

using SportData.Data.Models.Entities.OlympicGames;
using SportData.Services.Mapper.Interfaces;

public class VenueCache : IMapFrom<Venue>
{
    public int Id { get; set; }

    public int Number { get; set; }
}