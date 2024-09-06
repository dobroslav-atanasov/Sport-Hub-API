namespace SportHub.Converters.OlympicGames.Paris2024;

using System.Text.Json;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using SportHub.Converters.OlympicGames.Paris2024.Base;
using SportHub.Data.Entities.Crawlers;
using SportHub.Data.Entities.OlympicGames;
using SportHub.Data.Models.Crawlers.Paris2024.Team;
using SportHub.Services.Data.CrawlerStorageDb.Interfaces;
using SportHub.Services.Data.OlympicGamesDb;
using SportHub.Services.Data.OlympicGamesDb.Interfaces;
using SportHub.Services.Interfaces;

public class TeamConverter : Paris2024Converter
{
    private readonly DataService<Team> teamsService;
    private readonly DataService<Participation> participationsService;
    private readonly DataService<Person> personsService;
    private readonly DataService<Coach> coachesService;

    public TeamConverter(ILogger<BaseConverter> logger, ICrawlersService crawlersService, ILogsService logsService, IGroupsService groupsService, IZipService zipService,
        INormalizeService normalizeService, IDataCacheService dataCacheService, DataService<Team> teamsService, DataService<Participation> participationsService,
        DataService<Person> personsService, DataService<Coach> coachesService)
        : base(logger, crawlersService, logsService, groupsService, zipService, normalizeService, dataCacheService)
    {
        this.teamsService = teamsService;
        this.participationsService = participationsService;
        this.personsService = personsService;
        this.coachesService = coachesService;
    }

    protected override async Task ProcessGroupAsync(Group group)
    {
        var converterModel = this.PrepareConverterModel(group);
        var model = JsonSerializer.Deserialize<TeamCrawlerModel>(converterModel.Documents.GetValueOrDefault(1).Json);
        var teamJson = model.Team;

        var dbTeam = await this.teamsService.GetAsync(x => x.OriginalCode == teamJson.Code);
        if (dbTeam != null)
        {
            var @event = this.DataCacheService.Events.FirstOrDefault(x => x.Code.StartsWith(dbTeam.Code.Substring(0, 25)));

            if (teamJson.Medals != null && teamJson.Medals.Any())
            {
                dbTeam.Medal = int.Parse(teamJson.Medals.FirstOrDefault().MedalType);
            }

            dbTeam.Highlights = teamJson.TeamBio?.Highlights?.FirstOrDefault().Value;
            dbTeam.HighlightsType = teamJson.TeamBio?.Highlights?.FirstOrDefault().Type;
            dbTeam.AddInformation = teamJson.TeamBio?.Interest?.AddInformation;

            if (teamJson.Athletes != null)
            {
                foreach (var athlete in teamJson.Athletes)
                {
                    var participation = await this.participationsService.GetAsync(x => x.Code == athlete.Person.Code && x.EventId == @event.Id);
                    if (participation != null)
                    {
                        participation.TeamId = dbTeam.Id;
                        participation.Order = athlete.Order;
                        this.participationsService.Update(participation);
                    }
                }
            }

            if (teamJson.TeamCoaches != null)
            {
                foreach (var item in teamJson.TeamCoaches)
                {
                    var person = await this.personsService.GetAsync(x => x.Code == item.Coach.Code);
                    if (person != null)
                    {
                        var coach = new Coach
                        {
                            TeamId = dbTeam.Id,
                            Order = item.Order,
                            CoachId = person.Id
                        };

                        var exist = await this.coachesService.AnyAsync(x => x.TeamId == coach.TeamId && x.CoachId == coach.CoachId);
                        if (!exist)
                        {
                            await this.coachesService.AddAsync(coach);
                        }
                    }
                }
            }

            this.teamsService.Update(dbTeam);
        }
    }
}