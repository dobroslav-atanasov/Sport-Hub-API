namespace SportHub.Services.Data.OlympicGamesDb.Interfaces;

using SportHub.Data.ViewModels.OlympicGames.Games;

public interface IGamesService
{
    Task<GameViewModel> GetGameAsync(int id);

    IEnumerable<GameListViewModel> GetGames();
}