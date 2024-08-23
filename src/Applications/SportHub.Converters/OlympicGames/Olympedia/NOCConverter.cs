namespace SportHub.Converters.OlympicGames.Olympedia;

using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using SportHub.Common.Extensions;
using SportHub.Common.Helpers;
using SportHub.Converters.OlympicGames.Olympedia.Base;
using SportHub.Data.Entities.Crawlers;
using SportHub.Data.Entities.OlympicGames;
using SportHub.Data.Repositories;
using SportHub.Services.Data.CrawlerStorageDb.Interfaces;
using SportHub.Services.Data.OlympicGamesDb.Interfaces;
using SportHub.Services.Interfaces;

public class NOCConverter : OlympediaConverter
{
    private readonly OlympicGamesRepository<NOC> repository;

    public NOCConverter(ILogger<BaseConverter> logger, ICrawlersService crawlersService, ILogsService logsService, IGroupsService groupsService, IZipService zipService,
        INormalizeService normalizeService, IDataCacheService dataCacheService, OlympicGamesRepository<NOC> repository)
        : base(logger, crawlersService, logsService, groupsService, zipService, normalizeService, dataCacheService)
    {
        this.repository = repository;
    }

    protected override async Task ProcessGroupAsync(Group group)
    {
        var converterModel = this.PrepareConverterModel(group);
        var country = converterModel.Documents.GetValueOrDefault(1);

        var match = RegExpHelper.Match(country.Title, @"(.*?)\((.*?)\)");
        if (match != null)
        {
            var name = match.Groups[1].Value.Trim();
            var code = match.Groups[2].Value.Trim().ToUpper();

            if (code is not "UNK" and not "CRT")
            {
                var description = RegExpHelper.CutHtml(country.HtmlDocument
                    .DocumentNode
                    .SelectSingleNode("//div[@class='description']")
                    .OuterHtml
                    .Decode());

                var dbNOC = await this.repository.GetAsync(x => x.Code == code);
                if (dbNOC == null)
                {
                    var noc = new NOC
                    {
                        Name = name,
                        Code = code,
                        OfficialName = name,
                        IsMedal = true,
                        IsHistoric = true,
                        SEOName = name.ToLower().Replace(" ", "-")
                    };

                    await this.repository.AddAsync(noc);
                    await this.repository.SaveChangesAsync();

                    dbNOC = await this.repository.GetAsync(x => x.Code == code);
                }

                dbNOC.RelatedNOC = this.FindRelatedCountry(code);
                dbNOC.Description = description;

                var committee = converterModel.Documents.GetValueOrDefault(2);
                if (committee != null)
                {
                    var abbreavition = RegExpHelper.MatchFirstGroup(committee.HtmlDocument.DocumentNode.OuterHtml, @"<tr>\s*<th>Abbreviation<\/th>\s*<td>(.*?)<\/td>\s*<\/tr>");

                    dbNOC.OfficialCommitteeName = committee.Title;
                    dbNOC.CommitteeAbbreviation = !string.IsNullOrEmpty(abbreavition) ? abbreavition : null;
                }

                // TODO: NOCAdministration

                // TODO: FlagBearere

                this.repository.Update(dbNOC);
                await this.repository.SaveChangesAsync();
            }
        }
    }

    private string FindRelatedCountry(string code)
    {
        string relatedCountryCode = null;
        switch (code)
        {
            case "ANZ":
                relatedCountryCode = "AUS";
                break;
            case "TCH":
            case "BOH":
                relatedCountryCode = "CZE";
                break;
            case "GDR":
            case "FRG":
            case "SAA":
                relatedCountryCode = "GER";
                break;
            case "MAL":
            case "NBO":
                relatedCountryCode = "MAS";
                break;
            case "AHO":
                relatedCountryCode = "NED";
                break;
            case "ROC":
            case "EUN":
            case "URS":
                relatedCountryCode = "RUS";
                break;
            case "YUG":
            case "SCG":
                relatedCountryCode = "SRB";
                break;
            case "YMD":
            case "YAR":
                relatedCountryCode = "YEM";
                break;
        }

        return relatedCountryCode;
    }
}