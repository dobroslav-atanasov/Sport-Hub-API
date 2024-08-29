namespace SportHub.Services.Data.CrawlerStorageDb.Interfaces;

using SportHub.Data.Entities.Crawlers;

public interface ILogsService
{
    Task AddLogAsync(Log log);

    Task UpdateLogAsync(Guid identifier, int operation);

    Task<IEnumerable<Guid>> GetLogIdentifiersAsync(int crawlerId);
}