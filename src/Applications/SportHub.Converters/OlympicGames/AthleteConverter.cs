namespace SportHub.Converters.OlympicGames;

using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using SportHub.Common.Constants;
using SportHub.Converters;
using SportHub.Data.Models.Entities.Crawlers;
using SportHub.Data.Models.Entities.OlympicGames;
using SportHub.Data.Models.Entities.OlympicGames.Enumerations;
using SportHub.Services.Data.CrawlerStorageDb.Interfaces;
using SportHub.Services.Data.OlympicGamesDb;
using SportHub.Services.Data.OlympicGamesDb.Interfaces;
using SportHub.Services.Interfaces;

public class AthleteConverter : BaseOlympediaConverter
{
    private readonly IDateService dateService;
    private readonly OlympicGamesRepositoryService<Athlete> athletesService;
    private readonly OlympicGamesRepositoryService<Club> clubsService;
    private int count = 0;

    public AthleteConverter(ILogger<BaseConverter> logger, ICrawlersService crawlersService, ILogsService logsService, IGroupsService groupsService, IZipService zipService,
        IRegExpService regExpService, INormalizeService normalizeService, IOlympediaService olympediaService, IDataCacheService dataCacheService,
        IDateService dateService, OlympicGamesRepositoryService<Athlete> athletesService, OlympicGamesRepositoryService<Club> clubsService)
        : base(logger, crawlersService, logsService, groupsService, zipService, regExpService, normalizeService, olympediaService, dataCacheService)
    {
        this.dateService = dateService;
        this.athletesService = athletesService;
        this.clubsService = clubsService;
    }

    protected override async Task ProcessGroupAsync(Group group)
    {
        await Console.Out.WriteLineAsync($"{this.count++}");
        var document = this.CreateHtmlDocument(group.Documents.Single());
        var code = int.Parse(new Uri(group.Documents.Single().Url).Segments.Last());
        var name = document.DocumentNode.SelectSingleNode("//h1").InnerText.Trim();
        var typeMatch = this.RegExpService.Match(document.ParsedText, @"<tr>\s*<th>Roles<\/th>\s*<td>(.*?)<\/td><\/tr>");
        var genderMatch = this.RegExpService.Match(document.ParsedText, @"<tr>\s*<th>Sex<\/th>\s*<td>(Male|Female)<\/td><\/tr>");
        var gender = this.MapGenderEnum(genderMatch.Groups[1].Value);
        var bornMatch = this.RegExpService.Match(document.ParsedText, @"<tr>\s*<th>Born<\/th>\s*<td>(.*?)<\/td><\/tr>");
        var diedMatch = this.RegExpService.Match(document.ParsedText, @"<tr>\s*<th>Died<\/th>\s*<td>(.*?)<\/td><\/tr>");
        var roles = this.GetAthleteRoles(typeMatch.Groups[1].Value);
        var bornPlace = this.ExtractCityAndCountry(bornMatch);
        var diedPlace = this.ExtractCityAndCountry(diedMatch);
        var clubs = await this.GetClubsAsync(document.ParsedText);

        var athlete = new Athlete
        {
            Code = code,
            GenderId = (int)gender,
            Name = name,

            TranslateName = this.NormalizeService.ReplaceNonEnglishLetters(name),
            FullName = this.RegExpService.MatchFirstGroup(document.ParsedText, @"<tr>\s*<th>Full name<\/th>\s*<td>(.*?)<\/td><\/tr>")?.Replace("•", " "),
            OriginalName = this.RegExpService.MatchFirstGroup(document.ParsedText, @"<tr>\s*<th>Original name<\/th>\s*<td>(.*?)<\/td><\/tr>")?.Replace("•", " "),
            Citizenship = this.ExtractCitizenship(document.ParsedText),
            BirthDate = bornMatch != null ? this.dateService.ParseDate(bornMatch.Groups[1].Value).From : null,
            BirthCity = bornPlace.Item1,
            BirthCountry = bornPlace.Item2,
            DiedDate = diedMatch != null ? this.dateService.ParseDate(diedMatch.Groups[1].Value).From : null,
            DiedCity = diedPlace.Item1,
            DiedCountry = diedPlace.Item2,
            Description = this.RegExpService.CutHtml(this.RegExpService.MatchFirstGroup(document.ParsedText, @"<div class=(?:""|')description(?:""|')>(.*?)<\/div>")),
            Roles = roles.Select(x => new Role { AthleteTypeId = (int)x }).ToList(),
            AthletesClubs = clubs.Select(x => new AthleteClub { ClubId = x.Id }).ToList(),
        };

        var measurmentsMatch = this.RegExpService.Match(document.ParsedText, @"<tr>\s*<th>Measurements<\/th>\s*<td>(.*?)<\/td><\/tr>");
        if (measurmentsMatch != null)
        {
            var heightMatch = this.RegExpService.Match(measurmentsMatch.Groups[1].Value, @"([\d]+)\s*cm");
            if (heightMatch != null)
            {
                athlete.HeightInCentimeters = int.Parse(heightMatch.Groups[1].Value);
                athlete.HeightInInches = athlete.HeightInCentimeters / GlobalConstants.INCHES;
            }

            var weightMatch = this.RegExpService.Match(measurmentsMatch.Groups[1].Value, @"([\d]+)\s*kg");
            if (weightMatch != null)
            {
                athlete.WeightInKilograms = int.Parse(weightMatch.Groups[1].Value);
                athlete.WeightInPounds = (int)(athlete.WeightInKilograms * GlobalConstants.POUNDS);
            }
        }

        var dbAthlete = await this.athletesService.GetAsync(x => x.Code == code);
        if (dbAthlete != null)
        {
            var equals = athlete.Equals(dbAthlete);
            if (!equals)
            {
                this.athletesService.Update(dbAthlete);
            }
        }
        else
        {
            dbAthlete = await this.athletesService.AddAsync(athlete);
        }

        //var dbAthlete = await this.athleteRepository.GetAsync(x => x.Code == code);
        //if (dbAthlete != null)
        //{
        //    var equals = athlete.Equals(dbAthlete);
        //    if (!equals)
        //    {
        //        this.athleteRepository.Update(dbAthlete);
        //        await this.athleteRepository.SaveChangesAsync();
        //    }
        //}
        //else
        //{
        //    await this.athleteRepository.AddAsync(athlete);
        //    await this.athleteRepository.SaveChangesAsync();
        //}

        //foreach (var athleteTypeCache in roles)
        //{
        //    var dbRole = await this.roleRepository.GetAsync(x => x.AthleteId == dbAthlete.Id && x.AthleteTypeId == athleteTypeCache.Id);
        //    if (dbRole == null)
        //    {
        //        await this.roleRepository.AddAsync(new Role { AthleteId = dbAthlete.Id, AthleteTypeId = athleteTypeCache.Id });
        //        await this.roleRepository.SaveChangesAsync();
        //    }
        //}

        //foreach (var club in clubs)
        //{
        //    var dbAthleteClub = await this.athleteClubRepository.GetAsync(x => x.AthleteId == dbAthlete.Id && x.ClubId == club.Id);
        //    if (dbAthleteClub == null)
        //    {
        //        await this.athleteClubRepository.AddAsync(new AthleteClub { AthleteId = dbAthlete.Id, ClubId = club.Id });
        //        await this.athleteClubRepository.SaveChangesAsync();
        //    }
        //}
    }

