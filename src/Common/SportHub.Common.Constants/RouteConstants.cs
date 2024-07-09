namespace SportHub.Common.Constants;

public static class RouteConstants
{
    public const string API_DEFAULT_ROUTE = "/api/v{v:apiVersion}/[controller]";
    public const string API_OLYMPIC_GAMES_AREA_ROUTE = "/api/v{v:apiVersion}/OlympicGames/[controller]";

    public const string TOKENS_CREATE_REFRESH_TOKEN = "create-refresh-token";
    public const string TOKENS_DELETE_REFRESH_TOKENS = "delete-refresh-tokens";

    public const string OLYMPIC_GAMES_GAMES_ALL = "all";
}