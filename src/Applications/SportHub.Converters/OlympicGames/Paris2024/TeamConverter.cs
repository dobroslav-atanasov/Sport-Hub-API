namespace SportHub.Converters.OlympicGames.Paris2024;

using System.Text.Json;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using SportHub.Converters.OlympicGames.Paris2024.Base;
using SportHub.Data.Entities.Crawlers;
using SportHub.Data.Entities.OlympicGames;
using SportHub.Data.Models.Crawlers.Paris2024.Individuals.Team;
using SportHub.Services.Data.CrawlerStorageDb.Interfaces;
using SportHub.Services.Data.OlympicGamesDb;
using SportHub.Services.Data.OlympicGamesDb.Interfaces;
using SportHub.Services.Interfaces;

public class TeamConverter : Paris2024Converter
{
    private readonly DataService<Team> teamsService;
    private readonly DataService<Participation> participationsService;

    public TeamConverter(ILogger<BaseConverter> logger, ICrawlersService crawlersService, ILogsService logsService, IGroupsService groupsService, IZipService zipService,
        INormalizeService normalizeService, IDataCacheService dataCacheService, DataService<Team> teamsService, DataService<Participation> participationsService)
        : base(logger, crawlersService, logsService, groupsService, zipService, normalizeService, dataCacheService)
    {
        this.teamsService = teamsService;
        this.participationsService = participationsService;
    }

    protected override async Task ProcessGroupAsync(Group group)
    {
        var converterModel = this.PrepareConverterModel(group);
        var model = JsonSerializer.Deserialize<TeamCrawlerModel>(converterModel.Documents.GetValueOrDefault(1).Json);
        var teamJson = model.Team;

        var dbTeam = await this.teamsService.GetAsync(x => x.OriginalCode == teamJson.Code);
        var @event = this.DataCacheService.Events.FirstOrDefault(x => x.Code.StartsWith(dbTeam.Code.Substring(0, 25)));

        //dbTeam
        foreach (var athlete in teamJson.Athletes)
        {
            var participation = await this.participationsService.GetAsync(x => x.Code == athlete.Person.Code && x.EventId == @event.Id);
            if (participation != null)
            {
                participation.TeamId = dbTeam.Id;
                this.participationsService.Update(participation);
            }
        }
    }
}