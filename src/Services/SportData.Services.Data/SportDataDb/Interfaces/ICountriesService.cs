namespace SportData.Services.Data.SportDataDb.Interfaces;

using SportData.Data.Models.Entities.SportData;
using SportData.Data.ViewModels.Countries;

public interface ICountriesService
{
    Task<Country> AddOrUpdateAsync(Country country);

    Task<Country> GetAsync(string code);

    Task<IEnumerable<CountryViewModel>> GetAllAsync();
}