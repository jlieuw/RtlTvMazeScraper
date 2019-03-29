using System;
using System.Collections.Generic;
using System.Text;

namespace TvMazeScraper.Data
{
    public class ShowModel
    {
        public int Id { get; set; }
        public IEnumerable<Person> Cast { get; set; }
    }
}
