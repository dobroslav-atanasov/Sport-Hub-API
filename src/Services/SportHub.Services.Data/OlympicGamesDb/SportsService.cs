namespace SportHub.Services.Data.OlympicGamesDb;

using System.Collections.Generic;

using SportHub.Data.Models.DbEntities.OlympicGames;
using SportHub.Data.Repositories;
using SportHub.Data.ViewModels.OlympicGames.Sports;
using SportHub.Services.Data.OlympicGamesDb.Interfaces;
using SportHub.Services.Mapper.Extensions;

public class SportsService : ISportsService
{
    private readonly OlympicGamesRepository<Sport> repository;

    public SportsService(OlympicGamesRepository<Sport> repository)
    {
        this.repository = repository;
    }

    public IEnumerable<SportViewModel> GetSports()
    {
        var sports = this.repository
            .AllAsNoTracking()
            .To<SportViewModel>();

        return sports;
    }
}