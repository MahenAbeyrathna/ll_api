
using LL.Application.Common.Interfaces.Services.DBService.Jobs.JobsByOrganizationId;

namespace LL.Application.Common.Interfaces.Services.DataAPIService;

public interface IDataService
{
    Task<List<Job>> GetJobListAsync(string authorizationKey, Guid organizationId);
}
