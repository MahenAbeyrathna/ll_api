using MediatR;

namespace LL.Application.Queries.Job.GetJobById;

public sealed class GetJobByIdQuery : IRequest<GetJobByIdResponse>
{
    public Guid JobId { get; set; }
    public Guid OrganizationId { get; set; }
}
