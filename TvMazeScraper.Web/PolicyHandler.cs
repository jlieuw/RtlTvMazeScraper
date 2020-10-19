using Polly;
using Polly.Extensions.Http;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace TvMazeScraper
{
    public static class PolicyHandler
    {
        public static IAsyncPolicy<HttpResponseMessage> WaitAndRetry() =>
            HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
                .WaitAndRetryAsync(retryCount: 5,
            sleepDurationProvider: (retryCount, response, context) =>
            {
                var serverWaitDuration = GetServerWaitDuration(response.Result?.Headers.RetryAfter);
                var waitDuration = serverWaitDuration.TotalMilliseconds;
                return TimeSpan.FromMilliseconds(waitDuration);
            },
                    onRetryAsync: (e, ts, i, ctx) => Task.CompletedTask);

        private static TimeSpan GetServerWaitDuration(RetryConditionHeaderValue retryAfter)
        {
            if (retryAfter == null)
                return TimeSpan.Zero;

            return retryAfter.Date.HasValue
                ? retryAfter.Date.Value - DateTime.UtcNow
                : retryAfter.Delta.GetValueOrDefault(TimeSpan.Zero);
        }
        public static IAsyncPolicy<HttpResponseMessage> Timeout(int seconds = 2) =>
            Policy.TimeoutAsync<HttpResponseMessage>(TimeSpan.FromSeconds(seconds));
    }
}
