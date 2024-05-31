namespace SportData.Data.Seeders.OlympicGamesDb;

using System;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;

using SportData.Data.Models.Entities.OlympicGames;
using SportData.Data.Models.Entities.OlympicGames.Enumerations;
using SportData.Data.Repositories;
using SportData.Data.Seeders.Interfaces;

public class EventGenderTypesSeeder : ISeeder
{
    public async Task SeedAsync(IServiceProvider services)
    {
        var repository = services.GetService<OlympicGamesRepository<EventGenderType>>();

        var types = new List<EventGenderTypeEnum>
        {
            EventGenderTypeEnum.None,
            EventGenderTypeEnum.Men,
            EventGenderTypeEnum.Women,
            EventGenderTypeEnum.Mixed
        };

        foreach (var type in types)
        {
            var dbType = await repository.GetAsync(x => x.Name == type.ToString());
            if (dbType == null)
            {
                var eventGenderType = new EventGenderType
                {
                    Name = type.ToString()
                };

                await repository.AddAsync(eventGenderType);
                await repository.SaveChangesAsync();
            }
        }
    }
}