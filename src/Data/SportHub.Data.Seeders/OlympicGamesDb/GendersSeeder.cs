namespace SportHub.Data.Seeders.OlympicGamesDb;

using System;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;

using SportHub.Data.Models.DbEntities.OlympicGames.Enumerations;
using SportHub.Data.Models.Enumerations.OlympicGames;
using SportHub.Data.Repositories;
using SportHub.Data.Seeders.Interfaces;

public class GendersSeeder : ISeeder
{
    public async Task SeedAsync(IServiceProvider services)
    {
        var repository = services.GetService<OlympicGamesRepository<Gender>>();

        var types = new List<GenderEnum>
        {
            GenderEnum.Male,
            GenderEnum.Female,
            GenderEnum.None
        };

        foreach (var type in types)
        {
            var dbType = await repository.GetAsync(x => x.Name == type.ToString());
            if (dbType == null)
            {
                var gender = new Gender
                {
                    Name = type.ToString()
                };

                await repository.AddAsync(gender);
                await repository.SaveChangesAsync();
            }
        }
    }
}