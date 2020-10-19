using System.Collections.Generic;
using System.Threading.Tasks;
using TvMazeScraper.Web.ApiModels;

namespace TvMazeScraper.Web.Interfaces
{
    public interface IShowService
    {
        Task<List<ShowDTO>> GetShowDTOsAsync(int page = 0, int pageSize = 250);
        Task<ShowDTO> GetShowDTOAsync(int id);
    }
}
