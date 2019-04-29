using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace TvMazeScraper.Data
{
    public class Show
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<ShowPerson> Cast { get; set; }
    }
}
