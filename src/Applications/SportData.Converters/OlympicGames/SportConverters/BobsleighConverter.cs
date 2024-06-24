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

public class BobsleighConverter : BaseSportConverter
{
    public BobsleighConverter(IOlympediaService olympediaService, IDateService dateService, IDataCacheService dataCacheService, IRegExpService regExpService, IMapper mapper,
        INormalizeService normalizeService, OlympicGamesRepository<Team> teamRepository, OlympicGamesRepository<Participation> participationRepository,
        OlympicGamesRepository<Athlete> athleteRepository, OlympicGamesRepository<Result> resultRepository)
        : base(olympediaService, dateService, dataCacheService, regExpService, mapper, normalizeService, teamRepository, participationRepository, athleteRepository, resultRepository)
    {
    }

    public override async Task ProcessAsync(Options options)
    {
        switch (options.Discipline.Name)
        {
            case DisciplineConstants.BOBSLEIGH:
                await this.ProcessBobsleighAsync(options);
                break;
        }
    }

    private async Task ProcessBobsleighAsync(Options options)
    {
        var rounds = new List<Round<Bobsleigh>>();
        var roundData = options.Rounds.FirstOrDefault();
        var track = this.ExtractTrack(options.HtmlDocument);
        var round = this.CreateRound<Bobsleigh>(roundData, options.Event.Name, track);

        await Console.Out.WriteLineAsync($"{options.Game.Year} - {options.Event.Name}");
        await this.SetBobsleighTeamsAsync(round, roundData, options.Event.Id, options.Event.IsTeamEvent);

        foreach (var document in options.Documents.Where(x => !x.Title.StartsWith("Standings")))
        {
            this.SetRuns(round, document.Rounds.FirstOrDefault(), document.Title, options.Event.IsTeamEvent);
        }

        rounds.Add(round);
        await this.ProcessJsonAsync(rounds, options);
    }

    private void SetRuns(Round<Bobsleigh> round, RoundDataModel roundData, string info, bool isTeamEvent)
    {
        var runNumber = 0;
        switch (info)
        {
            case "Run #1":
                runNumber = 1;
                break;
            case "Run #2":
            case "Run #21":
                runNumber = 2;
                break;
            case "Run #3":
            case "Run #31":
                runNumber = 3;
                break;
            case "Run #4":
                runNumber = 4;
                break;
        }

        for (var i = 1; i < roundData.Rows.Count; i++)
        {
            var row = roundData.Rows[i];
            var data = row.Elements("td").ToList();
            var noc = this.OlympediaService.FindNOCCode(row.OuterHtml);
            var athleteModels = this.OlympediaService.FindAthletes(row.OuterHtml);

            if (noc == null)
            {
                continue;
            }

            if (athleteModels.Count == 0)
            {
                athleteModels = this.OlympediaService.FindAthletes(roundData.Rows[i + 1].OuterHtml);
            }

            if (isTeamEvent)
            {
                var teamName = this.GetString(roundData.Indexes, ConverterConstants.Name, data);
                var team = round.Teams.FirstOrDefault(x => x.Name == teamName && x.NOC == noc);
                team ??= round.Teams.FirstOrDefault(x => x.NOC == noc && x.Athletes.Any(x => x.Code == athleteModels.FirstOrDefault().Code));
                team.Runs.Add(new BobsleighRun
                {
                    Number = runNumber,
                    Time = this.GetTime(roundData.Indexes, ConverterConstants.Time, data),
                    Intermediate1 = this.GetTime(roundData.Indexes, ConverterConstants.Intermediate1, data),
                    Intermediate2 = this.GetTime(roundData.Indexes, ConverterConstants.Intermediate2, data),
                    Intermediate3 = this.GetTime(roundData.Indexes, ConverterConstants.Intermediate3, data),
                    Intermediate4 = this.GetTime(roundData.Indexes, ConverterConstants.Intermediate4, data),
                    Intermediate5 = this.GetTime(roundData.Indexes, ConverterConstants.Intermediate5, data),
                });
            }
            else
            {
                var athleteModel = athleteModels.FirstOrDefault();
                var athlete = round.Athletes.FirstOrDefault(x => x.Code == athleteModel.Code);
                athlete.Runs.Add(new BobsleighRun
                {
                    Number = runNumber,
                    Time = this.GetTime(roundData.Indexes, ConverterConstants.Time, data),
                    Intermediate1 = this.GetTime(roundData.Indexes, ConverterConstants.Intermediate1, data),
                    Intermediate2 = this.GetTime(roundData.Indexes, ConverterConstants.Intermediate2, data),
                    Intermediate3 = this.GetTime(roundData.Indexes, ConverterConstants.Intermediate3, data),
                    Intermediate4 = this.GetTime(roundData.Indexes, ConverterConstants.Intermediate4, data),
                    Intermediate5 = this.GetTime(roundData.Indexes, ConverterConstants.Intermediate5, data),
                });
            }
        }
    }

