using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TvMazeScraper.Data.Repositories
{
    public class ShowPersonRepository : IShowPersonRepository
    {
        private readonly TvMazeScraperContext _context;
        public ShowPersonRepository(TvMazeScraperContext context)
        {
            _context = context;
        }

        public void AddShowPersons(IEnumerable<ShowPerson> showPerson)
        {
            _context.ShowPerson.AddRange(showPerson);
        }

        public async Task<List<ShowPerson>> GetShowPersonsAsync() => await _context.ShowPerson.ToListAsync();

        public bool Save()
        {
            return (_context.SaveChanges() >= 0);
        }
    }
}
