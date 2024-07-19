﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using SportHub.Common.Constants;
using SportHub.Crawlers.Countries;
using SportHub.Crawlers.Olympedia;
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
services.AddTransient<NOCCrawler>();
services.AddTransient<GameCrawler>();
services.AddTransient<SportDisciplineCrawler>();
services.AddTransient<ResultCrawler>();
services.AddTransient<AthleteCrawler>();
services.AddTransient<VenueCrawler>();

var serviceProvider = services.BuildServiceProvider();

//await serviceProvider.GetService<CountryDataCrawler>().StartAsync();
//await serviceProvider.GetService<NOCCrawler>().StartAsync();
//await serviceProvider.GetService<GameCrawler>().StartAsync();
//await serviceProvider.GetService<SportDisciplineCrawler>().StartAsync();
//await serviceProvider.GetService<VenueCrawler>().StartAsync();
//await serviceProvider.GetService<ResultCrawler>().StartAsync();
await serviceProvider.GetService<AthleteCrawler>().StartAsync();