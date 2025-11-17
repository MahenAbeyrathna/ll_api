using LL.Application.Queries.Job.GetJobById;
using LL.Application.Queries.Job.GetJobListByOrganization;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LL.Presentation.API.Controllers;

[ApiController]
[Route("api/job")]
public class JobController : ControllerBase
{
    private readonly ILogger<JobController> _logger;
    private readonly IMediator _mediator;
    public JobController(ILogger<JobController> logger, IMediator mediator)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger)); 
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    [HttpGet("verify")]
    public IActionResult Get() => Ok($"Job API is running");


    [HttpGet("job-by-id")]
    public async Task<IActionResult> GetJobById([FromQuery] GetJobByIdQuery request)
    {
        _logger.LogInformation($"JobId: {request.JobId},API called at {DateTime.UtcNow}");
        var result = await _mediator.Send(request);
        return Ok(result);
    }

    [HttpGet("job-list-by-organization-id")]
    public async Task<IActionResult> GetJobListByOrganizationId([FromQuery] GetJobListByOrganizationQuery request)
    {
        _logger.LogInformation($"OrganizationId: {request.OrganizationId},API called at {DateTime.UtcNow}");
        var result = await _mediator.Send(request);
        return Ok(result);
    }
}