    public Tuple<string, string> ExtractCityAndCountry(System.Text.RegularExpressions.Match match)
    {
        if (match != null)
        {
            var newMatch = this.RegExpService.Match(match.Groups[1].Value, @">(.*?)?(\([A-Z]{3}\))?<");
            if (newMatch != null)
            {
                return new Tuple<string, string>(newMatch.Groups[1].Value.Trim(), newMatch.Groups[2].Value.Trim().Replace("(", string.Empty).Replace(")", string.Empty));
            }

        }

        return new Tuple<string, string>(null, null);
    }

    public List<AthleteTypeEnum> GetAthleteRoles(string text)
    {
        text = text?.Replace(" •", ",");
        var athleteTypes = new List<AthleteTypeEnum>();
        switch (text)
        {
            case "Competed in Intercalated Games":
            case "Competed in Intercalated Games, Non-starter":
            case "Competed in Olympic Games":
            case "Competed in Olympic Games (non-medal events)":
            case "Competed in Olympic Games (non-medal events), Competed in Intercalated Games":
            case "Competed in Olympic Games (non-medal events), Non-starter":
            case "Competed in Olympic Games, Competed in Intercalated Games":
            case "Competed in Olympic Games, Competed in Intercalated Games, Non-starter":
            case "Competed in Olympic Games, Competed in Olympic Games (non-medal events)":
            case "Competed in Olympic Games, Competed in Olympic Games (non-medal events), Competed in Intercalated Games":
            case "Competed in Olympic Games, Competed in Olympic Games (non-medal events), Non-starter":
            case "Competed in Olympic Games, Competed in Youth Olympic Games":
            case "Competed in Olympic Games, Competed in Youth Olympic Games (non-medal events)":
            case "Competed in Olympic Games, Competed in Youth Olympic Games, Non-starter":
            case "Competed in Olympic Games, Non-starter":
            case "Competed in Olympic Games, Other":
            case "Competed in Youth Olympic Games":
            case "Competed in Youth Olympic Games, Non-starter":
            case "Non-starter":
            case "Non-starter, Other":
            case "Other":
                athleteTypes.Add(AthleteTypeEnum.Athlete);
                break;
            case "Coach":
                athleteTypes.Add(AthleteTypeEnum.Coach);
                break;
            case "Referee":
                athleteTypes.Add(AthleteTypeEnum.Referee);
                break;
            case "Coach, Other":
            case "Competed in Olympic Games (non-medal events), Coach":
            case "Competed in Olympic Games, Coach":
            case "Competed in Olympic Games, Competed in Olympic Games (non-medal events), Coach":
            case "Competed in Olympic Games, Non-starter, Coach":
            case "Non-starter, Coach":
                athleteTypes.Add(AthleteTypeEnum.Athlete);
                athleteTypes.Add(AthleteTypeEnum.Coach);
                break;
            case "Coach, Referee":
                athleteTypes.Add(AthleteTypeEnum.Coach);
                athleteTypes.Add(AthleteTypeEnum.Referee);
                break;
            case "Competed in Olympic Games, Competed in Intercalated Games, Non-starter, Referee":
            case "Competed in Olympic Games, Competed in Intercalated Games, Referee":
            case "Competed in Olympic Games, Competed in Olympic Games (non-medal events), Competed in Intercalated Games, Referee":
            case "Competed in Olympic Games, Competed in Olympic Games (non-medal events), Referee":
            case "Competed in Olympic Games, Non-starter, Referee":
            case "Competed in Olympic Games, Referee":
            case "Non-starter, Referee":
            case "Referee, Other":
                athleteTypes.Add(AthleteTypeEnum.Athlete);
                athleteTypes.Add(AthleteTypeEnum.Referee);
                break;
            case "Competed in Olympic Games, Coach, Referee":
            case "Competed in Olympic Games, Non-starter, Coach, Referee":
            case "Non-starter, Coach, Referee":
                athleteTypes.Add(AthleteTypeEnum.Athlete);
                athleteTypes.Add(AthleteTypeEnum.Coach);
                athleteTypes.Add(AthleteTypeEnum.Referee);
                break;
            case "Competed in Olympic Games, IOC member":
            case "Competed in Olympic Games, Non-starter, IOC member":
                athleteTypes.Add(AthleteTypeEnum.Athlete);
                athleteTypes.Add(AthleteTypeEnum.IOCMember);
                break;
            case "Competed in Olympic Games, IOC member, Referee":
                athleteTypes.Add(AthleteTypeEnum.Athlete);
                athleteTypes.Add(AthleteTypeEnum.Referee);
                athleteTypes.Add(AthleteTypeEnum.IOCMember);
                break;
            case "IOC member":
                athleteTypes.Add(AthleteTypeEnum.IOCMember);
                break;
            case "IOC member, Coach":
                athleteTypes.Add(AthleteTypeEnum.Coach);
                athleteTypes.Add(AthleteTypeEnum.IOCMember);
                break;
            case "IOC member, Referee":
                athleteTypes.Add(AthleteTypeEnum.Referee);
                athleteTypes.Add(AthleteTypeEnum.IOCMember);
                break;
            default:
                athleteTypes.Add(AthleteTypeEnum.None);
                break;
        }

        return athleteTypes;
    }

