namespace SportHub.Converters.OlympicGames.Paris2024;

using System.Text.Json;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using SportHub.Converters.OlympicGames.Paris2024.Base;
using SportHub.Data.Entities.Crawlers;
using SportHub.Data.Entities.OlympicGames;
using SportHub.Data.Models.Crawlers.Paris2024.Rankings;
using SportHub.Services.Data.CrawlerStorageDb.Interfaces;
using SportHub.Services.Data.OlympicGamesDb;
using SportHub.Services.Data.OlympicGamesDb.Interfaces;
using SportHub.Services.Interfaces;

public class RankingsConverter : Paris2024Converter
{
    private readonly DataService<Participation> participantsService;
    private readonly DataService<Team> teamsService;

    public RankingsConverter(ILogger<BaseConverter> logger, ICrawlersService crawlersService, ILogsService logsService, IGroupsService groupsService, IZipService zipService,
        INormalizeService normalizeService, IDataCacheService dataCacheService, DataService<Participation> participantsService, DataService<Team> teamsService)
        : base(logger, crawlersService, logsService, groupsService, zipService, normalizeService, dataCacheService)
    {
        this.participantsService = participantsService;
        this.teamsService = teamsService;
    }

    protected override async Task ProcessGroupAsync(Group group)
    {
        var converterModel = this.PrepareConverterModel(group);
        var json = JsonSerializer.Deserialize<RankingsCrawlerModel>(converterModel.Documents.GetValueOrDefault(2).Json);
        var rankings = json.Event.Rankings;

        foreach (var ranking in rankings)
        {
            var code = this.GenerateCode("Summer", 2024, ranking.EventCode);
            var @event = this.DataCacheService.Events.FirstOrDefault(x => x.Code == code);
            if (@event != null)
            {
                if (ranking.Participant.Typename == "Person")
                {
                    var participation = await this.participantsService.GetAsync(x => x.Code == ranking.AthleteCode && x.EventId == @event.Id);
                }
                else
                {
                    var team = await this.teamsService.GetAsync(x => x.OriginalCode == ranking.TeamCode);
                }
            }
        }
    }
}