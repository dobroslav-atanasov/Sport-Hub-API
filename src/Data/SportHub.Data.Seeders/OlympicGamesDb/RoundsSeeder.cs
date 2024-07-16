namespace SportHub.Data.Seeders.OlympicGamesDb;

using System;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;

using SportHub.Data.Models.DbEntities.OlympicGames.Enumerations;
using SportHub.Data.Models.Enumerations.OlympicGames;
using SportHub.Data.Repositories;
using SportHub.Data.Seeders.Interfaces;

public class RoundsSeeder : ISeeder
{
    public async Task SeedAsync(IServiceProvider services)
    {
        var repository = services.GetService<OlympicGamesRepository<Round>>();

        var types = new List<RoundEnum>
        {
            RoundEnum.None,
            RoundEnum.Barrage,
            RoundEnum.BronzeMedalMatch,
            RoundEnum.Classification,
            RoundEnum.CompulsoryDance,
            RoundEnum.CompulsoryFigures,
            RoundEnum.ConsolationRound,
            RoundEnum.Eightfinals,
            RoundEnum.EliminationRound,
            RoundEnum.Final,
            RoundEnum.FinalRound,
            RoundEnum.FleetRaces,
            RoundEnum.FreeRoutine,
            RoundEnum.FreeSkating,
            RoundEnum.GoldMedalMatch,
            RoundEnum.GrandPrix,
            RoundEnum.Group,
            RoundEnum.Heat,
            RoundEnum.LuckyLoserRound,
            RoundEnum.OriginalSetPatternDance,
            RoundEnum.Playoff,
            RoundEnum.Pool,
            RoundEnum.PreliminaryRound,
            RoundEnum.Qualification,
            RoundEnum.Quarterfinals,
            RoundEnum.RaceEight,
            RoundEnum.RaceFive,
            RoundEnum.RaceFour,
            RoundEnum.RaceNine,
            RoundEnum.RaceOne,
            RoundEnum.RaceSeven,
            RoundEnum.RaceSix,
            RoundEnum.RaceTen,
            RoundEnum.RaceThree,
            RoundEnum.RaceTwo,
            RoundEnum.RankingRound,
            RoundEnum.Repechage,
            RoundEnum.RhythmDance,
            RoundEnum.RoundFive,
            RoundEnum.RoundFour,
            RoundEnum.RoundOne,
            RoundEnum.RoundRobin,
            RoundEnum.RoundSeven,
            RoundEnum.RoundSix,
            RoundEnum.RoundThree,
            RoundEnum.RoundTwo,
            RoundEnum.Semifinals,
            RoundEnum.ShortProgram,
            RoundEnum.SilverMedalMatch,
            RoundEnum.TechnicalRoutine,
            RoundEnum.Run1,
            RoundEnum.Run2,
            RoundEnum.Downhill,
            RoundEnum.Slalom
        };

        foreach (var type in types)
        {
            var dbType = await repository.GetAsync(x => x.Name == type.ToString());
            if (dbType == null)
            {
                var round = new Round
                {
                    Name = type.ToString()
                };

                await repository.AddAsync(round);
                await repository.SaveChangesAsync();
            }
        }
    }
}