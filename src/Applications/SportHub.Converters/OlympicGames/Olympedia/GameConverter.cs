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

public class GameConverter : OlympediaConverter
{
    private readonly OlympicGamesRepository<Game> repository;

    public GameConverter(ILogger<BaseConverter> logger, ICrawlersService crawlersService, ILogsService logsService, IGroupsService groupsService, IZipService zipService,
        INormalizeService normalizeService, IDataCacheService dataCacheService, OlympicGamesRepository<Game> repository)
        : base(logger, crawlersService, logsService, groupsService, zipService, normalizeService, dataCacheService)
    {
        this.repository = repository;
    }

    protected override async Task ProcessGroupAsync(Group group)
    {
        var converterModel = this.PrepareConverterModel(group);
        var model = converterModel.Documents.GetValueOrDefault(1);
        var match = RegExpHelper.Match(model.Title, @"(\d+)\s*(summer|winter|equestrian)");
        if (match != null)
        {
            var year = int.Parse(match.Groups[1].Value);
            var gameType = match.Groups[2].Value.Trim();
            if (gameType.Equals("equestrian", StringComparison.CurrentCultureIgnoreCase))
            {
                gameType = "Summer";
            }

            var type = gameType;
            var numberMatch = RegExpHelper.Match(model.HtmlDocument.DocumentNode.OuterHtml, @"<th>Number and Year<\/th>\s*<td>\s*([IVXLC]+)\s*\/(.*?)<\/td>");
            var hostCityMatch = RegExpHelper.Match(model.HtmlDocument.DocumentNode.OuterHtml, @"<tr>\s*<th>Host city<\/th>\s*<td>\s*([\w'\-\s.]+),\s*([\w'\-\s]+)");
            var hostCityName = this.NormalizeService.NormalizeHostCityName(hostCityMatch?.Groups[1].Value.Trim());
            var country = this.NormalizeService.MapCityToCountry(hostCityName);
            var openByMatch = RegExpHelper.Match(model.HtmlDocument.DocumentNode.OuterHtml, @"<tr>\s*<th>Officially opened by<\/th>\s*<td>(.*?)<\/td>\s*<\/tr>");
            var torchbearersMatch = RegExpHelper.Match(model.HtmlDocument.DocumentNode.OuterHtml, @"<tr>\s*<th>Torchbearer\(s\)<\/th>\s*<td>(.*?)<\/td>\s*<\/tr>");
            var athleteOathByMatch = RegExpHelper.Match(model.HtmlDocument.DocumentNode.OuterHtml, @"<tr>\s*<th>Taker of the Athlete's Oath<\/th>\s*<td>(.*?)<\/td>\s*<\/tr>");
            var judgeOathByMatch = RegExpHelper.Match(model.HtmlDocument.DocumentNode.OuterHtml, @"<tr>\s*<th>Taker of the Official's Oath<\/th>\s*<td>(.*?)<\/td>\s*<\/tr>");
            var coachOathByMatch = RegExpHelper.Match(model.HtmlDocument.DocumentNode.OuterHtml, @"<tr>\s*<th>Taker of the Coach's Oath<\/th>\s*<td>(.*?)<\/td>\s*<\/tr>");
            var olympicFlagBearersMatch = RegExpHelper.Match(model.HtmlDocument.DocumentNode.OuterHtml, @"<tr>\s*<th>Olympic Flag Bearers<\/th>\s*<td>(.*?)<\/td>\s*<\/tr>");
            var descriptionMatch = RegExpHelper.Match(model.HtmlDocument.DocumentNode.OuterHtml, @"<h2>\s*Overview\s*(?:<small><\/small>)?\s*<\/h2>\s*<div class=(?:'|"")description(?:'|"")>\s*(.*?)<\/div>");
            var bidProcessMatch = RegExpHelper.Match(model.HtmlDocument.DocumentNode.OuterHtml, @"<h2>Bid process<\/h2>\s*<div class=(?:'|"")description(?:'|"")>\s*(.*?)<\/div>");

            var game = new Game
            {
                Year = year,
                Type = type,
                Number = numberMatch?.Groups[1].Value.Trim(),
                OfficialName = this.SetOfficialName(hostCityName, year),
                Host = hostCityName,
                Country = country,
                OpenBy = openByMatch != null ? RegExpHelper.CutHtml(openByMatch.Groups[1].Value) : null,
                Torchbearers = torchbearersMatch != null ? RegExpHelper.CutHtml(torchbearersMatch.Groups[1].Value) : null,
                AthleteOathBy = athleteOathByMatch != null ? RegExpHelper.CutHtml(athleteOathByMatch.Groups[1].Value) : null,
                JudgeOathBy = judgeOathByMatch != null ? RegExpHelper.CutHtml(judgeOathByMatch.Groups[1].Value) : null,
                CoachOathBy = coachOathByMatch != null ? RegExpHelper.CutHtml(coachOathByMatch.Groups[1].Value) : null,
                OlympicFlagBearers = olympicFlagBearersMatch != null ? RegExpHelper.CutHtml(olympicFlagBearersMatch.Groups[1].Value) : null,
                Description = descriptionMatch != null ? RegExpHelper.CutHtml(descriptionMatch.Groups[1].Value) : null,
                BidProcess = bidProcessMatch != null ? RegExpHelper.CutHtml(bidProcessMatch.Groups[1].Value) : null
            };

            var openDateMatch = RegExpHelper.Match(model.HtmlDocument.DocumentNode.OuterHtml, @"<tr>\s*<th>Opening ceremony<\/th>\s*<td>\s*([\d]+)\s*([A-Za-z]+)\s*(\d+)?\s*<\/td>\s*<\/tr>");
            if (openDateMatch != null)
            {
                var day = int.Parse(openDateMatch.Groups[1].Value);
                var month = openDateMatch.Groups[2].Value.GetMonthNumber();
                game.OpeningCeremony = DateTime.ParseExact($"{day}-{month}-{(game.Year != 2020 ? game.Year : game.Year + 1)}", "d-M-yyyy", null);
            }

            var closeDateMatch = RegExpHelper.Match(model.HtmlDocument.DocumentNode.OuterHtml, @"<tr>\s*<th>Closing ceremony<\/th>\s*<td>\s*([\d]+)\s*([A-Za-z]+)\s*(\d+)?\s*<\/td>\s*<\/tr>");
            if (closeDateMatch != null)
            {
                var day = int.Parse(closeDateMatch.Groups[1].Value);
                var month = closeDateMatch.Groups[2].Value.GetMonthNumber();
                game.ClosingCeremony = DateTime.ParseExact($"{day}-{month}-{(game.Year != 2020 ? game.Year : game.Year + 1)}", "d-M-yyyy", null);
            }

            var competitionDateMatch = RegExpHelper.Match(model.HtmlDocument.DocumentNode.OuterHtml, @"<tr>\s*<th>Competition dates<\/th>\s*<td>\s*(\d+)\s*([A-Za-z]+)?\s*–\s*(\d+)\s*([A-Za-z]+)\s*(\d+)?\s*<\/td>\s*<\/tr>");
            if (competitionDateMatch != null)
            {
                var startDay = int.Parse(competitionDateMatch.Groups[1].Value);
                var startMonth = competitionDateMatch.Groups[2].Value != string.Empty ? competitionDateMatch.Groups[2].Value.GetMonthNumber() : competitionDateMatch.Groups[4].Value.GetMonthNumber();
                var endDay = int.Parse(competitionDateMatch.Groups[3].Value);
                var endMonth = competitionDateMatch.Groups[4].Value.GetMonthNumber();

                game.StartCompetitionDate = DateTime.ParseExact($"{startDay}-{startMonth}-{(game.Year != 2020 ? game.Year : game.Year + 1)}", "d-M-yyyy", null);
                game.EndCompetitionDate = DateTime.ParseExact($"{endDay}-{endMonth}-{(game.Year != 2020 ? game.Year : game.Year + 1)}", "d-M-yyyy", null);
            }

            if (game.Number == "XXXIII")
            {
                game.OpeningCeremony = new DateTime(2024, 7, 26);
                game.ClosingCeremony = new DateTime(2024, 8, 11);
                game.StartCompetitionDate = new DateTime(2024, 7, 24);
                game.EndCompetitionDate = new DateTime(2024, 8, 11);
            }

            var dbGame = await this.repository.GetAsync(x => x.Year == year && x.Type == type);
            if (dbGame != null)
            {
                var equals = true;
                if (game.Year == 1956 && game.Type == "Summer")
                {
                    dbGame.CoHost = game.Host;
                    dbGame.CoHostCountry = game.Country;
                    equals = false;
                }
                else
                {
                    equals = game.Equals(dbGame);
                }

                if (!equals)
                {
                    this.repository.Update(dbGame);
                    await this.repository.SaveChangesAsync();
                }
            }
            else
            {
                await this.repository.AddAsync(game);
                await this.repository.SaveChangesAsync();
            }
        }
    }

    private string SetOfficialName(string hostCity, int year)
    {
        if (hostCity == "Rio de Janeiro" && year == 2016)
        {
            hostCity = "Rio";
        }
        else if (hostCity == "Los Angeles" && year == 2028)
        {
            hostCity = "LA";
        }
        else if (hostCity == "Milano-Cortina d'Ampezzo" && year == 2026)
        {
            hostCity = "Milano Cortina";
        }

        return $"{hostCity} {year}";
    }
}