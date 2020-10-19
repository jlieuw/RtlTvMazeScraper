using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TvMazeScraper.Core.Interfaces;
using TvMazeScraper.Infrastructure;
using TvMazeScraper.Web.Configurations;
using TvMazeScraper.Web.Interfaces;
using TvMazeScraper.Web.Services;

namespace TvMazeScraper.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddNewtonsoftJson();

            services.AddDbContext(Configuration.GetConnectionString("TvMazeScraperConnectionString"));
            var mazeApiSettings = services.AddMazeApiSettings(Configuration);
            services.AddHttpClientWithPolicyHandler(mazeApiSettings);
            services.AddSwaggerDocumentation();
            services.AddScoped<IScraperService, ScraperService>();
            services.AddScoped<IShowService, ShowService>();
            services.AddHostedService<TimedHostedService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseSwaggerDocumentation();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        }
    }
}
