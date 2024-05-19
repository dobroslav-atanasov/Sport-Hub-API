namespace SportData.WebAPI.Controllers;

using Asp.Versioning;

using Microsoft.AspNetCore.Mvc;

using SportData.Common.Constants;

[ApiController]
[ApiVersion(1)]
[Route(RouteConstants.API_DEFAULT_ROUTE)]
public abstract class BaseController : ControllerBase
{
}