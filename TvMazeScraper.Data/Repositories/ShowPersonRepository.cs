using System.Collections.Generic;
using System.Linq;

namespace TvMazeScraper.Data.Repositories
{
    public class ShowPersonRepository : IShowPersonRepository
    {
        private TvMazeScraperContext context;
        public ShowPersonRepository(TvMazeScraperContext _context)
        {
            context = _context;
        }

        public void AddShowPersons(IEnumerable<ShowPerson> showPerson)
        {
            context.ShowPerson.AddRange(showPerson);
        }

        public IEnumerable<ShowPerson> GetShowPersons()
        {
            return context.ShowPerson.ToList();
        }

        public bool Save()
        {
            return (context.SaveChanges() >= 0);
        }
    }
}
