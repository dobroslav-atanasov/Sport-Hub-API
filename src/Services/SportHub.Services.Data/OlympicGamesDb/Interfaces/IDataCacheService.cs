namespace SportHub.Services.Data.OlympicGamesDb.Interfaces;

using SportHub.Data.Models.Cache;

public interface IDataCacheService
{
    ICollection<GameCache> Games { get; }

    ICollection<DisciplineCache> Disciplines { get; }

    ICollection<ClubCache> Clubs { get; }

    ICollection<EventCache> Events { get; }

    ICollection<NationalOlympicCommitteeCache> NationalOlympicCommittees { get; }
}