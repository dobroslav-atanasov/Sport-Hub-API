namespace SportHub.Converters.OlympicGames.Olympedia;

using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using SportHub.Converters;
using SportHub.Data.Models.DbEntities.Crawlers;
using SportHub.Data.Models.DbEntities.OlympicGames;
using SportHub.Data.Models.Enumerations.OlympicGames;
using SportHub.Services.Data.CrawlerStorageDb.Interfaces;
using SportHub.Services.Data.OlympicGamesDb;
using SportHub.Services.Data.OlympicGamesDb.Interfaces;
using SportHub.Services.Interfaces;

public class AthleteConverter : BaseOlympediaConverter
{
    private readonly IDateService dateService;
    private readonly DataService<Athlete> dataService;
    private int count = 0;

    public AthleteConverter(ILogger<BaseConverter> logger, ICrawlersService crawlersService, ILogsService logsService, IGroupsService groupsService, IZipService zipService,
        IRegExpService regExpService, INormalizeService normalizeService, IOlympediaService olympediaService, IDataCacheService dataCacheService,
        IDateService dateService, DataService<Athlete> dataService)
        : base(logger, crawlersService, logsService, groupsService, zipService, regExpService, normalizeService, olympediaService, dataCacheService)
    {
        this.dateService = dateService;
        this.dataService = dataService;
    }

    protected override async Task ProcessGroupAsync(Group group)
    {
        await Console.Out.WriteLineAsync($"{this.count++}");

        var model = this.PrepareConverterModel(group);
        var typeMatch = this.RegExpService.Match(model.HtmlDocument.ParsedText, @"<tr>\s*<th>Roles<\/th>\s*<td>(.*?)<\/td><\/tr>");
        var genderMatch = this.RegExpService.Match(model.HtmlDocument.ParsedText, @"<tr>\s*<th>Sex<\/th>\s*<td>(Male|Female)<\/td><\/tr>");
        var gender = this.MapGenderEnum(genderMatch.Groups[1].Value);
        var bornMatch = this.RegExpService.Match(model.HtmlDocument.ParsedText, @"<tr>\s*<th>Born<\/th>\s*<td>(.*?)<\/td><\/tr>");
        var diedMatch = this.RegExpService.Match(model.HtmlDocument.ParsedText, @"<tr>\s*<th>Died<\/th>\s*<td>(.*?)<\/td><\/tr>");
        var role = this.GetRole(typeMatch.Groups[1].Value);
        var bornPlace = this.ExtractCityAndCountry(bornMatch);
        var diedPlace = this.ExtractCityAndCountry(diedMatch);
        var clubs = this.GetClubs(model.HtmlDocument.ParsedText);

        var athlete = new Athlete
        {
            Code = model.PageId,
            Gender = gender,
            Name = model.Title,
            TranslateName = this.NormalizeService.ReplaceNonEnglishLetters(model.Title),
            FullName = this.RegExpService.MatchFirstGroup(model.HtmlDocument.ParsedText, @"<tr>\s*<th>Full name<\/th>\s*<td>(.*?)<\/td><\/tr>")?.Replace("•", " "),
            OriginalName = this.RegExpService.MatchFirstGroup(model.HtmlDocument.ParsedText, @"<tr>\s*<th>Original name<\/th>\s*<td>(.*?)<\/td><\/tr>")?.Replace("•", " "),
            Citizenship = this.ExtractCitizenship(model.HtmlDocument.ParsedText),
            BirthDate = bornMatch != null ? this.dateService.ParseDate(bornMatch.Groups[1].Value).From : null,
            BirthCity = bornPlace.Item1,
            BirthCountry = bornPlace.Item2,
            DiedDate = diedMatch != null ? this.dateService.ParseDate(diedMatch.Groups[1].Value).From : null,
            DiedCity = diedPlace.Item1,
            DiedCountry = diedPlace.Item2,
            Description = this.RegExpService.CutHtml(this.RegExpService.MatchFirstGroup(model.HtmlDocument.ParsedText, @"<div class=(?:""|')description(?:""|')>(.*?)<\/div>")),
            Role = role,
            Clubs = clubs
        };

        var measurmentsMatch = this.RegExpService.Match(model.HtmlDocument.ParsedText, @"<tr>\s*<th>Measurements<\/th>\s*<td>(.*?)<\/td><\/tr>");
        if (measurmentsMatch != null)
        {
            var heightMatch = this.RegExpService.Match(measurmentsMatch.Groups[1].Value, @"([\d]+)\s*cm");
            if (heightMatch != null)
            {
                athlete.Height = int.Parse(heightMatch.Groups[1].Value);
            }

            var weightMatch = this.RegExpService.Match(measurmentsMatch.Groups[1].Value, @"([\d]+)\s*kg");
            if (weightMatch != null)
            {
                athlete.Weight = int.Parse(weightMatch.Groups[1].Value);
            }
        }

        var dbAthlete = await this.dataService.GetAsync(x => x.Code == model.PageId);
        if (dbAthlete != null)
        {
            var equals = athlete.Equals(dbAthlete);
            if (!equals)
            {
                this.dataService.Update(dbAthlete);
            }
        }
        else
        {
            dbAthlete = await this.dataService.AddAsync(athlete);
        }
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

    public RoleEnum GetRole(string text)
    {
        text = text?.Replace(" •", ",");
        var role = RoleEnum.None;
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
                role = RoleEnum.Athlete;
                break;
            case "Coach":
                role = RoleEnum.Coach;
                break;
            case "Referee":
                role = RoleEnum.Referee;
                break;
            case "Coach, Other":
            case "Competed in Olympic Games (non-medal events), Coach":
            case "Competed in Olympic Games, Coach":
            case "Competed in Olympic Games, Competed in Olympic Games (non-medal events), Coach":
            case "Competed in Olympic Games, Non-starter, Coach":
            case "Non-starter, Coach":
                role = RoleEnum.AthleteCoach;
                break;
            case "Coach, Referee":
                role = RoleEnum.CoachReferee;
                break;
            case "Competed in Olympic Games, Competed in Intercalated Games, Non-starter, Referee":
            case "Competed in Olympic Games, Competed in Intercalated Games, Referee":
            case "Competed in Olympic Games, Competed in Olympic Games (non-medal events), Competed in Intercalated Games, Referee":
            case "Competed in Olympic Games, Competed in Olympic Games (non-medal events), Referee":
            case "Competed in Olympic Games, Non-starter, Referee":
            case "Competed in Olympic Games, Referee":
            case "Non-starter, Referee":
            case "Referee, Other":
                role = RoleEnum.AthleteReferee;
                break;
            case "Competed in Olympic Games, Coach, Referee":
            case "Competed in Olympic Games, Non-starter, Coach, Referee":
            case "Non-starter, Coach, Referee":
                role = RoleEnum.AthleteCoachReferee;
                break;
            case "Competed in Olympic Games, IOC member":
            case "Competed in Olympic Games, Non-starter, IOC member":
                role = RoleEnum.AthleteIOCMember;
                break;
            case "Competed in Olympic Games, IOC member, Referee":
                role = RoleEnum.AthleteRefereeIOCMember;
                break;
            case "IOC member":
                role = RoleEnum.IOCMember;
                break;
            case "IOC member, Coach":
                role = RoleEnum.CoachIOCMember;
                break;
            case "IOC member, Referee":
                role = RoleEnum.RefereeIOCMember;
                break;
        }

        return role;
    }

