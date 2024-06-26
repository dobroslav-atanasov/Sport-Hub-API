namespace SportHub.Data.Seeders.OlympicGamesDb;

using System;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;

using SportHub.Data.Models.Entities.OlympicGames;
using SportHub.Data.Models.Entities.OlympicGames.Enumerations;
using SportHub.Data.Repositories;
using SportHub.Data.Seeders.Interfaces;

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