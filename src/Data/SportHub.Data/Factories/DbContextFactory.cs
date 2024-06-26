namespace SportHub.Data.Factories;

using Microsoft.EntityFrameworkCore;

using SportHub.Data.Contexts;
using SportHub.Data.Factories.Interfaces;

public class DbContextFactory : IDbContextFactory
{
    private readonly DbContextOptions<CrawlerStorageDbContext> crawlerStorageDbOptions;
    private readonly DbContextOptions<OlympicGamesDbContext> olympicGamesDbOptions;

    public DbContextFactory(DbContextOptions<CrawlerStorageDbContext> crawlerStorageDbOptions, DbContextOptions<OlympicGamesDbContext> olympicGamesDbOptions)
    {
        this.crawlerStorageDbOptions = crawlerStorageDbOptions;
        this.olympicGamesDbOptions = olympicGamesDbOptions;
    }

    public CrawlerStorageDbContext CreateCrawlerStorageDbContext()
    {
        return new CrawlerStorageDbContext(this.crawlerStorageDbOptions);
    }

    public OlympicGamesDbContext CreateOlympicGamesDbContext()
    {
        return new OlympicGamesDbContext(this.olympicGamesDbOptions);
    }
}