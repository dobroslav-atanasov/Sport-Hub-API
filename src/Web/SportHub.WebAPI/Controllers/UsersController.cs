namespace SportHub.WebAPI.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using SportHub.Common.Constants;
using SportHub.Data.Models.Entities.SportHub;
using SportHub.Data.ViewModels.Users;
using SportHub.Services.Data.SportHubDb.Interfaces;

public class UsersController : BaseController
{
    private readonly UserManager<User> userManager;
    private readonly IUsersService usersService;

    public UsersController(UserManager<User> userManager, IUsersService usersService)
    {
        this.userManager = userManager;
        this.usersService = usersService;
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> CreateUser(RegisterViewModel input)
    {
        var userExist = await this.userManager.FindByNameAsync(input.Username);
        if (userExist != null)
        {
            return this.BadRequest(new { Message = MessageConstants.USER_ALREADY_EXIST });
        }

        var user = new User
        {
            Email = input.Email,
            UserName = input.Username,
            SecurityStamp = Guid.NewGuid().ToString(),
        };

        var result = await this.userManager.CreateAsync(user, input.Password);
        if (!result.Succeeded)
        {
            return this.BadRequest(new { Message = string.Join(", ", result.Errors.Select(x => x.Description)) });
        }

        await this.userManager.AddToRoleAsync(user, Roles.USER);

        return this.Ok(new { Message = MessageConstants.USER_CREATED_SUCCESSFULLY });
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetUsers()
    {
        var users = await this.usersService.GetUsersAsync();
        return this.Ok(users);
    }
}