namespace SportHub.Data.ViewModels.OlympicGames.Games;

using SportHub.Data.Models.DbEntities.OlympicGames;
using SportHub.Data.ViewModels.OlympicGames.Sports;
using SportHub.Services.Mapper.Interfaces;

public class GameViewModel : IMapFrom<Game>
{
    public int Id { get; set; }

    public int Year { get; set; }

    public string Number { get; set; }

    public int Type { get; set; }

    public string OfficialName { get; set; }

    public DateTime? OpeningDate { get; set; }

    public DateTime? ClosingDate { get; set; }

    public IEnumerable<SportViewModel> Sports { get; set; }
}