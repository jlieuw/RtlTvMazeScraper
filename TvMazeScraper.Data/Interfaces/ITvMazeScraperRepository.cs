using System;
using System.Collections.Generic;
using System.Text;

namespace TvMazeScraper.Data.Repositories
{
    public interface ITvMazeScraperRepository
    {
        void AddPersons(IEnumerable<Person> Persons);
        IEnumerable<int> GetPersonIds();
        IEnumerable<ShowPerson> GetShowPersons();
        void AddShowPersons(IEnumerable<ShowPerson> showPerson);
        void AddShows(IEnumerable<Show> Shows);
        IEnumerable<int> GetShowIds();
        IEnumerable<Show> GetShows(int page, int size);
        Show GetShow(int id);
        bool Save();
    }
}
