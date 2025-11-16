namespace LL.Application.Common.Interfaces.Services.DBService.Jobs.JobsByOrganizationId;


public class Job
{
    public Guid JobId { get; set; }
    public Guid ExternalId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime CreatedDateTime { get; set; }
    public bool IsDeleted { get; set; }

}
