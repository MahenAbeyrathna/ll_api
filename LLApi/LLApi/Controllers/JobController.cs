using Microsoft.AspNetCore.Mvc;

namespace LLApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class JobController : ControllerBase
    {
        private readonly ILogger<JobController> _logger;

        public JobController(ILogger<JobController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult GetJobById([FromQuery] Guid jobId)
        {
            return Ok();
        }

        [HttpGet("job-list")]
        public IActionResult GetJobList([FromQuery] Guid organizationId)
        {
            return Ok();
        }
    }
}
