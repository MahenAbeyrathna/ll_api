using Infrastructure.Data.JobService.JobByIdService;
using LL.Application.Common.Interfaces.Services.DBService.Jobs.JobById;
using LL.Application.Common.Interfaces.Services.DBService.Jobs.JobsByOrganizationId;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Data;

public static class DependencyInjection
{
    public static IServiceCollection AddDbServices(this IServiceCollection services)
    {
        services.AddScoped<IDbJobsByOrganizationIdService, IDbJobsByOrganizationIdService>();
        services.AddScoped<IDbJobByIdService, DbJobByIdService>();
        return services;
    }
}