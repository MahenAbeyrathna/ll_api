using LL.Application.Common.Interfaces.Services.DBService.Jobs.JobById;
using LL.Application.Common.Interfaces.Services.DBService.Jobs.JobsByOrganizationId;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Data.JobService.JobByIdService;

public class DbJobByIdService : IDbJobByIdService
{
    private readonly string _connectionString;
    public DbJobByIdService(IConfiguration config)
    {
        _connectionString = config.GetConnectionString("SqlServer");
    }

    public async Task<Job> GetJobByExternalIdAsync(Guid externalId)
    {
        var result = new Job();
        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            await conn.OpenAsync();

            string sql = @"
                SELECT Id, Title, Description, CreatedDateTime, IsDeleted ,ExternalId
                FROM Jobs
                WHERE ExternalId = @ExternalId";

            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@ExternalId", externalId);

                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        result = new Job
                        {
                            JobId = reader.GetGuid(reader.GetOrdinal("Id")),
                            ExternalId = reader.GetGuid(reader.GetOrdinal("ExternalId")),
                            Title = reader.GetString(reader.GetOrdinal("Title")),
                            Description = reader.GetString(reader.GetOrdinal("Description")),
                            CreatedDateTime = reader.GetDateTime(reader.GetOrdinal("CreatedDateTime")),
                            IsDeleted = reader.GetBoolean(reader.GetOrdinal("IsDeleted"))
                        };
                    }
                }
            }
        }
        return result.JobId != Guid.Empty ? result : null;
    }

    public async Task<Job> GetJobByIdAsync(Guid jobId)
    {
        var result = new Job();
        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            await conn.OpenAsync();

            string sql = @"
                SELECT Id, Title, Description, CreatedDateTime, IsDeleted 
                FROM Jobs
                WHERE Id = @JobId";

            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@JobId", jobId);

                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        result= new Job
                        {
                            JobId = reader.GetGuid(reader.GetOrdinal("Id")),
                            Title = reader.GetString(reader.GetOrdinal("Title")),
                            Description = reader.GetString(reader.GetOrdinal("Description")),
                            CreatedDateTime = reader.GetDateTime(reader.GetOrdinal("CreatedDateTime")),
                            IsDeleted = reader.GetBoolean(reader.GetOrdinal("IsDeleted"))
                        };
                    }
                }
            }
        }
        return result;
    }
}
