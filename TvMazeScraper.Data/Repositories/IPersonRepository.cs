using System;
using System.Collections.Generic;
using System.Text;

namespace TvMazeScraper.Data.Repositories
{
    public interface IPersonRepository
    {
        void AddPersons(IEnumerable<Person> Persons);
        IEnumerable<int> GetPersonIds();
        bool Save();
    }
}
