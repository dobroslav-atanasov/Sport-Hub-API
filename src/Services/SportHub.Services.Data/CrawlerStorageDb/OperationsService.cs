namespace SportHub.Services.Data.CrawlerStorageDb;

using SportHub.Data.Factories.Interfaces;
using SportHub.Data.Models.DbEntities.Crawlers;
using SportHub.Services.Data.CrawlerStorageDb.Interfaces;

public class OperationsService : IOperationsService
{
    private readonly IDbContextFactory dbContextFactory;

    public OperationsService(IDbContextFactory dbContextFactory)
    {
        this.dbContextFactory = dbContextFactory;
    }

    public async Task AddOperationAsync(string operationName)
    {
        using var context = this.dbContextFactory.CreateCrawlerStorageDbContext();

        var operation = new Operation { Name = operationName };

        await context.AddAsync(operation);
        await context.SaveChangesAsync();
    }

    public bool IsOperationTableFull()
    {
        using var context = this.dbContextFactory.CreateCrawlerStorageDbContext();
        return context.Operations.Any();
    }
}