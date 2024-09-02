namespace SportHub.Crawlers.OlympicGames.Paris2024;

using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using SportHub.Common.Constants;
using SportHub.Data.Entities.Crawlers;
using SportHub.Data.Models.Crawlers.Paris2024.Disciplines;
using SportHub.Services.Data.CrawlerStorageDb.Interfaces;
using SportHub.Services.Interfaces;

public class DisciplineCrawler : BaseCrawler
{
    public DisciplineCrawler(ILogger<BaseCrawler> logger, IConfiguration configuration, IHttpService httpService, ICrawlersService crawlersService, IGroupsService groupsService)
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
                    var unitsHttpModel = await this.HttpService.GetAsync(url);
                    var unitsDocument = this.CreateDocument(unitsHttpModel);

                    var positionsUrl = $"{this.Configuration.GetSection(CrawlerConstants.PARIS_2024_DISCIPLINE_POSITIONS_URL).Value}{discipline.Code}.json";
                    var positionsHttpModel = await this.HttpService.GetAsync(positionsUrl);
                    var positionsDocument = this.CreateDocument(positionsHttpModel);
                    positionsDocument.Order = 2;

                    var schedulesUrl = $"{this.Configuration.GetSection(CrawlerConstants.PARIS_2024_DISCIPLINE_SCHEDULES_URL).Value}{discipline.Code}.json";
                    var schedulesHttpModel = await this.HttpService.GetAsync(schedulesUrl);
                    var schedulesDocument = this.CreateDocument(schedulesHttpModel);
                    schedulesDocument.Order = 3;

                    var eventsUrl = $"{this.Configuration.GetSection(CrawlerConstants.PARIS_2024_DISCIPLINE_EVENTS_URL).Value}{discipline.Code}.json";
                    var eventsHttpModel = await this.HttpService.GetAsync(eventsUrl);
                    var eventsDocument = this.CreateDocument(eventsHttpModel);
                    eventsDocument.Order = 4;

                    var eventUnitsUrl = $"{this.Configuration.GetSection(CrawlerConstants.PARIS_2024_DISCIPLINE_EVENT_UNITS_URL).Value}{discipline.Code}.json";
                    var eventUnitsHttpModel = await this.HttpService.GetAsync(eventUnitsUrl);
                    var eventUnitsDocument = this.CreateDocument(eventUnitsHttpModel);
                    eventUnitsDocument.Order = 5;

                    var recordsUrl = $"{this.Configuration.GetSection(CrawlerConstants.PARIS_2024_DISCIPLINE_RECORDS_URL).Value}{discipline.Code}.json";
                    var recordsHttpModel = await this.HttpService.GetAsync(recordsUrl);
                    var recordsDocument = this.CreateDocument(recordsHttpModel);
                    recordsDocument.Order = 6;

                    var infoUrl = $"{this.Configuration.GetSection(CrawlerConstants.PARIS_2024_DISCIPLINE_INFO_URL).Value}{discipline.Code}.json";
                    var infoHttpModel = await this.HttpService.GetAsync(infoUrl);
                    var infoDocument = this.CreateDocument(infoHttpModel);
                    infoDocument.Order = 7;

                    await this.ProcessGroupAsync(unitsHttpModel, new List<Document>
                    {
                        unitsDocument,
                        positionsDocument,
                        schedulesDocument,
                        eventsDocument,
                        eventUnitsDocument,
                        recordsDocument,
                        infoDocument
                    });

                    //var imageUrl = $"{this.Configuration.GetSection(CrawlerConstants.PARIS_2024_DISCIPLINE_IMAGE_URL).Value}{discipline.Code}_big.svg";
                    //var imageBytes = await this.HttpService.DownloadBytesAsync(imageUrl);
                    //await File.WriteAllBytesAsync($"Images/Sports/{discipline.Code}.svg", imageBytes);
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