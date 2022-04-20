using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Headers;
using Yarp.ReverseProxy.Transforms;

namespace BffCore
{
    public static class ServiceCollectionExtensions
    {

        public static void AddReverseProxy(this IServiceCollection services, ConfigurationManager configuration)
        {
            var reverseProxyConfig = configuration.GetSection("ReverseProxy") ?? throw new ArgumentException("ReverseProxy section is missing!");

            services.AddReverseProxy()
                .LoadFromConfig(reverseProxyConfig)
                .AddTransforms(builderContext =>
                {
                    builderContext.AddRequestTransform(async transformContext =>
                    {
                        var accessToken = await transformContext.HttpContext.GetTokenAsync("access_token");
                        if (accessToken != null)
                        {
                            transformContext.ProxyRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                        }                      
                    });
                });
        }
        
    }
}