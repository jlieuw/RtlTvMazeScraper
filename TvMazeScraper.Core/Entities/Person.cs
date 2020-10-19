using System;
using System.Collections.Generic;

namespace TvMazeScraper.Core.Entities
{
    public class Person : BaseEntity
    {
        public string Name { get; set; }
        public DateTime? Birthday { get; set; }
        public IEnumerable<ShowPerson> ShowPersons { get; set; }
    }
}
