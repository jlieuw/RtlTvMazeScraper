using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using TvMazeScraper.Core;

namespace TvMazeScraper.Web.Configurations
{
    public static class MazeApiSettingsExtensions
    {
        public static MazeApiSettings AddMazeApiSettings(this IServiceCollection services, IConfiguration configuration)
        {
            var mazeApiSettings = new MazeApiSettings();
            new ConfigureFromConfigurationOptions<MazeApiSettings>(configuration.GetSection("MazeApiSettings")).Configure(mazeApiSettings);
            services.AddSingleton(mazeApiSettings);

            return mazeApiSettings;
        }
    }
}
