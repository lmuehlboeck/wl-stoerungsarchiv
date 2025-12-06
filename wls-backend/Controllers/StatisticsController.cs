using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using wls_backend.Models.DTOs;
using wls_backend.Services;

namespace wls_backend.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class StatisticsController : ControllerBase
    {
        private readonly StatisticsService _statisticsService;
        public StatisticsController(StatisticsService statisticsService)
        {
            _statisticsService = statisticsService;
        }
        
        [HttpGet]
        public async Task<ActionResult<StatisticsResponse>> GetStatistics([FromQuery] StatisticsRequest request)
        {
            try
            {
                return Ok(await _statisticsService.GetStatistics(request));
            }
            catch (ArgumentException exc)
            {
                return BadRequest(exc.Message);
            }
        }
    }
}
