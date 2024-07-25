namespace SportHub.Converters.OlympicGames.Olympedia;

using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using SportHub.Common.Constants;
using SportHub.Data.Models.Converters;
using SportHub.Data.Models.DbEntities.Crawlers;
using SportHub.Data.Models.DbEntities.OlympicGames;
using SportHub.Data.Repositories;
using SportHub.Services.Data.CrawlerStorageDb.Interfaces;
using SportHub.Services.Data.OlympicGamesDb.Interfaces;
using SportHub.Services.Interfaces;

public class CountryConverter : BaseOlympediaConverter
{
    private readonly IHttpService httpService;
    private readonly IConfiguration configuration;
    private readonly OlympicGamesRepository<NOC> repository;

    public CountryConverter(ILogger<BaseConverter> logger, ICrawlersService crawlersService, ILogsService logsService, IGroupsService groupsService, IZipService zipService,
        IRegExpService regExpService, INormalizeService normalizeService, IOlympediaService olympediaService, IDataCacheService dataCacheService, IHttpService httpService,
        IConfiguration configuration, OlympicGamesRepository<NOC> repository)
        : base(logger, crawlersService, logsService, groupsService, zipService, regExpService, normalizeService, olympediaService, dataCacheService)
    {
        this.httpService = httpService;
        this.configuration = configuration;
        this.repository = repository;
    }

    protected override async Task ProcessGroupAsync(Group group)
    {
        var model = this.PrepareConverterModel(group);
        var name = this.RegExpService.Match(model.Title, @"Flag of (.*)").Groups[1].Value.Trim();
        var country = new Country { Name = name };

        var rows = model.HtmlDocument
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
                    var countryCodeMatch = this.RegExpService.Match(tdTag, @"([A-Z]{2}),\s*([A-Z]{3})");
                    if (countryCodeMatch != null)
                    {
                        country.TwoDigitsCode = countryCodeMatch.Groups[1].Value;
                        country.Code = countryCodeMatch.Groups[2].Value;
                    }
                    else
                    {
                        countryCodeMatch = this.RegExpService.Match(tdTag, @"([A-Z-]{6})");
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
                    var populationMatch = this.RegExpService.Match(tdTag, @"([\d\s]+)\(([\d]{4})\)");
                    if (populationMatch != null)
                    {
                        var text = populationMatch.Groups[1].Value.Trim();
                        text = this.RegExpService.Replace(text, @"\s*", string.Empty);
                        country.Population = int.Parse(text);
                    }
                    break;
                case "total area":
                    var areaMatch = this.RegExpService.Match(tdTag, @"([\d\s]+)km");
                    if (areaMatch != null)
                    {
                        var text = areaMatch.Groups[1].Value.Trim();
                        text = this.RegExpService.Replace(text, @"\s*", string.Empty);
                        country.TotalArea = int.Parse(text);
                    }
                    break;
                case "highest point":
                    var highestPointMatch = this.RegExpService.Match(tdTag, @"(.*?)\s*\(([\d\s-]+)\s*m,\s*([\d\s-]+)\s*ft\)");
                    if (highestPointMatch != null)
                    {
                        country.HighestPointPlace = highestPointMatch.Groups[1].Value.Trim();
                        var text = highestPointMatch.Groups[2].Value.Trim();
                        text = this.RegExpService.Replace(text, @"\s*", string.Empty);
                        country.HighestPoint = int.Parse(text);
                    }
                    break;
                case "lowest point":
                    var lowestPointMatch = this.RegExpService.Match(tdTag, @"(.*?)\s*\(([\d\s-]+)\s*m,\s*([\d\s-]+)\s*ft\)");
                    if (lowestPointMatch != null)
                    {
                        country.LowestPointPlace = lowestPointMatch.Groups[1].Value.Trim();
                        var text = lowestPointMatch.Groups[2].Value.Trim();
                        text = this.RegExpService.Replace(text, @"\s*", string.Empty);
                        country.LowestPoint = int.Parse(text);
                    }
                    break;
            }
        }

        var coutnryCode = country.TwoDigitsCode != null ? country.TwoDigitsCode.ToLower() : country.Code.ToLower();
        var flag = await this.httpService.DownloadBytesAsync($"{this.configuration.GetSection(CrawlerConstants.WORLD_COUNTRIES_DOWNLOAD_IMAGE).Value}{coutnryCode}.png");
        country.Flag = flag;

        var nationalOlympicCommittee = await this.repository.GetAsync(x => x.WorldCode == country.Code);
        if (nationalOlympicCommittee != null)
        {
            nationalOlympicCommittee.Capital = country.Capital;
            nationalOlympicCommittee.Continent = country.Continent;
            nationalOlympicCommittee.Flag = country.Flag;

            this.repository.Update(nationalOlympicCommittee);
            await this.repository.SaveChangesAsync();
        }
    }
}