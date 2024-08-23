namespace SportHub.Converters.OlympicGames.Paris2024;

using System.Text.Json;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using SportHub.Data.Entities.Crawlers;
using SportHub.Data.Entities.OlympicGames;
using SportHub.Data.Models.Crawlers.Paris2024.Person;
using SportHub.Services.Data.CrawlerStorageDb.Interfaces;
using SportHub.Services.Data.OlympicGamesDb;
using SportHub.Services.Data.OlympicGamesDb.Interfaces;
using SportHub.Services.Interfaces;

public class PersonConverter : BaseConverter
{
    private readonly IDataService<Person> personsService;

    public PersonConverter(ILogger<BaseConverter> logger, ICrawlersService crawlersService, ILogsService logsService, IGroupsService groupsService, IZipService zipService,
        INormalizeService normalizeService, IDataCacheService dataCacheService, DataService<Person> personsService)
        : base(logger, crawlersService, logsService, groupsService, zipService, normalizeService, dataCacheService)
    {
        this.personsService = personsService;
    }

    protected override async Task ProcessGroupAsync(Group group)
    {
        var converterModel = this.PrepareParis2024ConverterModel(group);
        var model = JsonSerializer.Deserialize<PersonCrawlerModel>(converterModel.Paris2024Documents.GetValueOrDefault(1).Json);

        var dbPerson = await this.personsService.GetAsync(x => x.Code == model.Person.Code);
        dbPerson.FirstName = model.Person.GivenName;
        dbPerson.LastName = model.Person.FamilyName;
        dbPerson.Weight = model.Person.Weight;
        dbPerson.Height = dbPerson.Height == 0 ? model.Person.Height : 0;
        dbPerson.Nationality = model.Person.Nationality?.Code;
        dbPerson.PlaceOfBirth = model.Person.ParticipantBio?.PlaceofBirth;
        dbPerson.CountryOfBirth = model.Person.ParticipantBio?.CountryofBirth?.Code;
        dbPerson.Residence = model.Person.ParticipantBio?.PlaceofResidence;
        dbPerson.CountryOfResidence = model.Person.ParticipantBio?.CountryofResidence?.Code;
        dbPerson.Status = model.Person.Status?.Description;

        if (model.Person.ParticipantBio != null)
        {
            if (model.Person.ParticipantBio.Highlights != null)
            {
                dbPerson.HighlightsType = string.Join("|", model.Person.ParticipantBio.Highlights.Select(x => x.Type));
                dbPerson.Highlights = string.Join("|", model.Person.ParticipantBio.Highlights.Select(x => x.Value));
            }

            if (model.Person.ParticipantBio.Interest != null)
            {
                dbPerson.Nickname = model.Person.ParticipantBio.Interest.Nickname;
                dbPerson.Hobbies = model.Person.ParticipantBio.Interest.Hobbies;
                dbPerson.Occupation = model.Person.ParticipantBio.Interest.Occupation;
                dbPerson.Education = model.Person.ParticipantBio.Interest.Education;
                dbPerson.Family = model.Person.ParticipantBio.Interest.Family;
                dbPerson.LanguagesSpoken = model.Person.ParticipantBio.Interest.LangSpoken;
                dbPerson.Coach = model.Person.ParticipantBio.Interest.Coach;
                dbPerson.Debut = model.Person.ParticipantBio.Interest.Debut;
                dbPerson.Start = model.Person.ParticipantBio.Interest.Start;
                dbPerson.Reason = model.Person.ParticipantBio.Interest.Reason;
                dbPerson.Ambition = model.Person.ParticipantBio.Interest.Ambition;
                dbPerson.Milestones = model.Person.ParticipantBio.Interest.Milestones;
                dbPerson.Hero = model.Person.ParticipantBio.Interest.Hero;
                dbPerson.Influence = model.Person.ParticipantBio.Interest.Influence;
                dbPerson.Philosophy = model.Person.ParticipantBio.Interest.Philosophy;
                dbPerson.Award = model.Person.ParticipantBio.Interest.Award;
                dbPerson.AddInformation = model.Person.ParticipantBio.Interest.AddInformation;
            }
        }

        this.personsService.Update(dbPerson);
    }
}