namespace SportHub.Services.Data.OlympicGamesDb.Interfaces;

using SportHub.Data.ViewModels.OlympicGames.Sports;

public interface ISportsService
{
    IEnumerable<SportViewModel> GetSports();
}