namespace SportHub.Crawlers.OlympicGames.Olympedia;

using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using HtmlAgilityPack;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using SportHub.Common.Constants;
using SportHub.Data.Entities.Crawlers;
using SportHub.Data.Models.Http;
using SportHub.Services.Data.CrawlerStorageDb.Interfaces;
using SportHub.Services.Interfaces;

public class NOCCrawler : BaseOlympediaCrawler
{
    public NOCCrawler(ILogger<BaseCrawler> logger, IConfiguration configuration, IHttpService httpService, ICrawlersService crawlersService, IGroupsService groupsService)
        : base(logger, configuration, httpService, crawlersService, groupsService)
    {
    }

    public override async Task StartAsync()
    {
        this.Logger.LogInformation($"{this.GetType().FullName} Start!");

        try
        {
            var httpModel = await this.HttpService.GetAsync(this.Configuration.GetSection(CrawlerConstants.OLYMPEDIA_NOC_URL).Value);
            var countryUrls = this.ExtractCountryUrls(httpModel);

            foreach (var url in countryUrls)
            {
                try
                {
                    var countryHttpModel = await this.HttpService.GetAsync(url, true);
                    var documents = new List<Document>
                    {
                        this.CreateDocument(countryHttpModel)
                    };

                    var nocUrl = this.ExtractNocUrl(countryHttpModel);
                    if (nocUrl != null)
                    {
                        var nocHttpModel = await this.HttpService.GetAsync(nocUrl, true);
                        var nocDocument = this.CreateDocument(nocHttpModel);
                        nocDocument.Order = 2;
                        documents.Add(nocDocument);
                    }

                    var flagBearersUrl = $"{this.Configuration.GetSection(CrawlerConstants.OLYMPEDIA_FLAG_BEARERS_URL).Value}{countryHttpModel.Uri.Segments.Last()}";
                    var flagBearersHttpModel = await this.HttpService.GetAsync(flagBearersUrl, true);
                    if (flagBearersHttpModel.StatusCode == HttpStatusCode.OK)
                    {
                        var flagBearersDocument = this.CreateDocument(flagBearersHttpModel);
                        flagBearersDocument.Order = 3;
                        documents.Add(flagBearersDocument);
                    }

                    var participationsUrl = $"{this.Configuration.GetSection(CrawlerConstants.OLYMPEDIA_NOC_PARTICIPATIONS_URL).Value}{countryHttpModel.Uri.Segments.Last()}";
                    var participationsHttpModel = await this.HttpService.GetAsync(participationsUrl, true);
                    if (participationsHttpModel.StatusCode == HttpStatusCode.OK)
                    {
                        var participationsDocument = this.CreateDocument(participationsHttpModel);
                        participationsDocument.Order = 4;
                        documents.Add(participationsDocument);
                    }

                    await this.ProcessGroupAsync(countryHttpModel, documents);
                }
                catch (Exception ex)
                {
                    this.Logger.LogError(ex, $"Failed to process url: {url}");
                }
            }
        }
        catch (Exception ex)
        {
            this.Logger.LogError(ex, $"Failed to process url: {this.Configuration.GetSection(CrawlerConstants.OLYMPEDIA_NOC_URL).Value}");
        }

        this.Logger.LogInformation($"{this.GetType().FullName} End!");
    }

    private string ExtractNocUrl(HttpModel httpModel)
    {
        var url = httpModel
            .HtmlDocument
            .DocumentNode
            .SelectNodes("//a")
            .Select(x => x.Attributes["href"]?.Value.Trim())
            .Where(x => x.StartsWith("/organizations/"))
            .Select(x => this.CreateUrl(x, this.Configuration.GetSection(CrawlerConstants.OLYMPEDIA_MAIN_URL).Value))
            .FirstOrDefault();

        return url;
    }

    private IReadOnlyCollection<string> ExtractCountryUrls(HttpModel httpModel)
    {
        var tableNode = httpModel
            .HtmlDocument
            .DocumentNode
            .SelectNodes("//table[@class='table table-striped sortable']")
            .Take(1)
            .ToList();

        var document = new HtmlDocument();
        document.LoadHtml(tableNode.FirstOrDefault().OuterHtml);

        var urls = document
            .DocumentNode
            .SelectNodes("//tr")
            .Where(x => x.OuterHtml.Contains("glyphicon-ok"))
            .Select(x => Regex.Match(x.OuterHtml, "href=\"(.*?)\"").Groups[1].Value.Trim())
            .Where(x => x != null)
            .Select(x => this.CreateUrl(x, this.Configuration.GetSection(CrawlerConstants.OLYMPEDIA_MAIN_URL).Value))
            .Distinct()
            .ToList();

        return urls;
    }
}