using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace AuthCore
{
    public static class ServiceCollectionExtensions
    {
        public static void AddAuthenticationForWeb(this IServiceCollection services, IConfiguration configuration, OpenIdConnectEvents events = null)
        {
            var authenticationConfig = configuration.GetSection("Authentication") ?? throw new ArgumentException("Authentication section is missing!");

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
                options.DefaultSignOutScheme = OpenIdConnectDefaults.AuthenticationScheme;
            })
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
            {
                options.Cookie.Name = authenticationConfig["CookieName"] ?? "__BFF";
                options.Cookie.SameSite = SameSiteMode.Strict;
                options.Cookie.HttpOnly = false;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            })
            .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
            {
                // Bind OIDC authentication configuration
                configuration.GetSection("Authentication").Bind(options);

                options.Events = events;

                // Add dynamically scope values from configuration
                var scopes = configuration.GetSection("Authentication")["Scope"].Split(" ").ToList();
                options.Scope.Clear();
                scopes.ForEach(scope => options.Scope.Add(scope));

                options.TokenValidationParameters = new()
                {
                    NameClaimType = "name",
                    RoleClaimType = "role"
                };
            });       
        }

        public static void AddAuthenticationForApi(this IServiceCollection services, IConfiguration configuration, OpenIdConnectEvents events = null)
        { 
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    configuration.GetSection("Authentication").Bind(options);
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateAudience = false,
                        ValidTypes = new[] { "at+jwt" },
                        NameClaimType = "name",
                        RoleClaimType = "role"
                    };
                });
        }
    }
}