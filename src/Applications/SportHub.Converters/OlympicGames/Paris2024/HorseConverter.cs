namespace SportHub.Converters.OlympicGames.Paris2024;

using System.Text.Json;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using SportHub.Converters.OlympicGames.Paris2024.Base;
using SportHub.Data.Entities.Crawlers;
using SportHub.Data.Entities.OlympicGames;
using SportHub.Data.Entities.OlympicGames.Meta;
using SportHub.Data.Models.Crawlers.Paris2024.Horse;
using SportHub.Services.Data.CrawlerStorageDb.Interfaces;
using SportHub.Services.Data.OlympicGamesDb;
using SportHub.Services.Data.OlympicGamesDb.Interfaces;
using SportHub.Services.Interfaces;

public class HorseConverter : Paris2024Converter
{
    private readonly DataService<Horse> horsesService;

    public HorseConverter(ILogger<BaseConverter> logger, ICrawlersService crawlersService, ILogsService logsService, IGroupsService groupsService, IZipService zipService,
        INormalizeService normalizeService, IDataCacheService dataCacheService, DataService<Horse> horsesService)
        : base(logger, crawlersService, logsService, groupsService, zipService, normalizeService, dataCacheService)
    {
        this.horsesService = horsesService;
    }

    protected override async Task ProcessGroupAsync(Group group)
    {
        var converterModel = this.PrepareConverterModel(group);
        var model = JsonSerializer.Deserialize<HorseCrawlerModel>(converterModel.Documents.GetValueOrDefault(1).Json);
        var horseJson = model.Horse;

        var dbHorse = await this.horsesService.GetAsync(x => x.OriginalCode == horseJson.Code);
        dbHorse.Colour = horseJson.Colour?.Description;
        dbHorse.Breed = horseJson.Breed?.Description;
        dbHorse.BreedCode = horseJson.Breed?.Code;
        dbHorse.Gender = horseJson.Sex?.Description;
        dbHorse.GenderCode = horseJson.Sex?.Code;
        dbHorse.Info = horseJson.HorseBio?.Dam;
        dbHorse.FormerRider = horseJson.HorseBio?.FormerRider;
        dbHorse.CountryOfBirth = horseJson.HorseBio?.CountryofBirth;
        dbHorse.Achievements = horseJson.HorseBio?.Interest?.MajorAchievements;
        dbHorse.Meta = this.PrepareRecordMeta(horseJson.Entries);

        this.horsesService.Update(dbHorse);
    }

    private string PrepareRecordMeta(List<Entry> entries)
    {
        if (entries == null || entries.Count == 0)
        {
            return null;
        }

        var meta = new HorseMeta();

        entries.ForEach(x =>
        {
            var entry = new HorseDataMeta
            {
                Key = x.Code,
                Value = x.Value,
                Type = x.Type,
                Position = x.Pos,
            };

            meta.Data.Add(entry);
        });

        var json = JsonSerializer.Serialize(meta);
        return json;
    }
}