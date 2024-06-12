namespace SportData.Services.Data.CrawlerStorageDb;

using System.Threading.Tasks;

using SportData.Data.Factories.Interfaces;
using SportData.Data.Models.Entities.Crawlers;
using SportData.Services.Data.CrawlerStorageDb.Interfaces;

public class CrawlersService : ICrawlersService
{
    private readonly IDbContextFactory dbContextFactory;

    public CrawlersService(IDbContextFactory dbContextFactory)
    {
        this.dbContextFactory = dbContextFactory;
    }

    public async Task AddCrawler(string crawlerName)
    {
        using var context = this.dbContextFactory.CreateCrawlerStorageDbContext();
        var crawler = new Crawler { Name = crawlerName };

        await context.AddAsync(crawler);
        await context.SaveChangesAsync();
    }

    public async Task<int> GetCrawlerIdAsync(string crawlerName)
    {
        using var context = this.dbContextFactory.CreateCrawlerStorageDbContext();
        var crawler = context.Crawlers.FirstOrDefault(x => x.Name == crawlerName);

        if (crawler == null)
        {
            await this.AddCrawler(crawlerName);
            return await this.GetCrawlerIdAsync(crawlerName);
        }

        return crawler.Id;
    }
}