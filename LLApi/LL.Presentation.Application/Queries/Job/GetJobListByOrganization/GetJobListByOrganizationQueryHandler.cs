using LL.Application.Common.Interfaces.Services.DataAPIService;
using LL.Application.Common.Interfaces.Services.DBService.Jobs.JobById;
using LL.Application.Common.Interfaces.Services.DBService.Jobs.JobsByOrganizationId;
using LL.Application.Common.Interfaces.Services.DBService.Jobs.SaveJob;
using LL.Infrastructure.Integrations.DataAPI;
using MediatR;

namespace LL.Application.Queries.Job.GetJobListByOrganization;

public sealed class GetJobListByOrganizationQueryHandler : IRequestHandler<GetJobListByOrganizationQuery, List<GetJobListResult>>
{
    private IDbJobByIdService _jobByIdService;
    private IDataService _dataService;
    private DataAPIConfig _config;
    private IDbSaveJobAsync _dbSaveJob;
    private IDbJobsByOrganizationIdService _jobsByOrganizationIdService;

    public GetJobListByOrganizationQueryHandler(IDbJobByIdService jobByIdService, IDataService dataService,
                                   DataAPIConfig config, IDbSaveJobAsync dbSaveJob, IDbJobsByOrganizationIdService jobsByOrganizationIdService)
    {
        _jobByIdService = jobByIdService;
        _dataService = dataService;
        _config = config;
        _dbSaveJob = dbSaveJob;
        _jobsByOrganizationIdService = jobsByOrganizationIdService;
    }
    public async Task<List<GetJobListResult>> Handle(GetJobListByOrganizationQuery request, CancellationToken cancellationToken)
    {
        var jobList = await _jobsByOrganizationIdService.GetOrganizationJobsAsync(request.OrganizationId);

        if(jobList.Count == 0)
        {
            // get jobs from thirdparty api
            var jobs = await _dataService.GetJobListAsync(_config.DataAPIKey, request.OrganizationId);

            //save all jobs to database 
            foreach (var job in jobs)
            {
                var existingJob = await _jobByIdService.GetJobByExternalIdAsync(job.ExternalId);
                if (existingJob == null)
                {
                    await _dbSaveJob.SaveJobAsync(job, request.OrganizationId);
                }
            }

            var newJobList = await _jobsByOrganizationIdService.GetOrganizationJobsAsync(request.OrganizationId);
            return  GetJobList(newJobList);
        }
        return  GetJobList(jobList); 
    }

    private List<GetJobListResult> GetJobList(List<LL.Application.Common.Interfaces.Services.DBService.Jobs.JobsByOrganizationId.Job> jobs)
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
