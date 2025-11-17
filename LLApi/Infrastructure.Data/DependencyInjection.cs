using Infrastructure.Data.JobService.JobByIdService;
using Infrastructure.Data.JobService.JobsByOrganizationIdService;
using Infrastructure.Data.JobService.SaveJob;
using LL.Application.Common.Interfaces.Services.DataAPIService;
using LL.Application.Common.Interfaces.Services.DBService.Jobs.JobById;
using LL.Application.Common.Interfaces.Services.DBService.Jobs.JobsByOrganizationId;
using LL.Application.Common.Interfaces.Services.DBService.Jobs.SaveJob;
using LL.Infrastructure.Integrations.DataAPI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Data;

public static class DependencyInjection
{
    public static IServiceCollection AddDbServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IDbJobsByOrganizationIdService, DbJobsByOrganizationIdService>();
        services.AddScoped<IDbJobByIdService, DbJobByIdService>();
        services.AddScoped<IDbSaveJobAsync, DbSaveJobAsync>();
        services.AddTransient<IDataService, DataAPIService>();
        services.AddHttpClient<IDataService, DataAPIService>();
        services.AddSingleton(configuration.GetSection(nameof(DataAPIConfig)).Get<DataAPIConfig>());
        return services;
    }
}