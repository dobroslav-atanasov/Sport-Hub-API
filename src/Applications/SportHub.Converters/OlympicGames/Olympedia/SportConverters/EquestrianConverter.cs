namespace SportHub.Converters.OlympicGames.Olympedia.SportConverters;

using System;
using System.Threading.Tasks;

using AutoMapper;

using SportHub.Common.Constants;
using SportHub.Data.Models.Converters.OlympicGames;
using SportHub.Data.Models.Converters.OlympicGames.Base;
using SportHub.Data.Models.Converters.OlympicGames.Disciplines;
using SportHub.Data.Models.DbEntities.OlympicGames;
using SportHub.Data.Repositories;
using SportHub.Services.Data.OlympicGamesDb.Interfaces;
using SportHub.Services.Interfaces;

public class EquestrianConverter : BaseSportConverter
{
    public EquestrianConverter(IOlympediaService olympediaService, IDateService dateService, IDataCacheService dataCacheService, IRegExpService regExpService, IMapper mapper,
        INormalizeService normalizeService, OlympicGamesRepository<Team> teamRepository, OlympicGamesRepository<Participation> participationRepository,
        OlympicGamesRepository<Athlete> athleteRepository, OlympicGamesRepository<Result> resultRepository)
        : base(olympediaService, dateService, dataCacheService, regExpService, mapper, normalizeService, teamRepository, participationRepository, athleteRepository, resultRepository)
    {
    }

    public override async Task ProcessAsync(Options options)
    {
        switch (options.Discipline.Name)
        {
            case DisciplineConstants.EQUESTRIAN_DRESSAGE:
                await this.ProcessDressageAsync(options);
                break;
            case DisciplineConstants.EQUESTRIAN_DRIVING:
                break;
            case DisciplineConstants.EQUESTRIAN_EVENTING:
                break;
            case DisciplineConstants.EQUESTRIAN_JUMPING:
                break;
            case DisciplineConstants.EQUESTRIAN_VAULTING:
                break;
        }
    }

    private async Task ProcessDressageAsync(Options options)
    {
        var rounds = new List<Round<Equestrian>>();
        await Console.Out.WriteLineAsync($"{options.Game.Year} - {options.Event.Name}");

        var allRounds = options.Rounds;
        if (options.Rounds.Count != 1)
        {
            allRounds = options.Rounds.Skip(1).ToList();
        }

        foreach (var roundData in allRounds)
        {
            var round = this.CreateRound<Equestrian>(roundData, options.Event.Name, null);
            var judges = await this.GetJudgesAsync(allRounds.Count == 1 ? options.HtmlDocument.ParsedText : roundData.Html);
            round.Judges = judges;
            foreach (var item in roundData.Indexes)
            {
                await Console.Out.WriteLineAsync(item.Key);
            }

            //await this.SetDivingAthletesAsync(round, roundData, options, null);
            rounds.Add(round);

            //var documentNumbers = this.OlympediaService.FindResults(allRounds.Count == 1 ? options.HtmlDocument.ParsedText : roundData.Html);
            //foreach (var number in documentNumbers)
            //{
            //    var document = options.Documents.FirstOrDefault(x => x.Id == number && !x.Title.Contains("Summary", StringComparison.CurrentCultureIgnoreCase));
            //    if (document != null)
            //    {
            //        await this.SetDivingAthletesAsync(round, document.Rounds.FirstOrDefault(), options, document.Title);
            //    }
            //}
        }

        await this.ProcessJsonAsync(rounds, options);
    }
}