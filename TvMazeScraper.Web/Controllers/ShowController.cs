using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TvMazeScraper.Web.Interfaces;

namespace TvMazeScraper.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShowController : ControllerBase
    {
        private readonly IShowService _service;

        public ShowController(IShowService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Get(int? page = 0, int? pageSize = 250)
        {
            var pageNumber = (page ?? 0);
            var pagesize = (pageSize ?? 250);
            var items = (await _service.GetShowDTOsAsync(pageNumber, pagesize));
            return Ok(items);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var show = await _service.GetShowDTOAsync(id);
            if (show == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(show);
            }
        }
    }
}