using LL.Application.Common.Interfaces.Services.DBService.Jobs.JobsByOrganizationId;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Data.JobService.JobsByOrganizationIdService
{
    public class DbJobsByOrganizationIdService : IDbJobsByOrganizationIdService
    {
        private readonly string _connectionString;
        public DbJobsByOrganizationIdService(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("SqlServer");
        }
        public async Task<List<Job>> GetOrganizationJobsAsync(Guid organizationId)
        {
            var result = new List<Job>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();

                string sql = @"
                SELECT Id, Title, Description, CreatedDateTime, IsDeleted ,ExternalId
                FROM Jobs
                WHERE OrganizationId = @OrganizationId";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@OrganizationId", organizationId);

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            result.Add(new Job
                            {
                                JobId = reader.GetGuid(reader.GetOrdinal("Id")),
                                ExternalId = reader.GetGuid(reader.GetOrdinal("ExternalId")),
                                Title = reader.GetString(reader.GetOrdinal("Title")),
                                Description = reader.GetString(reader.GetOrdinal("Description")),
                                CreatedDateTime = reader.GetDateTime(reader.GetOrdinal("CreatedDateTime")),
                                IsDeleted = reader.GetBoolean(reader.GetOrdinal("IsDeleted"))
                            });
                        }
                    }
                }
            }
            return result.ToList();
        }
    }
}
