namespace LL.Application.Queries.Job.GetJobById;

public sealed class GetJobByIdResponse
{
    public Guid JobId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime CreatedDateTime { get; set; }
    public bool IsDeleted { get; set; }
}
