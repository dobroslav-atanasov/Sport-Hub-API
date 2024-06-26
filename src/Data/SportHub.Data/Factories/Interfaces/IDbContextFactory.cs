namespace SportHub.Data.Factories.Interfaces;

using SportHub.Data.Contexts;

public interface IDbContextFactory
{
    CrawlerStorageDbContext CreateCrawlerStorageDbContext();

    OlympicGamesDbContext CreateOlympicGamesDbContext();
}