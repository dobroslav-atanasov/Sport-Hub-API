namespace SportHub.Converters.OlympicGames;

using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using SportHub.Common.Constants;
using SportHub.Data.Models.Cache;
using SportHub.Data.Models.Entities.Crawlers;
using SportHub.Data.Models.Entities.OlympicGames;
using SportHub.Data.Models.Entities.OlympicGames.Enumerations;
using SportHub.Services.Data.CrawlerStorageDb.Interfaces;
using SportHub.Services.Data.OlympicGamesDb;
using SportHub.Services.Data.OlympicGamesDb.Interfaces;
using SportHub.Services.Interfaces;

public class ParticipantConverter : BaseOlympediaConverter
{
    private readonly OlympicGamesRepositoryService<Participation> participationsService;
    private readonly OlympicGamesRepositoryService<Athlete> athletesService;
    private readonly OlympicGamesRepositoryService<Team> teamsService;
    private readonly OlympicGamesRepositoryService<Squad> squadsService;

    public ParticipantConverter(ILogger<BaseConverter> logger, ICrawlersService crawlersService, ILogsService logsService, IGroupsService groupsService, IZipService zipService,
        IRegExpService regExpService, INormalizeService normalizeService, IOlympediaService olympediaService, IDataCacheService dataCacheService,
        OlympicGamesRepositoryService<Participation> participationsService, OlympicGamesRepositoryService<Athlete> athletesService, OlympicGamesRepositoryService<Team> teamsService,
        OlympicGamesRepositoryService<Squad> squadsService)
        : base(logger, crawlersService, logsService, groupsService, zipService, regExpService, normalizeService, olympediaService, dataCacheService)
    {
        this.participationsService = participationsService;
        this.athletesService = athletesService;
        this.teamsService = teamsService;
        this.squadsService = squadsService;
    }

