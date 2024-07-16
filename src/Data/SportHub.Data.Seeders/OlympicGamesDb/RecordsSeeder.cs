namespace SportHub.Data.Seeders.OlympicGamesDb;

using System;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;

using SportHub.Data.Models.DbEntities.OlympicGames.Enumerations;
using SportHub.Data.Models.Enumerations.OlympicGames;
using SportHub.Data.Repositories;
using SportHub.Data.Seeders.Interfaces;

public class RecordsSeeder : ISeeder
{
    public async Task SeedAsync(IServiceProvider services)
    {
        var repository = services.GetService<OlympicGamesRepository<Record>>();

        var types = new List<RecordEnum>
        {
            RecordEnum.None,
            RecordEnum.World,
            RecordEnum.Olympic,
        };

        foreach (var type in types)
        {
            var dbType = await repository.GetAsync(x => x.Name == type.ToString());
            if (dbType == null)
            {
                var record = new Record
                {
                    Name = type.ToString()
                };

                await repository.AddAsync(record);
                await repository.SaveChangesAsync();
            }
        }
    }
}