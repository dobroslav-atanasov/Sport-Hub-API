namespace SportHub.Common.Constants;

public static class GlobalConstants
{
    public const string LOGGING = "Logging";
    public const string LOG4NET_CORE = "Log4NetCore";

    public const string APP_SETTINGS_FILE = "appsettings.json";

    public const string CRAWLER_STORAGE_CONNECTION_STRING = "CrawlerStorageConnectionString";
    public const string OLYMPIC_GAMES_CONNECTION_STRING = "OlympicGamesConnectionString";
    public const string SPORT_DATA_CONNECTION_STRING = "SportHubConnectionString";

    public const string AUTOMAPPER_MODELS_ASSEMBLY = "SportHub.Data.Models";
    public const string AUTOMAPPER_VIEW_MODELS_ASSEMBLY = "SportHub.Data.ViewModels";

    public const string JWT_KEY = "Jwt:Key";
    public const string JWT_ISSUER = "Jwt:Issuer";
    public const string JWT_AUDIENCE = "Jwt:Audience";
    public const string JWT_TOKEN_VALIDITY_IN_MINUTES = "Jwt:TokenValidityInMinutes";
    public const string JWT_REFRESH_TOKEN_VALIDITY_IN_DAYS = "Jwt:RefreshTokenValidityInDays";

    public const string ALLOWED_ORIGINS = "AllowedOrigins";
    public const string API_CORS = "ApiCors";
    public const string BEARER = "Bearer";

    public const double INCHES = 2.54;
    public const double POUNDS = 2.20462;
}