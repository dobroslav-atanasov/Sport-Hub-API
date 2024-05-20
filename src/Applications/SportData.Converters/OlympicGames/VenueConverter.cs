namespace SportData.Converters.OlympicGames;

using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using SportData.Common.Extensions;
using SportData.Data.Models.Entities.Crawlers;
using SportData.Data.Models.Entities.OlympicGames;
using SportData.Data.Repositories;
using SportData.Services.Data.CrawlerStorageDb.Interfaces;
using SportData.Services.Data.OlympicGamesDb.Interfaces;
using SportData.Services.Interfaces;

public class VenueConverter : BaseOlympediaConverter
{
    private readonly OlympicGamesRepository<Venue> repository;

    public VenueConverter(ILogger<BaseConverter> logger, ICrawlersService crawlersService, ILogsService logsService, IGroupsService groupsService, IZipService zipService,
        IRegExpService regExpService, INormalizeService normalizeService, IOlympediaService olympediaService, IDataCacheService dataCacheService,
        OlympicGamesRepository<Venue> repository)
        : base(logger, crawlersService, logsService, groupsService, zipService, regExpService, normalizeService, olympediaService, dataCacheService)
    {
        this.repository = repository;
    }

    protected override async Task ProcessGroupAsync(Group group)
    {
        try
        {
            var document = this.CreateHtmlDocument(group.Documents.Single());
            var number = int.Parse(new Uri(group.Documents.Single().Url).Segments.Last());
            var fullName = document.DocumentNode.SelectSingleNode("//h1").InnerText.Decode();
            var name = this.RegExpService.MatchFirstGroup(document.DocumentNode.OuterHtml, @"<tr>\s*<th>Name<\/th>\s*<td>(.*?)<\/td>\s*<\/tr>")?.Decode();
            var cityName = this.RegExpService.MatchFirstGroup(document.DocumentNode.OuterHtml, @"<tr>\s*<th>Place<\/th>\s*<td>(.*?)<\/td>\s*<\/tr>");
            var englishName = this.RegExpService.MatchFirstGroup(document.DocumentNode.OuterHtml, @"<tr>\s*<th>English name<\/th>\s*<td>(.*?)<\/td>\s*<\/tr>")?.Decode();
            var opened = this.RegExpService.MatchFirstGroup(document.DocumentNode.OuterHtml, @"<tr>\s*<th>Opened<\/th>\s*<td>([\d]+)<\/td>\s*<\/tr>");
            var demolished = this.RegExpService.MatchFirstGroup(document.DocumentNode.OuterHtml, @"<tr>\s*<th>Demolished<\/th>\s*<td>([\d]+)<\/td>\s*<\/tr>");
            var capacity = this.RegExpService.MatchFirstGroup(document.DocumentNode.OuterHtml, @"<tr>\s*<th>Games Capacity<\/th>\s*<td>([\d]+)<\/td>\s*<\/tr>");
            var coordinatesMatch = this.RegExpService.Match(document.DocumentNode.OuterHtml, @"<tr>\s*<th>Coordinates<\/th>\s*<td>([\d\.\-]+),\s*([\d\.\-]+)");

            var venue = new Venue
            {
                Name = name,
                City = this.NormalizeService.NormalizeHostCityName(cityName),
                Number = number,
                EnglishName = englishName,
                FullName = fullName,
                OpenedYear = opened != null ? int.Parse(opened) : null,
                DemolishedYear = demolished != null ? int.Parse(demolished) : null,
                Capacity = capacity,
                LatitudeCoordinate = coordinatesMatch != null ? double.Parse(coordinatesMatch.Groups[1].Value.Trim().Replace(".", ",")) : null,
                LongitudeCoordinate = coordinatesMatch != null ? double.Parse(coordinatesMatch.Groups[2].Value.Trim().Replace(".", ",")) : null,
            };

            var dbVenue = await this.repository.GetAsync(x => x.Number == number);
            if (dbVenue != null)
            {
                var equals = venue.Equals(dbVenue);
                if (!equals)
                {
                    this.repository.Update(venue);
                    await this.repository.SaveChangesAsync();
                }
            }
            else
            {
                await this.repository.AddAsync(venue);
                await this.repository.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            this.Logger.LogError(ex, $"Failed to process group: {group.Identifier}");
        }
    }
}