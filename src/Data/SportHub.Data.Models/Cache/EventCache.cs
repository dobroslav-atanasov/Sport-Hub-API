namespace SportHub.Data.Models.Cache;

using SportHub.Data.Entities.OlympicGames;
using SportHub.Services.Mapper.Interfaces;

public class EventCache : IMapFrom<Event>
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public string Code { get; set; }

    public string Gender { get; set; }

    public bool IsTeam { get; set; }
}