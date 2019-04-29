using System.Collections.Generic;

namespace TvMazeScraper.Data
{
    public class ShowModel
    {
        public int Id { get; set; }
        public IEnumerable<Person> Cast { get; set; }
    }
}
