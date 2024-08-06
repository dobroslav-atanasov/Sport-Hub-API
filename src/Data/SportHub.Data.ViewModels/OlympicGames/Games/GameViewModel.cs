namespace SportHub.Data.ViewModels.OlympicGames.Games;

using SportHub.Data.ViewModels.OlympicGames.Sports;

public class GameViewModel
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