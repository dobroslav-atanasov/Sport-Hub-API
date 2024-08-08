namespace SportHub.WebAPI;

using System.Reflection;
using System.Text;

using Asp.Versioning;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

using SportHub.Common.Constants;
using SportHub.Data.Contexts;
using SportHub.Data.Factories;
using SportHub.Data.Factories.Interfaces;
using SportHub.Data.Models.DbEntities.SportHub;
using SportHub.Data.Options;
using SportHub.Data.Repositories;
using SportHub.Data.Seeders.SportHubDb;
using SportHub.Services;
using SportHub.Services.Data.CrawlerStorageDb;
using SportHub.Services.Data.CrawlerStorageDb.Interfaces;
using SportHub.Services.Data.SportHubDb;
using SportHub.Services.Data.SportHubDb.Interfaces;
using SportHub.Services.Interfaces;
using SportHub.Services.Mapper;
using SportHub.WebAPI.Infrastructure.Exceptions;
using SportHub.WebAPI.Infrastructure.Middlewares;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        ConfigureServices(builder.Services, builder.Configuration);
        var app = builder.Build();
        Configure(app);
        app.Run();
    }

    private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        // JWT token
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.SaveToken = true;
            options.RequireHttpsMetadata = false;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidIssuer = configuration[GlobalConstants.JWT_ISSUER],
                ValidAudience = configuration[GlobalConstants.JWT_AUDIENCE],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration[GlobalConstants.JWT_KEY])),
                ValidateIssuer = true,
                ValidateAudience = true,
            };
        }).AddBearerToken();

        services.AddAuthorization(options =>
        {
            options.AddPolicy(GlobalConstants.BEARER, new AuthorizationPolicyBuilder()
                .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                .RequireAuthenticatedUser()
                .Build());

            options.DefaultPolicy = options.GetPolicy(GlobalConstants.BEARER);
        });

        // Log to file
        services.AddLogging(config =>
        {
            config.AddConfiguration(configuration.GetSection(GlobalConstants.LOGGING));
            config.AddConsole();
            config.AddLog4Net(configuration.GetSection(GlobalConstants.LOG4NET_CORE).Get<Log4NetProviderOptions>());
        });

        // Identity
        services.AddIdentity<User, Role>(IdentityOptionsProvider.SetIdentityOptions)
            .AddRoles<Role>()
            .AddEntityFrameworkStores<SportHubDbContext>()
            .AddDefaultTokenProviders();

        // Databases options
        var crawlerStorageDbOptions = new DbContextOptionsBuilder<CrawlerStorageDbContext>()
            .UseSqlServer(configuration.GetConnectionString(GlobalConstants.CRAWLER_STORAGE_CONNECTION_STRING))
            .Options;

        var olympicGamesDbOptions = new DbContextOptionsBuilder<OlympicGamesDbContext>()
            .UseSqlServer(configuration.GetConnectionString(GlobalConstants.OLYMPIC_GAMES_CONNECTION_STRING))
            .Options;

        // Database factory
        var dbContextFactory = new DbContextFactory(crawlerStorageDbOptions, olympicGamesDbOptions);
        services.AddSingleton<IDbContextFactory>(dbContextFactory);

        services.AddDbContext<SportHubDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString(GlobalConstants.SPORT_DATA_CONNECTION_STRING));
        });

        services.AddDbContext<OlympicGamesDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString(GlobalConstants.OLYMPIC_GAMES_CONNECTION_STRING));
            options.UseLazyLoadingProxies();
        });

        // Automapper
        MapperConfig.RegisterMapper(Assembly.Load(GlobalConstants.AUTOMAPPER_MODELS_ASSEMBLY), Assembly.Load(GlobalConstants.AUTOMAPPER_VIEW_MODELS_ASSEMBLY));

        // API Version
        services.AddApiVersioning(options =>
        {
            options.DefaultApiVersion = new ApiVersion(1);
            options.ReportApiVersions = true;
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.ApiVersionReader = ApiVersionReader.Combine(new UrlSegmentApiVersionReader(), new HeaderApiVersionReader("X-Api-Version"));
        }).AddApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'V";
            options.SubstituteApiVersionInUrl = true;
        });

        // Repositories
        services.AddScoped(typeof(SportHubRepository<>));
        services.AddScoped(typeof(OlympicGamesRepository<>));

        // Middlewares
        services.AddTransient<JwtMiddleware>();
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();

        // CORS
        services.AddCors(options =>
        {
            options.AddPolicy(GlobalConstants.API_CORS, policy =>
            {
                var allowedOrigins = configuration.GetSection(GlobalConstants.ALLOWED_ORIGINS).Get<string[]>();
                policy.WithOrigins(allowedOrigins).AllowAnyHeader().AllowAnyMethod();
            });
        });

        RegisterServices(services);
    }

    private static void Configure(WebApplication app)
    {
        app.UseExceptionHandler();

        using (var serviceScope = app.Services.CreateScope())
        {
            var dbContext = serviceScope.ServiceProvider.GetRequiredService<SportHubDbContext>();
            dbContext.Database.Migrate();

            new SportHubDbSeeder().SeedAsync(serviceScope.ServiceProvider).GetAwaiter().GetResult();
        }

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }


        app.UseRouting();

        app.UseMiddleware<JwtMiddleware>();

        app.UseCors(GlobalConstants.API_CORS);

        app.UseHttpsRedirection();

        app.UseAuthentication();

        app.UseAuthorization();

        app.MapControllers().RequireAuthorization();

        app.MapAreaControllerRoute(name: "OlympicGames", areaName: "OlympicGames", pattern: "{area:exists}/{controller=Games}/{action=Get}/{id?}");

        app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");

        app.Run();
    }

    private static void RegisterServices(IServiceCollection services)
    {
        // Services
        services.AddScoped<IZipService, ZipService>();
        services.AddScoped<IHttpService, HttpService>();
        services.AddScoped<IMD5Hash, MD5Hash>();
        services.AddScoped<INormalizeService, NormalizeService>();
        services.AddScoped<IJwtService, JwtService>();

        // Data services
        services.AddScoped<IOperationsService, OperationsService>();
        services.AddScoped<ICrawlersService, CrawlersService>();
        services.AddScoped<IGroupsService, GroupsService>();
        services.AddScoped<ILogsService, LogsService>();
        services.AddScoped<IUsersService, UsersService>();
    }
}