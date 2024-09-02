namespace SportHub.Crawlers.OlympicGames.Paris2024;

using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using SportHub.Common.Constants;
using SportHub.Common.Helpers;
using SportHub.Data.Models.Crawlers.Paris2024.Disciplines;
using SportHub.Data.Models.Crawlers.Paris2024.UnitGroup;
using SportHub.Services.Data.CrawlerStorageDb.Interfaces;
using SportHub.Services.Interfaces;

public class PoolStandingCrawler : BaseCrawler
{
    public PoolStandingCrawler(ILogger<BaseCrawler> logger, IConfiguration configuration, IHttpService httpService, ICrawlersService crawlersService, IGroupsService groupsService)
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
                var unitsUrl = $"{this.Configuration.GetSection(CrawlerConstants.PARIS_2024_DISCIPLINE_UNITS_URL).Value}{discipline.Code}";

                try
                {
                    var disciplineHttpModel = await this.HttpService.GetAsync(unitsUrl);
                    var disciplineJson = Encoding.UTF8.GetString(disciplineHttpModel.Bytes);
                    var units = JsonSerializer.Deserialize<UnitGroupCrawlerModel>(disciplineJson);
                    var phases = units.Units.Select(x => x.PhaseId).Distinct().ToList();

                    foreach (var id in phases)
                    {
                        if (RegExpHelper.Match(id, @"[A-Z^0-9]{5,}") != null)
                        {
                            var poolStandingUrl = $"{this.Configuration.GetSection(CrawlerConstants.PARIS_2024_EVENT_POOL_STANDINGS_URL).Value}{id}.json";

                            try
                            {
                                var scheduleHttpModel = await this.HttpService.GetAsync(poolStandingUrl);
                                if (scheduleHttpModel.Content.Length > 10)
                                {
                                    await this.ProcessGroupAsync(scheduleHttpModel);
                                }
                            }
                            catch (Exception ex)
                            {
                                this.Logger.LogError(ex, $"Failed to process url: {poolStandingUrl};");
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