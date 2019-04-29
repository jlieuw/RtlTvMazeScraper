using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TvMazeScraper.Data.Repositories
{
    public interface IShowRepository
    {
        void AddShows(IEnumerable<Show> Shows);
        Task<List<int>> GetShowIdsAsync();
        IEnumerable<Show> GetShows(int page, int size);
        IEnumerable<ShowModel> GetShowModels(int page, int size);
        ShowModel GetShowModel(int id);
        bool Save();
    }
}
