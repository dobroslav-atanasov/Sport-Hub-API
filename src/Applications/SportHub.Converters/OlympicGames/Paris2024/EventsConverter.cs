namespace SportHub.Converters.OlympicGames.Paris2024;

using System.Text.Json;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using SportHub.Data.Models.Crawlers.Paris2024.Disciplines;
using SportHub.Data.Models.DbEntities.Crawlers;
using SportHub.Services.Data.CrawlerStorageDb.Interfaces;
using SportHub.Services.Interfaces;

public class EventsConverter : BaseConverter
{
    public EventsConverter(ILogger<BaseConverter> logger, ICrawlersService crawlersService, ILogsService logsService, IGroupsService groupsService, IZipService zipService,
        INormalizeService normalizeService, IRegExpService regExpService)
        : base(logger, crawlersService, logsService, groupsService, zipService, normalizeService, regExpService)
    {
    }

    protected override async Task ProcessGroupAsync(Group group)
    {
        var json = JsonSerializer.Deserialize<EventList>(this.Model.Paris2024Documents.GetValueOrDefault(4).Json);
        ;
    }
}