namespace SportHub.Converters.OlympicGames.Paris2024.Base;

using System.Text;

using Microsoft.Extensions.Logging;

using SportHub.Common.Helpers;
using SportHub.Data.Entities.Crawlers;
using SportHub.Data.Models.Converters.OlympicGames;
using SportHub.Data.Models.Converters.OlympicGames.Paris2024;
using SportHub.Services.Data.CrawlerStorageDb.Interfaces;
using SportHub.Services.Data.OlympicGamesDb.Interfaces;
using SportHub.Services.Interfaces;

public abstract class Paris2024Converter : BaseConverter
{
    protected Paris2024Converter(ILogger<BaseConverter> logger, ICrawlersService crawlersService, ILogsService logsService, IGroupsService groupsService, IZipService zipService,
        INormalizeService normalizeService, IDataCacheService dataCacheService)
        : base(logger, crawlersService, logsService, groupsService, zipService, normalizeService, dataCacheService)
    {
    }

    protected ConverterModel<DocumentConverterModel> PrepareConverterModel(Group group)
    {
        var model = new ConverterModel<DocumentConverterModel>
        {
            Identifier = group.Identifier,
            CrawlerId = group.CrawlerId,
            Name = group.Name,
        };

        foreach (var document in group.Documents)
        {
            var encoding = Encoding.GetEncoding(document.Encoding);
            model.Documents.Add(document.Order,
                new DocumentConverterModel
                {
                    Encoding = encoding,
                    Json = encoding.GetString(document.Content)
                });
        }

        return model;
    }

    protected CodeInfo ExtractCodeInfo(string code)
    {
        var match = RegExpHelper.Match(code, @"(S|W)-([\d]{4})-([\w]{3})-(M|W|X|O)-([\w\-]{12})-([\w\-]{4})-([\w\-]{8})");
        if (match != null)
        {
            var codeInfo = new CodeInfo
            {
                GameType = match.Groups[1].Value,
                Year = int.Parse(match.Groups[2].Value),
                Discipline = match.Groups[3].Value,
                Gender = match.Groups[4].Value,
                Event = match.Groups[5].Value.Replace("-", string.Empty).Trim(),
                Phase = match.Groups[6].Value.Replace("-", string.Empty).Trim(),
                Unit = match.Groups[7].Value.Replace("-", string.Empty).Trim()
            };

            return codeInfo;
        }

        return null;
    }

    protected string GenerateCode(string type, int year, string oldCode, bool isTeam = false)
    {
        var code = $"{type.Substring(0, 1)}-{year}-";

        if (!isTeam)
        {
            var match = RegExpHelper.Match(oldCode, @"^([\w]{3})(M|W|X|O)([\w\-]{18})([\w\-]{4})([\w\-]{8})");
            if (match != null)
            {
                var @eventStr = match.Groups[3].Value.Replace("SCULL2-L-", "SCULL2LW-").Replace("-", string.Empty);
                code = $"{code}{match.Groups[1].Value}-{match.Groups[2].Value}-{@eventStr}";
                code = code.PadRight(26, '-');
                var unitStr = match.Groups[5].Value.Replace("-", string.Empty);
                code = $"{code}{match.Groups[4].Value}-{unitStr}";
                code = code.PadRight(39, '0');

                return code;
            }

            match = RegExpHelper.Match(oldCode, @"^([\w]{3})(M|W|X|O)([\w\-]{18})([\w\-]{4})");
            if (match != null)
            {
                var @eventStr = match.Groups[3].Value.Replace("SCULL2-L-", "SCULL2LW-").Replace("-", string.Empty);
                code = $"{code}{match.Groups[1].Value}-{match.Groups[2].Value}-{@eventStr}";
                code = code.PadRight(26, '-');
                code = $"{code}{match.Groups[4].Value}";
                code = code.PadRight(39, '-');
                return code;
            }

            match = RegExpHelper.Match(oldCode, @"^([\w]{3})(M|W|O|X)([\w\-]{8})");
            if (match != null)
            {
                var @eventStr = match.Groups[3].Value.Replace("SCULL2-L-", "SCULL2LW-").Replace("-", string.Empty);
                code = $"{code}{match.Groups[1].Value}-{match.Groups[2].Value}-{@eventStr}";
                code = code.PadRight(39, '-');
                return code;
            }
        }
        else
        {
            var match = RegExpHelper.Match(oldCode, @"^([\w]{3})(M|W|X|O)([\w\-]{8})([\w]{3})([\d]{2})");
            if (match != null)
            {
                var discipline = match.Groups[1].Value;
                var gender = match.Groups[2].Value;
                var @event = match.Groups[3].Value.Replace("SCULL2-L-", "SCULL2LW-").Replace("-", string.Empty);
                var country = match.Groups[4].Value;
                var number = match.Groups[5].Value;

                if (discipline == "SWA" && @event == "TEAM8")
                {
                    gender = "O";
                }

                code = $"{code}{discipline}-{gender}-{@event}";
                code = code.PadRight(26, '-');
                code = $"{code}{country}-{number}";

                return code;
            }
        }

        return null;
    }
}