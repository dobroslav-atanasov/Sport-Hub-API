namespace SportHub.Data.Seeders.OlympicGamesDb;

using System;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;

using SportHub.Data.Models.DbEntities.OlympicGames.Enumerations;
using SportHub.Data.Models.Enumerations.OlympicGames;
using SportHub.Data.Repositories;
using SportHub.Data.Seeders.Interfaces;

public class EventGendersSeeder : ISeeder
{
    public async Task SeedAsync(IServiceProvider services)
    {
        var repository = services.GetService<OlympicGamesRepository<EventGender>>();

        var types = new List<EventGenderEnum>
        {
            EventGenderEnum.None,
            EventGenderEnum.Men,
            EventGenderEnum.Women,
            EventGenderEnum.Mixed
        };

        foreach (var type in types)
        {
            var dbType = await repository.GetAsync(x => x.Name == type.ToString());
            if (dbType == null)
            {
                var eventGender = new EventGender
                {
                    Name = type.ToString()
                };

                await repository.AddAsync(eventGender);
                await repository.SaveChangesAsync();
            }
        }
    }
}