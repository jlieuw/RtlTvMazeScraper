using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TvMazeScraper.Infrastructure.Data;
using TvMazeScraper.Web.ApiModels;
using TvMazeScraper.Web.Interfaces;

namespace TvMazeScraper.Web.Services
{
    public class ShowService : IShowService
    {
        private readonly TvMazeScraperContext _context;
        public ShowService(TvMazeScraperContext context)
        {
            _context = context;
        }
        public Task<ShowDTO> GetShowDTOAsync(int id)
        {
            return _context.Shows
                .Select(show => new ShowDTO
                {
                    Id = show.Id,
                    Name = show.Name,
                    Cast = show.Cast.Select(castItem => new PersonDTO
                    {
                        Id = castItem.Person.Id,
                        Name = castItem.Person.Name,
                        Birthday = castItem.Person.Birthday
                    }).OrderByDescending(persondDto => persondDto.Birthday)
                })
                .FirstOrDefaultAsync(show => show.Id == id);
        }

        public Task<List<ShowDTO>> GetShowDTOsAsync(int page = 0, int pageSize = 250)
        {
            return (_context.Shows
                .Skip(page * pageSize)
                .Take(pageSize)
                .Select(show => new ShowDTO
                {
                    Id = show.Id,
                    Name = show.Name,
                    Cast = show.Cast.Select(castItem => new PersonDTO
                    {
                        Id = castItem.Person.Id,
                        Name = castItem.Person.Name,
                        Birthday = castItem.Person.Birthday
                    }).OrderByDescending(persondDto => persondDto.Birthday)
                })
                .ToListAsync());
        }
    }
}
