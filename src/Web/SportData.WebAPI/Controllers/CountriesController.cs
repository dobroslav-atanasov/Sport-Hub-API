namespace SportData.WebAPI.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using SportData.Services.Data.SportDataDb.Interfaces;

[Authorize]
public class CountriesController : BaseController
{
    private readonly ICountriesService countriesService;

    public CountriesController(ICountriesService countriesService)
    {
        this.countriesService = countriesService;
    }

    [HttpGet]
    public async Task<IActionResult> GetCountries()
    {
        var countries = await this.countriesService.GetAllAsync();
        return this.Ok(countries);
    }
}