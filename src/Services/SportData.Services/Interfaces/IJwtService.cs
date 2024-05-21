namespace SportData.Services.Interfaces;

using System.Security.Claims;

using SportData.Data.Models.Entities.SportData;
using SportData.Data.Models.Users;

public interface IJwtService
{
    TokenModel GenerateToken(User user, IList<string> roles);

    string GenerateRefreshToken();

    ClaimsPrincipal ValidateToken(string token);
}