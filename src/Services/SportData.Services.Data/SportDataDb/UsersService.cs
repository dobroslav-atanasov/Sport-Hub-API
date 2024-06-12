namespace SportData.Services.Data.SportDataDb;

using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using SportData.Data.Contexts;
using SportData.Data.Models.Entities.SportData;
using SportData.Services.Data.SportDataDb.Interfaces;

public class UsersService : IUsersService
{
    private readonly SportDataDbContext context;

    public UsersService(SportDataDbContext context)
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