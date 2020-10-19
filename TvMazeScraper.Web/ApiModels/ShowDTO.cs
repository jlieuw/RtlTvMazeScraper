using System.Collections.Generic;

namespace TvMazeScraper.Web.ApiModels
{
    public class ShowDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<PersonDTO> Cast { get; set; }
    }
}
