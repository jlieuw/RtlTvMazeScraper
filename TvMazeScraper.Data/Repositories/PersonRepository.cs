using System;
using System.Collections.Generic;
using System.Linq;

namespace TvMazeScraper.Data.Repositories
{
    public class PersonRepository : IPersonRepository
    {
        private readonly TvMazeScraperContext _context;
        public PersonRepository(TvMazeScraperContext context)
        {
            _context = context;
        }

        public void AddPersons(IEnumerable<Person> Persons)
        {
            _context.Persons.AddRange(Persons);
        }

        public IEnumerable<int> GetPersonIds() => _context.Persons.Select(p => p.Id).ToList();

        public bool Save()
        {
            return (_context.SaveChanges() >= 0);
        }
    }
}
