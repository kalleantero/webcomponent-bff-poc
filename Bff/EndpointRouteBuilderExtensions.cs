using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;

namespace BffCore
{
    public static class EndpointRouteBuilderExtensions
    {
        public static void AddBffEndpoints(this IEndpointRouteBuilder endpoints, IConfiguration configuration)
        {
            endpoints.MapGet("/login", async (HttpContext httpContext, string redirectUri) =>
            {
                await httpContext.ChallengeAsync(OpenIdConnectDefaults.AuthenticationScheme, new AuthenticationProperties { RedirectUri = redirectUri });
            });
            endpoints.MapGet("/logout", async (HttpContext httpContext, string redirectUri) =>
            {
                await httpContext.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme);
                var props = new AuthenticationProperties
                {
                    RedirectUri = redirectUri ?? "/"
                };
                await httpContext.SignOutAsync(props);
            });

            var webComponentScripts = configuration.GetSection("WebcomponentScripts").Get<List<WebComponentScript>>();

            if (webComponentScripts != null)
            {
                foreach (var webComponentScript in webComponentScripts)
                {
                    endpoints.MapGet(webComponentScript.EndpointName, async ([FromServices] IHttpClientFactory httpClientFactory) =>
                    {
                        var client = httpClientFactory.CreateClient();
                        var response = await client.GetAsync(webComponentScript.ScriptUrl);
                        var responseStream = await response.Content.ReadAsStreamAsync();
                        var reader = new StreamReader(responseStream);
                        return reader.ReadToEnd();
                    });
                }
            }         
        }
    }

    public class WebComponentScript
    {
        public string EndpointName { get; set; }
        public string ScriptUrl { get; set; }
    }
}