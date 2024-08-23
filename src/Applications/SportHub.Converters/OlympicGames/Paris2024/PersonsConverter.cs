namespace SportHub.Converters.OlympicGames.Paris2024;

using System.Text.Json;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using SportHub.Converters.OlympicGames.Paris2024.Base;
using SportHub.Data.Entities.Crawlers;
using SportHub.Data.Entities.OlympicGames;
using SportHub.Data.Models.Crawlers.Paris2024;
using SportHub.Services.Data.CrawlerStorageDb.Interfaces;
using SportHub.Services.Data.OlympicGamesDb;
using SportHub.Services.Data.OlympicGamesDb.Interfaces;
using SportHub.Services.Interfaces;

public class PersonsConverter : Paris2024Converter
{
    private readonly IDataService<Person> dataService;

    public PersonsConverter(ILogger<BaseConverter> logger, ICrawlersService crawlersService, ILogsService logsService, IGroupsService groupsService, IZipService zipService,
        INormalizeService normalizeService, IDataCacheService dataCacheService, DataService<Person> dataService)
        : base(logger, crawlersService, logsService, groupsService, zipService, normalizeService, dataCacheService)
    {
        this.dataService = dataService;
    }

    protected override async Task ProcessGroupAsync(Group group)
    {
        var converterModel = this.PrepareConverterModel(group);
        var model = JsonSerializer.Deserialize<PersonsCrawlerModel>(converterModel.Documents.GetValueOrDefault(1).Json);

        var persons = new List<Person>();
        foreach (var item in model.Persons)
        {
            var person = new Person
            {
                Code = item.Code,
                BirthDate = string.IsNullOrEmpty(item.BirthDate) ? null : DateTime.ParseExact(item.BirthDate, "yyyy-MM-dd", null),
                Gender = item.PersonGender.Description,
                Height = item.Height,
                Name = item.Name,
                ShortName = item.ShortName,
                TvName = item.TVName,
                Organisation = item.Organisation?.Code,
                CategoryCode = item.MainFunction?.FunctionCode ?? "JU",
                CategoryGroup = item.MainFunction?.Category ?? "J",
                CategoryName = item.MainFunction?.Description ?? "Judge",
            };

            persons.Add(person);
        }

        await this.dataService.AddRangeAsync(persons);
    }
}