namespace DataAPI
{
    public class JobList
    {
        public Guid ExternalId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
    public class OrganizationJobs
    {
        public Guid OrganizationId { get; set; }
        public List<JobList> JobList { get; set; }
    }
}
