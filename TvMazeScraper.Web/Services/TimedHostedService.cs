using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using TvMazeScraper.Core.Interfaces;

namespace TvMazeScraper.Web.Services
{
    public class TimedHostedService : IHostedService, IDisposable
    {
        private readonly ILogger _logger;
        private Timer _timer;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public TimedHostedService(ILogger<TimedHostedService> logger, IServiceScopeFactory serviceScopeFactory)
        {
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Timed Background Service is starting.");

            _timer = new Timer(async state => await(DoWork(cancellationToken)), null, TimeSpan.Zero,
                TimeSpan.FromHours(24)); // Runs daily

            return Task.CompletedTask;
        }

        private async Task DoWork(object state)
        {
            _logger.LogInformation("Timed Background Service is working.");

            using var scope = _serviceScopeFactory.CreateScope();
            var scraperService = scope.ServiceProvider.GetService<IScraperService>();
            await scraperService.StartScrapingAsync();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Timed Background Service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}