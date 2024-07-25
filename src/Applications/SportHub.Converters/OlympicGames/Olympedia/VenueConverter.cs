namespace SportHub.Converters.OlympicGames.Olympedia;

using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using SportHub.Common.Extensions;
using SportHub.Data.Models.DbEntities.Crawlers;
using SportHub.Data.Models.DbEntities.OlympicGames;
using SportHub.Data.Repositories;
using SportHub.Services.Data.CrawlerStorageDb.Interfaces;
using SportHub.Services.Data.OlympicGamesDb.Interfaces;
using SportHub.Services.Interfaces;

public class VenueConverter : BaseOlympediaConverter
{
    private readonly OlympicGamesRepository<Venue> venueRepository;
    private readonly OlympicGamesRepository<Event> eventRepository;

    public VenueConverter(ILogger<BaseConverter> logger, ICrawlersService crawlersService, ILogsService logsService, IGroupsService groupsService, IZipService zipService,
        IRegExpService regExpService, INormalizeService normalizeService, IOlympediaService olympediaService, IDataCacheService dataCacheService,
        OlympicGamesRepository<Venue> venueRepository, OlympicGamesRepository<Event> eventRepository)
        : base(logger, crawlersService, logsService, groupsService, zipService, regExpService, normalizeService, olympediaService, dataCacheService)
    {
        this.venueRepository = venueRepository;
        this.eventRepository = eventRepository;
    }

    protected override async Task ProcessGroupAsync(Group group)
    {
        var model = this.PrepareConverterModel(group);
        var fullName = model.HtmlDocument.DocumentNode.SelectSingleNode("//h1").InnerText.Decode();
        var name = this.RegExpService.MatchFirstGroup(model.HtmlDocument.DocumentNode.OuterHtml, @"<tr>\s*<th>Name<\/th>\s*<td>(.*?)<\/td>\s*<\/tr>")?.Decode();
        var cityName = this.RegExpService.MatchFirstGroup(model.HtmlDocument.DocumentNode.OuterHtml, @"<tr>\s*<th>Place<\/th>\s*<td>(.*?)<\/td>\s*<\/tr>");
        var englishName = this.RegExpService.MatchFirstGroup(model.HtmlDocument.DocumentNode.OuterHtml, @"<tr>\s*<th>English name<\/th>\s*<td>(.*?)<\/td>\s*<\/tr>")?.Decode();
        var opened = this.RegExpService.MatchFirstGroup(model.HtmlDocument.DocumentNode.OuterHtml, @"<tr>\s*<th>Opened<\/th>\s*<td>([\d]+)<\/td>\s*<\/tr>");
        var demolished = this.RegExpService.MatchFirstGroup(model.HtmlDocument.DocumentNode.OuterHtml, @"<tr>\s*<th>Demolished<\/th>\s*<td>([\d]+)<\/td>\s*<\/tr>");
        var capacity = this.RegExpService.MatchFirstGroup(model.HtmlDocument.DocumentNode.OuterHtml, @"<tr>\s*<th>Games Capacity<\/th>\s*<td>([\d]+)<\/td>\s*<\/tr>");
        var coordinatesMatch = this.RegExpService.Match(model.HtmlDocument.DocumentNode.OuterHtml, @"<tr>\s*<th>Coordinates<\/th>\s*<td>([\d\.\-]+),\s*([\d\.\-]+)");

        var venue = new Venue
        {
            Name = name,
            City = this.NormalizeService.NormalizeHostCityName(cityName),
            Number = model.PageId,
            EnglishName = englishName,
            FullName = fullName,
            OpenedYear = opened != null ? int.Parse(opened) : null,
            DemolishedYear = demolished != null ? int.Parse(demolished) : null,
            Capacity = capacity,
            LatitudeCoordinate = coordinatesMatch != null ? double.Parse(coordinatesMatch.Groups[1].Value.Trim().Replace(".", ",")) : null,
            LongitudeCoordinate = coordinatesMatch != null ? double.Parse(coordinatesMatch.Groups[2].Value.Trim().Replace(".", ",")) : null,
        };

        var dbVenue = await this.venueRepository.GetAsync(x => x.Number == model.PageId);
        if (dbVenue != null)
        {
            var equals = venue.Equals(dbVenue);
            if (!equals)
            {
                this.venueRepository.Update(dbVenue);
                await this.venueRepository.SaveChangesAsync();
            }
        }

        var eventsTable = model.HtmlDocument.DocumentNode.SelectSingleNode("//table[@class='table table-striped']");
        var rows = eventsTable.Elements("tr").ToList();
        foreach (var row in rows)
        {
            var tdElements = row.Elements("td").ToList();
            var yearText = this.RegExpService.MatchFirstGroup(tdElements[0].InnerText, @"([\d]+)");
            if (!string.IsNullOrEmpty(yearText))
            {
                var year = int.Parse(yearText);
                var resultId = this.OlympediaService.FindResultNumber(tdElements[2].OuterHtml);
                var @event = await this.eventRepository.GetAsync(x => x.OriginalResultId == resultId);

                if (@event != null)
                {
                    @event.Venues.Add(venue);
                    this.eventRepository.Update(@event);
                    await this.eventRepository.SaveChangesAsync();
                }
            }
        }
    }
}