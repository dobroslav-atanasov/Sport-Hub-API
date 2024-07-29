namespace SportHub.Converters.OlympicGames.Olympedia;

using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using SportHub.Data.Models.DbEntities.Crawlers;
using SportHub.Data.Models.DbEntities.OlympicGames;
using SportHub.Data.Repositories;
using SportHub.Services.Data.CrawlerStorageDb.Interfaces;
using SportHub.Services.Data.OlympicGamesDb.Interfaces;
using SportHub.Services.Interfaces;

public class NOCConverter : BaseOlympediaConverter
{
    private readonly OlympicGamesRepository<NOC> repository;

    public NOCConverter(ILogger<BaseConverter> logger, ICrawlersService crawlersService, ILogsService logsService, IGroupsService groupsService, IZipService zipService,
        IRegExpService regExpService, INormalizeService normalizeService, IOlympediaService olympediaService, IDataCacheService dataCacheService,
        OlympicGamesRepository<NOC> repository)
        : base(logger, crawlersService, logsService, groupsService, zipService, regExpService, normalizeService, olympediaService, dataCacheService)
    {
        this.repository = repository;
    }

    protected override async Task ProcessGroupAsync(Group group)
    {
        var models = this.PrepareConverterModels(group);
        var country = models.ElementAtOrDefault(0);

        var match = this.RegExpService.Match(country.Title, @"(.*?)\((.*?)\)");
        if (match != null)
        {
            var name = match.Groups[1].Value.Trim();
            var code = match.Groups[2].Value.Trim().ToUpper();

            if (code is not "UNK" and not "CRT")
            {
                var dbNOC = await this.repository.GetAsync(x => x.Code == code);
                if (dbNOC == null)
                {
                    ;
                }
                else
                {

                }
            }
        }
        ;
    }
}