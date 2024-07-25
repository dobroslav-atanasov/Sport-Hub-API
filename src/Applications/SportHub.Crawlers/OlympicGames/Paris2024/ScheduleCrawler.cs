namespace SportHub.Crawlers.OlympicGames.Paris2024;

using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using SportHub.Common.Constants;
using SportHub.Services.Data.CrawlerStorageDb.Interfaces;
using SportHub.Services.Interfaces;

public class ScheduleCrawler : BaseCrawler
{
    public ScheduleCrawler(ILogger<BaseCrawler> logger, IConfiguration configuration, IHttpService httpService, ICrawlersService crawlersService, IGroupsService groupsService)
        : base(logger, configuration, httpService, crawlersService, groupsService)
    {
    }

    public override async Task StartAsync()
    {
        this.Logger.LogInformation($"{this.GetType().FullName} Start!");

        var days = new List<string>();

        var start = DateTime.ParseExact("24.07.2024", "dd.MM.yyyy", null);
        var end = DateTime.ParseExact("11.08.2024", "dd.MM.yyyy", null);
        for (var counter = start; counter <= end; counter = counter.AddDays(1))
        {
            days.Add($"{counter.Year}-{counter.Month:D2}-{counter.Day:D2}");
        }

        try
        {
            foreach (var day in days)
            {
                var url = $"{this.Configuration.GetSection(CrawlerConstants.PARIS_2024_SCHEDULE_URL).Value}{day}";
                try
                {
                    var httpModel = await this.HttpService.GetAsync(url);
                    await this.ProcessGroupAsync(httpModel);
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