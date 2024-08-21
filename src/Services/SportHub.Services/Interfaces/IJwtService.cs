namespace SportHub.Services.Interfaces;

using System.Security.Claims;

using SportHub.Data.Entities.SportHub;
using SportHub.Data.Models.Users;

public interface IJwtService
{
    TokenModel GenerateToken(User user, IList<string> roles);

    string GenerateRefreshToken();

    ClaimsPrincipal ValidateToken(string token);
}