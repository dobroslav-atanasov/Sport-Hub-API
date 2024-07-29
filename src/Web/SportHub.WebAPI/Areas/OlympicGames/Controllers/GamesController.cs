namespace SportHub.WebAPI.Areas.OlympicGames.Controllers;

using Microsoft.AspNetCore.Mvc;

using SportHub.Common.Constants;

public class GamesController : OlympicaGamesBaseController
{
    private readonly ILogger<GamesController> logger;

    public GamesController(ILogger<GamesController> logger)
    {
        this.logger = logger;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        return this.Ok();
    }

    [HttpGet]
    [Route(RouteConstants.OLYMPIC_GAMES_GAMES_ALL)]
    public IActionResult All()
    {
        return this.Ok();
    }
}