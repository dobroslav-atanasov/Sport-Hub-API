namespace SportHub.Data.Options;

using Microsoft.AspNetCore.Identity;

public static class IdentityOptionsProvider
{
    public static void SetIdentityOptions(IdentityOptions options)
    {
        options.SignIn.RequireConfirmedEmail = false;

        options.Password.RequireLowercase = true;
        options.Password.RequireUppercase = true;
        options.Password.RequireNonAlphanumeric = true;
        options.Password.RequiredUniqueChars = 0;
        options.Password.RequiredLength = 6;
        options.User.RequireUniqueEmail = true;
    }
}