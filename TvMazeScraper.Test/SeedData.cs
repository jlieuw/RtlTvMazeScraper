using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using TvMazeScraper.Core.Entities;
using TvMazeScraper.Infrastructure.Data;

namespace TvMazeScraper.Test
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using var dbContext = new TvMazeScraperContext(serviceProvider.GetRequiredService<DbContextOptions<TvMazeScraperContext>>());
            if (dbContext.Shows.Any())
            {
                return;
            }

            PopulateTestData(dbContext);
        }
        public static void PopulateTestData(TvMazeScraperContext dbContext)
        {
            var shows = new List<Show>
            {
                new Show{ Id =1, Name = "Test show 1"},
                new Show{ Id =2, Name = "Test show 2"}
            };

            var persons = new List<Person>
            {
                new Person{ Id = 1,Name = "Test person 1", Birthday = new DateTime(1950,1,1)},
                new Person{ Id = 2,Name = "Test person 2", Birthday = new DateTime(1950,1,1)}
            };

            var cast = new List<ShowPerson>
            {
                new ShowPerson{ PersonId = 1, ShowId = 1},
                new ShowPerson{ PersonId = 2, ShowId = 2}
            };

            dbContext.Shows.AddRange(shows);
            dbContext.Persons.AddRange(persons);
            dbContext.ShowPerson.AddRange(cast);

            dbContext.SaveChanges();
        }
    }
}
