﻿namespace SportHub.Converters.Countries;

using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using SportHub.Common.Constants;
using SportHub.Converters;
using SportHub.Data.Models.Entities.Crawlers;
using SportHub.Data.Models.Entities.SportHub;
using SportHub.Data.Repositories;
using SportHub.Services.Data.CrawlerStorageDb.Interfaces;
using SportHub.Services.Interfaces;

public class CountryDataConverter : BaseConverter
{
    private readonly IRegExpService regExpService;
    private readonly IConfiguration configuration;
    private readonly IHttpService httpService;
    private readonly SportHubRepository<Country> repository;

    public CountryDataConverter(ILogger<BaseConverter> logger, ICrawlersService crawlersService, ILogsService logsService, IGroupsService groupsService, IZipService zipService,
        IRegExpService regExpService, IConfiguration configuration, IHttpService httpService, SportHubRepository<Country> repository)
        : base(logger, crawlersService, logsService, groupsService, zipService)
    {
        this.regExpService = regExpService;
        this.configuration = configuration;
        this.httpService = httpService;
        this.repository = repository;
    }

    protected override async Task ProcessGroupAsync(Group group)
    {
        var document = this.CreateHtmlDocument(group.Documents.Single());
        var header = document
            .DocumentNode
            .SelectSingleNode("//h1")
            .InnerText;

        var name = this.regExpService.Match(header, @"Flag of (.*)").Groups[1].Value.Trim();
        var country = new Country { Name = name };

        var rows = document
            .DocumentNode
            .SelectNodes("//table[@class='table-dl']//tr");

        foreach (var row in rows)
        {
            var thTag = row.Elements("th").Single().InnerText.Trim();
            var tdTag = row.Elements("td").Single().InnerText.Trim();

            switch (thTag.ToLower())
            {
                case "independent":
                    country.IsIndependent = tdTag.ToLower() == "yes";
                    break;
                case "country codes":
                    var countryCodeMatch = this.regExpService.Match(tdTag, @"([A-Z]{2}),\s*([A-Z]{3})");
                    if (countryCodeMatch != null)
                    {
                        country.TwoDigitsCode = countryCodeMatch.Groups[1].Value;
                        country.Code = countryCodeMatch.Groups[2].Value;
                    }
                    else
                    {
                        countryCodeMatch = this.regExpService.Match(tdTag, @"([A-Z-]{6})");
                        if (countryCodeMatch != null)
                        {
                            country.Code = countryCodeMatch.Groups[1].Value;
                        }
                    }
                    break;
                case "official name":
                    country.OfficialName = tdTag;
                    break;
                case "capital city":
                    country.Capital = tdTag;
                    break;
                case "continent":
                    country.Continent = tdTag;
                    break;
                case "member of":
                    country.MemberOf = tdTag;
                    break;
                case "population":
                    var populationMatch = this.regExpService.Match(tdTag, @"([\d\s]+)\(([\d]{4})\)");
                    if (populationMatch != null)
                    {
                        var text = populationMatch.Groups[1].Value.Trim();
                        text = this.regExpService.Replace(text, @"\s*", string.Empty);
                        country.Population = int.Parse(text);
                    }
                    break;
                case "total area":
                    var areaMatch = this.regExpService.Match(tdTag, @"([\d\s]+)km");
                    if (areaMatch != null)
                    {
                        var text = areaMatch.Groups[1].Value.Trim();
                        text = this.regExpService.Replace(text, @"\s*", string.Empty);
                        country.TotalArea = int.Parse(text);
                    }
                    break;
                case "highest point":
                    var highestPointMatch = this.regExpService.Match(tdTag, @"(.*?)\s*\(([\d\s-]+)\s*m,\s*([\d\s-]+)\s*ft\)");
                    if (highestPointMatch != null)
                    {
                        country.HighestPointPlace = highestPointMatch.Groups[1].Value.Trim();
                        var text = highestPointMatch.Groups[2].Value.Trim();
                        text = this.regExpService.Replace(text, @"\s*", string.Empty);
                        country.HighestPoint = int.Parse(text);
                    }
                    break;
                case "lowest point":
                    var lowestPointMatch = this.regExpService.Match(tdTag, @"(.*?)\s*\(([\d\s-]+)\s*m,\s*([\d\s-]+)\s*ft\)");
                    if (lowestPointMatch != null)
                    {
                        country.LowestPointPlace = lowestPointMatch.Groups[1].Value.Trim();
                        var text = lowestPointMatch.Groups[2].Value.Trim();
                        text = this.regExpService.Replace(text, @"\s*", string.Empty);
                        country.LowestPoint = int.Parse(text);
                    }
                    break;
            }
        }

        var coutnryCode = country.TwoDigitsCode != null ? country.TwoDigitsCode.ToLower() : country.Code.ToLower();
        var flag = await this.httpService.DownloadBytesAsync($"{this.configuration.GetSection(CrawlerConstants.WORLD_COUNTRIES_DOWNLOAD_IMAGE).Value}{coutnryCode}.png");
        country.Flag = flag;

        await File.WriteAllBytesAsync($"flags\\{country.Code}.png", flag);

        await this.repository.AddAsync(country);
        await this.repository.SaveChangesAsync();
    }
}