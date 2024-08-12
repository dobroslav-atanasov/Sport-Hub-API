namespace SportHub.Services.Data.OlympicGamesDb.Interfaces;

using SportHub.Data.Models.Cache;

public interface IDataCacheService
{
    ICollection<GameCache> Games { get; }

    ICollection<DisciplineCache> Disciplines { get; }

    ICollection<EventCache> Events { get; }

    ICollection<NOCCache> NOCs { get; }
}