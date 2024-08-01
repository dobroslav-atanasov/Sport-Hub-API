namespace SportHub.Crawlers.OlympicGames.Paris2024;

using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using SportHub.Common.Constants;
using SportHub.Data.Models.Crawlers.Paris2024.Disciplines;
using SportHub.Services.Data.CrawlerStorageDb.Interfaces;
using SportHub.Services.Interfaces;

public class ResultCrawler : BaseCrawler
{
    private readonly IRegExpService regExpService;

    public ResultCrawler(ILogger<BaseCrawler> logger, IConfiguration configuration, IHttpService httpService, ICrawlersService crawlersService, IGroupsService groupsService,
        IRegExpService regExpService)
        : base(logger, configuration, httpService, crawlersService, groupsService)
    {
        this.regExpService = regExpService;
    }

    public override async Task StartAsync()
    {
        this.Logger.LogInformation($"{this.GetType().FullName} Start!");

        var days = new List<string>();

        var start = DateTime.ParseExact("24.07.2024", "dd.MM.yyyy", null);
        var end = DateTime.Now.AddDays(-1);
        for (var counter = start; counter <= end; counter = counter.AddDays(1))
        {
            days.Add($"{counter.Year}-{counter.Month:D2}-{counter.Day:D2}");
        }
        var count = 0;
        try
        {
            foreach (var day in days)
            {
                var url = $"{this.Configuration.GetSection(CrawlerConstants.PARIS_2024_SCHEDULE_URL).Value}{day}";
                try
                {
                    var httpModel = await this.HttpService.GetAsync(url);
                    var json = Encoding.UTF8.GetString(httpModel.Bytes);
                    var model = JsonSerializer.Deserialize<DisciplineSchedule>(json);

                    foreach (var unit in model.Units)
                    {
                        await Console.Out.WriteLineAsync($"{count++}");
                        var discipline = unit.DisciplineCode;
                        var id = unit.Id;

                        var resultUrl = $"{this.Configuration.GetSection(CrawlerConstants.PARIS_2024_RESULT_URL).Value}{discipline}~rscResult={id}.json";

                        try
                        {
                            var resultHttpModel = await this.HttpService.GetAsync(resultUrl);
                            await this.ProcessGroupAsync(resultHttpModel);
                        }
                        catch (Exception ex)
                        {
                            this.Logger.LogError(ex, $"Failed to process url: {resultUrl};");
                        }
                    }
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