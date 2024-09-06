namespace SportHub.Converters.OlympicGames.Paris2024;

using System.Text.Json;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using SportHub.Converters.OlympicGames.Paris2024.Base;
using SportHub.Data.Entities.Crawlers;
using SportHub.Data.Entities.OlympicGames;
using SportHub.Data.Models.Crawlers.Paris2024.Person;
using SportHub.Services.Data.CrawlerStorageDb.Interfaces;
using SportHub.Services.Data.OlympicGamesDb;
using SportHub.Services.Data.OlympicGamesDb.Interfaces;
using SportHub.Services.Interfaces;

public class ParticipantConverter : Paris2024Converter
{
    private readonly IDataService<Participation> participationsService;
    private readonly IDataService<Person> personsService;

    public ParticipantConverter(ILogger<BaseConverter> logger, ICrawlersService crawlersService, ILogsService logsService, IGroupsService groupsService, IZipService zipService,
        INormalizeService normalizeService, IDataCacheService dataCacheService, DataService<Participation> participationsService, DataService<Person> personsService)
        : base(logger, crawlersService, logsService, groupsService, zipService, normalizeService, dataCacheService)
    {
        this.participationsService = participationsService;
        this.personsService = personsService;
    }

    protected override async Task ProcessGroupAsync(Group group)
    {
        var converterModel = this.PrepareConverterModel(group);
        var model = JsonSerializer.Deserialize<PersonCrawlerModel>(converterModel.Documents.GetValueOrDefault(1).Json);
        var personJson = model.Person;

        var medals = new Dictionary<string, int>();
        if (personJson.Medals != null)
        {
            foreach (var medal in personJson.Medals)
            {
                var eventCode = this.GenerateCode("Summer", 2024, medal.EventCode);

                if (!medals.ContainsKey(eventCode))
                {
                    medals[eventCode] = int.Parse(medal.MedalType);
                }
            }
        }

        var person = await this.personsService.GetAsync(x => x.Code == personJson.Code);
        var noc = this.DataCacheService.NOCs.FirstOrDefault(x => x.Code == personJson.Organisation.Code);
        var game = this.DataCacheService.Games.FirstOrDefault(x => x.Year == 2024);
        var age = this.CalculateAge(game.OpeningCeremony ?? game.StartCompetitionDate.Value, person.BirthDate);

        if (personJson.RegisteredEvents != null)
        {
            foreach (var regEvent in personJson.RegisteredEvents)
            {
                var eventCode = this.GenerateCode("Summer", 2024, regEvent.Event.Code);
                var @event = this.DataCacheService.Events.FirstOrDefault(x => x.Code == eventCode);

                if (@event != null)
                {
                    var participation = new Participation
                    {
                        Code = personJson.Code,
                        PersonId = person.Id,
                        NOCId = noc.Id,
                        EventId = @event.Id,
                        AgeYears = age.Item1,
                        AgeDays = age.Item2,
                        Medal = medals.GetValueOrDefault(eventCode),
                        Rank = regEvent.Event.Rankings?.FirstOrDefault(x => x.AthleteCode == person.Code)?.RkRank,
                        RankEqual = regEvent.Event.Rankings?.FirstOrDefault(x => x.AthleteCode == person.Code)?.RkRankEqual == "Y",
                        RankResultType = regEvent.Event.Rankings?.FirstOrDefault(x => x.AthleteCode == person.Code)?.RkResultType,
                        RankType = regEvent.Event.Rankings?.FirstOrDefault(x => x.AthleteCode == person.Code)?.RkType,
                    };

                    var dbParticipation = await this.participationsService
                        .GetAsync(x => x.PersonId == participation.PersonId && x.EventId == participation.EventId && x.NOCId == participation.NOCId);
                    if (dbParticipation != null)
                    {
                        var equals = participation.Equals(dbParticipation);
                        if (!equals)
                        {
                            this.participationsService.Update(dbParticipation);
                        }
                    }
                    else
                    {
                        await this.participationsService.AddAsync(participation);
                    }
                }
            }
        }
    }

    private Tuple<int, int> CalculateAge(DateTime? startDate, DateTime? endDate)
    {
        var age = new Tuple<int, int>(0, 0);
        if (!startDate.HasValue || !endDate.HasValue)
        {
            return age;
        }

        var totalDays = (startDate.Value - endDate.Value).TotalDays;
        var year = (int)Math.Floor(totalDays / 365.25);
        var newYear = endDate.Value.AddYears(year);
        var days = (startDate.Value - newYear).TotalDays;
        age = new Tuple<int, int>(year, (int)days);

        return age;
    }
}