    private GenderTypeEnum MapGenderEnum(string text)
    {
        var gender = GenderTypeEnum.None;

        if (text.StartsWith("male", StringComparison.CurrentCultureIgnoreCase))
        {
            gender = GenderTypeEnum.Male;
        }
        else if (text.StartsWith("female", StringComparison.CurrentCultureIgnoreCase))
        {
            gender = GenderTypeEnum.Female;
        }

        return gender;
    }

    private string ExtractCitizenship(string text)
    {
        var match = this.RegExpService.Match(text, @"<tr>\s*<th>Nationality<\/th>\s*<td>(.*?)<\/td><\/tr>");
        if (match != null)
        {
            var countryMatch = this.RegExpService.Match(match.Groups[1].Value, @"<a href=""/countries/(.*?)"">(.*?)</a>");
            if (countryMatch != null)
            {
                return countryMatch.Groups[2].Value.Trim();
            }
        }

        return null;
    }

    private async Task<IEnumerable<Club>> GetClubsAsync(string text)
    {
        var associationMatch = this.RegExpService.Match(text, @"<tr>\s*<th>Affiliations<\/th>\s*<td>(.*?)<\/td><\/tr>");
        var clubs = new List<Club>();
        if (associationMatch != null)
        {
            var matches = this.RegExpService.Matches(associationMatch.Groups[1].Value, @">(.*?)?(?:\s*,\s*)(.*?)?(\([A-Z]{3}\))?<");
            foreach (System.Text.RegularExpressions.Match match in matches)
            {
                var club = new Club
                {
                    Name = match.Groups[1].Value,
                    City = match.Groups[2].Value,
                    Country = match.Groups[3].Value.Replace("(", string.Empty).Replace(")", string.Empty),
                };

                var dbClub = await this.clubsService.GetAsync(x => x.Name == club.Name && x.City == club.City && x.Country == club.Country);
                dbClub ??= await this.clubsService.AddAsync(club);

                clubs.Add(dbClub);
            }
        }

        return clubs;
    }
}