    private GenderEnum MapGenderEnum(string text)
    {
        var gender = GenderEnum.None;

        if (text.StartsWith("male", StringComparison.CurrentCultureIgnoreCase))
        {
            gender = GenderEnum.Male;
        }
        else if (text.StartsWith("female", StringComparison.CurrentCultureIgnoreCase))
        {
            gender = GenderEnum.Female;
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

    private string GetClubs(string text)
    {
        var associationMatch = this.RegExpService.Match(text, @"<tr>\s*<th>Affiliations<\/th>\s*<td>(.*?)<\/td><\/tr>");
        var clubs = new List<string>();
        if (associationMatch != null)
        {
            var matches = this.RegExpService.Matches(associationMatch.Groups[1].Value, @">(.*?)?(?:\s*,\s*)(.*?)?(\([A-Z]{3}\))?<");
            foreach (System.Text.RegularExpressions.Match match in matches)
            {
                var club = new List<string> { match.Groups[1].Value.Trim(), match.Groups[2].Value.Trim(), match.Groups[3].Value.Trim().Replace("(", string.Empty).Replace(")", string.Empty) };
                clubs.Add(string.Join(", ", club.Where(x => !string.IsNullOrEmpty(x))));
            }
        }

        if (clubs.Count == 0)
        {
            return null;
        }

        return string.Join("|", clubs);
    }
}