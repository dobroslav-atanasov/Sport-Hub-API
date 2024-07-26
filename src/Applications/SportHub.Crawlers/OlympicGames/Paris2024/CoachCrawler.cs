namespace SportHub.Crawlers.OlympicGames.Paris2024;

using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using SportHub.Common.Constants;
using SportHub.Data.Models.Crawlers.Paris2024.Athletes;
using SportHub.Services.Data.CrawlerStorageDb.Interfaces;
using SportHub.Services.Interfaces;

public class CoachCrawler : BaseCrawler
{
    public CoachCrawler(ILogger<BaseCrawler> logger, IConfiguration configuration, IHttpService httpService, ICrawlersService crawlersService, IGroupsService groupsService)
        : base(logger, configuration, httpService, crawlersService, groupsService)
    {
    }

    public override async Task StartAsync()
    {
        this.Logger.LogInformation($"{this.GetType().FullName} Start!");

        try
        {
            var httpModel = await this.HttpService.GetAsync(this.Configuration.GetSection(CrawlerConstants.PARIS_2024_COACHES_URL).Value);
            var json = Encoding.UTF8.GetString(httpModel.Bytes);

            var model = JsonSerializer.Deserialize<AthletesList>(json);

            var count = 0;
            foreach (var person in model.Persons)
            {
                await Console.Out.WriteLineAsync($"{count++}");
                var url = $"{this.Configuration.GetSection(CrawlerConstants.PARIS_2024_ATHLETE_URL).Value}{person.Code}.json";

                try
                {
                    var personHttpModel = await this.HttpService.GetAsync(url);
                    await this.ProcessGroupAsync(personHttpModel);

                    var imageCode = person.Code.Substring(1, 3);
                    if (person.Image != null && person.Image.ImageExtension != null)
                    {
                        var imageUrl = $"{this.Configuration.GetSection(CrawlerConstants.PARIS_2024_ATHLETE_IMAGE_URL).Value}{imageCode}/medium/{person.Code}{person.Image.ImageExtension}";
                        var imageBytes = await this.HttpService.DownloadBytesAsync(imageUrl);
                        await File.WriteAllBytesAsync($"Images/Coaches/{person.Code}{person.Image.ImageExtension}", imageBytes);
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
            this.Logger.LogError(ex, $"Failed to process url: {this.Configuration.GetSection(CrawlerConstants.PARIS_2024_COACHES_URL).Value};");
        }

        this.Logger.LogInformation($"{this.GetType().FullName} End!");
    }
}