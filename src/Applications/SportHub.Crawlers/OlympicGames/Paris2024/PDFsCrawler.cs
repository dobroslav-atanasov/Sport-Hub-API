namespace SportHub.Crawlers.OlympicGames.Paris2024;

using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using SportHub.Common.Constants;
using SportHub.Data.Models.Crawlers.Paris2024.Disciplines;
using SportHub.Data.Models.Crawlers.Paris2024.PDFs;
using SportHub.Data.Models.DbEntities.Crawlers;
using SportHub.Services.Data.CrawlerStorageDb.Interfaces;
using SportHub.Services.Interfaces;

public class PDFsCrawler : BaseCrawler
{
    public PDFsCrawler(ILogger<BaseCrawler> logger, IConfiguration configuration, IHttpService httpService, ICrawlersService crawlersService, IGroupsService groupsService)
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

            var model = JsonSerializer.Deserialize<DisciplinesList>(json);
            var disciplines = model.Disciplines.Where(x => x.IsSport && x.Scheduled);

            foreach (var discipline in disciplines)
            {
                var pdfsUrl = $"{this.Configuration.GetSection(CrawlerConstants.PARIS_2024_PDFS_URL).Value}{discipline.Code}.json";

                try
                {
                    var pdfsHttpModel = await this.HttpService.GetAsync(pdfsUrl);
                    var pdfJson = Encoding.UTF8.GetString(pdfsHttpModel.Bytes);
                    var pdfModel = JsonSerializer.Deserialize<PDFList>(pdfJson);

                    var paths = pdfModel.Pdfs.Select(x => x.FullPath).Distinct().ToList();

                    var documents = new List<Document> { this.CreateDocument(pdfsHttpModel) };
                    var order = 2;

                    foreach (var path in paths)
                    {
                        var pdfUrl = $"{this.Configuration.GetSection(CrawlerConstants.PARIS_2024_PDF_URL).Value}{path}";
                        try
                        {
                            var resultHttpModel = await this.HttpService.GetAsync(pdfUrl);
                            var document = this.CreateDocument(resultHttpModel);
                            document.Order = order;
                            order++;
                            documents.Add(document);
                        }
                        catch (Exception ex)
                        {
                            this.Logger.LogError(ex, $"Failed to process data: {pdfUrl};");
                        }
                    }

                    await this.ProcessGroupAsync(pdfsHttpModel, documents);
                }
                catch (Exception ex)
                {
                    this.Logger.LogError(ex, $"Failed to process url: {pdfsUrl};");
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