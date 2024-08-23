namespace SportHub.Converters.OlympicGames.Paris2024;

using System.Text.Json;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using SportHub.Converters.OlympicGames.Paris2024.Base;
using SportHub.Data.Entities.Crawlers;
using SportHub.Data.Entities.OlympicGames;
using SportHub.Services.Data.CrawlerStorageDb.Interfaces;
using SportHub.Services.Data.OlympicGamesDb;
using SportHub.Services.Data.OlympicGamesDb.Interfaces;
using SportHub.Services.Interfaces;

public class NOCsConverter : Paris2024Converter
{
    private readonly DataService<NOC> nocsService;

    public NOCsConverter(ILogger<BaseConverter> logger, ICrawlersService crawlersService, ILogsService logsService, IGroupsService groupsService, IZipService zipService,
        INormalizeService normalizeService, IDataCacheService dataCacheService, DataService<NOC> nocsService)
        : base(logger, crawlersService, logsService, groupsService, zipService, normalizeService, dataCacheService)
    {
        this.nocsService = nocsService;
    }

    protected override async Task ProcessGroupAsync(Group group)
    {
        var converterModel = this.PrepareConverterModel(group);
        var json = JsonSerializer.Deserialize<Data.Models.Crawlers.Paris2024.NOCs.NOCList>(converterModel.Documents.GetValueOrDefault(1).Json);

        foreach (var nocItem in json.NOCs)
        {
            var noc = new NOC
            {
                Code = nocItem.Code,
                Name = nocItem.Description,
                OfficialName = nocItem.LongDescription,
                SEOName = nocItem.Seodescription,
                IsMedal = nocItem.Medal == "Y",
                IsHistoric = nocItem.Note == "H",
            };

            var dbNOC = await this.nocsService.GetAsync(x => x.Code == noc.Code);
            if (dbNOC != null)
            {
                var equals = noc.Equals(dbNOC);
                if (!equals)
                {
                    this.nocsService.Update(dbNOC);
                }
            }
            else
            {
                await this.nocsService.AddAsync(noc);
            }
        }
    }
}