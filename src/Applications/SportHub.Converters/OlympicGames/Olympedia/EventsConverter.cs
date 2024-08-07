namespace SportHub.Converters.OlympicGames.Olympedia;

using System.Threading.Tasks;

using HtmlAgilityPack;

using Microsoft.Extensions.Logging;

using SportHub.Common.Helpers;
using SportHub.Data.Models.DbEntities.Crawlers;
using SportHub.Data.Models.DbEntities.OlympicGames;
using SportHub.Data.Repositories;
using SportHub.Services.Data.CrawlerStorageDb.Interfaces;
using SportHub.Services.Data.OlympicGamesDb.Interfaces;
using SportHub.Services.Interfaces;

public class EventsConverter : BaseConverter
{
    private readonly OlympicGamesRepository<Event> eventRepository;

    public EventsConverter(ILogger<BaseConverter> logger, ICrawlersService crawlersService, ILogsService logsService, IGroupsService groupsService, IZipService zipService,
        INormalizeService normalizeService, IDataCacheService dataCacheService, OlympicGamesRepository<Event> eventRepository)
        : base(logger, crawlersService, logsService, groupsService, zipService, normalizeService, dataCacheService)
    {
        this.eventRepository = eventRepository;
    }

    protected override async Task ProcessGroupAsync(Group group)
    {
        var model = this.Model.OlympediaDocuments.GetValueOrDefault(1);

        if (model.IsValidEvent && !model.EventInfo.IsForbidden)
        {
            var @event = new Event
            {
                Name = model.EventInfo.FullName,
                Order = 1, // TODO 
                LongName = model.EventInfo.FullName,
                ShortName = this.NormalizeService.GetShortEventName(model.EventInfo.Name),
                IsTeam = this.IsTeamEvent(model.HtmlDocument, model.EventInfo.FullName),
                DisciplineId = model.Discipline.Id,
                Description = model.EventInfo.Description,
                Gender = this.GetGender(model.EventInfo.Gender)
            };

            var code = this.GenerateEventCode(model.Game.Type, model.Game.Year, model.Discipline.Code, @event.Gender, @event.ShortName);
            @event.Code = code;
            @event.SEOName = this.NormalizeService.CreateSEOName(@event.Name);

            var dbEvent = await this.eventRepository.GetAsync(x => x.Code == code);
            //if (dbEvent != null)
            //{
            //    var equals = @event.Equals(dbEvent);
            //    if (!equals)
            //    {
            //        this.eventRepository.Update(dbEvent);
            //        await this.eventRepository.SaveChangesAsync();
            //    }
            //}
            //else
            //{
            //    await this.eventRepository.AddAsync(@event);
            //    await this.eventRepository.SaveChangesAsync();
            //}

            //dbEvent = await this.eventRepository.GetAsync(x => x.Code == code);


        }
    }

    private bool IsTeamEvent(HtmlDocument document, string name)
    {
        var table = document.DocumentNode.SelectSingleNode("//table[@class='table table-striped']");
        var athletes = OlympediaHelper.FindAthletes(table.OuterHtml);
        var codes = OlympediaHelper.FindNOCCodes(table.OuterHtml);
        var isTeamEvent = false;
        if (athletes.Count != codes.Count)
        {
            isTeamEvent = true;
        }

        if (name.ToLower().Contains("individual"))
        {
            isTeamEvent = false;
        }

        return isTeamEvent;
    }
}