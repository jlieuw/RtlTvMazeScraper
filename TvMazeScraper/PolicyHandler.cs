using Polly;
using Polly.Extensions.Http;
using Polly.Timeout;
using System;
using System.Net.Http;

namespace TvMazeScraper
{
    public static class PolicyHandler
    {
        public static IAsyncPolicy<HttpResponseMessage> WaitAndRetry(int retryCount = 5) =>
            HttpPolicyExtensions
                .HandleTransientHttpError()
                .Or<HttpRequestException>()
                .Or<TimeoutRejectedException>()
                .WaitAndRetryAsync(retryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

        public static IAsyncPolicy<HttpResponseMessage> Timeout(int seconds = 2) =>
            Policy.TimeoutAsync<HttpResponseMessage>(TimeSpan.FromSeconds(seconds));
    }
}
