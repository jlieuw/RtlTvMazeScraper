using System;
using System.Collections.Generic;
using System.Text;

namespace TvMazeScraper.Data.Repositories
{
    public interface IShowRepository
    {
        void AddShows(IEnumerable<Show> Shows);
        IEnumerable<int> GetShowIds();
        IEnumerable<Show> GetShows(int page, int size);
        IEnumerable<ShowModel> GetShowModels(int page, int size);
        ShowModel GetShowModel(int id);
        bool Save();
    }
}
