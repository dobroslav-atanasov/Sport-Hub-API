namespace SportData.Data.Seeders.OlympicGamesDb;

using System;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;

using SportData.Data.Models.Entities.OlympicGames;
using SportData.Data.Models.Entities.OlympicGames.Enumerations;
using SportData.Data.Repositories;
using SportData.Data.Seeders.Interfaces;

public class FinishTypesSeeder : ISeeder
{
    public async Task SeedAsync(IServiceProvider services)
    {
        var repository = services.GetService<OlympicGamesRepository<FinishType>>();

        var types = new List<FinishTypeEnum>
        {
            FinishTypeEnum.None,
            FinishTypeEnum.Finish,
            FinishTypeEnum.DidNotFinish,
            FinishTypeEnum.Disqualified,
            FinishTypeEnum.AlsoCompeted,
            FinishTypeEnum.DidNotStart,
            FinishTypeEnum.TimeNotKnow
        };

        foreach (var type in types)
        {
            var dbType = await repository.GetAsync(x => x.Name == type.ToString());
            if (dbType == null)
            {
                var finishType = new FinishType
                {
                    Name = type.ToString()
                };

                await repository.AddAsync(finishType);
                await repository.SaveChangesAsync();
            }
        }
    }
}