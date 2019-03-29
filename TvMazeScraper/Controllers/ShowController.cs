using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TvMazeScraper.Data.Repositories;

namespace TvMazeScraper.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShowController : ControllerBase
    {
        private readonly IShowRepository _showRepository;

        public ShowController(IShowRepository showRepository)
        {
            _showRepository = showRepository;
        }

        [HttpGet]
        public IActionResult Get(int? page = 0, int? pageSize = 250)
        {
            return Ok(_showRepository.GetShowModels((page ?? 0), (pageSize ?? 250)));
        }


        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return Ok(_showRepository.GetShowModel(id));
        }
    }
}