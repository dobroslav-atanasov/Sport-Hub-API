namespace SportHub.Converters.OlympicGames;

using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using SportHub.Common.Extensions;
using SportHub.Data.Models.Entities.Crawlers;
using SportHub.Data.Models.Entities.OlympicGames;
using SportHub.Data.Models.Entities.OlympicGames.Enumerations;
using SportHub.Data.Repositories;
using SportHub.Services.Data.CrawlerStorageDb.Interfaces;
using SportHub.Services.Data.OlympicGamesDb.Interfaces;
using SportHub.Services.Interfaces;

public class SportDisciplineConverter : BaseOlympediaConverter
{
    private readonly IDataCacheService dataCacheService;
    private readonly OlympicGamesRepository<Sport> sportRepository;
    private readonly OlympicGamesRepository<Discipline> disciplineRepository;

    public SportDisciplineConverter(ILogger<BaseConverter> logger, ICrawlersService crawlersService, ILogsService logsService, IGroupsService groupsService, IZipService zipService,
        IRegExpService regExpService, INormalizeService normalizeService, IOlympediaService olympediaService, IDataCacheService dataCacheService,
        OlympicGamesRepository<Sport> sportRepository, OlympicGamesRepository<Discipline> disciplineRepository)
        : base(logger, crawlersService, logsService, groupsService, zipService, regExpService, normalizeService, olympediaService, dataCacheService)
    {
        this.dataCacheService = dataCacheService;
        this.sportRepository = sportRepository;
        this.disciplineRepository = disciplineRepository;
    }

    protected override async Task ProcessGroupAsync(Group group)
    {
        try
        {
            var document = this.CreateHtmlDocument(group.Documents.Single());
            var lines = document.DocumentNode.SelectNodes("//table[@class='table table-striped sortable']//tr");

            foreach (var line in lines)
            {
                if (line.OuterHtml.Contains("glyphicon-ok"))
                {
                    var elements = line.Elements("td").ToList();
                    var name = elements[2].InnerText.Trim();
                    if (name is not "Air Sports" and not "Mountaineering and Climbing" and not "Art Competitions")
                    {
                        var olympicGameTypeEnum = elements[3].InnerText.Trim().ToEnum<OlympicGameTypeEnum>();
                        //var olympicGameType = this.dataCacheService.OlympicGameTypes.FirstOrDefault(x => x.Name.Equals(elements[3].InnerText.Trim(), StringComparison.CurrentCultureIgnoreCase));
                        var sportCode = this.RegExpService.MatchFirstGroup(elements[2].OuterHtml, @"/sport_groups/(.*?)""");
                        var sport = new Sport
                        {
                            Name = name,
                            OlympicGameTypeId = (int)olympicGameTypeEnum,
                            Code = sportCode
                        };

                        var dbSport = await this.sportRepository.GetAsync(x => x.Name == name);
                        if (dbSport != null)
                        {
                            sport = dbSport;
                        }
                        else
                        {
                            await this.sportRepository.AddAsync(sport);
                            await this.sportRepository.SaveChangesAsync();
                        }

                        var disciplineName = elements[1].InnerText.Trim();
                        var disciplineAbbreviation = elements[0].InnerText.Trim();
                        var disciplines = new List<Discipline>();
                        if (sport.Name == "Wrestling")
                        {
                            disciplines.Add(new Discipline
                            {
                                Name = "Wrestling Freestyle",
                                Code = "WRF",
                                SportId = sport.Id
                            });

                            disciplines.Add(new Discipline
                            {
                                Name = "Wrestling Greco-Roman",
                                Code = "WRG",
                                SportId = sport.Id
                            });
                        }
                        else
                        {
                            disciplines.Add(new Discipline
                            {
                                Name = disciplineName,
                                Code = disciplineAbbreviation,
                                SportId = sport.Id
                            });
                        }

                        foreach (var discipline in disciplines.Where(x => x.Name != "Canoe Marathon"))
                        {
                            var dbDiscipline = await this.disciplineRepository.GetAsync(x => x.Name == discipline.Name);
                            if (dbDiscipline == null)
                            {
                                await this.disciplineRepository.AddAsync(discipline);
                                await this.disciplineRepository.SaveChangesAsync();
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            this.Logger.LogError(ex, $"Failed to process group: {group.Identifier}");
        }
    }
}