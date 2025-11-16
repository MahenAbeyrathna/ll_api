using MediatR;

namespace LL.Application.Queries.Job.GetJobListByOrganization;

public sealed class GetJobListByOrganizationQuery : IRequest<List<GetJobListResult>>
{
    public Guid OrganizationId { get; set; }
}
