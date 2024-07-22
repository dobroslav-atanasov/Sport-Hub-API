namespace SportHub.Data.Seeders.OlympicGamesDb;

using System;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;

using SportHub.Data.Models.DbEntities.OlympicGames.Enumerations;
using SportHub.Data.Models.Enumerations.OlympicGames;
using SportHub.Data.Repositories;
using SportHub.Data.Seeders.Interfaces;

public class FinishTypesSeeder : ISeeder
{
    public async Task SeedAsync(IServiceProvider services)
    {
        var repository = services.GetService<OlympicGamesRepository<FinishStatus>>();

        var types = new List<FinishStatusEnum>
        {
            FinishStatusEnum.None,
            FinishStatusEnum.Finish,
            FinishStatusEnum.DidNotFinish,
            FinishStatusEnum.Disqualified,
            FinishStatusEnum.AlsoCompeted,
            FinishStatusEnum.DidNotStart,
            FinishStatusEnum.TimeNotKnow
        };

        foreach (var type in types)
        {
            var dbType = await repository.GetAsync(x => x.Name == type.ToString());
            if (dbType == null)
            {
                var finishType = new FinishStatus
                {
                    Name = type.ToString()
                };

                await repository.AddAsync(finishType);
                await repository.SaveChangesAsync();
            }
        }
    }
}