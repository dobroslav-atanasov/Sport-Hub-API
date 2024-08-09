namespace SportHub.Converters.OlympicGames.Paris2024;

using System.Text.Json;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using SportHub.Data.Models.Crawlers.Paris2024;
using SportHub.Data.Models.Crawlers.Paris2024.Events;
using SportHub.Data.Models.DbEntities.Crawlers;
using SportHub.Data.Models.DbEntities.OlympicGames;
using SportHub.Services.Data.CrawlerStorageDb.Interfaces;
using SportHub.Services.Data.OlympicGamesDb.Interfaces;
using SportHub.Services.Interfaces;

public class UnitsConverter : BaseConverter
{
    public UnitsConverter(ILogger<BaseConverter> logger, ICrawlersService crawlersService, ILogsService logsService, IGroupsService groupsService, IZipService zipService,
        INormalizeService normalizeService, IDataCacheService dataCacheService)
        : base(logger, crawlersService, logsService, groupsService, zipService, normalizeService, dataCacheService)
    {
    }

    protected override async Task ProcessGroupAsync(Group group)
    {
        var eventUnitsJson = JsonSerializer.Deserialize<EventUnitList>(this.Model.Paris2024Documents.GetValueOrDefault(5).Json);
        var unitsJson = JsonSerializer.Deserialize<UnitGroupList>(this.Model.Paris2024Documents.GetValueOrDefault(1).Json);

        foreach (var item in unitsJson.Units)
        {
            var eventUnit = eventUnitsJson.EventUnits.FirstOrDefault(x => x.Code == item.Id);

            var unit = new Unit
            {
                OriginalCode = item.Id,
                Name = eventUnit.Description,
                LongName = eventUnit.LongDescription,
                ShortName = eventUnit.ShortDescription,
                SEOName = eventUnit.Seodescription,
            };

            await Console.Out.WriteLineAsync(unit.OriginalCode);
        }
    }
}