    private async Task SetBobsleighTeamsAsync(Round<Bobsleigh> round, RoundDataModel roundData, Guid eventId, bool isTeamEvent)
    {
        for (var i = 1; i < roundData.Rows.Count; i++)
        {
            var row = roundData.Rows[i];
            var data = row.Elements("td").ToList();
            var noc = this.OlympediaService.FindNOCCode(row.OuterHtml);
            var athleteModels = this.OlympediaService.FindAthletes(row.OuterHtml);

            if (noc == null)
            {
                continue;
            }

            if (athleteModels.Count == 0)
            {
                athleteModels = this.OlympediaService.FindAthletes(roundData.Rows[i + 1].OuterHtml);
            }

            var nocCache = this.DataCacheService.NOCs.FirstOrDefault(x => x.Code == noc);

            if (isTeamEvent)
            {
                var teamName = this.GetString(roundData.Indexes, ConverterConstants.Name, data);
                var dbTeam = await this.TeamRepository.GetAsync(x => x.Name == teamName && x.NOCId == nocCache.Id && x.EventId == eventId);

                var team = new Bobsleigh
                {
                    Id = dbTeam.Id,
                    Name = dbTeam.Name,
                    NOC = noc,
                    FinishStatus = this.OlympediaService.FindStatus(row.OuterHtml),
                    Time = this.GetTime(roundData.Indexes, ConverterConstants.Time, data),
                };

                foreach (var athleteModel in athleteModels)
                {
                    var participant = await this.ParticipationRepository.GetAsync(x => x.Code == athleteModel.Code && x.EventId == eventId);
                    team.Athletes.Add(new Bobsleigh
                    {
                        Id = participant.Id,
                        Name = athleteModel.Name,
                        NOC = noc,
                        Code = athleteModel.Code,
                        FinishStatus = this.OlympediaService.FindStatus(row.OuterHtml),
                    });
                }

                round.Teams.Add(team);
            }
            else
            {
                var model = athleteModels.FirstOrDefault();
                var participant = await this.ParticipationRepository.GetAsync(x => x.Code == model.Code && x.EventId == eventId);
                var athlete = new Bobsleigh
                {
                    Id = participant.Id,
                    Name = model.Name,
                    NOC = noc,
                    Code = model.Code,
                    FinishStatus = this.OlympediaService.FindStatus(row.OuterHtml),
                    Time = this.GetTime(roundData.Indexes, ConverterConstants.Time, data),
                };

                round.Athletes.Add(athlete);
            }
        }
    }

    private Track ExtractTrack(HtmlDocument htmlDocument)
    {
        var curves = this.RegExpService.MatchFirstGroup(htmlDocument.DocumentNode.OuterHtml, @"Curves:(.*?)<br>");
        var length = this.RegExpService.MatchFirstGroup(htmlDocument.DocumentNode.OuterHtml, @"Length:(.*?)<br>");
        var startAltitude = this.RegExpService.MatchFirstGroup(htmlDocument.DocumentNode.OuterHtml, @"Start Altitude:(.*?)<br>");
        var verticalDrop = this.RegExpService.MatchFirstGroup(htmlDocument.DocumentNode.OuterHtml, @"Vertical Drop:(.*?)<\/td>");

        return new Track
        {
            Turns = this.RegExpService.MatchInt(curves),
            Length = this.RegExpService.MatchInt(length),
            StartAltitude = this.RegExpService.MatchInt(startAltitude),
            HeightDifference = this.RegExpService.MatchInt(verticalDrop),
        };
    }
}