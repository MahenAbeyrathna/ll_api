using LL.Application.Common.Interfaces.Services.DataAPIService;
using LL.Application.Common.Interfaces.Services.DBService.Jobs.JobById;
using LL.Application.Common.Interfaces.Services.DBService.Jobs.SaveJob;
using LL.Infrastructure.Integrations.DataAPI;
using MediatR;

namespace LL.Application.Queries.Job.GetJobById;

public sealed class GetJobByIdQueryHandler : IRequestHandler<GetJobByIdQuery, GetJobByIdResponse>
{
    private IDbJobByIdService _jobByIdService;
    private IDataService _dataService;
    private DataAPIConfig _config;
    private IDbSaveJobAsync _dbSaveJob;

    public GetJobByIdQueryHandler(IDbJobByIdService jobByIdService, IDataService dataService,
                                   DataAPIConfig config, IDbSaveJobAsync dbSaveJob)
    {
        _jobByIdService = jobByIdService;
        _dataService = dataService;
        _config = config;
        _dbSaveJob = dbSaveJob;
    }
    public async Task<GetJobByIdResponse> Handle(GetJobByIdQuery request, CancellationToken cancellationToken)
    {
        var jobDetails = await _jobByIdService.GetJobByExternalIdAsync(request.JobId);

        if (jobDetails == null)
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

            // check for searched job from saved thirdparty jobs
            if(jobs.Any(j=> j.ExternalId == request.JobId))
            {
                var newJob = await _jobByIdService.GetJobByExternalIdAsync(request.JobId);
                return new GetJobByIdResponse()
                {
                    JobId = newJob.JobId,
                    Description = newJob.Description,
                    Title = newJob.Title,
                    IsDeleted = newJob.IsDeleted,
                    CreatedDateTime = newJob.CreatedDateTime
                };
            }
        }
        return new GetJobByIdResponse()
        {
            JobId = jobDetails.JobId,
            Description = jobDetails.Description,
            Title = jobDetails.Title,
            IsDeleted= jobDetails.IsDeleted,
            CreatedDateTime = jobDetails.CreatedDateTime
        };
    }
}
