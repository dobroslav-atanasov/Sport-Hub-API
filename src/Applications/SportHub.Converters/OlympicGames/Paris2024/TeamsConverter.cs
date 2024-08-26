namespace SportHub.Converters.OlympicGames.Paris2024;

using System.Text.Json;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using SportHub.Converters.OlympicGames.Paris2024.Base;
using SportHub.Data.Entities.Crawlers;
using SportHub.Data.Entities.OlympicGames;
using SportHub.Data.Models.Crawlers.Paris2024.Teams;
using SportHub.Services.Data.CrawlerStorageDb.Interfaces;
using SportHub.Services.Data.OlympicGamesDb;
using SportHub.Services.Data.OlympicGamesDb.Interfaces;
using SportHub.Services.Interfaces;

public class TeamsConverter : Paris2024Converter
{
    private readonly DataService<Team> teamsService;

    public TeamsConverter(ILogger<BaseConverter> logger, ICrawlersService crawlersService, ILogsService logsService, IGroupsService groupsService, IZipService zipService,
        INormalizeService normalizeService, IDataCacheService dataCacheService, DataService<Team> teamsService)
        : base(logger, crawlersService, logsService, groupsService, zipService, normalizeService, dataCacheService)
    {
        this.teamsService = teamsService;
    }

    protected override async Task ProcessGroupAsync(Group group)
    {
        var converterModel = this.PrepareConverterModel(group);
        var model = JsonSerializer.Deserialize<TeamsCrawlerModel>(converterModel.Documents.GetValueOrDefault(1).Json);

        var teams = new List<Team>();
        foreach (var item in model.Teams.Where(x => x.Current))
        {
            // TODO Generate team code.

            var team = new Team
            {
                Code = "1",
                OriginalCode = item.Code,
                Name = item.Name,
                ShortName = item.ShortName,
                TvName = item.Name,
                Gender = item.TeamGender,
                Type = item.TeamType,
                Organisation = item.Organisation.Code
            };


            teams.Add(team);
        }

        await this.teamsService.AddRangeAsync(teams);
    }
}