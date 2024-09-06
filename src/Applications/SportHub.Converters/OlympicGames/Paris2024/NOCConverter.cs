namespace SportHub.Converters.OlympicGames.Paris2024;

using System.Text.Json;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using SportHub.Common.Helpers;
using SportHub.Converters.OlympicGames.Paris2024.Base;
using SportHub.Data.Entities.Crawlers;
using SportHub.Data.Entities.OlympicGames;
using SportHub.Data.Models.Crawlers.Paris2024.NOC;
using SportHub.Services.Data.CrawlerStorageDb.Interfaces;
using SportHub.Services.Data.OlympicGamesDb;
using SportHub.Services.Data.OlympicGamesDb.Interfaces;
using SportHub.Services.Interfaces;

public class NOCConverter : Paris2024Converter
{
    private readonly DataService<NOC> nocsService;

    public NOCConverter(ILogger<BaseConverter> logger, ICrawlersService crawlersService, ILogsService logsService, IGroupsService groupsService, IZipService zipService,
        INormalizeService normalizeService, IDataCacheService dataCacheService, DataService<NOC> nocsService)
        : base(logger, crawlersService, logsService, groupsService, zipService, normalizeService, dataCacheService)
    {
        this.nocsService = nocsService;
    }

    protected override async Task ProcessGroupAsync(Group group)
    {
        var converterModel = this.PrepareConverterModel(group);
        var model = JsonSerializer.Deserialize<NOCInfo>(converterModel.Documents.GetValueOrDefault(1).Json);

        if (model.NocBio != null)
        {
            var noc = await this.nocsService.GetAsync(x => x.Code == model.NocBio.OrganisationId);
            noc.Highlights = model.NocBio.Interest.Highlights;
            noc.Information = model.NocBio.Interest.AddInformation;
            noc.Summary = model.NocBio.Participation.Summary;

            if (int.TryParse(model.NocBio.Membership?.FoundingDate, out var foundingDate))
            {
                noc.FoundingDate = foundingDate;
            }

            if (int.TryParse(model.NocBio.Membership?.DateIOCRecognition, out var recognitionDate))
            {
                noc.IOCRecognitionDate = recognitionDate;
            }

            if (int.TryParse(model.NocBio.Participation?.FirstOGAppearance, out var firstAppearance))
            {
                noc.FirstAppearence = firstAppearance;
            }

            var appearanceMatch = RegExpHelper.Match(model.NocBio.Participation.NumOGAppearance, @"^(\d+)");
            if (appearanceMatch != null && int.TryParse(appearanceMatch.Groups[1].Value, out var appearance))
            {
                noc.Appearance = appearance;
            }

            this.nocsService.Update(noc);
        }
    }
}