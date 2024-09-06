namespace SportHub.Converters.OlympicGames.Paris2024;

using System.Text.Json;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using SportHub.Converters.OlympicGames.Paris2024.Base;
using SportHub.Data.Entities.Crawlers;
using SportHub.Data.Entities.OlympicGames;
using SportHub.Data.Entities.OlympicGames.Meta;
using SportHub.Data.Models.Crawlers.Paris2024.Records;
using SportHub.Services.Data.CrawlerStorageDb.Interfaces;
using SportHub.Services.Data.OlympicGamesDb;
using SportHub.Services.Data.OlympicGamesDb.Interfaces;
using SportHub.Services.Interfaces;

public class RecordsConverter : Paris2024Converter
{
    private readonly DataService<Record> recordsService;

    public RecordsConverter(ILogger<BaseConverter> logger, ICrawlersService crawlersService, ILogsService logsService, IGroupsService groupsService, IZipService zipService,
        INormalizeService normalizeService, IDataCacheService dataCacheService, DataService<Record> recordsService)
        : base(logger, crawlersService, logsService, groupsService, zipService, normalizeService, dataCacheService)
    {
        this.recordsService = recordsService;
    }

    protected override async Task ProcessGroupAsync(Group group)
    {
        var converterModel = this.PrepareConverterModel(group);
        var json = JsonSerializer.Deserialize<RecordsCrawlerModel>(converterModel.Documents.GetValueOrDefault(6).Json);

        if (json.Records != null)
        {
            foreach (var item in json.Records)
            {
                var code = this.GenerateCode("Summer", 2024, item.Record.Event);
                var @event = this.DataCacheService.Events.FirstOrDefault(x => x.Code.StartsWith(code.Substring(0, 25)));
                if (@event != null)
                {
                    var codeInfo = this.ExtractCodeInfo(code);
                    var recordCode = $"{(item.RecordType.Description == "World Record" ? "WR" : "OR")}-{codeInfo.Discipline}-{codeInfo.Gender}-{codeInfo.Event}".PadRight(21, '-');

                    foreach (var data in item.RecordData)
                    {
                        var record = new Record
                        {
                            Name = item.RecordType.Description,
                            Type = item.RecordType.Code,
                            Description = item.Record.Description,
                            EventId = @event.Id,
                            Code = recordCode,
                            Order = data.Order,
                            Current = data.Current,
                            Date = string.IsNullOrEmpty(data.RecordDate) ? null : DateTime.ParseExact(data.RecordDate, "yyyy-MM-dd", null),
                            Result = data.Result,
                            ResultType = data.ResultType,
                            Country = data.Country,
                            Place = data.Place,
                            Competition = data.Competition,
                            Meta = this.PrepareRecordMeta(data),
                        };

                        var dbRecord = await this.recordsService.GetAsync(x => x.Code == recordCode && x.Result == record.Result);
                        if (dbRecord != null)
                        {
                            var equals = record.Equals(dbRecord);
                            if (!equals)
                            {
                                this.recordsService.Update(dbRecord);
                            }
                        }
                        else
                        {
                            await this.recordsService.AddAsync(record);
                        }
                    }
                }
            }
        }
    }

    private string PrepareRecordMeta(RecordDatum data)
    {
        if (data == null || data.Competitor == null)
        {
            return null;
        }

        var meta = new RecordMeta
        {
            Competitor = new CompetitorMeta
            {
                Code = data.Competitor.Code,
                OrganisationCode = data.Competitor.Organisation.Code,
                OrganisationName = data.Competitor.Organisation.Description,
                OrganisationLongName = data.Competitor.Organisation.LongDescription,
                Description = data.Competitor.Description,
                IsTeam = data.Competitor.Participant.Typename == "Team",
                Name = data.Competitor.Participant.Name,
                ShortName = data.Competitor.Participant.ShortName,
                FirstName = data.Competitor.Participant?.GivenName,
                LastName = data.Competitor.Participant?.FamilyName,
            }
        };

        data.Competitor.Athletes.ForEach(x =>
        {
            var participant = new ParticipantMeta
            {
                Code = x.Code,
                Order = x.Order,
                BirthDate = string.IsNullOrEmpty(x.BirthDate) ? null : DateTime.ParseExact(x.BirthDate, "yyyy-MM-dd", null),
                Gender = x.Gender,
                FirstName = x.GivenName,
                LastName = x.FamilyName,
                Organisation = x.Organisation,
                IsTeam = x.Participant.Typename == "Team",
                Name = x.Participant.Name,
                ShortName = x.Participant.ShortName,
            };

            meta.Competitor.Participants.Add(participant);
        });

        if (data.RecordExtensions != null && data.RecordExtensions.Count > 0)
        {
            foreach (var item in data.RecordExtensions)
            {
                meta.Data.Add(new RecordDataMeta
                {
                    Key = item.Code,
                    Value = item.Value,
                });
            }
        }

        var json = JsonSerializer.Serialize(meta);
        return json;
    }
}