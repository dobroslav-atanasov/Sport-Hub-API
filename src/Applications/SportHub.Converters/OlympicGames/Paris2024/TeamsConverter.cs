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
    private readonly DataService<Participation> participationsService;
    private readonly DataService<Person> personsService;

    public TeamsConverter(ILogger<BaseConverter> logger, ICrawlersService crawlersService, ILogsService logsService, IGroupsService groupsService, IZipService zipService,
        INormalizeService normalizeService, IDataCacheService dataCacheService, DataService<Team> teamsService, DataService<Participation> participationsService,
        DataService<Person> personsService)
        : base(logger, crawlersService, logsService, groupsService, zipService, normalizeService, dataCacheService)
    {
        this.teamsService = teamsService;
        this.participationsService = participationsService;
        this.personsService = personsService;
    }

    protected override async Task ProcessGroupAsync(Group group)
    {
        var converterModel = this.PrepareConverterModel(group);
        var model = JsonSerializer.Deserialize<TeamsCrawlerModel>(converterModel.Documents.GetValueOrDefault(1).Json);

        var teams = new List<Team>();
        foreach (var item in model.Teams.Where(x => x.Current))
        {
            var code = this.GenerateCode("Summer", 2024, item.Code, true);

            var team = new Team
            {
                Code = code,
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