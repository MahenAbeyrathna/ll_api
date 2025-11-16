using LL.Infrastructure.Integrations.DataAPI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LL.Application;
public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton(x => configuration.GetSection(nameof(DataAPIConfig)));
        return services;
    }
}
