using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using wls_backend.Models.DTOs;
using wls_backend.Services;

namespace wls_backend.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class DisturbancesController : ControllerBase
    {
        private readonly DisturbanceService _disturbanceService;
        public DisturbancesController(DisturbanceService disturbanceService)
        {
            _disturbanceService = disturbanceService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DisturbanceResponse>> GetDisturbance(string id)
        {
            var disturbance = await _disturbanceService.GetDisturbance(id);
            if (disturbance == null)
            {
                return NotFound();
            }
            return disturbance;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DisturbanceResponse>>> GetDisturbances([FromQuery] DisturbanceFilterRequest filter)
        {
            try
            {
                var disturbances = await _disturbanceService.GetDisturbances(filter);
                return Ok(disturbances);
            }
            catch (ArgumentException exc)
            {
                return BadRequest(exc.Message);
            }
        }
    }
}
