namespace SportHub.Converters.OlympicGames.Paris2024;

using System.Text.Json;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using SportHub.Converters.OlympicGames.Paris2024.Base;
using SportHub.Data.Entities.Crawlers;
using SportHub.Data.Entities.OlympicGames;
using SportHub.Data.Models.Crawlers.Paris2024.UnitGroup;
using SportHub.Services.Data.CrawlerStorageDb.Interfaces;
using SportHub.Services.Data.OlympicGamesDb;
using SportHub.Services.Data.OlympicGamesDb.Interfaces;
using SportHub.Services.Interfaces;

public class ScheduleConverter : Paris2024Converter
{
    private readonly DataService<Unit> unitsService;

    public ScheduleConverter(ILogger<BaseConverter> logger, ICrawlersService crawlersService, ILogsService logsService, IGroupsService groupsService, IZipService zipService,
        INormalizeService normalizeService, IDataCacheService dataCacheService, DataService<Unit> unitsService)
        : base(logger, crawlersService, logsService, groupsService, zipService, normalizeService, dataCacheService)
    {
        this.unitsService = unitsService;
    }

    protected override async Task ProcessGroupAsync(Group group)
    {
        var converterModel = this.PrepareConverterModel(group);
        var json = JsonSerializer.Deserialize<UnitGroupCrawlerModel>(converterModel.Documents.GetValueOrDefault(1).Json);

        foreach (var item in json.Units)
        {
            var code = this.GenerateCode("Summer", 2024, item.Id);
            var dbUnit = await this.unitsService.GetAsync(x => x.Code == code);

            if (dbUnit != null)
            {
                dbUnit.Number = string.IsNullOrEmpty(item.UnitNum) ? null : item.UnitNum;
                dbUnit.LocationCode = item.Location;
                dbUnit.LocationName = item.LocationDescription;
                dbUnit.VenueCode = item.Venue;
                dbUnit.VenueName = item.VenueDescription;
                dbUnit.OlympicDay = DateTime.ParseExact(item.OlympicDay, "yyyy-MM-dd", null);
                dbUnit.Session = item.SessionCode;
                dbUnit.Group = item.GroupId;

                this.unitsService.Update(dbUnit);
            }
        }
    }
}