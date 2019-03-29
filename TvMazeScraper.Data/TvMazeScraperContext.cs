using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace TvMazeScraper.Data
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
            modelBuilder.Entity<ShowPerson>()
                .HasKey(s => new { s.ShowId, s.PersonId });
            base.OnModelCreating(modelBuilder);
        }
    }
}
