namespace SportData.Converters.OlympicGames.SportConverters;

using System;
using System.Threading.Tasks;

using AutoMapper;

using SportData.Data.Models.Converters.OlympicGames;
using SportData.Data.Models.Converters.OlympicGames.Base;
using SportData.Data.Models.Converters.OlympicGames.Disciplines;
using SportData.Data.Models.Entities.OlympicGames;
using SportData.Data.Repositories;
using SportData.Services.Data.OlympicGamesDb.Interfaces;
using SportData.Services.Interfaces;

public class FencingConverter : BaseSportConverter
{
    public FencingConverter(IOlympediaService olympediaService, IDateService dateService, IDataCacheService dataCacheService, IRegExpService regExpService, IMapper mapper,
        INormalizeService normalizeService, OlympicGamesRepository<Team> teamRepository, OlympicGamesRepository<Participation> participationRepository,
        OlympicGamesRepository<Athlete> athleteRepository, OlympicGamesRepository<Result> resultRepository)
        : base(olympediaService, dateService, dataCacheService, regExpService, mapper, normalizeService, teamRepository, participationRepository, athleteRepository, resultRepository)
    {
    }

    public override async Task ProcessAsync(Options options)
    {
        var rounds = new List<Round<Fencing>>();
        await Console.Out.WriteLineAsync($"{options.Game.Year} - {options.Event.Name}");

        var allRounds = options.Rounds;
        if (options.Rounds.Count != 1)
        {
            allRounds = options.Rounds.Skip(1).ToList();
        }

        if (options.Game.Year == 1988)
        {
            ;
        }

        foreach (var roundData in allRounds)
        {
            var round = this.CreateRound<Fencing>(roundData, options.Event.Name, null);
            await Console.Out.WriteLineAsync(" --- " + roundData.Type.ToString());
            await this.SetMatchesAsync(round, roundData, options);
            rounds.Add(round);
        }

        await this.ProcessJsonAsync(rounds, options);
    }

    private async Task SetMatchesAsync(Round<Fencing> round, RoundDataModel roundData, Options options)
    {
        foreach (var item in roundData.Indexes)
        {
            await Console.Out.WriteLineAsync(item.Key);
        }

        //foreach (var row in roundData.Rows)
        //{
        //    var data = row.Elements("td").ToList();
        //    if (data.Count != 0 /*&& (data[0].InnerText.Contains("Bout ") || data[0].InnerText.Contains("Match "))*/)
        //    {
        //        Console.WriteLine(data[0].InnerText);
        //        //await Console.Out.WriteLineAsync($"{data.Count}");
        //    }
        //}
    }
}