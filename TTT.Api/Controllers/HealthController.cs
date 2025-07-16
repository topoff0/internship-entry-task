using Microsoft.AspNetCore.Mvc;

namespace TTT.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HealthController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new { status = "healthy" });
        }
    }
}