namespace SportHub.Converters.OlympicGames.Olympedia;

using System.Threading.Tasks;

using HtmlAgilityPack;

using Microsoft.Extensions.Logging;

using SportHub.Common.Extensions;
using SportHub.Data.Models.DbEntities.Crawlers;
using SportHub.Data.Models.DbEntities.OlympicGames;
using SportHub.Data.Repositories;
using SportHub.Services.Data.CrawlerStorageDb.Interfaces;
using SportHub.Services.Data.OlympicGamesDb.Interfaces;
using SportHub.Services.Interfaces;

public class NationalOlympicCommitteeConverter : BaseOlympediaConverter
{
    private readonly OlympicGamesRepository<NOC> repository;

    public NationalOlympicCommitteeConverter(ILogger<BaseConverter> logger, ICrawlersService crawlersService, ILogsService logsService, IGroupsService groupsService, IZipService zipService,
        IRegExpService regExpService, INormalizeService normalizeService, IOlympediaService olympediaService, IDataCacheService dataCacheService,
        OlympicGamesRepository<NOC> repository)
        : base(logger, crawlersService, logsService, groupsService, zipService, regExpService, normalizeService, olympediaService, dataCacheService)
    {
        this.repository = repository;
    }

    protected override async Task ProcessGroupAsync(Group group)
    {
        var model = this.PrepareConverterModel(group);
        var match = this.RegExpService.Match(model.Title, @"(.*?)\((.*?)\)");
        if (match != null)
        {
            var name = match.Groups[1].Value.Decode().Trim();
            var code = match.Groups[2].Value.Decode().Trim().ToUpper();

            if (code is not null and not "UNK" and not "CRT")
            {
                var description = this.RegExpService.CutHtml(model.HtmlDocument
                    .DocumentNode
                    .SelectSingleNode("//div[@class='description']")
                    .OuterHtml
                    .Decode());

                var nationalOlympicCommittee = new NOC
                {
                    Name = name,
                    Code = code,
                    RelatedCode = this.FindRelatedCountry(code),
                    Description = description
                };

                if (group.Documents.Count > 1)
                {
                    var committeeDocument = this.CreateHtmlDocument(group.Documents.Single(d => d.Order == 2));
                    var officialName = committeeDocument.DocumentNode.SelectSingleNode("//h1").InnerText.Decode();
                    nationalOlympicCommittee.OfficialName = officialName;

                    var abbreavition = this.RegExpService.MatchFirstGroup(committeeDocument.DocumentNode.OuterHtml, @"<tr>\s*<th>Abbreviation<\/th>\s*<td>(.*?)<\/td>\s*<\/tr>");
                    var foundedYear = this.RegExpService.MatchFirstGroup(committeeDocument.DocumentNode.OuterHtml, @"<tr>\s*<th>Founded<\/th>\s*<td>([\d]*)<\/td>\s*<\/tr>");
                    var disbandedYear = this.RegExpService.MatchFirstGroup(committeeDocument.DocumentNode.OuterHtml, @"<tr>\s*<th>Disbanded<\/th>\s*<td>([\d]*)<\/td>\s*<\/tr>");
                    var recognizedYear = this.RegExpService.MatchFirstGroup(committeeDocument.DocumentNode.OuterHtml, @"<tr>\s*<th>Recognized by the IOC<\/th>\s*<td>([\d]*)<\/td>\s*<\/tr>");

                    nationalOlympicCommittee.Abbreviation = !string.IsNullOrEmpty(abbreavition) ? abbreavition : null;
                    nationalOlympicCommittee.Created = !string.IsNullOrEmpty(foundedYear) ? int.Parse(foundedYear) : null;
                    nationalOlympicCommittee.Disbanded = !string.IsNullOrEmpty(disbandedYear) ? int.Parse(disbandedYear) : null;
                    nationalOlympicCommittee.Recognized = !string.IsNullOrEmpty(recognizedYear) ? int.Parse(recognizedYear) : null;

                    var presidents = this.ExtractNOCPresidents(committeeDocument);
                    nationalOlympicCommittee.Presidents = presidents;
                }

                var worldCode = this.NormalizeService.MapOlympicGamesCountriesAndWorldCountries(nationalOlympicCommittee.Code);
                nationalOlympicCommittee.WorldCode = worldCode;

                var dbNoc = await this.repository.GetAsync(x => x.Name == name && x.Code == code);
                if (dbNoc != null)
                {
                    var equals = nationalOlympicCommittee.Equals(dbNoc);
                    if (!equals)
                    {
                        this.repository.Update(dbNoc);
                        await this.repository.SaveChangesAsync();
                    }
                }
                else
                {
                    await this.repository.AddAsync(nationalOlympicCommittee);
                    await this.repository.SaveChangesAsync();
                }
            }
        }
    }

    private IList<President> ExtractNOCPresidents(HtmlDocument document)
    {
        var presidentTable = document.DocumentNode.SelectSingleNode("//table[@class='table table-striped']");
        var presidents = new List<President>();
        if (presidentTable != null)
        {
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(presidentTable.OuterHtml);

            var rows = htmlDocument.DocumentNode.SelectNodes("//tr");
            if (rows != null)
            {
                foreach (var row in rows.Skip(1))
                {
                    var tdElements = row.Elements("td").ToList();
                    var years = this.GetYears(tdElements[0].InnerText);
                    var president = new President
                    {
                        Name = tdElements[1].InnerText.Trim(),
                        From = years.Item1,
                        To = years.Item2,
                    };

                    presidents.Add(president);
                }
            }
        }

        return presidents;
    }

    private Tuple<int?, int?> GetYears(string text)
    {
        var match = this.RegExpService.Match(text, @"(\d+)?—(\d+)?");
        var tuple = Tuple.Create<int?, int?>(null, null);
        if (match != null)
        {
            int.TryParse(match.Groups[1].Value, out var from);
            int.TryParse(match.Groups[2].Value, out var to);

            tuple = Tuple.Create<int?, int?>(from == 0 ? null : from, to == 0 ? null : to);
        }

        return tuple;
    }

    public string FindRelatedCountry(string code)
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