using LL.Application.Common.Interfaces.Services.DataAPIService;
using LL.Application.Common.Interfaces.Services.DBService.Jobs.JobById;
using LL.Application.Common.Interfaces.Services.DBService.Jobs.SaveJob;
using LL.Infrastructure.Integrations.DataAPI;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LL.Application.Queries.Job.GetJobById;

public sealed class GetJobByIdQueryHandler : IRequestHandler<GetJobByIdQuery, GetJobByIdResponse>
{
    private IDbJobByIdService _jobByIdService;
    private IDataService _dataService;
    private DataAPIConfig _config;
    private IDbSaveJobAsync _dbSaveJob;
    private ILogger<GetJobByIdQueryHandler> _logger;

    public GetJobByIdQueryHandler(IDbJobByIdService jobByIdService, IDataService dataService,
                                   DataAPIConfig config, IDbSaveJobAsync dbSaveJob, ILogger<GetJobByIdQueryHandler> logger)
    {
        _jobByIdService = jobByIdService;
        _dataService = dataService;
        _config = config;
        _dbSaveJob = dbSaveJob;
        _logger = logger;

    }
    public async Task<GetJobByIdResponse> Handle(GetJobByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            GetJobByIdResponse response = new GetJobByIdResponse();
            _logger.LogInformation($"Get job by externalid : {request.JobId}");
            var jobDetails = await _jobByIdService.GetJobByExternalIdAsync(request.JobId);

            if (jobDetails == null)
            {
                _logger.LogInformation($"Sync jobs from api");
                var jobs = await _dataService.GetJobListAsync(_config.DataAPIKey, request.OrganizationId);

                _logger.LogInformation($"Retrived job count : {jobs.Count} ");
                foreach (var job in jobs)
                {
                    var existingJob = await _jobByIdService.GetJobByExternalIdAsync(job.ExternalId);
                    if (existingJob == null)// todo validate
                    {
                        _logger.LogInformation($"Saving job externalid :  {job.ExternalId} ");
                        await _dbSaveJob.SaveJobAsync(job, request.OrganizationId);
                    }
                }
                _logger.LogInformation($"Jobs saved to database");
                if (jobs.Any(j => j.ExternalId == request.JobId))
                {
                    var newJob = await _jobByIdService.GetJobByExternalIdAsync(request.JobId);
                    response= new GetJobByIdResponse()
                    {
                        JobId = newJob.JobId,
                        Description = newJob.Description,
                        Title = newJob.Title,
                        IsDeleted = newJob.IsDeleted,
                        CreatedDateTime = newJob.CreatedDateTime
                    };
                }
                else
                {
                    _logger.LogInformation($"Job is not available in api");
                    return null;
                }
            }
            else
            {
                response = new GetJobByIdResponse()
                {
                    JobId = jobDetails.JobId,
                    Description = jobDetails.Description,
                    Title = jobDetails.Title,
                    IsDeleted = jobDetails.IsDeleted,
                    CreatedDateTime = jobDetails.CreatedDateTime
                };
            }
            _logger.LogInformation($"Job processing completed");
            return response;


        }
        catch (Exception ex)
        {
            _logger.LogError($"Error occured : {ex}");
            throw;
        }
       
    }
}
