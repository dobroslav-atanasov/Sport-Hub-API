namespace SportHub.Services.Data.OlympicGamesDb;

using System.Collections.Generic;

using SportHub.Data.Models.Cache;
using SportHub.Data.Models.DbEntities.OlympicGames;
using SportHub.Data.Repositories;
using SportHub.Services.Data.OlympicGamesDb.Interfaces;
using SportHub.Services.Mapper.Extensions;

public class DataCacheService : IDataCacheService
{
    //private readonly Lazy<ICollection<GameCache>> games;
    //private readonly Lazy<ICollection<DisciplineCache>> disciplines;
    //private readonly Lazy<ICollection<EventCache>> events;
    private readonly Lazy<ICollection<NOCCache>> nocs;

    //private readonly OlympicGamesRepository<Game> gameRepository;
    //private readonly OlympicGamesRepository<Discipline> disciplineRepository;
    //private readonly OlympicGamesRepository<Event> eventRepository;
    private readonly OlympicGamesRepository<NOC> nocRepository;

    public DataCacheService(/*OlympicGamesRepository<Game> gameRepository, OlympicGamesRepository<Discipline> disciplineRepository,*/
        /*OlympicGamesRepository<Event> eventRepository, */OlympicGamesRepository<NOC> nocRepository)
    {
        //this.games = new Lazy<ICollection<GameCache>>(() => this.GetAllGames());
        //this.disciplines = new Lazy<ICollection<DisciplineCache>>(() => this.GetAllDisciplines());
        //this.events = new Lazy<ICollection<EventCache>>(() => this.GetAllEvents());
        this.nocs = new Lazy<ICollection<NOCCache>>(() => this.GetAllNOCs());

        //this.gameRepository = gameRepository;
        //this.disciplineRepository = disciplineRepository;
        //this.eventRepository = eventRepository;
        this.nocRepository = nocRepository;
    }

    private ICollection<NOCCache> GetAllNOCs()
    {
        return this.nocRepository
            .AllAsNoTracking()
            .To<NOCCache>()
            .ToList();
    }

    //private ICollection<EventCache> GetAllEvents()
    //{
    //    return this.eventRepository
    //        .AllAsNoTracking()
    //        .To<EventCache>()
    //        .ToList();
    //}

    //private ICollection<DisciplineCache> GetAllDisciplines()
    //{
    //    return this.disciplineRepository
    //        .AllAsNoTracking()
    //        .To<DisciplineCache>()
    //        .ToList();
    //}

    //private ICollection<GameCache> GetAllGames()
    //{
    //    return this.gameRepository
    //        .AllAsNoTracking()
    //        .To<GameCache>()
    //        .ToList();
    //}

    //public ICollection<GameCache> Games => this.games.Value;

    //public ICollection<DisciplineCache> Disciplines => this.disciplines.Value;

    //public ICollection<EventCache> Events => this.events.Value;

    public ICollection<NOCCache> NOCs => this.nocs.Value;
}