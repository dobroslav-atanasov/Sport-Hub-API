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

        try
        {
            var httpModel = await this.HttpService.GetAsync(this.Configuration.GetSection(CrawlerConstants.PARIS_2024_DISCIPLINES_URL).Value);
            var json = Encoding.UTF8.GetString(httpModel.Bytes);

            var model = JsonSerializer.Deserialize<DisciplinesList>(json);
            var disciplines = model.Disciplines.Where(x => x.IsSport && x.Scheduled);

            foreach (var discipline in disciplines)
            {
                var unitsUrl = $"{this.Configuration.GetSection(CrawlerConstants.PARIS_2024_DISCIPLINE_UNITS_URL).Value}{discipline.Code}";

                try
                {
                    var disciplineHttpModel = await this.HttpService.GetAsync(unitsUrl);
                    var disciplineJson = Encoding.UTF8.GetString(disciplineHttpModel.Bytes);
                    var schedule = JsonSerializer.Deserialize<DisciplineSchedule>(disciplineJson);
                    var units = schedule.Units.Select(x => x.Id).Distinct().ToList();

                    foreach (var unit in units)
                    {
                        if (this.regExpService.Match(unit, @"[A-Z^0-9]{5,}") != null)
                        {
                            var resultUrl = $"{this.Configuration.GetSection(CrawlerConstants.PARIS_2024_RESULT_URL).Value}{discipline.Code}~rscResult={unit}.json";

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
                }
                catch (Exception ex)
                {
                    this.Logger.LogError(ex, $"Failed to process url: {unitsUrl};");
                }
            }
        }
        catch (Exception ex)
        {
            this.Logger.LogError(ex, $"Failed to process url: {this.Configuration.GetSection(CrawlerConstants.PARIS_2024_DISCIPLINES_URL).Value};");
        }

        this.Logger.LogInformation($"{this.GetType().FullName} End!");
    }
}