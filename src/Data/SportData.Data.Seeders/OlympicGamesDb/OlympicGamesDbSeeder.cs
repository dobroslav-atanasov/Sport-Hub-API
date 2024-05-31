﻿namespace SportData.Data.Seeders.OlympicGamesDb;

using System;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using SportData.Data.Seeders.Interfaces;

public class OlympicGamesDbSeeder : ISeeder
{
    public async Task SeedAsync(IServiceProvider services)
    {
        var logger = services.GetService<ILogger<OlympicGamesDbSeeder>>();

        try
        {
            var seeders = new List<ISeeder>
            {
                new AthleteTypesSeeder(),
                new EventGenderTypesSeeder(),
                new FinishTypesSeeder(),
                new GenderSeeder(),
                new MedalsSeeder(),
                new OlympicGameTypesSeeder()
            };

            foreach (var seeder in seeders)
            {
                await seeder.SeedAsync(services);
                logger.LogInformation($"Seeder {seeder.GetType().Name} done.");
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message, ex);
        }
    }
}