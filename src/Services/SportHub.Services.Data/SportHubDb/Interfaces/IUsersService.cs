namespace SportHub.Services.Data.SportHubDb.Interfaces;

using SportHub.Data.Models.Entities.SportHub;

public interface IUsersService
{
    Task<IEnumerable<User>> GetUsersAsync();
}