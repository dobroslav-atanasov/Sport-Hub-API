namespace SportHub.WebAPI.Infrastructure.Middlewares;

using System.Security.Claims;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;

using SportHub.Services.Interfaces;

public class JwtMiddleware : IMiddleware
{
    private readonly IJwtService jwtService;

    public JwtMiddleware(IJwtService jwtService)
    {
        this.jwtService = jwtService;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var token = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", string.Empty);

        if (!string.IsNullOrEmpty(token))
        {
            var claimsPrincipal = this.jwtService.ValidateToken(token);
            var email = claimsPrincipal.FindFirstValue(ClaimTypes.Email);
            var asd = claimsPrincipal.FindFirstValue(ClaimTypes.Gender);

            context.Items["email"] = email;
        }

        await next(context);
    }
}