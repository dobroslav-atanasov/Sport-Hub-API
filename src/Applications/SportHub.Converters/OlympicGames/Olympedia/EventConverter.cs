namespace SportHub.Converters.OlympicGames.Olympedia;

using System.Threading.Tasks;

using HtmlAgilityPack;

using Microsoft.Extensions.Logging;

using SportHub.Data.Models.DbEntities.Crawlers;
using SportHub.Data.Models.DbEntities.OlympicGames;
using SportHub.Data.Models.Enumerations.OlympicGames;
using SportHub.Data.Repositories;
using SportHub.Services.Data.CrawlerStorageDb.Interfaces;
using SportHub.Services.Data.OlympicGamesDb.Interfaces;
using SportHub.Services.Interfaces;

public class EventConverter : BaseOlympediaConverter
{
    private readonly IDateService dateService;
    private readonly OlympicGamesRepository<Event> eventRepository;

    public EventConverter(ILogger<BaseConverter> logger, ICrawlersService crawlersService, ILogsService logsService, IGroupsService groupsService, IZipService zipService,
        IRegExpService regExpService, INormalizeService normalizeService, IOlympediaService olympediaService, IDataCacheService dataCacheService, IDateService dateService,
        OlympicGamesRepository<Event> eventRepository)
        : base(logger, crawlersService, logsService, groupsService, zipService, regExpService, normalizeService, olympediaService, dataCacheService)
    {
        this.dateService = dateService;
        this.eventRepository = eventRepository;
    }

    protected override async Task ProcessGroupAsync(Group group)
    {
        var model = this.PrepareConverterModel(group);

        if (model.IsValidEvent && !model.EventInfo.IsForbidden)
        {
            var format = this.RegExpService.MatchFirstGroup(model.HtmlDocument.DocumentNode.OuterHtml, @"<tr>\s*<th>Format<\/th>\s*<td colspan=""2"">(.*?)<\/td>\s*<\/tr>");
            var description = model.HtmlDocument.DocumentNode.SelectSingleNode("//div[@class='description']")?.OuterHtml;
            var isTeamEvent = this.IsTeamEvent(model.HtmlDocument, model.EventInfo.NormalizedName);
            var eventGender = this.MapEventGender(model.EventInfo.Name);

            var @event = new Event
            {
                OriginalResultId = model.PageId,
                OriginalName = model.EventInfo.OriginalName,
                Name = model.EventInfo.Name,
                NormalizedName = model.EventInfo.NormalizedName,
                AdditionalInfo = model.EventInfo.AdditionalInfo,
                DisciplineId = model.DisciplineCache.Id,
                GameId = model.GameCache.Id,
                Format = format,
                Description = description != null ? this.RegExpService.CutHtml(description) : null,
                IsTeamEvent = isTeamEvent,
                Gender = eventGender,
            };

            var dateMatch = this.RegExpService.Match(model.HtmlDocument.DocumentNode.OuterHtml, @"<tr>\s*<th>Date<\/th>\s*<td>(.*?)<\/td>\s*<\/tr>");
            if (dateMatch != null)
            {
                var dateModel = this.dateService.ParseDate(dateMatch.Groups[1].Value.Trim());
                @event.StartDate = dateModel.From;
                @event.EndDate = dateModel.To;
            }

            var dbEvent = await this.eventRepository
                .GetAsync(x => x.OriginalName == @event.OriginalName && x.DisciplineId == @event.DisciplineId && x.GameId == @event.GameId && x.Gender == @event.Gender);

            if (dbEvent != null)
            {
                var equals = @event.Equals(dbEvent);
                if (!equals)
                {
                    this.eventRepository.Update(dbEvent);
                    await this.eventRepository.SaveChangesAsync();
                }
            }
            else
            {
                await this.eventRepository.AddAsync(@event);
                await this.eventRepository.SaveChangesAsync();
            }
        }
    }

    private EventGenderEnum MapEventGender(string text)
    {
        var gender = EventGenderEnum.None;

        if (text.StartsWith("men", StringComparison.CurrentCultureIgnoreCase))
        {
            gender = EventGenderEnum.Men;
        }
        else if (text.StartsWith("women", StringComparison.CurrentCultureIgnoreCase))
        {
            gender = EventGenderEnum.Women;
        }
        else if (text.StartsWith("mixed", StringComparison.CurrentCultureIgnoreCase) || text.StartsWith("open", StringComparison.CurrentCultureIgnoreCase))
        {
            gender = EventGenderEnum.Mixed;
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