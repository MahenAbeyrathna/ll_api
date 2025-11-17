using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace DataAPI.Controllers
{
    [ApiController]
    [Route("api/jobs")]
    public class JobsController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get([FromQuery]Guid organizationId)
        {
            var result = new List<OrganizationJobs>
            {
                new OrganizationJobs()
                {
                    OrganizationId = Guid.Parse("a3f2c7a2-4f8e-4c3e-9e54-5dce99f77b12"),
                    JobList = new List<JobList>
                     {
                        new JobList
                        {
                            ExternalId = Guid.Parse("d1b0e6ff-7a45-4ef3-bcd8-98c71f67c111"),
                            Title = "Electric Issue",
                            Description = "Applience not working"
                        },
                        new JobList
                        {
                            ExternalId = Guid.Parse("b7f33d9f-1b21-4c37-95c6-112a5e52f298"),
                            Title = "Tap",
                            Description = "Leak"
                        },
                        new JobList
                        {
                            ExternalId = Guid.Parse("c92877d1-3d7e-4beb-9c38-e9278bb234aa"),
                            Title = "Rollerdoor",
                            Description = "broken"
                        }
                    }
                },
                new OrganizationJobs()
                {
                    OrganizationId = Guid.Parse("a3f2c7a2-4f8e-4c3e-9e54-5dce99f77b44"),
                    JobList = new List<JobList>
                     {
                        new JobList
                        {
                            ExternalId = Guid.Parse("d1b0e6ff-7a45-4ef3-bcd8-98c71f67c144"),
                            Title = "Electric Issue 2",
                            Description = "Applience not working 2"
                        },
                        new JobList
                        {
                            ExternalId = Guid.Parse("b7f33d9f-1b21-4c37-95c6-112a5e52f244"),
                            Title = "Tap 2",
                            Description = "Leak 2"
                        },
                        new JobList
                        {
                            ExternalId = Guid.Parse("c92877d1-3d7e-4beb-9c38-e9278bb23444"),
                            Title = "Rollerdoor 2",
                            Description = "broken 2"
                        }
                    } 
                }
            };
            var response = result.Where(r => r.OrganizationId == organizationId).Select(r => r.JobList).FirstOrDefault();
            return  Ok(JsonConvert.SerializeObject(response));
        }
    }
}
