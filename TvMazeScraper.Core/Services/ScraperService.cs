using Microsoft.Extensions.Logging;
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
        private readonly IShowRepository _showRepository;
        private readonly IPersonRepository _personRepository;
        private readonly IShowPersonRepository _showPersonRepository;
        private readonly ILogger _logger;
        private readonly IHttpClientFactory _clientFactory;

        public ScraperService(IShowRepository showRepository, 
            IPersonRepository personRepository, 
            IShowPersonRepository showPersonRepository,
            ILogger<ScraperService> logger,
            IHttpClientFactory httpClientFactory)
        {
            _showRepository = showRepository;
            _personRepository = personRepository;
            _showPersonRepository = showPersonRepository;
            _logger = logger;
            _clientFactory = httpClientFactory;
        }

        public async Task StartScrapingAsync()
        {
            using (var client = _clientFactory.CreateClient())
            {
                int page = 0;
                client.BaseAddress = new Uri(baseUrl);
                while (true)
                {
                    try
                    {

                        // TODO: Refactor code
                        var response = await client.GetAsync(string.Format(showUrl, page));
                        response.EnsureSuccessStatusCode();

                        var stringResult = await response.Content.ReadAsStringAsync();
                        var existingShowIds = await _showRepository.GetShowIdsAsync();
                        var shows = JsonConvert.DeserializeObject<IEnumerable<Show>>(stringResult).Where(s => !existingShowIds.Contains(s.Id));
                        _showRepository.AddShows(shows);
                        foreach (Show show in shows)
                        {
                            var castresponse = await client.GetAsync(string.Format(personUrl, show.Id));
                            var stringCastResult = await castresponse.Content.ReadAsStringAsync();
                            var existingPersonsIds = _personRepository.GetPersonIds();
                            var castItems = JsonConvert.DeserializeObject<IEnumerable<CastItem>>(stringCastResult);
                            var persons = castItems.Select(ci => ci.Person).ToList();
                            var personsToAdd = persons.Where(s => !existingPersonsIds.Contains(s.Id)).GroupBy(p => p.Id).Select(x => x.First());

                            var existingShowsPersons = _showPersonRepository.GetShowPersons();
                            var showPersons = persons.Select(p => new ShowPerson() { PersonId = p.Id, ShowId = show.Id })
                                .Where(sp => !existingShowsPersons.Any(esp => esp.PersonId == sp.PersonId && esp.ShowId == sp.ShowId))
                                .GroupBy(p => new { p.PersonId, p.ShowId }).Select(x => x.First());

                            _personRepository.AddPersons(personsToAdd);
                            _showPersonRepository.AddShowPersons(showPersons);
                            _showRepository.Save();
                        }

                    }
                    catch (HttpRequestException httpRequestException)
                    {
                        break;
                    }catch(Exception otherEx)
                    {
                        var x = otherEx.GetType();
                    }
                    page++;
                }
            }
        }

        public void ScrapeShows()
        {

        }

        public void ScrapePersons()
        {

        }
    }
}
