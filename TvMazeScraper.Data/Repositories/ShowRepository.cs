using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;

namespace TvMazeScraper.Data.Repositories
{
    public class ShowRepository : IShowRepository
    {
        private TvMazeScraperContext _context;
        public ShowRepository(TvMazeScraperContext context)
        {
            _context = context;
        }

        public void AddShows(IEnumerable<Show> Shows)
        {
            _context.Shows.AddRange(Shows);
        }

        public ShowModel GetShowModel(int id)
        {
            return _context.Shows
                .Select(s =>
               new ShowModel()
               {
                   Id = s.Id,
                   Cast = s.Cast.Select(c => c.Person).OrderByDescending(p => p.Birthday)
               })
                .FirstOrDefault(s => s.Id == id);
        }

        public Task<List<int>> GetShowIdsAsync()
        {
            return _context.Shows.Select(s => s.Id).ToListAsync();
        }

        public IEnumerable<Show> GetShows(int page, int size)
        {
            return _context.Shows.Include(s => s.Cast)
                .ThenInclude(sp => sp.Person)
                .Skip(page * size)
                .Take(size)
                .ToList();
        }

        public bool Save()
        {
            return (_context.SaveChanges() >= 0);
        }

        public IEnumerable<ShowModel> GetShowModels(int page, int size)
        {
            return _context.Shows
                .Skip(page * size)
                .Take(size)
                .Select(s =>
               new ShowModel()
               {
                   Id = s.Id,
                   Cast = s.Cast.Select(c => c.Person).OrderByDescending(p => p.Birthday)
               });
        }
    }
}
