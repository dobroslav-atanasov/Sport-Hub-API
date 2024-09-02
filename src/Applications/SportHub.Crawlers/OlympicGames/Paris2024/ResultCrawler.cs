namespace SportHub.Crawlers.OlympicGames.Paris2024;

using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using SportHub.Common.Constants;
using SportHub.Data.Models.Crawlers.Paris2024.Disciplines;
using SportHub.Data.Models.Crawlers.Paris2024.EventUnit;
using SportHub.Services.Data.CrawlerStorageDb.Interfaces;
using SportHub.Services.Interfaces;

public class ResultCrawler : BaseCrawler
{
    public ResultCrawler(ILogger<BaseCrawler> logger, IConfiguration configuration, IHttpService httpService, ICrawlersService crawlersService, IGroupsService groupsService)
        : base(logger, configuration, httpService, crawlersService, groupsService)
    {
    }

    public override async Task StartAsync()
    {
        this.Logger.LogInformation($"{this.GetType().FullName} Start!");

        try
        {
            var httpModel = await this.HttpService.GetAsync(this.Configuration.GetSection(CrawlerConstants.PARIS_2024_DISCIPLINES_URL).Value);
            var json = Encoding.UTF8.GetString(httpModel.Bytes);

            var model = JsonSerializer.Deserialize<DisciplinesCrawlerModel>(json);
            var disciplines = model.Disciplines.Where(x => x.IsSport && x.Scheduled);

            foreach (var discipline in disciplines)
            {
                var url = $"{this.Configuration.GetSection(CrawlerConstants.PARIS_2024_DISCIPLINE_UNITS_URL).Value}{discipline.Code}";
                try
                {
                    var eventUnitsUrl = $"{this.Configuration.GetSection(CrawlerConstants.PARIS_2024_DISCIPLINE_EVENT_UNITS_URL).Value}{discipline.Code}.json";
                    var eventUnitsHttpModel = await this.HttpService.GetAsync(eventUnitsUrl);
                    var eventUnitsJson = Encoding.UTF8.GetString(eventUnitsHttpModel.Bytes);

                    var eventUnits = JsonSerializer.Deserialize<EventUnitCrawlerModel>(eventUnitsJson);
                    var eventUnitsList = eventUnits.EventUnits.Where(x => x.Type != "NONE");

                    foreach (var eventUnit in eventUnitsList)
                    {
                        var resultUrl = $"{this.Configuration.GetSection(CrawlerConstants.PARIS_2024_RESULT_URL).Value}{discipline.Code}~rscResult={eventUnit.Code}.json";

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

                await Console.Out.WriteLineAsync(discipline.Description);
            }
        }
        catch (Exception ex)
        {
            this.Logger.LogError(ex, $"Failed to process url: {this.Configuration.GetSection(CrawlerConstants.PARIS_2024_DISCIPLINES_URL).Value};");
        }

        this.Logger.LogInformation($"{this.GetType().FullName} End!");
    }
}