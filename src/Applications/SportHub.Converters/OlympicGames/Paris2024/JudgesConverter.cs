namespace SportHub.Converters.OlympicGames.Paris2024;

using System.Text.Json;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using SportHub.Data.Entities.Crawlers;
using SportHub.Data.Entities.OlympicGames;
using SportHub.Data.Models.Crawlers.Paris2024;
using SportHub.Data.Repositories;
using SportHub.Services.Data.CrawlerStorageDb.Interfaces;
using SportHub.Services.Data.OlympicGamesDb.Interfaces;
using SportHub.Services.Interfaces;

public class JudgesConverter : BaseConverter
{
    private readonly OlympicGamesRepository<Person> repository;

    public JudgesConverter(ILogger<BaseConverter> logger, ICrawlersService crawlersService, ILogsService logsService, IGroupsService groupsService, IZipService zipService,
        INormalizeService normalizeService, IDataCacheService dataCacheService, OlympicGamesRepository<Person> repository)
        : base(logger, crawlersService, logsService, groupsService, zipService, normalizeService, dataCacheService)
    {
        this.repository = repository;
    }

    protected override async Task ProcessGroupAsync(Group group)
    {
        var persons = JsonSerializer.Deserialize<PersonsCrawlerModel>(this.Model.Paris2024Documents.GetValueOrDefault(1).Json);

        var count = 1030001;
        foreach (var item in persons.Persons)
        {
            var person = new Person
            {
                Code = $"{item.MainFunction?.FunctionCode}-{count++}",
                BirthDate = string.IsNullOrEmpty(item.BirthDate) ? null : DateTime.ParseExact(item.BirthDate, "yyyy-MM-dd", null),
                Gender = item.PersonGender.Description,
                Height = item.Height,
                Category = item.MainFunction?.Description,
                Name = item.Name,
                OriginalCode = item.Code,
                ShortName = item.ShortName,
                TvName = item.TVName
            };

            var dbPerson = await this.repository.GetAsync(x => x.OriginalCode == item.Code);
            if (dbPerson != null)
            {
                var equals = person.Equals(dbPerson);
                if (!equals)
                {
                    this.repository.Update(dbPerson);
                    await this.repository.SaveChangesAsync();
                }
            }
            else
            {
                await this.repository.AddAsync(person);
                await this.repository.SaveChangesAsync();
            }
        }
    }
}