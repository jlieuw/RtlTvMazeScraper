using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        public async Task<List<int>> GetPersonIdsAsync() => await _context.Persons.Select(p => p.Id).ToListAsync();

        public bool Save()
        {
            return (_context.SaveChanges() >= 0);
        }
    }
}
