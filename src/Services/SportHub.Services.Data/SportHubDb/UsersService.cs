namespace SportHub.Services.Data.SportHubDb;

using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using SportHub.Data.Contexts;
using SportHub.Data.Models.Entities.SportHub;
using SportHub.Services.Data.SportHubDb.Interfaces;

public class UsersService : IUsersService
{
    private readonly SportHubDbContext context;

    public UsersService(SportHubDbContext context)
    {
        this.context = context;
    }

    public async Task<IEnumerable<User>> GetUsersAsync()
    {
        var users = await this.context
            .Users
            .ToListAsync();

        return users;
    }
}