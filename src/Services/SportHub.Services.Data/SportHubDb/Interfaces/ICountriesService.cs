namespace SportHub.Services.Data.SportHubDb.Interfaces;

using SportHub.Data.Models.DbEntities.SportHub;
using SportHub.Data.ViewModels.Countries;

public interface ICountriesService
{
    Task<Country> AddOrUpdateAsync(Country country);

    Task<Country> GetAsync(string code);

    Task<IEnumerable<CountryViewModel>> GetAllAsync();
}