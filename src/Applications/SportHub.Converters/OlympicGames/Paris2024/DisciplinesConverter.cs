namespace SportHub.Converters.OlympicGames.Paris2024;

using System.Text.Json;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using SportHub.Data.Models.Crawlers.Paris2024.Disciplines;
using SportHub.Data.Models.DbEntities.Crawlers;
using SportHub.Data.Models.DbEntities.OlympicGames;
using SportHub.Data.Repositories;
using SportHub.Services.Data.CrawlerStorageDb.Interfaces;
using SportHub.Services.Interfaces;

public class DisciplinesConverter : BaseConverter
{
    private readonly OlympicGamesRepository<Discipline> repository;

    public DisciplinesConverter(ILogger<BaseConverter> logger, ICrawlersService crawlersService, ILogsService logsService, IGroupsService groupsService, IZipService zipService,
        INormalizeService normalizeService, IRegExpService regExpService, OlympicGamesRepository<Discipline> repository)
        : base(logger, crawlersService, logsService, groupsService, zipService, normalizeService, regExpService)
    {
        this.repository = repository;
    }

    protected override async Task ProcessGroupAsync(Group group)
    {
        var json = JsonSerializer.Deserialize<DisciplinesList>(this.Model.Paris2024Documents.GetValueOrDefault(1).Json);
        var disciplines = json.Disciplines.Where(x => x.IsSport == true && x.Scheduled == true).ToList();

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

            var dbDiscipline = await this.repository.GetAsync(x => x.Code == item.Code);
            if (dbDiscipline != null)
            {
                var equals = discipline.Equals(dbDiscipline);
                if (!equals)
                {
                    this.repository.Update(dbDiscipline);
                    await this.repository.SaveChangesAsync();
                }
            }
            else
            {
                await this.repository.AddAsync(discipline);
                await this.repository.SaveChangesAsync();
            }
        }
    }
}