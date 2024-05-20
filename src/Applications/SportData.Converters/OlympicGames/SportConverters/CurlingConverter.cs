namespace SportData.Converters.OlympicGames.SportConverters;

using System;
using System.Threading.Tasks;

using AutoMapper;

using HtmlAgilityPack;

using SportData.Common.Constants;
using SportData.Data.Models.Converters.OlympicGames;
using SportData.Data.Models.Converters.OlympicGames.Base;
using SportData.Data.Models.Converters.OlympicGames.Disciplines;
using SportData.Data.Models.Entities.OlympicGames;
using SportData.Data.Repositories;
using SportData.Services.Data.OlympicGamesDb.Interfaces;
using SportData.Services.Interfaces;

public class CurlingConverter : BaseSportConverter
{
    public CurlingConverter(IOlympediaService olympediaService, IDateService dateService, IDataCacheService dataCacheService, IRegExpService regExpService, IMapper mapper,
        INormalizeService normalizeService, OlympicGamesRepository<Team> teamRepository, OlympicGamesRepository<Participation> participationRepository,
        OlympicGamesRepository<Athlete> athleteRepository, OlympicGamesRepository<Result> resultRepository)
        : base(olympediaService, dateService, dataCacheService, regExpService, mapper, normalizeService, teamRepository, participationRepository, athleteRepository, resultRepository)
    {
    }

    public override async Task ProcessAsync(Options options)
    {
        var rounds = new List<Round<Curling>>();
        await Console.Out.WriteLineAsync($"{options.Game.Year} - {options.Event.Name}");

        foreach (var roundData in options.Rounds.Skip(1))
        {
            var round = this.CreateRound<Curling>(roundData, options.Event.Name, null);
            await this.SetMatchesAsync(round, options, roundData);
            rounds.Add(round);
        }

        await this.ProcessJsonAsync(rounds, options);
    }

    private async Task SetMatchesAsync(Round<Curling> round, Options options, RoundDataModel roundData)
    {
        var year = options.Game.Year;
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
                IsTeam = options.Event.IsTeamEvent,
                IsDoubles = false,
                HomeName = null,
                HomeNOC = data[2].OuterHtml,
                Result = data[3].OuterHtml,
                AwayName = null,
                AwayNOC = data[4].OuterHtml,
                AnyParts = false,
                RoundType = roundData.Type,
                RoundSubType = roundData.SubType,
                Location = null,
            };
            var matchModel = await this.GetMatchAsync(matchInputModel);
            var match = this.Mapper.Map<TeamMatch<Curling>>(matchModel);

            var document = options.Documents.FirstOrDefault(x => x.Id == match.ResultId);
            if (document != null)
            {
                this.SetEnds(match, document.HtmlDocument);
                await this.SetStatisticsAsync(match.Team1, document.Rounds.ElementAtOrDefault(0), options.Event.Id);
                await this.SetStatisticsAsync(match.Team2, document.Rounds.ElementAtOrDefault(1), options.Event.Id);
            }

