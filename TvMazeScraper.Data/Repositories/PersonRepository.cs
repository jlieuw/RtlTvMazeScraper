using System;
using System.Collections.Generic;
using System.Linq;

namespace TvMazeScraper.Data.Repositories
{
    public class PersonRepository : IPersonRepository
    {
        private TvMazeScraperContext context;
        public PersonRepository(TvMazeScraperContext _context)
        {
            context = _context;
        }

        public void AddPersons(IEnumerable<Person> Persons)
        {
            context.Persons.AddRange(Persons);
        }

        public IEnumerable<int> GetPersonIds() => context.Persons.Select(p => p.Id).ToList();

        public bool Save()
        {
            return (context.SaveChanges() >= 0);
        }
    }
}
