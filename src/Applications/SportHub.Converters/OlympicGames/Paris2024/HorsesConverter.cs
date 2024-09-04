namespace SportHub.Converters.OlympicGames.Paris2024;

using System.Text.Json;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using SportHub.Converters.OlympicGames.Paris2024.Base;
using SportHub.Data.Entities.Crawlers;
using SportHub.Data.Entities.OlympicGames;
using SportHub.Data.Models.Crawlers.Paris2024.Horses;
using SportHub.Services.Data.CrawlerStorageDb.Interfaces;
using SportHub.Services.Data.OlympicGamesDb;
using SportHub.Services.Data.OlympicGamesDb.Interfaces;
using SportHub.Services.Interfaces;

public class HorsesConverter : Paris2024Converter
{
    private readonly DataService<Horse> horsesService;

    public HorsesConverter(ILogger<BaseConverter> logger, ICrawlersService crawlersService, ILogsService logsService, IGroupsService groupsService, IZipService zipService,
        INormalizeService normalizeService, IDataCacheService dataCacheService, DataService<Horse> horsesService)
        : base(logger, crawlersService, logsService, groupsService, zipService, normalizeService, dataCacheService)
    {
        this.horsesService = horsesService;
    }

    protected override async Task ProcessGroupAsync(Group group)
    {
        var converterModel = this.PrepareConverterModel(group);
        var model = JsonSerializer.Deserialize<HorsesCrawlerModel>(converterModel.Documents.GetValueOrDefault(1).Json);

        var horses = new List<Horse>();
        foreach (var item in model.Horses)
        {
            var horse = new Horse
            {
                Code = "H",
                OriginalCode = item.Code,
                Current = item.Current,
                Discipline = item.Discipline?.Code,
                Groom = item.Groom,
                Name = item.Name,
                Organisation = item.Organisation?.Code,
                Owner = item.Owner,
                Passport = item.Passport,
                SecondOwner = item.SecondOwner,
                ShortName = item.ShortName,
                Sire = item.Sire,
                YearBirth = string.IsNullOrEmpty(item.YearBirth) ? 0 : int.Parse(item.YearBirth)
            };

            horses.Add(horse);
        }

        await this.horsesService.AddRangeAsync(horses);
    }
}