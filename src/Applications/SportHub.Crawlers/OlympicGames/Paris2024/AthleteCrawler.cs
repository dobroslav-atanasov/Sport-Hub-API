﻿namespace SportHub.Crawlers.OlympicGames.Paris2024;

using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using SportHub.Common.Constants;
using SportHub.Data.Models.Crawlers.Paris2024.Athletes;
using SportHub.Services.Data.CrawlerStorageDb.Interfaces;
using SportHub.Services.Interfaces;

public class AthleteCrawler : BaseCrawler
{
    private readonly IRegExpService regExpService;

    public AthleteCrawler(ILogger<BaseCrawler> logger, IConfiguration configuration, IHttpService httpService, ICrawlersService crawlersService, IGroupsService groupsService,
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
            var httpModel = await this.HttpService.GetAsync(this.Configuration.GetSection(CrawlerConstants.PARIS_2024_ATHLETES_URL).Value);
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
                    //var documents = new List<Document>
                    //{
                    //    this.CreateDocument(personHttpModel)
                    //};

                    //var pageHtmlHttpModel = await this.HttpService.GetAsync($"{this.Configuration.GetSection(CrawlerConstants.PARIS_2024_ATHLETE_PAGE_URL).Value}{person.Code}");
                    //var imageSiteUrl = this.regExpService.MatchFirstGroup(pageHtmlHttpModel.Content, @"<img class=""wrsBiosDetail__image"" alt="""" src=""(.*?)"">");

                    //if (string.IsNullOrEmpty(imageSiteUrl))
                    //{
                    //    var imageUrl = $"{this.Configuration.GetSection(CrawlerConstants.PARIS_2024_MAIN_URL).Value}{imageSiteUrl}";
                    //    var imageHttpModel = await this.HttpService.GetAsync(imageUrl);
                    //    var imageDocument = this.CreateDocument(imageHttpModel);
                    //    imageDocument.Order = 2;
                    //    documents.Add(imageDocument);
                    //}

                    //await this.ProcessGroupAsync(personHttpModel, documents);
                }
                catch (Exception ex)
                {
                    this.Logger.LogError(ex, $"Failed to process url: {url};");
                }

            }
        }
        catch (Exception ex)
        {
            this.Logger.LogError(ex, $"Failed to process url: {this.Configuration.GetSection(CrawlerConstants.PARIS_2024_ATHLETES_URL).Value};");
        }

        this.Logger.LogInformation($"{this.GetType().FullName} End!");
    }
}