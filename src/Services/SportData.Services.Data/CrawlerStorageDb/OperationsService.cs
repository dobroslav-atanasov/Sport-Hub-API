namespace SportData.Services.Data.CrawlerStorageDb;

using SportData.Data.Factories.Interfaces;
using SportData.Data.Models.Entities.Crawlers;
using SportData.Services.Data.CrawlerStorageDb.Interfaces;

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