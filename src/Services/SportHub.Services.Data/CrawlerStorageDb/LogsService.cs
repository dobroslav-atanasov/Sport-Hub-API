namespace SportHub.Services.Data.CrawlerStorageDb;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using SportHub.Data.Factories.Interfaces;
using SportHub.Data.Models.DbEntities.Crawlers;
using SportHub.Data.Models.DbEntities.Crawlers.Enumerations;
using SportHub.Services.Data.CrawlerStorageDb.Interfaces;

public class LogsService : ILogsService
{
    private readonly IDbContextFactory dbContextFactory;

    public LogsService(IDbContextFactory dbContextFactory)
    {
        this.dbContextFactory = dbContextFactory;
    }

    public async Task AddLogAsync(Log log)
    {
        using var context = this.dbContextFactory.CreateCrawlerStorageDbContext();

        await context.AddAsync(log);
        await context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Guid>> GetLogIdentifiersAsync(int crawlerId)
    {
        using var context = this.dbContextFactory.CreateCrawlerStorageDbContext();

        var identifiers = await context
            .Logs
            .Where(l => l.CrawlerId == crawlerId && l.Operation != (int)OperationType.Delete)
            .Select(l => l.Identifier)
            .ToListAsync();

        return identifiers;
    }

    public async Task UpdateLogAsync(Guid identifier, int operation)
    {
        using var context = this.dbContextFactory.CreateCrawlerStorageDbContext();

        var log = await context.Logs.FirstOrDefaultAsync(x => x.Identifier == identifier);

        if (log != null)
        {
            log.Operation = operation;

            context.Update(log);
            await context.SaveChangesAsync();
        }
    }
}