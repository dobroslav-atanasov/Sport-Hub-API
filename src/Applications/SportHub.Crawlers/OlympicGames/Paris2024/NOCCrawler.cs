namespace SportHub.Crawlers.OlympicGames.Paris2024;

using System.Text.Json;
using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using SportHub.Common.Constants;
using SportHub.Data.Models.Crawlers.Paris2024.NOCs;
using SportHub.Services.Data.CrawlerStorageDb.Interfaces;
using SportHub.Services.Interfaces;

public class NOCCrawler : BaseCrawler
{
    public NOCCrawler(ILogger<BaseCrawler> logger, IConfiguration configuration, IHttpService httpService, ICrawlersService crawlersService, IGroupsService groupsService)
        : base(logger, configuration, httpService, crawlersService, groupsService)
    {
    }

    public override async Task StartAsync()
    {
        this.Logger.LogInformation($"{this.GetType().FullName} Start!");

        try
        {
            var httpModel = await this.HttpService.GetAsync(this.Configuration.GetSection(CrawlerConstants.PARIS_2024_NOCS_URL).Value);
            var model = JsonSerializer.Deserialize<NOCList>(httpModel.Content);

            foreach (var noc in model.NOCs)
            {
                var url = $"{this.Configuration.GetSection(CrawlerConstants.PARIS_2024_NOC_URL).Value}{noc.Code}.json";

                try
                {
                    var nocHttpModel = await this.HttpService.GetAsync(url);
                    await this.ProcessGroupAsync(nocHttpModel);
                }
                catch (Exception ex)
                {
                    this.Logger.LogError(ex, $"Failed to process url: {url};");
                }
            }

        }
        catch (Exception ex)
        {
            this.Logger.LogError(ex, $"Failed to process url: {this.Configuration.GetSection(CrawlerConstants.PARIS_2024_NOCS_URL).Value};");
        }

        this.Logger.LogInformation($"{this.GetType().FullName} End!");
    }
}