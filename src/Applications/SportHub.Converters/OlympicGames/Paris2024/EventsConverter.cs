namespace SportHub.Converters.OlympicGames.Paris2024;

using System.Text.Json;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using SportHub.Common.Helpers;
using SportHub.Converters.OlympicGames.Paris2024.Base;
using SportHub.Data.Entities.Crawlers;
using SportHub.Data.Entities.OlympicGames;
using SportHub.Data.Models.Crawlers.Paris2024.Event;
using SportHub.Services.Data.CrawlerStorageDb.Interfaces;
using SportHub.Services.Data.OlympicGamesDb;
using SportHub.Services.Data.OlympicGamesDb.Interfaces;
using SportHub.Services.Interfaces;

public class EventsConverter : Paris2024Converter
{
    private readonly DataService<Event> eventsService;

    public EventsConverter(ILogger<BaseConverter> logger, ICrawlersService crawlersService, ILogsService logsService, IGroupsService groupsService, IZipService zipService,
        INormalizeService normalizeService, IDataCacheService dataCacheService, DataService<Event> eventsService)
        : base(logger, crawlersService, logsService, groupsService, zipService, normalizeService, dataCacheService)
    {
        this.eventsService = eventsService;
    }

    protected override async Task ProcessGroupAsync(Group group)
    {
        var converterModel = this.PrepareConverterModel(group);
        var json = JsonSerializer.Deserialize<EventCrawlerModel>(converterModel.Documents.GetValueOrDefault(3).Json);
        var eventCrawlerModel = json.Event;

        var disciplineCode = eventCrawlerModel.Code.Substring(0, 3);
        var discipline = this.DataCacheService.Disciplines.FirstOrDefault(x => x.Code == disciplineCode);
        var code = this.GenerateCode("Summer", 2024, eventCrawlerModel.Code);
        var codeInfo = this.ExtractCodeInfo(code);
        var game = this.DataCacheService.Games.FirstOrDefault(x => x.Year == 2024);

        var @event = new Event
        {
            Code = code,
            Name = this.ClearName(eventCrawlerModel.Description),
            Gender = codeInfo.Gender,
            IsTeam = eventCrawlerModel.IsTeam,
            LongName = eventCrawlerModel.LongDescription,
            DisciplineId = discipline.Id,
            OriginalCode = eventCrawlerModel.Code,
            ShortName = codeInfo.Event,
            GameId = game.Id
        };

        foreach (var phaseEvent in eventCrawlerModel.Phases.Where(x => x.Units != null))
        {
            var phaseCode = this.GenerateCode("Summer", 2024, phaseEvent.Code);
            var phase = new Phase
            {
                Code = phaseCode,
                Name = this.ClearName(phaseEvent.Description),
                LongName = this.ClearName(phaseEvent.LongDescription),
                ShortName = phaseEvent.ShortDescription,
                Order = phaseEvent.Order,
                OriginalCode = phaseEvent.Code,
            };

            foreach (var unitEvent in phaseEvent.Units)
            {
                var unitCode = this.GenerateCode("Summer", 2024, unitEvent.Code);

                var unit = new Unit
                {
                    Code = unitCode,
                    Name = this.ClearName(unitEvent.Description),
                    LongName = this.ClearName(unitEvent.LongDescription),
                    ShortName = this.ClearName(unitEvent.ShortDescription),
                    Scheduled = unitEvent.Scheduled,
                    Type = unitEvent.Type,
                    Order = unitEvent.Order,
                    StartDate = unitEvent.Schedule.StartDate,
                    EndDate = unitEvent.Schedule.EndDate,
                    OriginalCode = unitEvent.Code,
                    HasMedal = unitEvent.Schedule.Medal == 1,
                    Status = unitEvent.Schedule.Status.Code
                };

                phase.Units.Add(unit);
            }

            @event.Phases.Add(phase);
        }

        var dbEvent = await this.eventsService.GetAsync(x => x.Code == code);
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

    private string ClearName(string name)
    {
        var spaceMatch = RegExpHelper.Match(name, @"([\d]+) kg");
        if (spaceMatch != null)
        {
            name = name.Replace(spaceMatch.Groups[0].Value, $"{spaceMatch.Groups[1].Value}kg");
        }

        var parts = name.Split(" ", StringSplitOptions.RemoveEmptyEntries).ToList();

        var firstPart = string.Empty;
        var list = new List<string>();
        foreach (var part in parts)
        {
            if (part.StartsWith("Men"))
            {
                firstPart = "Men's";
            }
            else if (part.StartsWith("Women"))
            {
                firstPart = "Women's";
            }
            else if (part.StartsWith("Mixed"))
            {
                firstPart = part;
            }
            else
            {
                var match = RegExpHelper.Match(part, @"(\+|-)([\d]+)\s*kg");
                if (match != null)
                {
                    var text = match.Groups[1].Value == "+" ? "+" : string.Empty;
                    text = $"{text}{match.Groups[2].Value}kg";
                    list.Add(text);
                }
                else
                {
                    list.Add(part);
                }
            }
        }

        return $"{firstPart} {string.Join(" ", list)}".Trim();
    }
}