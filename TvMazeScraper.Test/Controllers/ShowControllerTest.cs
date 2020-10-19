using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using TvMazeScraper.Web;
using TvMazeScraper.Web.ApiModels;
using Xunit;

namespace TvMazeScraper.Test.Controllers
{

    public class ShowControllerTest : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;

        public ShowControllerTest(CustomWebApplicationFactory<Startup> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetShows()
        {
            var response = await _client.GetAsync("/api/show");
            response.EnsureSuccessStatusCode();
            var stringResponse = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<IEnumerable<ShowDTO>>(stringResponse).ToList();

            Assert.Equal(2, result.Count());
            Assert.Contains(result, i => i.Id == 1);
            Assert.Contains(result, i => i.Name == "Test show 2");
            Assert.Contains(result, i => i.Cast.Single().Name == "Test person 2");
        }

        [Fact]
        public async Task GetShowById()
        {
            var response = await _client.GetAsync("/api/show/1");

            response.EnsureSuccessStatusCode();
            var stringResponse = await response.Content.ReadAsStringAsync();
            var show = JsonConvert.DeserializeObject<ShowDTO>(stringResponse);

            Assert.Equal(1, show.Id);
            Assert.Equal("Test show 1", show.Name);
            Assert.Equal("Test person 1", show.Cast.Single().Name);
            Assert.Equal(new DateTime(1950, 1, 1), show.Cast.Single().Birthday);
        }

        [Fact]
        public async Task GetShowByIdNotFound()
        {
            var response = await _client.GetAsync("/api/show/3");

            Assert.True(response.StatusCode == System.Net.HttpStatusCode.NotFound);
        }
    }
}