    protected override async Task ProcessGroupAsync(Group group)
    {
        try
        {
            var document = this.CreateHtmlDocument(group.Documents.Single(x => x.Order == 1));
            var originalEventName = document.DocumentNode.SelectSingleNode("//ol[@class='breadcrumb']/li[@class='active']").InnerText;
            var gameCache = this.GetGameFromDatabase(document);
            var disciplineCache = this.GetDisciplineFromDatabase(document);
            var eventModel = this.CreateEventModel(originalEventName, gameCache, disciplineCache);
            if (eventModel != null)
            {
                var eventCache = this.DataCacheService.Events
                    .FirstOrDefault(x => x.OriginalName == eventModel.OriginalName && x.GameId == eventModel.GameId && x.DisciplineId == eventModel.DisciplineId);
                if (eventCache != null)
                {
                    var trRows = document.DocumentNode.SelectSingleNode("//table[@class='table table-striped']").Elements("tr");
                    var indexes = this.GetHeaderIndexes(document);
                    if (eventCache.IsTeamEvent)
                    {
                        Team team = null;
                        foreach (var trRow in trRows)
                        {
                            var tdNodes = trRow.Elements("td").ToList();
                            var countryCode = this.OlympediaService.FindNOCCode(trRow.OuterHtml);
                            if (countryCode != null && !trRow.InnerHtml.Contains("coach", StringComparison.CurrentCultureIgnoreCase))
                            {
                                var nocCache = this.DataCacheService.NOCs.FirstOrDefault(x => x.Code == countryCode);
                                var medalTypeEnum = this.OlympediaService.FindMedal(trRow.OuterHtml);
                                var finishTypeEnum = this.OlympediaService.FindStatus(trRow.OuterHtml);
                                team = new Team
                                {
                                    NOCId = nocCache.Id,
                                    EventId = eventCache.Id,
                                    Name = tdNodes[indexes[ConverterConstants.INDEX_NAME]].InnerText.Trim(),
                                    MedalId = (int)medalTypeEnum,
                                    FinishTypeId = (int)finishTypeEnum
                                };

                                var dbTeam = await this.teamsService.GetAsync(x => x.Name == team.Name && x.EventId == team.EventId && x.NOCId == team.NOCId);
                                if (dbTeam != null)
                                {
                                    var equals = team.Equals(dbTeam);
                                    if (!equals)
                                    {
                                        this.teamsService.Update(dbTeam);
                                    }

                                    team = dbTeam;
                                }
                                else
                                {
                                    await this.teamsService.AddAsync(team);
                                }
                            }

                            if (trRow.InnerHtml.ToLower().Contains("coach"))
                            {
                                var athleteModel = this.OlympediaService.FindAthlete(trRow.OuterHtml);
                                if (athleteModel != null)
                                {
                                    var coach = await this.athletesService.GetAsync(x => x.Code == athleteModel.Code);
                                    if (coach != null)
                                    {
                                        team.CoachId = coach.Id;
                                        team = this.teamsService.Update(team);
                                    }
                                }
                            }
                            else
                            {
                                var athleteNumbers = this.OlympediaService.FindAthletes(trRow.OuterHtml);
                                var nocCode = this.DataCacheService.NOCs.FirstOrDefault(x => x.Id == team.NOCId);
                                foreach (var athleteModel in athleteNumbers)
                                {
                                    var participant = await this.CreateParticipantAsync(athleteModel.Code, nocCode.Code, (MedalTypeEnum)team.MedalId, (FinishTypeEnum)team.FinishTypeId, eventCache, gameCache);
                                    if (participant != null)
                                    {
                                        var dbSquad = await this.squadsService.GetAsync(x => x.ParticipationId == participant.Id && x.TeamId == team.Id);
                                        if (dbSquad == null)
                                        {
                                            await this.squadsService.AddAsync(new Squad { ParticipationId = participant.Id, TeamId = team.Id });
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        foreach (var trRow in trRows)
                        {
                            var athleteModel = this.OlympediaService.FindAthlete(trRow.OuterHtml);
                            var countryCode = this.OlympediaService.FindNOCCode(trRow.OuterHtml);
                            var medalType = this.OlympediaService.FindMedal(trRow.OuterHtml);
                            var finishStatus = this.OlympediaService.FindStatus(trRow.OuterHtml);
                            if (athleteModel != null && countryCode != null)
                            {
                                await this.CreateParticipantAsync(athleteModel.Code, countryCode, medalType, finishStatus, eventCache, gameCache);
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            this.Logger.LogError(ex, $"Failed to process group: {group.Identifier}");
        }
    }

    private async Task<Participation> CreateParticipantAsync(int athleteCode, string countryCode, MedalTypeEnum medalTypeEnum, FinishTypeEnum finishTypeEnum, EventCache eventCache, GameCache gameCache)
    {
        var athlete = await this.athletesService.GetAsync(x => x.Code == athleteCode);
        if (athlete != null)
        {
            var nocCacheModel = this.DataCacheService.NOCs.FirstOrDefault(c => c.Code == countryCode);
            if (nocCacheModel != null)
            {
                var participation = new Participation
                {
                    AthleteId = athlete.Id,
                    EventId = eventCache.Id,
                    NOCId = nocCacheModel.Id,
                    Code = athleteCode,
                    MedalId = (int)medalTypeEnum,
                    FinishTypeId = (int)finishTypeEnum,
                };

                if (athlete.BirthDate.HasValue)
                {
                    var age = this.CalculateAge(gameCache.OpeningDate ?? gameCache.StartCompetitionDate.Value, athlete.BirthDate.Value);
                    participation.AgeYears = age.Item1;
                    participation.AgeDays = age.Item2;
                }

                var dbParticipation = await this.participationsService.GetAsync(x => x.AthleteId == participation.AthleteId && x.EventId == participation.EventId && x.NOCId == participation.NOCId);
                if (dbParticipation != null)
                {
                    var equals = participation.Equals(dbParticipation);
                    if (!equals)
                    {
                        this.participationsService.Update(dbParticipation);
                    }
                }
                else
                {
                    dbParticipation = await this.participationsService.AddAsync(participation);
                }

                return dbParticipation;
            }
        }

        return null;
    }

    private Tuple<int?, int?> CalculateAge(DateTime? startDate, DateTime? endDate)
    {
        var age = new Tuple<int?, int?>(null, null);
        if (!startDate.HasValue || !endDate.HasValue)
        {
            return age;
        }

        var totalDays = (startDate.Value - endDate.Value).TotalDays;
        var year = (int)Math.Floor(totalDays / 365.25);
        var newYear = endDate.Value.AddYears(year);
        var days = (startDate.Value - newYear).TotalDays;
        age = new Tuple<int?, int?>(year, (int)days);

        return age;
    }
}