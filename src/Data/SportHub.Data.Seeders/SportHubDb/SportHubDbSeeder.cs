namespace SportHub.Data.Seeders.SportHubDb;

using System;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using SportHub.Data.Seeders.Interfaces;

public class SportHubDbSeeder : ISeeder
{
    public async Task SeedAsync(IServiceProvider services)
    {
        var logger = services.GetService<ILogger<SportHubDbSeeder>>();

        try
        {
            var seeders = new List<ISeeder>
            {
                new RolesSeeder(),
                new UsersSeeder()
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