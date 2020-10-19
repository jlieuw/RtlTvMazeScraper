using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using TvMazeScraper.Infrastructure.Data;
using TvMazeScraper.Web;
using TvMazeScraper.Web.Services;

namespace TvMazeScraper.Test
{
    public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<Startup>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder
                .UseSolutionRelativeContentRoot("TvMazeScraper.Web")
                .ConfigureServices(services =>
            {

                var context = services.SingleOrDefault(
                    d => d.ServiceType ==
                        typeof(DbContextOptions<TvMazeScraperContext>));

                if (context != null)
                {
                    services.Remove(context);
                }
                var hostedService = services.SingleOrDefault(
                    d => d.ServiceType ==
                        typeof(TimedHostedService));

                if (hostedService != null)
                {
                    services.Remove(hostedService);
                }


                services.AddDbContext<TvMazeScraperContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryDbForTesting");
                });


                // Build the service provider.
                var sp = services.BuildServiceProvider();

                // Create a scope to obtain a reference to the database
                using var scope = sp.CreateScope();
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<TvMazeScraperContext>();

                var logger = scopedServices
                    .GetRequiredService<ILogger<CustomWebApplicationFactory<TStartup>>>();

                db.Database.EnsureCreated();

                try
                {
                    // Seed the database with test data.
                    SeedData.PopulateTestData(db);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred seeding the " +
                        $"database with test messages. Error: {ex.Message}");
                }
            });
        }
    }
}
