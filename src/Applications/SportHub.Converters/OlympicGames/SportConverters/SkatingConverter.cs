namespace SportHub.Converters.OlympicGames.SportConverters;

using System;
using System.Threading.Tasks;

using AutoMapper;

using SportHub.Common.Constants;
using SportHub.Data.Models.Converters.OlympicGames;
using SportHub.Data.Models.Converters.OlympicGames.Base;
using SportHub.Data.Models.Converters.OlympicGames.Disciplines;
using SportHub.Data.Models.Entities.OlympicGames;
using SportHub.Data.Repositories;
using SportHub.Services.Data.OlympicGamesDb.Interfaces;
using SportHub.Services.Interfaces;

public class SkatingConverter : BaseSportConverter
{
    public SkatingConverter(IOlympediaService olympediaService, IDateService dateService, IDataCacheService dataCacheService, IRegExpService regExpService, IMapper mapper,
        INormalizeService normalizeService, OlympicGamesRepository<Team> teamRepository, OlympicGamesRepository<Participation> participationRepository,
        OlympicGamesRepository<Athlete> athleteRepository, OlympicGamesRepository<Result> resultRepository)
        : base(olympediaService, dateService, dataCacheService, regExpService, mapper, normalizeService, teamRepository, participationRepository, athleteRepository, resultRepository)
    {
    }

    public override async Task ProcessAsync(Options options)
    {
        switch (options.Discipline.Name)
        {
            case DisciplineConstants.FIGURE_SKATING:
                await this.ProcessFigureSkatingAsync(options);
                break;
        }
    }

    private async Task ProcessFigureSkatingAsync(Options options)
    {
        var rounds = new List<Round<FigureSkating>>();
        await Console.Out.WriteLineAsync($"{options.Game.Year} - {options.Event.Name}");

        var allRounds = options.Rounds;
        if (options.Rounds.Count != 1)
        {
            allRounds = options.Rounds.Skip(1).ToList();
        }

        foreach (var roundData in allRounds)
        {
            await Console.Out.WriteLineAsync(roundData.Type.ToString());
            var round = this.CreateRound<FigureSkating>(roundData, options.Event.Name, null);
            rounds.Add(round);
        }

        await this.ProcessJsonAsync(rounds, options);
    }
}