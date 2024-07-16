namespace SportHub.Data.Seeders.OlympicGamesDb;

using System;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;

using SportHub.Data.Models.DbEntities.OlympicGames.Enumerations;
using SportHub.Data.Models.Enumerations.OlympicGames;
using SportHub.Data.Repositories;
using SportHub.Data.Seeders.Interfaces;

public class GameTypesSeeder : ISeeder
{
    public async Task SeedAsync(IServiceProvider services)
    {
        var repository = services.GetService<OlympicGamesRepository<GameType>>();

        var types = new List<GameTypeEnum>
        {
            GameTypeEnum.Summer,
            GameTypeEnum.Winter
        };

        foreach (var type in types)
        {
            var dbType = await repository.GetAsync(x => x.Name == type.ToString());
            if (dbType == null)
            {
                var olympicGameType = new GameType
                {
                    Name = type.ToString()
                };

                await repository.AddAsync(olympicGameType);
                await repository.SaveChangesAsync();
            }
        }
    }
}