using System.Collections.Generic;
using System.Linq;

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

        public IEnumerable<ShowPerson> GetShowPersons()
        {
            return _context.ShowPerson.ToList();
        }

        public bool Save()
        {
            return (_context.SaveChanges() >= 0);
        }
    }
}
