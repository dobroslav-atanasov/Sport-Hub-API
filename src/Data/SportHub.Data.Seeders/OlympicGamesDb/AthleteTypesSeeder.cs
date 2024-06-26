namespace SportHub.Data.Seeders.OlympicGamesDb;

using System;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;

using SportHub.Data.Models.Entities.OlympicGames;
using SportHub.Data.Models.Entities.OlympicGames.Enumerations;
using SportHub.Data.Repositories;
using SportHub.Data.Seeders.Interfaces;

public class AthleteTypesSeeder : ISeeder
{
    public async Task SeedAsync(IServiceProvider services)
    {
        var repository = services.GetService<OlympicGamesRepository<AthleteType>>();

        var types = new List<AthleteTypeEnum>
        {
            AthleteTypeEnum.None,
            AthleteTypeEnum.Athlete,
            AthleteTypeEnum.Coach,
            AthleteTypeEnum.Referee,
            AthleteTypeEnum.IOCMember
        };

        foreach (var type in types)
        {
            var dbType = await repository.GetAsync(x => x.Name == type.ToString());
            if (dbType == null)
            {
                var athleteType = new AthleteType
                {
                    Name = type.ToString()
                };

                await repository.AddAsync(athleteType);
                await repository.SaveChangesAsync();
            }
        }
    }
}