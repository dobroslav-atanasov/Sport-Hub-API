namespace SportHub.Services.Data.SportHubDb;

using System.Collections.Generic;

using SportHub.Data.Models.Entities.SportHub;
using SportHub.Data.Repositories;
using SportHub.Data.ViewModels.Countries;
using SportHub.Services.Data.SportHubDb.Interfaces;
using SportHub.Services.Mapper.Extensions;

public class CountriesService : ICountriesService
{
    private readonly SportHubRepository<Country> repository;

    public CountriesService(SportHubRepository<Country> repository)
    {
        this.repository = repository;
    }

    public async Task<Country> AddOrUpdateAsync(Country country)
    {
        var dbCountry = await this.repository.GetAsync(x => x.Code == country.Code);
        if (dbCountry == null)
        {
            await this.repository.AddAsync(country);
            await this.repository.SaveChangesAsync();
        }
        else
        {
            var isUpdated = dbCountry.IsUpdated(country);
            if (isUpdated)
            {
                this.repository.Update(dbCountry);
                await this.repository.SaveChangesAsync();
            }

            country = dbCountry;
        }

        return country;
    }

    public async Task<IEnumerable<CountryViewModel>> GetAllAsync()
    {
        var countries = this.repository
            .AllAsNoTracking()
            .To<CountryViewModel>();

        return countries;
    }

    public async Task<Country> GetAsync(string code)
    {
        var country = await this.repository.GetAsync(x => x.Code == code);
        return country;
    }
}