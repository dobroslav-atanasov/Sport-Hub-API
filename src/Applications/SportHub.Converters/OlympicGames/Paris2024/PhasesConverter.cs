namespace SportHub.Converters.OlympicGames.Paris2024;

using System.Text.Json;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using SportHub.Converters.OlympicGames.Paris2024.Base;
using SportHub.Data.Entities.Crawlers;
using SportHub.Data.Entities.OlympicGames;
using SportHub.Data.Models.Crawlers.Paris2024.Event;
using SportHub.Data.Models.Crawlers.Paris2024.RankingsPhases;
using SportHub.Services.Data.CrawlerStorageDb.Interfaces;
using SportHub.Services.Data.OlympicGamesDb;
using SportHub.Services.Data.OlympicGamesDb.Interfaces;
using SportHub.Services.Interfaces;

public class PhasesConverter : Paris2024Converter
{
    private readonly DataService<Phase> phasesService;

    public PhasesConverter(ILogger<BaseConverter> logger, ICrawlersService crawlersService, ILogsService logsService, IGroupsService groupsService, IZipService zipService,
        INormalizeService normalizeService, IDataCacheService dataCacheService, DataService<Phase> phasesService)
        : base(logger, crawlersService, logsService, groupsService, zipService, normalizeService, dataCacheService)
    {
        this.phasesService = phasesService;
    }

    protected override async Task ProcessGroupAsync(Group group)
    {
        var converterModel = this.PrepareConverterModel(group);
        var json = JsonSerializer.Deserialize<RankingsPhasesCrawlerModel>(converterModel.Documents.GetValueOrDefault(2).Json);
        var eventPhasesJson = JsonSerializer.Deserialize<EventCrawlerModel>(converterModel.Documents.GetValueOrDefault(3).Json);
        var phasesJson = json.Event.Phases;

        var phases = phasesJson.Where(x => x.Units != null);
        foreach (var item in phasesJson.Where(x => x.Units != null))
        {
            var units = item.Units.Where(x => x.Result != null);
            if (units.Any())
            {
                var originalPhaseCode = units.Select(x => x.Result.EventUnitCode.Substring(0, 26)).ToHashSet().FirstOrDefault();
                var phaseCode = this.GenerateCode("Summer", 2024, originalPhaseCode);
                var phaseCodeInfo = this.ExtractCodeInfo(phaseCode);
                var @event = this.DataCacheService.Events.FirstOrDefault(x => x.OriginalCode.StartsWith(originalPhaseCode.Substring(0, 22)));

                var phase = new Phase
                {
                    Code = phaseCode,
                    EventId = @event.Id,
                    OriginalCode = @event.OriginalCode,
                    Name = this.ClearName(item.Description),
                    LongName = this.ClearName(item.LongDescription),
                    ShortName = item.ShortDescription,
                    Order = item.Order,
                };

                foreach (var unitItem in units)
                {
                    var eventPhases = eventPhasesJson.Event.Phases.FirstOrDefault(x => x.Code == originalPhaseCode);
                    var searchedUnit = eventPhases.Units.FirstOrDefault(x => x.Code == unitItem.Result.EventUnitCode);
                    var unitCode = this.GenerateCode("Summer", 2024, unitItem.Result.EventUnitCode);

                    var unit = new Unit
                    {
                        Code = unitCode,
                        Name = this.ClearName(unitItem.Description),
                        LongName = this.ClearName(unitItem.LongDescription),
                        ShortName = this.ClearName(unitItem.ShortDescription),
                        Scheduled = unitItem.Scheduled,
                        Type = unitItem.Type,
                        Order = unitItem.Order,
                        StartDate = searchedUnit.Schedule.StartDate,
                        EndDate = searchedUnit.Schedule.EndDate,
                        OriginalCode = unitItem.Result.EventUnitCode,
                        HasMedal = searchedUnit.Schedule.Medal == 1,
                        Status = searchedUnit.Schedule.Status.Code
                    };

                    phase.Units.Add(unit);
                }

                var dbPhase = await this.phasesService.GetAsync(x => x.Code == phaseCode);
                if (dbPhase != null)
                {
                    var equals = phase.Equals(dbPhase);
                    if (!equals)
                    {
                        this.phasesService.Update(dbPhase);
                    }
                }
                else
                {
                    await this.phasesService.AddAsync(phase);
                }
            }
        }
    }
}