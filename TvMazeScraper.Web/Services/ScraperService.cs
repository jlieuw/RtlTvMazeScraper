using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TvMazeScraper.Core;
using TvMazeScraper.Core.Entities;
using TvMazeScraper.Core.Interfaces;
using TvMazeScraper.Infrastructure.Data;
using TvMazeScraper.Web.ApiModels;

namespace TvMazeScraper.Web.Services
{
    public class ScraperService : IScraperService
    {
        private readonly ILogger _logger;
        private readonly ITvMazeHttpClient _httpClient;
        private readonly MazeApiSettings _mazeApiSettings;
        private readonly TvMazeScraperContext _context;

        public ScraperService(
            ILogger<ScraperService> logger,
            ITvMazeHttpClient httpClient,
            MazeApiSettings mazeApiSettings,
            TvMazeScraperContext context)
        {
            _logger = logger;
            _httpClient = httpClient;
            _mazeApiSettings = mazeApiSettings;
            _context = context;
        }

        public async Task StartScrapingAsync()
        {
            while (true)
            {
                try
                {
                    var shows = await ScrapeShowsAsync(_mazeApiSettings.PageNumber).ConfigureAwait(false);

                    var existingShowIds = _context.Shows.Select(show => show.Id);

                    var showsToImport = shows.Where(show => !existingShowIds.Contains(show.Id));

                    await _context.Shows.AddRangeAsync(showsToImport).ConfigureAwait(false);

                    foreach (var show in showsToImport)
                    {
                        var cast = await ScrapeCastAsync(show.Id).ConfigureAwait(false);
                        var existingPersonsIds = _context.Persons.Select(person => person.Id);
                        var personsToAdd = cast.Where(s => !existingPersonsIds.Contains(s.Id)).GroupBy(p => p.Id).Select(x => x.First());

                        var existingShowsPersons = _context.ShowPerson;
                        var showPersons = personsToAdd.Select(p => new ShowPerson() { Person = p, PersonId = p.Id, ShowId = show.Id })
                            .Where(sp => !existingShowsPersons.Any(esp => esp.PersonId == sp.PersonId && esp.ShowId == sp.ShowId))
                            .GroupBy(p => new { p.PersonId, p.ShowId }).Select(x => x.First());

                        await _context.ShowPerson.AddRangeAsync(showPersons).ConfigureAwait(false);
                        await _context.SaveChangesAsync().ConfigureAwait(false);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    break;
                }
                _mazeApiSettings.PageNumber++;
            }
        }

        private async Task<IEnumerable<Show>> ScrapeShowsAsync(int page)
        {
            var response = await _httpClient.GetShows(page).ConfigureAwait(false);
            var stringResult = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            System.Diagnostics.Debug.WriteLine(stringResult);
            return JsonConvert.DeserializeObject<IEnumerable<Show>>(stringResult);
        }

        private async Task<IEnumerable<Person>> ScrapeCastAsync(int showid)
        {
            var castresponse = await _httpClient.GetCast(showid).ConfigureAwait(false);
            var stringCastResult = await castresponse.Content.ReadAsStringAsync().ConfigureAwait(false);
            var castItems = JsonConvert.DeserializeObject<IEnumerable<CastDTO>>(stringCastResult);
            return castItems.Select(ci => ci.Person).ToList();
        }
    }
}
