namespace SportHub.Converters.OlympicGames.SportConverters;

using System;
using System.Threading.Tasks;

using AutoMapper;

using SportHub.Data.Models.Converters.OlympicGames;
using SportHub.Data.Models.Converters.OlympicGames.Base;
using SportHub.Data.Models.Converters.OlympicGames.Disciplines;
using SportHub.Data.Models.Entities.OlympicGames;
using SportHub.Data.Repositories;
using SportHub.Services.Data.OlympicGamesDb.Interfaces;
using SportHub.Services.Interfaces;

public class FootballConverter : BaseSportConverter
{
    public FootballConverter(IOlympediaService olympediaService, IDateService dateService, IDataCacheService dataCacheService, IRegExpService regExpService, IMapper mapper,
        INormalizeService normalizeService, OlympicGamesRepository<Team> teamRepository, OlympicGamesRepository<Participation> participationRepository,
        OlympicGamesRepository<Athlete> athleteRepository, OlympicGamesRepository<Result> resultRepository)
        : base(olympediaService, dateService, dataCacheService, regExpService, mapper, normalizeService, teamRepository, participationRepository, athleteRepository, resultRepository)
    {
    }

    public override async Task ProcessAsync(Options options)
    {
        var rounds = new List<Round<Football>>();
        await Console.Out.WriteLineAsync($"{options.Game.Year} - {options.Event.Name}");

        var allRounds = options.Rounds;
        if (options.Rounds.Count != 1)
        {
            allRounds = options.Rounds.Skip(1).ToList();
        }

        if (options.Game.Year == 2016)
        {
            ;
        }

        foreach (var roundData in allRounds)
        {
            await Console.Out.WriteLineAsync(roundData.Type.ToString());
            var round = this.CreateRound<Football>(roundData, options.Event.Name, null);
            await this.SetMatchesAsync(round, roundData, options);
            rounds.Add(round);
        }

        await this.ProcessJsonAsync(rounds, options);
    }

    private async Task SetMatchesAsync(Round<Football> round, RoundDataModel roundData, Options options)
    {
        foreach (var row in roundData.Rows.Where(x => this.OlympediaService.FindResultNumber(x.OuterHtml) != 0))
        {
            var data = row.Elements("td").ToList();
            var matchInputModel = new MatchInputModel
            {
                Row = row.OuterHtml,
                Number = data[0].OuterHtml,
                Date = data[1].InnerText,
                Year = options.Game.Year,
                EventId = options.Event.Id,
                IsTeam = true,
                HomeName = data[2].OuterHtml,
                HomeNOC = data[3].OuterHtml,
                Result = data[4].InnerHtml,
                AwayName = data[5].OuterHtml,
                AwayNOC = data[6].OuterHtml,
                AnyParts = false,
                RoundType = roundData.Type,
                RoundSubType = roundData.SubType,
                Location = null
            };
            var matchModel = await this.GetMatchAsync(matchInputModel);
            var match = this.Mapper.Map<TeamMatch<Football>>(matchModel);
            var document = options.Documents.FirstOrDefault(x => x.Id == match.ResultId);
            if (document != null)
            {
                match.Judges = await this.GetJudgesAsync(document.Html);
                match.Location = this.OlympediaService.FindLocation(document.Html);
                match.Attendance = this.RegExpService.MatchInt(this.RegExpService.MatchFirstGroup(document.HtmlDocument.ParsedText, @"<th>Attendance<\/th>\s*<td(?:.*?)>(.*?)<\/td>"));

                //await this.SetAthletesAsync(match.Team1, options.Game.Year <= 2016 ? document.Rounds.FirstOrDefault() : document.Rounds[1], options.Event.Id, options.Game.Year);
                //await this.SetAthletesAsync(match.Team2, options.Game.Year <= 2016 ? document.Rounds.LastOrDefault() : document.Rounds[2], options.Event.Id, options.Game.Year);

                //if (options.Game.Year <= 2016)
                //{
                //    this.SetHalfPoints(match, document.HtmlDocument);
                //}

                //this.SetStatistics(match.Team1);
                //this.SetStatistics(match.Team2);
            }

            round.TeamMatches.Add(match);
        }
    }
}