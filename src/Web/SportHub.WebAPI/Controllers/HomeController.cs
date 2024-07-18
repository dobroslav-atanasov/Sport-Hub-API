namespace SportHub.WebAPI.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

public class HomeController : BaseController
{
    [HttpGet]
    [AllowAnonymous]
    public IActionResult Index()
    {
        return this.Ok();
    }
}