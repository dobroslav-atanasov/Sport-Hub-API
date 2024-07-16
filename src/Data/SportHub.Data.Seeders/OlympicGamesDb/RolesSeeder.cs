namespace SportHub.Data.Seeders.OlympicGamesDb;

using System;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;

using SportHub.Data.Models.DbEntities.OlympicGames.Enumerations;
using SportHub.Data.Models.Enumerations.OlympicGames;
using SportHub.Data.Repositories;
using SportHub.Data.Seeders.Interfaces;

public class RolesSeeder : ISeeder
{
    public async Task SeedAsync(IServiceProvider services)
    {
        var repository = services.GetService<OlympicGamesRepository<Role>>();

        var types = new List<RoleEnum>
        {
            RoleEnum.None,
            RoleEnum.Athlete,
            RoleEnum.Coach,
            RoleEnum.Referee,
            RoleEnum.IOCMember
        };

        foreach (var type in types)
        {
            var dbType = await repository.GetAsync(x => x.Name == type.ToString());
            if (dbType == null)
            {
                var athleteType = new Role
                {
                    Name = type.ToString()
                };

                await repository.AddAsync(athleteType);
                await repository.SaveChangesAsync();
            }
        }
    }
}