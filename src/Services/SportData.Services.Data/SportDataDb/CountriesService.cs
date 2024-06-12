namespace SportData.Services.Data.SportDataDb;

using System.Collections.Generic;

using SportData.Data.Models.Entities.SportData;
using SportData.Data.Repositories;
using SportData.Data.ViewModels.Countries;
using SportData.Services.Data.SportDataDb.Interfaces;
using SportData.Services.Mapper.Extensions;

public class CountriesService : ICountriesService
{
    private readonly SportDataRepository<Country> repository;

    public CountriesService(SportDataRepository<Country> repository)
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