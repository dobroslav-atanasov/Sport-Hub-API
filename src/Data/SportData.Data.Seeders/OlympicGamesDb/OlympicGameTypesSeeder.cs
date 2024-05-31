namespace SportData.Data.Seeders.OlympicGamesDb;

using System;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;

using SportData.Data.Models.Entities.OlympicGames;
using SportData.Data.Models.Entities.OlympicGames.Enumerations;
using SportData.Data.Repositories;
using SportData.Data.Seeders.Interfaces;

public class OlympicGameTypesSeeder : ISeeder
{
    public async Task SeedAsync(IServiceProvider services)
    {
        var repository = services.GetService<OlympicGamesRepository<OlympicGameType>>();

        var types = new List<OlympicGameTypeEnum>
        {
            OlympicGameTypeEnum.Summer,
            OlympicGameTypeEnum.Winter
        };

        foreach (var type in types)
        {
            var dbType = await repository.GetAsync(x => x.Name == type.ToString());
            if (dbType == null)
            {
                var olympicGameType = new OlympicGameType
                {
                    Name = type.ToString()
                };

                await repository.AddAsync(olympicGameType);
                await repository.SaveChangesAsync();
            }
        }
    }
}