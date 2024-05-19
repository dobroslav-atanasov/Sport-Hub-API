namespace SportData.WebAPI.Infrastructure.Filters;

using Microsoft.AspNetCore.Mvc.Filters;

public class CustomAuthorizationFilterAttribute : IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        ;
    }
}