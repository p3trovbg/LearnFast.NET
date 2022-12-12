namespace LearnFast.Data
{
    using LearnFast.Data.Models;
    using Microsoft.AspNetCore.Identity;
    using System;

    public static class IdentityOptionsProvider
    {
        public static void GetIdentityOptions(IdentityOptions options)
        {
            options.Password.RequireDigit = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequiredLength = 6;
            options.User.RequireUniqueEmail = false;
            options.SignIn.RequireConfirmedAccount = true;
        }
    }
}
