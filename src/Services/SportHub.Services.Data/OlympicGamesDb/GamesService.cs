namespace SportHub.Services.Data.OlympicGamesDb;

using System.Collections.Generic;
using System.Threading.Tasks;

using SportHub.Data.Models.DbEntities.OlympicGames;
using SportHub.Data.Repositories;
using SportHub.Data.ViewModels.OlympicGames.Games;
using SportHub.Services.Data.OlympicGamesDb.Interfaces;
using SportHub.Services.Mapper;
using SportHub.Services.Mapper.Extensions;

public class GamesService : IGamesService
{
    private readonly OlympicGamesRepository<Game> repository;

    public GamesService(OlympicGamesRepository<Game> repository)
    {
        this.repository = repository;
    }

    public async Task<GameViewModel> GetGameAsync(int id)
    {
        var game = await this.repository.GetAsync(id);
        if (game == null)
        {
            return null;
        }

        var model = MapperConfig.Map<GameViewModel>(game);
        return model;
    }

    public IEnumerable<GameListViewModel> GetGames()
    {
        var games = this.repository
            .AllAsNoTracking()
            .To<GameListViewModel>();

        return games;
    }
}