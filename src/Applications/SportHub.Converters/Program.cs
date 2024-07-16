using System.Reflection;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using SportHub.Common.Constants;
using SportHub.Converters.Countries;
using SportHub.Converters.OlympicGames;
using SportHub.Converters.OlympicGames.SportConverters;
using SportHub.Converters.Profiles;
using SportHub.Data.Contexts;
using SportHub.Data.Factories;
using SportHub.Data.Factories.Interfaces;
using SportHub.Data.Repositories;
using SportHub.Data.Seeders.OlympicGamesDb;
using SportHub.Services;
using SportHub.Services.Data.CrawlerStorageDb;
using SportHub.Services.Data.CrawlerStorageDb.Interfaces;
using SportHub.Services.Data.OlympicGamesDb;
using SportHub.Services.Data.OlympicGamesDb.Interfaces;
using SportHub.Services.Interfaces;
using SportHub.Services.Mapper;

var configuration = new ConfigurationBuilder()
    .AddJsonFile(GlobalConstants.APP_SETTINGS_FILE, false, true)
    .Build();

var services = new ServiceCollection();

// CONFIGURATION
services.AddSingleton<IConfiguration>(configuration);

// LOGGING
services.AddLogging(config =>
{
    config.AddConfiguration(configuration.GetSection(GlobalConstants.LOGGING));
    config.AddConsole();
    config.AddLog4Net(configuration.GetSection(GlobalConstants.LOG4NET_CORE).Get<Log4NetProviderOptions>());
});

// AUTOMAPPER
services.AddAutoMapper(typeof(OlympicGamesProfile));
MapperConfig.RegisterMapper(Assembly.Load(GlobalConstants.AUTOMAPPER_MODELS_ASSEMBLY), Assembly.Load(GlobalConstants.AUTOMAPPER_VIEW_MODELS_ASSEMBLY));

// DATABASE
var crawlerStorageDbOptions = new DbContextOptionsBuilder<CrawlerStorageDbContext>()
    .UseLazyLoadingProxies(true)
    .UseSqlServer(configuration.GetConnectionString(GlobalConstants.CRAWLER_STORAGE_CONNECTION_STRING))
    .Options;

var sportDataDbOptions = new DbContextOptionsBuilder<CrawlerStorageDbContext>()
    .UseLazyLoadingProxies(true)
    .UseSqlServer(configuration.GetConnectionString(GlobalConstants.SPORT_DATA_CONNECTION_STRING))
    .Options;

var olympicGamesDbOptions = new DbContextOptionsBuilder<OlympicGamesDbContext>()
    .UseLazyLoadingProxies(true)
    .UseSqlServer(configuration.GetConnectionString(GlobalConstants.OLYMPIC_GAMES_CONNECTION_STRING))
    .Options;

var dbContextFactory = new DbContextFactory(crawlerStorageDbOptions, olympicGamesDbOptions);
services.AddSingleton<IDbContextFactory>(dbContextFactory);

services.AddDbContext<SportHubDbContext>(options =>
{
    options.UseSqlServer(configuration.GetConnectionString(GlobalConstants.SPORT_DATA_CONNECTION_STRING));
    options.UseLazyLoadingProxies();
});

services.AddDbContext<CrawlerStorageDbContext>(options =>
{
    options.UseSqlServer(configuration.GetConnectionString(GlobalConstants.CRAWLER_STORAGE_CONNECTION_STRING));
    options.UseLazyLoadingProxies();
});

services.AddDbContext<OlympicGamesDbContext>(options =>
{
    options.UseSqlServer(configuration.GetConnectionString(GlobalConstants.OLYMPIC_GAMES_CONNECTION_STRING));
    options.UseLazyLoadingProxies();
});

services.AddScoped(typeof(SportHubRepository<>));
services.AddScoped(typeof(OlympicGamesRepository<>));
services.AddScoped(typeof(DataService<>));

