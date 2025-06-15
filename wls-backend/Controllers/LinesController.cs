using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using wls_backend.Models.DTOs;
using wls_backend.Services;

namespace wls_backend.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class LinesController : ControllerBase
    {
        private readonly LineService _lineService;
        public LinesController(LineService lineService)
        {
            _lineService = lineService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LineResponse>>> GetLines()
        {
            var lines = await _lineService.GetLines();
            return Ok(lines);
        }
    }
}
