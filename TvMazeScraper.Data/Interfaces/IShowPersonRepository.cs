using System.Collections.Generic;

namespace TvMazeScraper.Data.Repositories
{
    public interface IShowPersonRepository
    {
        IEnumerable<ShowPerson> GetShowPersons();
        void AddShowPersons(IEnumerable<ShowPerson> showPerson);
        bool Save();
    }
}
