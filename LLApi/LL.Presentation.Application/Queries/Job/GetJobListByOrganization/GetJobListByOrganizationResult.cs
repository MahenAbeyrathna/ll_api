namespace LL.Application.Queries.Job.GetJobListByOrganization;

public sealed class GetJobListResult
{
    public Guid JobId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime CreatedDateTime { get; set; }
    public bool IsDeleted { get; set; }
}
