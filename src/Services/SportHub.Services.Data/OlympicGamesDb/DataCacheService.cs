namespace SportHub.Services.Data.OlympicGamesDb;

using System.Collections.Generic;

using SportHub.Data.Models.Cache;
using SportHub.Data.Models.Entities.OlympicGames;
using SportHub.Data.Repositories;
using SportHub.Services.Data.OlympicGamesDb.Interfaces;
using SportHub.Services.Mapper.Extensions;

public class DataCacheService : IDataCacheService
{
    private readonly Lazy<ICollection<GameCache>> games;
    private readonly Lazy<ICollection<DisciplineCache>> disciplines;
    private readonly Lazy<ICollection<VenueCache>> venues;
    private readonly Lazy<ICollection<ClubCache>> clubs;
    private readonly Lazy<ICollection<EventCache>> events;
    private readonly Lazy<ICollection<NOCCache>> nocs;

    private readonly OlympicGamesRepository<Game> gameRepository;
    private readonly OlympicGamesRepository<Discipline> disciplineRepository;
    private readonly OlympicGamesRepository<Venue> venueRepository;
    private readonly OlympicGamesRepository<Club> clubRepository;
    private readonly OlympicGamesRepository<Event> eventRepository;
    private readonly OlympicGamesRepository<NOC> nocRepository;

    public DataCacheService(OlympicGamesRepository<Game> gameRepository, OlympicGamesRepository<Discipline> disciplineRepository, OlympicGamesRepository<Venue> venueRepository,
         OlympicGamesRepository<Club> clubRepository, OlympicGamesRepository<Event> eventRepository, OlympicGamesRepository<NOC> nocRepository)
    {
        this.games = new Lazy<ICollection<GameCache>>(() => this.GetAllGames());
        this.disciplines = new Lazy<ICollection<DisciplineCache>>(() => this.GetAllDisciplines());
        this.venues = new Lazy<ICollection<VenueCache>>(() => this.GetAllVenues());
        this.clubs = new Lazy<ICollection<ClubCache>>(() => this.GetAllClubs());
        this.events = new Lazy<ICollection<EventCache>>(() => this.GetAllEvents());
        this.nocs = new Lazy<ICollection<NOCCache>>(() => this.GetAllNOCs());
        this.gameRepository = gameRepository;
        this.disciplineRepository = disciplineRepository;
        this.venueRepository = venueRepository;
        this.clubRepository = clubRepository;
        this.eventRepository = eventRepository;
        this.nocRepository = nocRepository;
    }

    private ICollection<NOCCache> GetAllNOCs()
    {
        return this.nocRepository
            .AllAsNoTracking()
            .To<NOCCache>()
            .ToList();
    }

    private ICollection<EventCache> GetAllEvents()
    {
        return this.eventRepository
            .AllAsNoTracking()
            .To<EventCache>()
            .ToList();
    }

    private ICollection<ClubCache> GetAllClubs()
    {
        return this.clubRepository
            .AllAsNoTracking()
            .To<ClubCache>()
            .ToList();
    }

    private ICollection<VenueCache> GetAllVenues()
    {
        return this.venueRepository
            .AllAsNoTracking()
            .To<VenueCache>()
            .ToList();
    }

    private ICollection<DisciplineCache> GetAllDisciplines()
    {
        return this.disciplineRepository
            .AllAsNoTracking()
            .To<DisciplineCache>()
            .ToList();
    }

    private ICollection<GameCache> GetAllGames()
    {
        return this.gameRepository
            .AllAsNoTracking()
            .To<GameCache>()
            .ToList();
    }

    public ICollection<GameCache> Games => this.games.Value;

    public ICollection<DisciplineCache> Disciplines => this.disciplines.Value;

    public ICollection<VenueCache> Venues => this.venues.Value;

    public ICollection<ClubCache> Clubs => this.clubs.Value;

    public ICollection<EventCache> Events => this.events.Value;

    public ICollection<NOCCache> NOCs => this.nocs.Value;
}