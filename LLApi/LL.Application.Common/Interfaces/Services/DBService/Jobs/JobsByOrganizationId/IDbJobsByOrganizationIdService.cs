namespace LL.Application.Common.Interfaces.Services.DBService.Jobs.JobsByOrganizationId;

public  interface IDbJobsByOrganizationIdService
{
    Task<List<Job>> GetOrganizationJobsAsync(Guid organizationId);
}
