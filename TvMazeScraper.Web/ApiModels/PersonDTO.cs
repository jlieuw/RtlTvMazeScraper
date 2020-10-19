using System;

namespace TvMazeScraper.Web.ApiModels
{
    public class PersonDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime? Birthday { get; set; }
    }
}
