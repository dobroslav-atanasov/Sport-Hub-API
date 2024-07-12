namespace SportHub.WebAPI.Areas.OlympicGames.Controllers;

using Microsoft.AspNetCore.Mvc;

using SportHub.Common.Constants;
using SportHub.Services.Data.OlympicGamesDb.Interfaces;

public class GamesController : OlympicaGamesBaseController
{
    private readonly ILogger<GamesController> logger;
    private readonly IGamesService gamesService;
    private readonly ISportsService sportsService;

    public GamesController(ILogger<GamesController> logger, IGamesService gamesService, ISportsService sportsService)
    {
        this.logger = logger;
        this.gamesService = gamesService;
        this.sportsService = sportsService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var game = await this.gamesService.GetGameAsync(id);
        var sports = this.sportsService.GetSports();
        game.Sports = sports;

        return this.Ok(game);
    }

    [HttpGet]
    [Route(RouteConstants.OLYMPIC_GAMES_GAMES_ALL)]
    public IActionResult All()
    {
        var games = this.gamesService.GetGames();
        return this.Ok(games);
    }
}