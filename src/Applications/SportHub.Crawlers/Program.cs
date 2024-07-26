using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using SportHub.Common.Constants;
using SportHub.Crawlers.Countries;
using SportHub.Crawlers.OlympicGames.Olympedia;
using SportHub.Crawlers.OlympicGames.Paris2024;
using SportHub.Data.Contexts;
using SportHub.Data.Factories;
using SportHub.Data.Factories.Interfaces;
using SportHub.Services;
using SportHub.Services.Data.CrawlerStorageDb;
using SportHub.Services.Data.CrawlerStorageDb.Interfaces;
using SportHub.Services.Interfaces;

var configuration = new ConfigurationBuilder()
    .AddJsonFile(GlobalConstants.APP_SETTINGS_FILE, false, true)
    .Build();

var services = new ServiceCollection();

// CONFIGURATION
services.AddSingleton<IConfiguration>(configuration);

// LOGGING
services.AddLogging(options =>
{
    options.AddConfiguration(configuration.GetSection(GlobalConstants.LOGGING));
    options.AddConsole();
    options.AddLog4Net(configuration.GetSection(GlobalConstants.LOG4NET_CORE).Get<Log4NetProviderOptions>());
});

// DATABASE
var crawlerStorageDbOptions = new DbContextOptionsBuilder<CrawlerStorageDbContext>()
    .UseLazyLoadingProxies(true)
    .UseSqlServer(configuration.GetConnectionString(GlobalConstants.CRAWLER_STORAGE_CONNECTION_STRING))
    .Options;

var dbContextFactory = new DbContextFactory(crawlerStorageDbOptions, null);
services.AddSingleton<IDbContextFactory>(dbContextFactory);

services.AddDbContext<CrawlerStorageDbContext>(options =>
{
    options.UseLazyLoadingProxies();
    options.UseSqlServer(configuration.GetConnectionString(GlobalConstants.CRAWLER_STORAGE_CONNECTION_STRING));
});

services.AddScoped<IHttpService, HttpService>();
services.AddScoped<IMD5Hash, MD5Hash>();
services.AddScoped<IZipService, ZipService>();
services.AddScoped<IRegExpService, RegExpService>();
services.AddScoped<IOlympediaService, OlympediaService>();
services.AddScoped<IDateService, DateService>();

services.AddScoped<ICrawlersService, CrawlersService>();
services.AddScoped<IGroupsService, GroupsService>();
services.AddScoped<ILogsService, LogsService>();
services.AddScoped<IOperationsService, OperationsService>();

services.AddTransient<CountryDataCrawler>();
services.AddTransient<SportHub.Crawlers.OlympicGames.Olympedia.NOCCrawler>();
services.AddTransient<GameCrawler>();
services.AddTransient<SportDisciplineCrawler>();
services.AddTransient<SportHub.Crawlers.OlympicGames.Olympedia.ResultCrawler>();
services.AddTransient<SportHub.Crawlers.OlympicGames.Olympedia.AthleteCrawler>();
services.AddTransient<VenueCrawler>();

// PARIS 2024
services.AddTransient<LabelsCrawler>();
services.AddTransient<NOCsCrawler>();
services.AddTransient<SportHub.Crawlers.OlympicGames.Paris2024.NOCCrawler>();
services.AddTransient<LinksCrawler>();
services.AddTransient<SportCodesCrawler>();
services.AddTransient<EventCodesCrawler>();
services.AddTransient<DisciplinesCrawler>();
services.AddTransient<TeamsCrawler>();
services.AddTransient<HorsesCrawler>();
services.AddTransient<ParticipationsCrawler>();
services.AddTransient<AthletesCrawler>();
services.AddTransient<CoachesCrawler>();
services.AddTransient<JudgesCrawler>();
services.AddTransient<SportHub.Crawlers.OlympicGames.Paris2024.AthleteCrawler>();
services.AddTransient<CoachCrawler>();
services.AddTransient<JudgeCrawler>();
services.AddTransient<HorseCrawler>();
services.AddTransient<TeamCrawler>();
services.AddTransient<ScheduleCrawler>();
services.AddTransient<DisciplineCrawler>();
services.AddTransient<EventCrawler>();
services.AddTransient<SportHub.Crawlers.OlympicGames.Paris2024.ResultCrawler>();
services.AddTransient<PDFsCrawler>();

var serviceProvider = services.BuildServiceProvider();

//await serviceProvider.GetService<CountryDataCrawler>().StartAsync();

// OLYMPEDIA
//await serviceProvider.GetService<SportHub.Crawlers.Olympedia.NOCCrawler>().StartAsync();
//await serviceProvider.GetService<GameCrawler>().StartAsync();
//await serviceProvider.GetService<SportDisciplineCrawler>().StartAsync();
//await serviceProvider.GetService<VenueCrawler>().StartAsync();
//await serviceProvider.GetService<SportHub.Crawlers.Olympedia.ResultCrawler>().StartAsync();
//await serviceProvider.GetService<AthleteCrawler>().StartAsync();

// PARIS 2024
await serviceProvider.GetService<LabelsCrawler>().StartAsync();
await serviceProvider.GetService<NOCsCrawler>().StartAsync();
await serviceProvider.GetService<SportHub.Crawlers.OlympicGames.Paris2024.NOCCrawler>().StartAsync();
await serviceProvider.GetService<LinksCrawler>().StartAsync();
await serviceProvider.GetService<SportCodesCrawler>().StartAsync();
await serviceProvider.GetService<EventCodesCrawler>().StartAsync();
await serviceProvider.GetService<DisciplinesCrawler>().StartAsync();
await serviceProvider.GetService<TeamsCrawler>().StartAsync();
await serviceProvider.GetService<HorsesCrawler>().StartAsync();
await serviceProvider.GetService<ParticipationsCrawler>().StartAsync();
await serviceProvider.GetService<AthletesCrawler>().StartAsync();
await serviceProvider.GetService<CoachesCrawler>().StartAsync();
await serviceProvider.GetService<JudgesCrawler>().StartAsync();
await serviceProvider.GetService<SportHub.Crawlers.OlympicGames.Paris2024.AthleteCrawler>().StartAsync();
await serviceProvider.GetService<CoachCrawler>().StartAsync();
await serviceProvider.GetService<JudgeCrawler>().StartAsync();
await serviceProvider.GetService<HorseCrawler>().StartAsync();
await serviceProvider.GetService<TeamCrawler>().StartAsync();
await serviceProvider.GetService<ScheduleCrawler>().StartAsync();
await serviceProvider.GetService<DisciplineCrawler>().StartAsync();
await serviceProvider.GetService<EventCrawler>().StartAsync();
await serviceProvider.GetService<SportHub.Crawlers.OlympicGames.Paris2024.ResultCrawler>().StartAsync();
//await serviceProvider.GetService<PDFsCrawler>().StartAsync();