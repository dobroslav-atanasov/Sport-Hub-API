namespace SportHub.Converters.OlympicGames.Paris2024;

using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using SportHub.Data.Models.DbEntities.Crawlers;
using SportHub.Data.Models.DbEntities.OlympicGames;
using SportHub.Data.Repositories;
using SportHub.Services.Data.CrawlerStorageDb.Interfaces;
using SportHub.Services.Interfaces;

public class NOCConverter : BaseConverter
{
    private readonly OlympicGamesRepository<NOC> repository;
    private readonly IRegExpService regExpService;

    public NOCConverter(ILogger<BaseConverter> logger, ICrawlersService crawlersService, ILogsService logsService, IGroupsService groupsService, IZipService zipService,
        OlympicGamesRepository<NOC> repository, IRegExpService regExpService)
        : base(logger, crawlersService, logsService, groupsService, zipService)
    {
        this.repository = repository;
        this.regExpService = regExpService;
    }

    protected override async Task ProcessGroupAsync(Group group)
    {
        var document = group.Documents.FirstOrDefault();
        var bytes = document.Content;
        var encoding = Encoding.GetEncoding(document.Encoding);
        var json = encoding.GetString(bytes);
        var model = JsonSerializer.Deserialize<Data.Models.Crawlers.Paris2024.NOCs.NOCInfo>(json);

        if (model.NocBio != null)
        {
            var noc = await this.repository.GetAsync(x => x.Code == model.NocBio.OrganisationId);
            noc.Highlights = model.NocBio.Interest.Highlights;
            noc.Information = model.NocBio.Interest.AddInformation;
            noc.Summary = model.NocBio.Participation.Summary;

            if (int.TryParse(model.NocBio.Membership.FoundingDate, out var foundingDate))
            {
                noc.FoundingDate = foundingDate;
            }

            if (int.TryParse(model.NocBio.Membership.DateIOCRecognition, out var recognitionDate))
            {
                noc.IOCRecognitionDate = recognitionDate;
            }

            if (int.TryParse(model.NocBio.Participation.FirstOGAppearance, out var firstAppearance))
            {
                noc.FirstAppearence = firstAppearance;
            }

            var appearanceMatch = this.regExpService.Match(model.NocBio.Participation.NumOGAppearance, @"^(\d+)");
            if (appearanceMatch != null && int.TryParse(appearanceMatch.Groups[1].Value, out var appearance))
            {
                noc.Appearance = appearance;
            }

            this.repository.Update(noc);
            await this.repository.SaveChangesAsync();
        }
    }
}