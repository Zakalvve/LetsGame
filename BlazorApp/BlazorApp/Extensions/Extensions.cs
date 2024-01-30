using BlazorApp.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;
using Microsoft.IdentityModel.JsonWebTokens;

namespace BlazorApp.Extensions
{
    public static class Extensions
    {
        public static void AddApplicationServices(this IHostApplicationBuilder builder)
        {

            builder.AddAuthenticationServices();

            builder.Services.AddHttpClient<CoreApiClient>(c =>
            {
                c.BaseAddress = new("http://core-api");
            });

            builder.Services.AddSingleton(builder.Configuration);

            // Add services to the container.
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents()
                .AddInteractiveWebAssemblyComponents();

            builder.Services.AddServerSideBlazor().AddCircuitOptions(option => { option.DetailedErrors = true; });
        }

        public static void AddAuthenticationServices(this IHostApplicationBuilder builder)
        {
            var configuration = builder.Configuration;
            var services = builder.Services;

            JsonWebTokenHandler.DefaultInboundClaimTypeMap.Remove("sub");

            var identityUrl = configuration.GetRequiredValue("IdentityUrl");
            var callBackUrl = configuration.GetRequiredValue("CallBackUrl");
            var sessionCookieLifetime = configuration.GetValue("SessionCookieLifetimeMinutes", 60);

            services.AddAuthorization();

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            })
            .AddCookie(options => options.ExpireTimeSpan = TimeSpan.FromMinutes(sessionCookieLifetime))
            .AddOpenIdConnect(options =>
            {
                options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.Authority = identityUrl;
                options.SignedOutRedirectUri = callBackUrl;
                options.ClientId = "webapp";
                options.ClientSecret = "secret";
                options.ResponseType = "code";
                options.SaveTokens = true;
                options.GetClaimsFromUserInfoEndpoint = true;
                options.RequireHttpsMetadata = false;
                options.Scope.Add("openid");
                options.Scope.Add("core-api");
            });

            services.AddScoped<AuthenticationStateProvider, ServerAuthenticationStateProvider>();
            services.AddCascadingAuthenticationState();
        }

        public static async Task<string?> GetBuyerIdAsync(this AuthenticationStateProvider authenticationStateProvider)
        {
            var authState = await authenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;
            return user.FindFirst("sub")?.Value;
        }

        public static async Task<string?> GetUserNameAsync(this AuthenticationStateProvider authenticationStateProvider)
        {
            var authState = await authenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;
            return user.FindFirst("name")?.Value;
        }
    }
}
