using System.Collections.Generic;
using System.Threading.Tasks;

namespace TvMazeScraper.Data.Repositories
{
    public interface IShowPersonRepository
    {
        Task<List<ShowPerson>> GetShowPersonsAsync();
        void AddShowPersons(IEnumerable<ShowPerson> showPerson);
        bool Save();
    }
}
