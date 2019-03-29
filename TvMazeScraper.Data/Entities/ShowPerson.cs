using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TvMazeScraper.Data
{
    public class ShowPerson
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ShowId { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int PersonId { get; set; }
        public Show Show { get; set; }
        public Person Person { get; set; }
    }
}
