using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using Identity.API.Configuration;
using Identity.API.Data;
using Identity.API.Models;
using Identity.API.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MudBlazor.Services;

namespace Identity.API.Extensions
{
    public static class Extensions
    {
        public static void AddApplicationServices(this IHostApplicationBuilder builder)
        {
            builder.AddSqlServerDbContext<ApplicationContext>("IdentityDb");

            // Seed the database with default values
            //builder.Services.AddMigration<ApplicationContext, UsersSeed>();

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationContext>()
                .AddDefaultTokenProviders();

            builder.Services.AddIdentityServer(options =>
            {
                options.IssuerUri = "null";
                options.Authentication.CookieLifetime = TimeSpan.FromHours(2);

                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;
            })
            .AddInMemoryIdentityResources(Config.GetResources())
            .AddInMemoryApiScopes(Config.GetApiScopes())
            .AddInMemoryApiResources(Config.GetApis())
            .AddInMemoryClients(Config.GetClients(builder.Configuration))
            .AddAspNetIdentity<ApplicationUser>()
            .AddDeveloperSigningCredential();

            builder.Services.AddTransient<IProfileService, ProfileService>();
            builder.Services.AddTransient<ILoginService<ApplicationUser>, EFLoginService>();
            builder.Services.AddTransient<IRedirectService, RedirectService>();

            builder.Services.AddMudServices();
        }

        public static bool IsNativeClient(this AuthorizationRequest context)
        {
            return !context.RedirectUri.StartsWith("https", StringComparison.Ordinal)
               && !context.RedirectUri.StartsWith("http", StringComparison.Ordinal);
        }

        public static IActionResult LoadingPage(this Controller controller, string viewName, string redirectUri)
        {
            controller.HttpContext.Response.StatusCode = 200;
            controller.HttpContext.Response.Headers["Location"] = "";

            return controller.View(viewName, new RedirectViewModel { RedirectUrl = redirectUri });
        }
    }
}
