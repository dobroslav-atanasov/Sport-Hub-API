namespace SportHub.Converters.OlympicGames.Paris2024;

using System.Text.Json;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using SportHub.Converters.OlympicGames.Paris2024.Base;
using SportHub.Data.Entities.Crawlers;
using SportHub.Data.Models.Crawlers.Paris2024;
using SportHub.Services.Data.CrawlerStorageDb.Interfaces;
using SportHub.Services.Data.OlympicGamesDb.Interfaces;
using SportHub.Services.Interfaces;

public class ResultConverter : Paris2024Converter
{
    public ResultConverter(ILogger<BaseConverter> logger, ICrawlersService crawlersService, ILogsService logsService, IGroupsService groupsService, IZipService zipService,
        INormalizeService normalizeService, IDataCacheService dataCacheService)
        : base(logger, crawlersService, logsService, groupsService, zipService, normalizeService, dataCacheService)
    {
    }

    protected override async Task ProcessGroupAsync(Group group)
    {
        var converterModel = this.PrepareConverterModel(group);
        var json = JsonSerializer.Deserialize<ResultCrawlerModel>(converterModel.Documents.GetValueOrDefault(1).Json);

        var list = File.ReadAllLines("aresult.txt").ToList();
        if (json.Result == null)
        {
            list.Add(group.Documents.FirstOrDefault().Url);
        }
        else
        {
            list.Add(json.Result.EventUnitCode);
        }
        File.WriteAllLines("aresult.txt", list);
    }
}