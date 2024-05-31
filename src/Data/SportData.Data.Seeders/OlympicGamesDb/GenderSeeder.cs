namespace SportData.Data.Seeders.OlympicGamesDb;

using System;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;

using SportData.Data.Models.Entities.OlympicGames;
using SportData.Data.Models.Entities.OlympicGames.Enumerations;
using SportData.Data.Repositories;
using SportData.Data.Seeders.Interfaces;

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