services.AddScoped<IZipService, ZipService>();
services.AddScoped<IRegExpService, RegExpService>();
services.AddScoped<IHttpService, HttpService>();
services.AddScoped<IMD5Hash, MD5Hash>();
services.AddScoped<INormalizeService, NormalizeService>();
services.AddScoped<IOlympediaService, OlympediaService>();
services.AddScoped<IDateService, DateService>();

services.AddScoped<ICrawlersService, CrawlersService>();
services.AddScoped<IGroupsService, GroupsService>();
services.AddScoped<ILogsService, LogsService>();
services.AddScoped<SportHub.Services.Data.SportHubDb.Interfaces.ICountriesService, SportHub.Services.Data.SportHubDb.CountriesService>();
services.AddScoped<IDataCacheService, DataCacheService>();

services.AddScoped<CountryDataConverter>();
services.AddScoped<NationalOlympicCommitteeConverter>();
services.AddScoped<CountryConverter>();
services.AddScoped<GameConverter>();
services.AddScoped<SportDisciplineConverter>();
services.AddScoped<VenueConverter>();
services.AddScoped<EventConverter>();
services.AddScoped<AthleteConverter>();
services.AddScoped<ParticipantConverter>();
services.AddScoped<ResultConverter>();
services.AddScoped<BasketballConverter>();
services.AddScoped<SkiingConverter>();
services.AddScoped<ArcheryConverter>();
services.AddScoped<GymnasticsConverter>();
services.AddScoped<AquaticsConverter>();
services.AddScoped<AthleticsConverter>();
services.AddScoped<BadmintonConverter>();
services.AddScoped<BaseballSoftballConverter>();
services.AddScoped<OldSportsConverter>();
services.AddScoped<VolleyballConverter>();
services.AddScoped<BiathlonConverter>();
services.AddScoped<BobsleighConverter>();
services.AddScoped<BoxingConverter>();
services.AddScoped<CanoeConverter>();
services.AddScoped<CurlingConverter>();
services.AddScoped<CyclingConverter>();
services.AddScoped<EquestrianConverter>();
services.AddScoped<FencingConverter>();
services.AddScoped<SkatingConverter>();
services.AddScoped<FootballConverter>();

var serviceProvider = services.BuildServiceProvider();

var dbContext = serviceProvider.GetRequiredService<OlympicGamesDbContext>();
dbContext.Database.Migrate();
new OlympicGamesDbSeeder().SeedAsync(serviceProvider).GetAwaiter().GetResult();

//await serviceProvider.GetService<GameConverter>().ConvertAsync(ConverterConstants.OLYMPEDIA_GAME_CONVERTER);
//await serviceProvider.GetService<SportDisciplineConverter>().ConvertAsync(ConverterConstants.OLYMPEDIA_SPORT_DISCIPLINE_CONVERTER);
await serviceProvider.GetService<EventConverter>().ConvertAsync(ConverterConstants.OLYMPEDIA_RESULT_CONVERTER);
//await serviceProvider.GetService<VenueConverter>().ConvertAsync(ConverterConstants.OLYMPEDIA_VENUE_CONVERTER);
//await serviceProvider.GetService<NationalOlympicCommitteeConverter>().ConvertAsync(ConverterConstants.OLYMPEDIA_NOC_CONVERTER);
//await serviceProvider.GetService<CountryConverter>().ConvertAsync(ConverterConstants.COUNTRY_CONVERTER);

//await serviceProvider.GetService<AthleteConverter>().ConvertAsync(ConverterConstants.OLYMPEDIA_ATHELETE_CONVERTER);
//await serviceProvider.GetService<ParticipantConverter>().ConvertAsync(ConverterConstants.OLYMPEDIA_RESULT_CONVERTER);
//await serviceProvider.GetService<ResultConverter>().ConvertAsync(ConverterConstants.OLYMPEDIA_RESULT_CONVERTER);