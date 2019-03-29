using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace TvMazeScraper.Data.Repositories
{
    public class ShowRepository : IShowRepository
    {
        private TvMazeScraperContext context;
        public ShowRepository(TvMazeScraperContext _context)
        {
            context = _context;
        }

        public void AddShows(IEnumerable<Show> Shows)
        {
            context.Shows.AddRange(Shows);
        }

        public ShowModel GetShowModel(int id)
        {
            return context.Shows
                .Select(s =>
               new ShowModel()
               {
                   Id = s.Id,
                   Cast = s.Cast.Select(c => c.Person).OrderByDescending(p => p.Birthday)
               })
                .FirstOrDefault(s => s.Id == id);
        }

        public IEnumerable<int> GetShowIds() => context.Shows.Select(s => s.Id).ToList();


        public IEnumerable<Show> GetShows(int page, int size)
        {
            return context.Shows.Include(s => s.Cast)
                .ThenInclude(sp => sp.Person)
                .Skip(page * size)
                .Take(size)
                .ToList();
        }

        public bool Save()
        {
            return (context.SaveChanges() >= 0);
        }

        public IEnumerable<ShowModel> GetShowModels(int page, int size)
        {
            return context.Shows
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
