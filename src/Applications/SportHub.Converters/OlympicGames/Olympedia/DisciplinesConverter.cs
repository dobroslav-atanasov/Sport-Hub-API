namespace SportHub.Converters.OlympicGames.Olympedia;

using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using SportHub.Common.Helpers;
using SportHub.Converters.OlympicGames.Olympedia.Base;
using SportHub.Data.Entities.Crawlers;
using SportHub.Data.Entities.OlympicGames;
using SportHub.Data.Repositories;
using SportHub.Services.Data.CrawlerStorageDb.Interfaces;
using SportHub.Services.Data.OlympicGamesDb.Interfaces;
using SportHub.Services.Interfaces;

public class DisciplinesConverter : OlympediaConverter
{
    private readonly OlympicGamesRepository<Discipline> repository;

    public DisciplinesConverter(ILogger<BaseConverter> logger, ICrawlersService crawlersService, ILogsService logsService, IGroupsService groupsService, IZipService zipService,
        INormalizeService normalizeService, IDataCacheService dataCacheService, OlympicGamesRepository<Discipline> repository)
        : base(logger, crawlersService, logsService, groupsService, zipService, normalizeService, dataCacheService)
    {
        this.repository = repository;
    }

    protected override async Task ProcessGroupAsync(Group group)
    {
        var converterModel = this.PrepareConverterModel(group);
        var lines = converterModel.Documents.GetValueOrDefault(1).HtmlDocument.DocumentNode.SelectNodes("//table[@class='table table-striped sortable']//tr");

        foreach (var line in lines)
        {
            if (line.OuterHtml.Contains("glyphicon-ok"))
            {
                var elements = line.Elements("td").ToList();
                var sportName = elements[2].InnerText.Trim();
                var sportCode = RegExpHelper.MatchFirstGroup(elements[2].OuterHtml, @"/sport_groups/(.*?)""");
                if (sportName is not "Air Sports" and not "Mountaineering and Climbing" and not "Art Competitions")
                {
                    var disciplineName = elements[1].InnerText.Trim();
                    var disciplineCode = elements[0].InnerText.Trim();

                    disciplineName = this.NormalizeService.NormalizeDisciplineName(disciplineName);
                    var dbDiscipline = await this.repository.GetAsync(x => x.Name == disciplineName);
                    if (dbDiscipline == null)
                    {
                        var info = this.GetDisciplineInfo(disciplineName);
                        var sport = this.NormalizeService.MapDisciplineToSport(disciplineName);
                        var discipline = new Discipline
                        {
                            Name = disciplineName,
                            Code = info.Item2,
                            IsHistoric = this.IsHistoric(disciplineName),
                            SEOName = disciplineName.ToLower().Replace(" ", "-"),
                            Sport = sport.Item1,
                            SportCode = info.Item2,
                        };

                        await this.repository.AddAsync(discipline);
                        await this.repository.SaveChangesAsync();
                    }
                }
            }
        }
    }

    private bool IsHistoric(string discipline)
    {
        switch (discipline)
        {
            case "Baseball":
            case "Basque pelota":
            case "Cricket":
            case "Croquet":
            case "Jeu De Paume":
            case "Karate":
            case "Lacrosse":
            case "Military Ski Patrol":
            case "Motorboating":
            case "Polo":
            case "Racquets":
            case "Roque":
            case "Rugby":
            case "Softball":
            case "Tug-Of-War":
                return true;
            default:
                return false;
        }
    }

    private Tuple<string, string, bool> GetDisciplineInfo(string discipline)
    {
        Tuple<string, string, bool> info = null;
        switch (discipline)
        {
            case "Alpine Skiing": info = new Tuple<string, string, bool>("Alpine Skiing", "ALP", false); break;
            case "Baseball": info = new Tuple<string, string, bool>("Baseball", "BSB", true); break;
            case "Basque pelota": info = new Tuple<string, string, bool>("Basque pelota", "PEL", true); break;
            case "Biathlon": info = new Tuple<string, string, bool>("Biathlon", "BTH", false); break;
            case "Bobsleigh": info = new Tuple<string, string, bool>("Bobsleigh", "BOB", false); break;
            case "Cricket": info = new Tuple<string, string, bool>("Cricket", "CKT", true); break;
            case "Croquet": info = new Tuple<string, string, bool>("Croquet", "CQT", true); break;
            case "Cross Country Skiing": info = new Tuple<string, string, bool>("Cross Country Skiing", "CCS", false); break;
            case "Curling": info = new Tuple<string, string, bool>("Curling", "CUR", false); break;
            case "Figure Skating": info = new Tuple<string, string, bool>("Figure Skating", "FSK", false); break;
            case "Freestyle Skiing": info = new Tuple<string, string, bool>("Freestyle Skiing", "FRS", false); break;
            case "Ice Hockey": info = new Tuple<string, string, bool>("Ice Hockey", "IHO", false); break;
            case "Jeu De Paume": info = new Tuple<string, string, bool>("Jeu De Paume", "JDP", true); break;
            case "Karate": info = new Tuple<string, string, bool>("Karate", "KTE", true); break;
            case "Lacrosse": info = new Tuple<string, string, bool>("Lacrosse", "LAX", true); break;
            case "Luge": info = new Tuple<string, string, bool>("Luge", "KUG", false); break;
            case "Military Ski Patrol": info = new Tuple<string, string, bool>("Military Ski Patrol", "MPT", true); break;
            case "Motorboating": info = new Tuple<string, string, bool>("Motorboating", "PBT", true); break;
            case "Nordic Combined": info = new Tuple<string, string, bool>("Nordic Combined", "NCB", false); break;
            case "Polo": info = new Tuple<string, string, bool>("Polo", "POL", true); break;
            case "Racquets": info = new Tuple<string, string, bool>("Racquets", "RQT", true); break;
            case "Roque": info = new Tuple<string, string, bool>("Roque", "RQE", true); break;
            case "Rugby": info = new Tuple<string, string, bool>("Rugby", "RUG", true); break;
            case "Short Track Speed Skating": info = new Tuple<string, string, bool>("Short Track Speed Skating", "STK", false); break;
            case "Skeleton": info = new Tuple<string, string, bool>("Skeleton", "SKN", false); break;
            case "Ski Jumping": info = new Tuple<string, string, bool>("Ski Jumping", "SJP", false); break;
            case "Snowboarding": info = new Tuple<string, string, bool>("Snowboarding", "SBD", false); break;
            case "Softball": info = new Tuple<string, string, bool>("Softball", "SBL", true); break;
            case "Speed Skating": info = new Tuple<string, string, bool>("Speed Skating", "SSK", false); break;
            case "Tug-Of-War": info = new Tuple<string, string, bool>("Tug-Of-War", "TOW", true); break;
        }

        return info;
    }
}