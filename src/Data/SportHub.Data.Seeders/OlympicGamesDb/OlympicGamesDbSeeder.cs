namespace SportHub.Data.Seeders.OlympicGamesDb;

using System;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using SportHub.Data.Seeders.Interfaces;

public class OlympicGamesDbSeeder : ISeeder
{
    public async Task SeedAsync(IServiceProvider services)
    {
        var logger = services.GetService<ILogger<OlympicGamesDbSeeder>>();

        try
        {
            var seeders = new List<ISeeder>
            {
                new GameTypesSeeder(),
                new EventGendersSeeder(),
                new GendersSeeder(),
                new RolesSeeder(),
                new MedalsSeeder(),
                new FinishTypesSeeder(),
                new DecisionsSeeder(),
                new RecordsSeeder(),
                new RoundsSeeder()
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