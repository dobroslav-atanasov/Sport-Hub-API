namespace SportHub.Crawlers.OlympicGames.Paris2024;

using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using SportHub.Common.Constants;
using SportHub.Data.Models.Crawlers.Paris2024.Teams;
using SportHub.Services.Data.CrawlerStorageDb.Interfaces;
using SportHub.Services.Interfaces;

public class TeamCrawler : BaseCrawler
{
    public TeamCrawler(ILogger<BaseCrawler> logger, IConfiguration configuration, IHttpService httpService, ICrawlersService crawlersService, IGroupsService groupsService)
        : base(logger, configuration, httpService, crawlersService, groupsService)
    {
    }

    public override async Task StartAsync()
    {
        this.Logger.LogInformation($"{this.GetType().FullName} Start!");

        try
        {
            var httpModel = await this.HttpService.GetAsync(this.Configuration.GetSection(CrawlerConstants.PARIS_2024_TEAMS_URL).Value);
            var json = Encoding.UTF8.GetString(httpModel.Bytes);

            var model = JsonSerializer.Deserialize<TeamsList>(json);

            var count = 0;
            foreach (var team in model.Teams)
            {
                await Console.Out.WriteLineAsync($"{count++}");
                var url = $"{this.Configuration.GetSection(CrawlerConstants.PARIS_2024_TEAM_URL).Value}{team.Code}.json";

                try
                {
                    var teamHttpModel = await this.HttpService.GetAsync(url);
                    await this.ProcessGroupAsync(teamHttpModel);
                }
                catch (Exception ex)
                {
                    this.Logger.LogError(ex, $"Failed to process url: {url};");
                }

            }
        }
        catch (Exception ex)
        {
            this.Logger.LogError(ex, $"Failed to process url: {this.Configuration.GetSection(CrawlerConstants.PARIS_2024_TEAMS_URL).Value};");
        }

        this.Logger.LogInformation($"{this.GetType().FullName} End!");
    }
}