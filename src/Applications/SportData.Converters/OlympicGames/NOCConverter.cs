namespace SportData.Converters.OlympicGames;

using System.Threading.Tasks;

using HtmlAgilityPack;

using Microsoft.Extensions.Logging;

using SportData.Common.Extensions;
using SportData.Data.Models.Entities.Crawlers;
using SportData.Data.Models.Entities.OlympicGames;
using SportData.Data.Repositories;
using SportData.Services.Data.CrawlerStorageDb.Interfaces;
using SportData.Services.Data.OlympicGamesDb.Interfaces;
using SportData.Services.Interfaces;

public class NOCConverter : BaseOlympediaConverter
{
    private readonly OlympicGamesRepository<NOC> repository;

    public NOCConverter(ILogger<BaseConverter> logger, ICrawlersService crawlersService, ILogsService logsService, IGroupsService groupsService, IZipService zipService,
        IRegExpService regExpService, INormalizeService normalizeService, IOlympediaService olympediaService, IDataCacheService dataCacheService,
        OlympicGamesRepository<NOC> repository)
        : base(logger, crawlersService, logsService, groupsService, zipService, regExpService, normalizeService, olympediaService, dataCacheService)
    {
        this.repository = repository;
    }

    protected override async Task ProcessGroupAsync(Group group)
    {
        try
        {
            var document = this.CreateHtmlDocument(group.Documents.Single(d => d.Order == 1));
            var header = document.DocumentNode.SelectSingleNode("//h1");
            var match = this.RegExpService.Match(header.InnerText, @"(.*?)\((.*?)\)");
            if (match != null)
            {
                var name = match.Groups[1].Value.Decode().Trim();
                var code = match.Groups[2].Value.Decode().Trim().ToUpper();

                if (code is not null and not "UNK" and not "CRT")
                {
                    var description = this.RegExpService.CutHtml(document
                        .DocumentNode
                        .SelectSingleNode("//div[@class='description']")
                        .OuterHtml
                        .Decode());

                    var noc = new NOC
                    {
                        Name = name,
                        Code = code,
                        RelatedNOCCode = this.FindRelatedCountry(code),
                        Description = description
                    };

                    if (group.Documents.Count > 1)
                    {
                        var committeeDocument = this.CreateHtmlDocument(group.Documents.Single(d => d.Order == 2));
                        var officialName = committeeDocument.DocumentNode.SelectSingleNode("//h1").InnerText.Decode();
                        noc.OfficialName = officialName;

                        var abbreavition = this.RegExpService.MatchFirstGroup(committeeDocument.DocumentNode.OuterHtml, @"<tr>\s*<th>Abbreviation<\/th>\s*<td>(.*?)<\/td>\s*<\/tr>");
                        var foundedYear = this.RegExpService.MatchFirstGroup(committeeDocument.DocumentNode.OuterHtml, @"<tr>\s*<th>Founded<\/th>\s*<td>([\d]*)<\/td>\s*<\/tr>");
                        var disbandedYear = this.RegExpService.MatchFirstGroup(committeeDocument.DocumentNode.OuterHtml, @"<tr>\s*<th>Disbanded<\/th>\s*<td>([\d]*)<\/td>\s*<\/tr>");
                        var recognizedYear = this.RegExpService.MatchFirstGroup(committeeDocument.DocumentNode.OuterHtml, @"<tr>\s*<th>Recognized by the IOC<\/th>\s*<td>([\d]*)<\/td>\s*<\/tr>");

                        noc.Abbreviation = !string.IsNullOrEmpty(abbreavition) ? abbreavition : null;
                        noc.Created = !string.IsNullOrEmpty(foundedYear) ? int.Parse(foundedYear) : null;
                        noc.Disbanded = !string.IsNullOrEmpty(disbandedYear) ? int.Parse(disbandedYear) : null;
                        noc.Recognized = !string.IsNullOrEmpty(recognizedYear) ? int.Parse(recognizedYear) : null;

                        var presidents = this.ExtractNOCPresidents(committeeDocument);
                        noc.NOCPresidents = presidents;
                    }

                    var flagCode = this.NormalizeService.MapOlympicGamesCountriesAndWorldCountries(noc.Code);
                    if (flagCode != null)
                    {
                        noc.FlagCode = flagCode;
                    }

                    var dbNoc = await this.repository.GetAsync(x => x.Name == name && x.Code == code);
                    if (dbNoc != null)
                    {
                        var equals = noc.Equals(dbNoc);
                        if (!equals)
                        {
                            this.repository.Update(dbNoc);
                            await this.repository.SaveChangesAsync();
                        }
                    }
                    else
                    {
                        await this.repository.AddAsync(noc);
                        await this.repository.SaveChangesAsync();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            this.Logger.LogError(ex, $"Failed to process group: {group.Identifier}");
        }
    }

    private IList<NOCPresident> ExtractNOCPresidents(HtmlDocument document)
    {
        var presidentTable = document.DocumentNode.SelectSingleNode("//table[@class='table table-striped']");
        var presidents = new List<NOCPresident>();
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
                    var president = new NOCPresident
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