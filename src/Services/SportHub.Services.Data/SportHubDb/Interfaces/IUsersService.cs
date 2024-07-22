namespace SportHub.Services.Data.SportHubDb.Interfaces;

using SportHub.Data.Models.DbEntities.SportHub;

public interface IUsersService
{
    Task<IEnumerable<User>> GetUsersAsync();
}