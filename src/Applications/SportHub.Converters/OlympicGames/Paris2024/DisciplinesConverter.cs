namespace SportHub.Converters.OlympicGames.Paris2024;

using System.Text.Json;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using SportHub.Converters.OlympicGames.Paris2024.Base;
using SportHub.Data.Entities.Crawlers;
using SportHub.Data.Entities.OlympicGames;
using SportHub.Data.Models.Crawlers.Paris2024.Disciplines;
using SportHub.Services.Data.CrawlerStorageDb.Interfaces;
using SportHub.Services.Data.OlympicGamesDb;
using SportHub.Services.Data.OlympicGamesDb.Interfaces;
using SportHub.Services.Interfaces;

public class DisciplinesConverter : Paris2024Converter
{
    private readonly DataService<Discipline> disciplinesService;

    public DisciplinesConverter(ILogger<BaseConverter> logger, ICrawlersService crawlersService, ILogsService logsService, IGroupsService groupsService, IZipService zipService,
        INormalizeService normalizeService, IDataCacheService dataCacheService, DataService<Discipline> disciplinesService)
        : base(logger, crawlersService, logsService, groupsService, zipService, normalizeService, dataCacheService)
    {
        this.disciplinesService = disciplinesService;
    }

    protected override async Task ProcessGroupAsync(Group group)
    {
        var converterModel = this.PrepareConverterModel(group);
        var model = JsonSerializer.Deserialize<DisciplinesList>(converterModel.Documents.GetValueOrDefault(1).Json);
        var disciplines = model.Disciplines.Where(x => x.IsSport == true && x.Scheduled == true).ToList();

        foreach (var item in disciplines)
        {
            var sport = this.NormalizeService.MapDisciplineToSport(item.Description);
            var discipline = new Discipline
            {
                Name = item.Description,
                Code = item.Code,
                Sport = sport.Item1,
                SportCode = sport.Item2,
                SEOName = item.Description.ToLower().Replace(" ", "-"),
                IsHistoric = false
            };

            var dbDiscipline = await this.disciplinesService.GetAsync(x => x.Code == item.Code);
            if (dbDiscipline != null)
            {
                var equals = discipline.Equals(dbDiscipline);
                if (!equals)
                {
                    this.disciplinesService.Update(dbDiscipline);
                }
            }
            else
            {
                await this.disciplinesService.AddAsync(discipline);
            }
        }
    }
}