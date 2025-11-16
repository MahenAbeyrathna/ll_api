using LL.Application.Common.Interfaces.Services.DBService.Jobs.JobsByOrganizationId;

namespace LL.Application.Common.Interfaces.Services.DBService.Jobs.JobById;

public  interface IDbJobByIdService
{
    Task<Job> GetJobByIdAsync(Guid jobId);
    Task<Job> GetJobByExternalIdAsync(Guid externalId);
}
