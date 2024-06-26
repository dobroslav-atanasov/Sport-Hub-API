namespace SportHub.Data.Seeders.OlympicGamesDb;

using System;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;

using SportHub.Data.Models.Entities.OlympicGames;
using SportHub.Data.Models.Entities.OlympicGames.Enumerations;
using SportHub.Data.Repositories;
using SportHub.Data.Seeders.Interfaces;

public class GenderSeeder : ISeeder
{
    public async Task SeedAsync(IServiceProvider services)
    {
        var repository = services.GetService<OlympicGamesRepository<Gender>>();

        var types = new List<GenderTypeEnum>
        {
            GenderTypeEnum.Male,
            GenderTypeEnum.Female,
            GenderTypeEnum.None
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