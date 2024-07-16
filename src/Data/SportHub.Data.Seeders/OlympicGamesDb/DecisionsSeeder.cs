namespace SportHub.Data.Seeders.OlympicGamesDb;

using Microsoft.Extensions.DependencyInjection;

using SportHub.Data.Models.DbEntities.OlympicGames.Enumerations;
using SportHub.Data.Models.Enumerations.OlympicGames;
using SportHub.Data.Repositories;
using SportHub.Data.Seeders.Interfaces;

public class DecisionsSeeder : ISeeder
{
    public async Task SeedAsync(IServiceProvider services)
    {
        var repository = services.GetService<OlympicGamesRepository<Decision>>();

        var types = new List<DecisionEnum>
        {
            DecisionEnum.None,
            DecisionEnum.Buy,
            DecisionEnum.Walkover,
        };

        foreach (var type in types)
        {
            var dbType = await repository.GetAsync(x => x.Name == type.ToString());
            if (dbType == null)
            {
                var decision = new Decision
                {
                    Name = type.ToString()
                };

                await repository.AddAsync(decision);
                await repository.SaveChangesAsync();
            }
        }
    }
}