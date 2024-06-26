namespace SportHub.Services.Data.SportHubDb.Interfaces;

using SportHub.Data.Models.Entities.SportHub;
using SportHub.Data.ViewModels.Countries;

public interface ICountriesService
{
    Task<Country> AddOrUpdateAsync(Country country);

    Task<Country> GetAsync(string code);

    Task<IEnumerable<CountryViewModel>> GetAllAsync();
}