using Microsoft.Extensions.DependencyInjection;
using System;
using TvMazeScraper.Core;
using TvMazeScraper.Core.Interfaces;
using TvMazeScraper.Infrastructure;

namespace TvMazeScraper.Web.Configurations
{
    public static class HttpClientExtensions
    {
        public static void AddHttpClientWithPolicyHandler(this IServiceCollection services, MazeApiSettings mazeApiSettings)
        {
            services
                .AddHttpClient<ITvMazeHttpClient, TvMazeHttpClient>(client =>
                {
                    client.BaseAddress = new Uri(mazeApiSettings.BaseUrl);
                })
                .AddPolicyHandler(PolicyHandler.WaitAndRetry())
                .AddPolicyHandler(PolicyHandler.Timeout());
        }
    }
}
