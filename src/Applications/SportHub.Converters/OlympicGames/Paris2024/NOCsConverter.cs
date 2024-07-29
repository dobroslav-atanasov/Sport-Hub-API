namespace SportHub.Converters.OlympicGames.Paris2024;

using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using SportHub.Data.Models.DbEntities.Crawlers;
using SportHub.Data.Models.DbEntities.OlympicGames;
using SportHub.Data.Repositories;
using SportHub.Services.Data.CrawlerStorageDb.Interfaces;
using SportHub.Services.Interfaces;

public class NOCsConverter : BaseConverter
{
    private readonly OlympicGamesRepository<NOC> repository;

    public NOCsConverter(ILogger<BaseConverter> logger, ICrawlersService crawlersService, ILogsService logsService, IGroupsService groupsService, IZipService zipService,
        OlympicGamesRepository<NOC> repository)
        : base(logger, crawlersService, logsService, groupsService, zipService)
    {
        this.repository = repository;
    }

    protected override async Task ProcessGroupAsync(Group group)
    {
        var document = group.Documents.FirstOrDefault();
        var bytes = document.Content;
        var encoding = Encoding.GetEncoding(document.Encoding);
        var json = encoding.GetString(bytes);
        var model = JsonSerializer.Deserialize<Data.Models.Crawlers.Paris2024.NOCs.NOCList>(json);

        foreach (var nocItem in model.NOCs)
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

            var dbNOC = await this.repository.GetAsync(x => x.Code == noc.Code);
            if (dbNOC != null)
            {
                var equals = noc.Equals(dbNOC);
                if (!equals)
                {
                    this.repository.Update(dbNOC);
                    await this.repository.SaveChangesAsync();
                }
            }
            else
            {
                await this.repository.AddAsync(noc);
                await this.repository.SaveChangesAsync();
            }
        }
    }
}