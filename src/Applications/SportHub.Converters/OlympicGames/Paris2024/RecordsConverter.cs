namespace SportHub.Converters.OlympicGames.Paris2024;

using System.Text.Json;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using SportHub.Converters.OlympicGames.Paris2024.Base;
using SportHub.Data.Entities.Crawlers;
using SportHub.Data.Entities.OlympicGames;
using SportHub.Data.Models.Crawlers.Paris2024.Records;
using SportHub.Services.Data.CrawlerStorageDb.Interfaces;
using SportHub.Services.Data.OlympicGamesDb.Interfaces;
using SportHub.Services.Interfaces;

public class RecordsConverter : Paris2024Converter
{
    public RecordsConverter(ILogger<BaseConverter> logger, ICrawlersService crawlersService, ILogsService logsService, IGroupsService groupsService, IZipService zipService,
        INormalizeService normalizeService, IDataCacheService dataCacheService)
        : base(logger, crawlersService, logsService, groupsService, zipService, normalizeService, dataCacheService)
    {
    }

    protected override async Task ProcessGroupAsync(Group group)
    {
        var converterModel = this.PrepareConverterModel(group);
        var json = JsonSerializer.Deserialize<RecordsCrawlerModel>(converterModel.Documents.GetValueOrDefault(6).Json);

        if (json.Records != null)
        {
            foreach (var item in json.Records)
            {
                var code = this.GenerateCode("Summer", 2024, item.RecordCode);
                if (item.RecordData.Count > 1)
                {
                    ;
                }
                var record = new Record
                {
                    Code = item.RecordId,
                    NotEstablished = item.NotEstablished,
                    Order = item.Order,
                    Name = item.RecordType.Description,
                    Type = item.RecordType.Code,
                    Description = item.Record.Description,
                };
            }
        }
    }
}