namespace SportData.Converters.OlympicGames;

using System.Threading.Tasks;

using HtmlAgilityPack;

using Microsoft.Extensions.Logging;

using SportData.Data.Models.Entities.Crawlers;
using SportData.Data.Models.Entities.OlympicGames;
using SportData.Data.Models.Entities.OlympicGames.Enumerations;
using SportData.Services.Data.CrawlerStorageDb.Interfaces;
using SportData.Services.Data.OlympicGamesDb;
using SportData.Services.Data.OlympicGamesDb.Interfaces;
using SportData.Services.Interfaces;

public class EventConverter : BaseOlympediaConverter
{
    private readonly IDateService dateService;
    private readonly OlympicGamesRepositoryService<Event> eventsService;

    public EventConverter(ILogger<BaseConverter> logger, ICrawlersService crawlersService, ILogsService logsService, IGroupsService groupsService, IZipService zipService,
        IRegExpService regExpService, INormalizeService normalizeService, IOlympediaService olympediaService, IDataCacheService dataCacheService, IDateService dateService,
        OlympicGamesRepositoryService<Event> eventsService)
        : base(logger, crawlersService, logsService, groupsService, zipService, regExpService, normalizeService, olympediaService, dataCacheService)
    {
        this.dateService = dateService;
        this.eventsService = eventsService;
    }

    protected override async Task ProcessGroupAsync(Group group)
    {
        try
        {
            var document = this.CreateHtmlDocument(group.Documents.OrderBy(d => d.Order).FirstOrDefault());
            var originalEventName = document.DocumentNode.SelectSingleNode("//ol[@class='breadcrumb']/li[@class='active']").InnerText;
            var gameCache = this.GetGameFromDatabase(document);
            var disciplineCache = this.GetDisciplineFromDatabase(document);
            var eventModel = this.CreateEventModel(originalEventName, gameCache, disciplineCache);
            if (eventModel != null && !this.CheckForbiddenEvent(eventModel.OriginalName, disciplineCache.Name, gameCache.Year))
            {
                var format = this.RegExpService.MatchFirstGroup(document.DocumentNode.OuterHtml, @"<tr>\s*<th>Format<\/th>\s*<td colspan=""2"">(.*?)<\/td>\s*<\/tr>");
                var description = document.DocumentNode.SelectSingleNode("//div[@class='description']")?.OuterHtml;
                var isTeamEvent = this.IsTeamEvent(document, eventModel.NormalizedName);
                var eventGenderTypeEnum = this.MapEventGenderType(eventModel.Name);

                var @event = new Event
                {
                    OriginalName = eventModel.OriginalName,
                    Name = eventModel.Name,
                    NormalizedName = eventModel.NormalizedName,
                    AdditionalInfo = eventModel.AdditionalInfo,
                    DisciplineId = disciplineCache.Id,
                    GameId = gameCache.Id,
                    Format = format,
                    Description = description != null ? this.RegExpService.CutHtml(description) : null,
                    IsTeamEvent = isTeamEvent,
                    EventGenderTypeId = (int)eventGenderTypeEnum,
                };

                var dateMatch = this.RegExpService.Match(document.DocumentNode.OuterHtml, @"<tr>\s*<th>Date<\/th>\s*<td>(.*?)<\/td>\s*<\/tr>");
                if (dateMatch != null)
                {
                    var dateModel = this.dateService.ParseDate(dateMatch.Groups[1].Value.Trim());
                    @event.StartDate = dateModel.From;
                    @event.EndDate = dateModel.To;
                }

                var match = this.RegExpService.Match(document.DocumentNode.OuterHtml, @"<tr>\s*<th>Location<\/th>\s*<td>(.*?)<\/td>\s*<\/tr>");
                if (match != null)
                {
                    var venues = this.OlympediaService.FindVenues(match.Groups[1].Value);

                    foreach (var venue in venues)
                    {
                        var venueCache = this.DataCacheService.Venues.FirstOrDefault(v => v.Number == venue);
                        if (venueCache != null)
                        {
                            @event.EventsVenues.Add(new EventVenue { VenueId = venueCache.Id });
                        }
                    }
                }

                var dbEvent = await this.eventsService.GetAsync(x => x.OriginalName == @event.OriginalName && x.DisciplineId == @event.DisciplineId
                    && x.GameId == @event.GameId && x.EventGenderTypeId == @event.EventGenderTypeId);

                if (dbEvent != null)
                {
                    var equals = @event.Equals(dbEvent);
                    if (!equals)
                    {
                        this.eventsService.Update(dbEvent);
                    }
                }
                else
                {
                    await this.eventsService.AddAsync(@event);
                }
            }
        }
        catch (Exception ex)
        {
            this.Logger.LogError(ex, $"Failed to process group: {group.Identifier}");
        }
    }

    private EventGenderTypeEnum MapEventGenderType(string text)
    {
        var gender = EventGenderTypeEnum.None;

        if (text.StartsWith("men", StringComparison.CurrentCultureIgnoreCase))
        {
            gender = EventGenderTypeEnum.Men;
        }
        else if (text.StartsWith("women", StringComparison.CurrentCultureIgnoreCase))
        {
            gender = EventGenderTypeEnum.Women;
        }
        else if (text.StartsWith("mixed", StringComparison.CurrentCultureIgnoreCase) || text.StartsWith("open", StringComparison.CurrentCultureIgnoreCase))
        {
            gender = EventGenderTypeEnum.Mixed;
        }

        return gender;
    }

    private bool IsTeamEvent(HtmlDocument document, string eventNormalizeName)
    {
        var table = document.DocumentNode.SelectSingleNode("//table[@class='table table-striped']");
        var athletes = this.OlympediaService.FindAthletes(table.OuterHtml);
        var codes = this.OlympediaService.FindNOCCodes(table.OuterHtml);
        var isTeamEvent = false;
        if (athletes.Count != codes.Count)
        {
            isTeamEvent = true;
        }

        if (eventNormalizeName.ToLower().Contains("individual"))
        {
            isTeamEvent = false;
        }

        return isTeamEvent;
    }
}