namespace TvMazeScraper.Core.Entities
{
    public class ShowPerson 
    {
        public int ShowId { get; set; }
        public int PersonId { get; set; }
        public Show Show { get; set; }
        public Person Person { get; set; }
    }
}
