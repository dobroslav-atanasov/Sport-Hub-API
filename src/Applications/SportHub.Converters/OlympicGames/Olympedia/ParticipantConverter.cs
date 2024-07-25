namespace SportHub.Converters.OlympicGames.Olympedia;

using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using SportHub.Common.Constants;
using SportHub.Data.Models.Cache;
using SportHub.Data.Models.DbEntities.Crawlers;
using SportHub.Data.Models.DbEntities.OlympicGames;
using SportHub.Data.Models.Enumerations.OlympicGames;
using SportHub.Services.Data.CrawlerStorageDb.Interfaces;
using SportHub.Services.Data.OlympicGamesDb;
using SportHub.Services.Data.OlympicGamesDb.Interfaces;
using SportHub.Services.Interfaces;

public class ParticipantConverter : BaseOlympediaConverter
{
    private readonly DataService<Participation> participationsService;
    private readonly DataService<Athlete> athletesService;
    private readonly DataService<Team> teamsService;

    public ParticipantConverter(ILogger<BaseConverter> logger, ICrawlersService crawlersService, ILogsService logsService, IGroupsService groupsService, IZipService zipService,
        IRegExpService regExpService, INormalizeService normalizeService, IOlympediaService olympediaService, IDataCacheService dataCacheService,
        DataService<Participation> participationsService, DataService<Athlete> athletesService, DataService<Team> teamsService)
        : base(logger, crawlersService, logsService, groupsService, zipService, regExpService, normalizeService, olympediaService, dataCacheService)
    {
        this.participationsService = participationsService;
        this.athletesService = athletesService;
        this.teamsService = teamsService;
    }

    protected override async Task ProcessGroupAsync(Group group)
    {
        var model = this.PrepareConverterModel(group);
        if (model.IsValidEvent && model.EventCache != null)
        {
            var trRows = model.HtmlDocument.DocumentNode.SelectSingleNode("//table[@class='table table-striped']").Elements("tr");
            var indexes = this.GetHeaderIndexes(model.HtmlDocument);
            if (model.EventCache.IsTeamEvent)
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
                        var finishStatus = this.OlympediaService.FindFinishStatus(trRow.OuterHtml);
                        team = new Team
                        {
                            NOCId = nocCache.Id,
                            EventId = model.EventCache.Id,
                            Name = tdNodes[indexes[ConverterConstants.INDEX_NAME]].InnerText.Trim(),
                            Medal = medalTypeEnum,
                            FinishStatus = finishStatus
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
                            var participant = await this.CreateParticipantAsync(athleteModel.Code, nocCode.Code, team.Medal, team.FinishStatus, model.EventCache, model.GameCache);
                            if (participant != null)
                            {
                                team.Participations.Add(participant);
                            }
                        }

                        if (athleteNumbers.Count > 0)
                        {
                            this.teamsService.Update(team);
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
                    var finishStatus = this.OlympediaService.FindFinishStatus(trRow.OuterHtml);
                    if (athleteModel != null && countryCode != null)
                    {
                        await this.CreateParticipantAsync(athleteModel.Code, countryCode, medalType, finishStatus, model.EventCache, model.GameCache);
                    }
                }
            }
        }
    }

    private async Task<Participation> CreateParticipantAsync(int athleteCode, string countryCode, MedalEnum medal, FinishStatusEnum finishStatus, EventCache eventCache, GameCache gameCache)
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
                    Medal = medal,
                    FinishStatus = finishStatus,
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