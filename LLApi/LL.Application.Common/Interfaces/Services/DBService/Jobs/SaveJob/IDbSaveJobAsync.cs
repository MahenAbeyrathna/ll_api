using LL.Application.Common.Interfaces.Services.DBService.Jobs.JobsByOrganizationId;

namespace LL.Application.Common.Interfaces.Services.DBService.Jobs.SaveJob
{
    public interface IDbSaveJobAsync
    {
       Task<bool> SaveJobAsync(Job job, Guid organizationId);
    }
}
