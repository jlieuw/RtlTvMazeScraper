using System.Collections.Generic;

namespace TvMazeScraper.Core.Entities
{
    public class Show : BaseEntity
    {
        public string Name { get; set; }
        public IEnumerable<ShowPerson> Cast { get; set; }
    }
}
