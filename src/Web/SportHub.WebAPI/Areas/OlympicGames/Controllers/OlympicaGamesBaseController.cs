namespace SportHub.WebAPI.Areas.OlympicGames.Controllers;

using Microsoft.AspNetCore.Mvc;

using SportHub.Common.Constants;
using SportHub.WebAPI.Controllers;

[Area(AreaConstants.AREA_OLYMPIC_GAMES)]
[Route(RouteConstants.API_OLYMPIC_GAMES_AREA_ROUTE)]
public abstract class OlympicaGamesBaseController : BaseController
{
}