namespace SportData.Services.Data.SportDataDb.Interfaces;

using SportData.Data.Models.Entities.SportData;

public interface IUsersService
{
    Task<IEnumerable<User>> GetUsersAsync();
}