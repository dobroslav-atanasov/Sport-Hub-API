namespace SportData.Data.Models.Users;

public class TokenModel
{
    public string AccessToken { get; set; }

    public string RefreshToken { get; set; }

    //public int? ExpirationInSeconds { get; set; }

    public string Username { get; set; }
}