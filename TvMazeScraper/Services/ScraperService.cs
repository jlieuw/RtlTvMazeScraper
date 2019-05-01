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
        private readonly HttpClient _httpClient;

        public ScraperService(IShowRepository showRepository,
            IPersonRepository personRepository,
            IShowPersonRepository showPersonRepository,
            ILogger<ScraperService> logger,
            HttpClient httpClient)
        {
            _showRepository = showRepository;
            _personRepository = personRepository;
            _showPersonRepository = showPersonRepository;
            _logger = logger;
            _httpClient = httpClient;
        }

        public async Task StartScrapingAsync()
        {
            int page = 0;
            while (true)
            {
                try
                {
                    // TODO: Refactor code

                    var shows = await ScrapeShowsAsync(page);
                    _showRepository.AddShows(shows);
                    foreach (Show show in shows)
                    {
                        var persons = await ScrapCastAsync(show.Id);
                        var existingPersonsIds = await _personRepository.GetPersonIdsAsync();
                        var personsToAdd = persons.Where(s => !existingPersonsIds.Contains(s.Id)).GroupBy(p => p.Id).Select(x => x.First());

                        var existingShowsPersons = await _showPersonRepository.GetShowPersonsAsync();
                        var showPersons = persons.Select(p => new ShowPerson() { PersonId = p.Id, ShowId = show.Id })
                            .Where(sp => !existingShowsPersons.Any(esp => esp.PersonId == sp.PersonId && esp.ShowId == sp.ShowId))
                            .GroupBy(p => new { p.PersonId, p.ShowId }).Select(x => x.First());

                        _personRepository.AddPersons(personsToAdd);
                        _showPersonRepository.AddShowPersons(showPersons);
                        _showRepository.Save();
                    }

                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    break;
                }
                page++;
            }

        }

        public async Task<IEnumerable<Show>> ScrapeShowsAsync(int page)
        {
            var response = await _httpClient.GetAsync(string.Format(showUrl, page));
            response.EnsureSuccessStatusCode();

            var stringResult = await response.Content.ReadAsStringAsync();
            var existingShowIds = await _showRepository.GetShowIdsAsync();
            return JsonConvert.DeserializeObject<IEnumerable<Show>>(stringResult).Where(s => !existingShowIds.Contains(s.Id));
        }

        public async Task<IEnumerable<Person>> ScrapCastAsync(int showid)
        {
            var castresponse = await _httpClient.GetAsync(string.Format(personUrl, showid));
            var stringCastResult = await castresponse.Content.ReadAsStringAsync();
            var castItems = JsonConvert.DeserializeObject<IEnumerable<CastItem>>(stringCastResult);
            return castItems.Select(ci => ci.Person).ToList();
        }
    }
}
