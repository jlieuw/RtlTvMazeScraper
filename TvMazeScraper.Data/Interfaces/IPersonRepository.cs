using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TvMazeScraper.Data.Repositories
{
    public interface IPersonRepository
    {
        void AddPersons(IEnumerable<Person> Persons);
        Task<List<int>> GetPersonIdsAsync();
        bool Save();
    }
}
