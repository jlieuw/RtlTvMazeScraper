using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using TvMazeScraper.Data;
using TvMazeScraper.Data.Repositories;

namespace TvMazeScraper.Services
{
    public class ScraperService : IScraperService
    {
        const string baseUrl = "http://api.tvmaze.com";
        const string showUrl = "/shows?page={0}";
        const string personUrl = "/shows/{0}/cast";
        private IShowRepository showRepository;
        private IPersonRepository personRepository;
        private IShowPersonRepository showPersonRepository;

        public ScraperService(IShowRepository showRepository, IPersonRepository personRepository, IShowPersonRepository showPersonRepository)
        {
            this.showRepository = showRepository;
            this.personRepository = personRepository;
            this.showPersonRepository = showPersonRepository;
        }

        public void StartScraping()
        {
            using (var client = new HttpClient())
            {
                int page = 0;
                client.BaseAddress = new Uri(baseUrl);
                while (true)
                {
                    try
                    {

                        // TODO: Refactor code
                        // TODO: Auto-retry mechanism
                        var response = client.GetAsync(string.Format(showUrl, page)).Result;
                        response.EnsureSuccessStatusCode();

                        var stringResult = response.Content.ReadAsStringAsync().Result;
                        var existingShowIds = showRepository.GetShowIds();
                        var shows = JsonConvert.DeserializeObject<IEnumerable<Show>>(stringResult).Where(s => !existingShowIds.Contains(s.Id));
                        showRepository.AddShows(shows);
                        foreach (Show show in shows)
                        {
                            var castresponse = client.GetAsync(string.Format(personUrl, show.Id)).Result;
                            var stringCastResult = castresponse.Content.ReadAsStringAsync().Result;
                            var existingPersonsIds = personRepository.GetPersonIds();
                            var castItems = JsonConvert.DeserializeObject<IEnumerable<CastItem>>(stringCastResult);
                            var persons = castItems.Select(ci => ci.Person);
                            var personsToAdd = persons.Where(s => !existingPersonsIds.Contains(s.Id)).GroupBy(p => p.Id).Select(x => x.First());

                            var existingShowsPersons = showPersonRepository.GetShowPersons();
                            var showPersons = persons.Select(p => new ShowPerson() { PersonId = p.Id, ShowId = show.Id })
                                .Where(sp => !existingShowsPersons.Any(esp => esp.PersonId == sp.PersonId && esp.ShowId == sp.ShowId))
                                .GroupBy(p => new { p.PersonId, p.ShowId}).Select(x => x.First());

                            personRepository.AddPersons(personsToAdd);
                            showPersonRepository.AddShowPersons(showPersons);
                            showRepository.Save();
                        }

                    }
                    catch (HttpRequestException httpRequestException)
                    {
                        break;
                    }
                    page++;
                }
            }
        }
    }
}
