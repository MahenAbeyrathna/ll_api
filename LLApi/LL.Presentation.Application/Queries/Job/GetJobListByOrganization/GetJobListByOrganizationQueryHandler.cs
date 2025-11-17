using LL.Application.Common.Interfaces.Services.DataAPIService;
using LL.Application.Common.Interfaces.Services.DBService.Jobs.JobById;
using LL.Application.Common.Interfaces.Services.DBService.Jobs.JobsByOrganizationId;
using LL.Application.Common.Interfaces.Services.DBService.Jobs.SaveJob;
using LL.Application.Queries.Job.GetJobById;
using LL.Infrastructure.Integrations.DataAPI;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LL.Application.Queries.Job.GetJobListByOrganization;

public sealed class GetJobListByOrganizationQueryHandler : IRequestHandler<GetJobListByOrganizationQuery, List<GetJobListResult>>
{
    private IDbJobByIdService _jobByIdService;
    private IDataService _dataService;
    private DataAPIConfig _config;
    private IDbSaveJobAsync _dbSaveJob;
    private IDbJobsByOrganizationIdService _jobsByOrganizationIdService;
    private ILogger<GetJobListByOrganizationQueryHandler> _logger;

    public GetJobListByOrganizationQueryHandler(IDbJobByIdService jobByIdService, IDataService dataService, ILogger<GetJobListByOrganizationQueryHandler> logger,
                                   DataAPIConfig config, IDbSaveJobAsync dbSaveJob, IDbJobsByOrganizationIdService jobsByOrganizationIdService)
    {
        _jobByIdService = jobByIdService;
        _dataService = dataService;
        _config = config;
        _dbSaveJob = dbSaveJob;
        _jobsByOrganizationIdService = jobsByOrganizationIdService;
        _logger = logger;
    }
    public async Task<List<GetJobListResult>> Handle(GetJobListByOrganizationQuery request, CancellationToken cancellationToken)
    {
        try
        {
            List<GetJobListResult> jobData = new List<GetJobListResult>();
            _logger.LogInformation($"Get jobs by organization : {request.OrganizationId}");
            var jobList = await _jobsByOrganizationIdService.GetOrganizationJobsAsync(request.OrganizationId);

            if (jobList.Count == 0)
            {
                _logger.LogInformation($"Get jobs from api, organization : {request.OrganizationId}");
                var jobs = await _dataService.GetJobListAsync(_config.DataAPIKey, request.OrganizationId);


                _logger.LogInformation($"Job count : {jobs.Count}");
                //save all jobs to database 
                foreach (var job in jobs)
                {
                    var existingJob = await _jobByIdService.GetJobByExternalIdAsync(job.ExternalId);
                    if (existingJob == null)
                    {
                        await _dbSaveJob.SaveJobAsync(job, request.OrganizationId);
                    }
                }
                _logger.LogInformation($"Jobs saved to database");
                var newJobList = await _jobsByOrganizationIdService.GetOrganizationJobsAsync(request.OrganizationId);
                jobData= GetJobList(newJobList);
            }
            jobData = GetJobList(jobList);
            _logger.LogInformation($"Job list processing completed");
            return jobData;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error occured {ex}");
            throw;
        }
        
    }

    private List<GetJobListResult> GetJobList(List<Common.Interfaces.Services.DBService.Jobs.JobsByOrganizationId.Job> jobs)
    {
        List<GetJobListResult> jobListResults = new List<GetJobListResult>();
        foreach (var job in jobs)
        {
            GetJobListResult result = new GetJobListResult()
            {
                JobId = job.JobId,
                Title = job.Title,
                Description = job.Description,
                IsDeleted = job.IsDeleted,
                CreatedDateTime = job.CreatedDateTime
            };
            jobListResults.Add(result);
        }
        return jobListResults;
    }
}
