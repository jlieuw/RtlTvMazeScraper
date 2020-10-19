using System.Net.Http;
using System.Threading.Tasks;
using TvMazeScraper.Core;
using TvMazeScraper.Core.Interfaces;

namespace TvMazeScraper.Infrastructure
{
    public class TvMazeHttpClient : ITvMazeHttpClient
    {
        private readonly HttpClient _httpClient;
        private readonly MazeApiSettings _mazeApiSettings;
        public TvMazeHttpClient(HttpClient httpClient, MazeApiSettings mazeapisettings)
        {
            _httpClient = httpClient;
            _mazeApiSettings = mazeapisettings;
        }

        public async Task<HttpResponseMessage> GetShows(int page)
        {
            return await _httpClient.GetAsync(string.Format(_mazeApiSettings.ShowUrl, page));
        }
        public async Task<HttpResponseMessage> GetCast(int showId)
        {
            return await _httpClient.GetAsync(string.Format(_mazeApiSettings.PersonUrl, showId));
        }
    }
}
