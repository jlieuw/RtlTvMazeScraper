using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TvMazeScraper.Infrastructure.Data;

namespace TvMazeScraper.Infrastructure
{
    public static class StartupSetup
    {
        public static void AddDbContext(this IServiceCollection services, string connectionString) =>
            services.AddDbContext<TvMazeScraperContext>(opts =>
            {
                opts.UseSqlServer(connectionString);
            });
    }
}
