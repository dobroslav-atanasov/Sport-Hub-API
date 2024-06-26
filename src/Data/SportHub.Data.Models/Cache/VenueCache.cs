namespace SportHub.Data.Models.Cache;

using SportHub.Data.Models.Entities.OlympicGames;
using SportHub.Services.Mapper.Interfaces;

public class VenueCache : IMapFrom<Venue>
{
    public int Id { get; set; }

    public int Number { get; set; }
}