            round.TeamMatches.Add(match);
        }
    }

    private void SetEnds(TeamMatch<Curling> match, HtmlDocument htmlDocument)
    {
        var scoreMatch = this.RegExpService.Match(htmlDocument.ParsedText, @"<h2 class=""match_part_title"">Score<\/h2>(.*?)<h2");
        if (scoreMatch != null)
        {
            var document = new HtmlDocument();
            document.LoadHtml(scoreMatch.Groups[1].Value);

            var headers = document.DocumentNode.SelectNodes("//th");
            var rows = document.DocumentNode.SelectNodes("//tr").Skip(1).ToList();
            var homeTd = rows[0].Elements("td").Skip(2).ToList();
            var awayTd = rows[1].Elements("td").Skip(2).ToList();

            for (var i = 0; i < headers.Count; i++)
            {
                var header = headers[i].InnerText.Trim();

                switch (header)
                {
                    case "1":
                        match.Team1.Ends.Add(new() { Number = 1, Points = this.RegExpService.MatchInt(homeTd[i].InnerText) });
                        match.Team2.Ends.Add(new() { Number = 1, Points = this.RegExpService.MatchInt(awayTd[i].InnerText) });
                        break;
                    case "2":
                        match.Team1.Ends.Add(new() { Number = 2, Points = this.RegExpService.MatchInt(homeTd[i].InnerText) });
                        match.Team2.Ends.Add(new() { Number = 2, Points = this.RegExpService.MatchInt(awayTd[i].InnerText) });
                        break;
                    case "3":
                        match.Team1.Ends.Add(new() { Number = 3, Points = this.RegExpService.MatchInt(homeTd[i].InnerText) });
                        match.Team2.Ends.Add(new() { Number = 3, Points = this.RegExpService.MatchInt(awayTd[i].InnerText) });
                        break;
                    case "4":
                        match.Team1.Ends.Add(new() { Number = 4, Points = this.RegExpService.MatchInt(homeTd[i].InnerText) });
                        match.Team2.Ends.Add(new() { Number = 4, Points = this.RegExpService.MatchInt(awayTd[i].InnerText) });
                        break;
                    case "5":
                        match.Team1.Ends.Add(new() { Number = 5, Points = this.RegExpService.MatchInt(homeTd[i].InnerText) });
                        match.Team2.Ends.Add(new() { Number = 5, Points = this.RegExpService.MatchInt(awayTd[i].InnerText) });
                        break;
                    case "6":
                        match.Team1.Ends.Add(new() { Number = 6, Points = this.RegExpService.MatchInt(homeTd[i].InnerText) });
                        match.Team2.Ends.Add(new() { Number = 6, Points = this.RegExpService.MatchInt(awayTd[i].InnerText) });
                        break;
                    case "7":
                        match.Team1.Ends.Add(new() { Number = 7, Points = this.RegExpService.MatchInt(homeTd[i].InnerText) });
                        match.Team2.Ends.Add(new() { Number = 7, Points = this.RegExpService.MatchInt(awayTd[i].InnerText) });
                        break;
                    case "8":
                        match.Team1.Ends.Add(new() { Number = 8, Points = this.RegExpService.MatchInt(homeTd[i].InnerText) });
                        match.Team2.Ends.Add(new() { Number = 8, Points = this.RegExpService.MatchInt(awayTd[i].InnerText) });
                        break;
                    case "9":
                        match.Team1.Ends.Add(new() { Number = 9, Points = this.RegExpService.MatchInt(homeTd[i].InnerText) });
                        match.Team2.Ends.Add(new() { Number = 9, Points = this.RegExpService.MatchInt(awayTd[i].InnerText) });
                        break;
                    case "10":
                        match.Team1.Ends.Add(new() { Number = 10, Points = this.RegExpService.MatchInt(homeTd[i].InnerText) });
                        match.Team2.Ends.Add(new() { Number = 10, Points = this.RegExpService.MatchInt(awayTd[i].InnerText) });
                        break;
                }
            }
        }
    }

    private async Task SetStatisticsAsync(Curling team, RoundDataModel roundData, Guid eventId)
    {
        foreach (var row in roundData.Rows.Skip(1))
        {
            var data = row.Elements("td").ToList();
            var athleteModel = this.OlympediaService.FindAthlete(row.OuterHtml);
            if (athleteModel != null)
            {
                var participant = await this.ParticipationRepository.GetAsync(x => x.Code == athleteModel.Code && x.EventId == eventId);
                if (participant != null)
                {
                    var athlete = new Curling
                    {
                        Id = participant.Id,
                        Name = athleteModel.Name,
                        NOC = team.NOC,
                        Code = athleteModel.Code,
                        Position = this.GetString(roundData.Indexes, ConverterConstants.Position, data),
                        Percent = this.GetInt(roundData.Indexes, ConverterConstants.Percent, data),
                    };

                    team.Athletes.Add(athlete);
                }
            }
        }
    }
}