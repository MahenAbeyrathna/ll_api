using LL.Application.Common.Interfaces.Services.DBService.Jobs.JobsByOrganizationId;
using LL.Application.Common.Interfaces.Services.DBService.Jobs.SaveJob;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Data.JobService.SaveJob
{
    public class DbSaveJobAsync : IDbSaveJobAsync
    {
        private readonly string _connectionString;
        public DbSaveJobAsync(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("DefaultConnection");
        }

        public async Task<bool> SaveJobAsync(Job job, Guid organizationId)
        {
            using SqlConnection conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            string sql = @"
            INSERT INTO Jobs (Id,ExternalId, OrganizationId, Title, Description, CreatedDateTime, IsDeleted)
            VALUES (@Id,@ExternalId, @OrganizationId, @Title, @Description, @CreatedDateTime, @IsDeleted);";

            using SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Id", new Guid());
            cmd.Parameters.AddWithValue("@ExternalId", job.ExternalId);
            cmd.Parameters.AddWithValue("@OrganizationId", organizationId);
            cmd.Parameters.AddWithValue("@Title", job.Title);
            cmd.Parameters.AddWithValue("@Description", job.Description);
            cmd.Parameters.AddWithValue("@CreatedDateTime", DateTime.UtcNow);
            cmd.Parameters.AddWithValue("@IsDeleted", false);

            return await cmd.ExecuteNonQueryAsync() > 0;  
        }
    }
}
