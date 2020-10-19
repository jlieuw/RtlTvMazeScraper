using Microsoft.EntityFrameworkCore;
using System.Reflection;
using TvMazeScraper.Core.Entities;

namespace TvMazeScraper.Infrastructure.Data
{
    public class TvMazeScraperContext : DbContext
    {
        public TvMazeScraperContext(DbContextOptions<TvMazeScraperContext> options) : base(options)
        {
        }

        public DbSet<Show> Shows { get; set; }
        public DbSet<Person> Persons { get; set; }
        public DbSet<ShowPerson> ShowPerson { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }
    }
}
