using System.Net.Http;
using System.Threading.Tasks;

namespace TvMazeScraper.Core.Interfaces
{
    public interface ITvMazeHttpClient
    {
        Task<HttpResponseMessage> GetShows(int page);
        Task<HttpResponseMessage> GetCast(int showId);
    }
}
