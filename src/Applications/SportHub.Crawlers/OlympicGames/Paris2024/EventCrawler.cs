namespace SportHub.Crawlers.OlympicGames.Paris2024;

using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using SportHub.Common.Constants;
using SportHub.Data.Models.Crawlers.Paris2024.Disciplines;
using SportHub.Data.Models.DbEntities.Crawlers;
using SportHub.Services.Data.CrawlerStorageDb.Interfaces;
using SportHub.Services.Interfaces;

public class EventCrawler : BaseCrawler
{
    private readonly IRegExpService regExpService;

    public EventCrawler(ILogger<BaseCrawler> logger, IConfiguration configuration, IHttpService httpService, ICrawlersService crawlersService, IGroupsService groupsService,
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
                    var units = JsonSerializer.Deserialize<DisciplineSchedule>(disciplineJson);
                    var events = units.Units.Select(x => x.EventId).Distinct().ToList();

                    foreach (var id in events)
                    {
                        if (this.regExpService.Match(id, @"[A-Z^0-9]{5,}") != null)
                        {
                            var shortId = id.Substring(0, 22);
                            var scheduleUrl = $"{this.Configuration.GetSection(CrawlerConstants.PARIS_2024_EVENT_SCHEDULE_URL).Value}{id}";

                            try
                            {
                                var scheduleHttpModel = await this.HttpService.GetAsync(scheduleUrl);
                                var scheduleDocument = this.CreateDocument(scheduleHttpModel);

                                var documents = new List<Document> { scheduleDocument };

                                var rankingUrl = $"{this.Configuration.GetSection(CrawlerConstants.PARIS_2024_EVENT_RANKING_URL).Value}{shortId}.json";
                                var rankingHttpModel = await this.HttpService.GetAsync(rankingUrl);
                                var rankingDocument = this.CreateDocument(rankingHttpModel);
                                rankingDocument.Order = 2;
                                documents.Add(rankingDocument);

                                var phasesUrl = $"{this.Configuration.GetSection(CrawlerConstants.PARIS_2024_EVENT_PHASES_URL).Value}{shortId}.json";
                                var phasesHttpModel = await this.HttpService.GetAsync(phasesUrl);
                                var phasesDocument = this.CreateDocument(phasesHttpModel);
                                phasesDocument.Order = 3;
                                documents.Add(phasesDocument);

                                var bracketsUrl = $"{this.Configuration.GetSection(CrawlerConstants.PARIS_2024_EVENT_BRACKETS_URL).Value}{id}.json";
                                var bracketsHttpModel = await this.HttpService.GetAsync(bracketsUrl);
                                if (bracketsHttpModel.Content.Length > 10)
                                {
                                    var bracketsDocument = this.CreateDocument(bracketsHttpModel);
                                    bracketsDocument.Order = 4;
                                    documents.Add(bracketsDocument);
                                }

                                await this.ProcessGroupAsync(scheduleHttpModel, documents);
                            }
                            catch (Exception ex)
                            {
                                this.Logger.LogError(ex, $"Failed to process url: {scheduleUrl};");
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