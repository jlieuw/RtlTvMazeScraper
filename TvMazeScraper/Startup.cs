using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using System;
using TvMazeScraper.Data;
using TvMazeScraper.Data.Repositories;
using TvMazeScraper.ScheduledServices;
using TvMazeScraper.Services;

namespace TvMazeScraper
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddDbContext<TvMazeScraperContext>(opts =>
            {
                opts.UseSqlServer(Configuration.GetConnectionString("TvMazeScraperConnectionString"));
            });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "TvMazeScraper API", Version = "v1" });
            });

            services
                .AddHttpClient<IScraperService, ScraperService>(client =>
                {
                    client.BaseAddress = new Uri("http://api.tvmaze.com");
                })
                .AddPolicyHandler(PolicyHandler.WaitAndRetry())
                .AddPolicyHandler(PolicyHandler.Timeout());


            services.AddHostedService<TimedHostedService>();
            services.AddScoped<IShowRepository, ShowRepository>();
            services.AddScoped<IPersonRepository, PersonRepository>();
            services.AddScoped<IShowPersonRepository, ShowPersonRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "TvMazeScraper API V1");
            });
            app.UseMvc();
        }
    }
}
