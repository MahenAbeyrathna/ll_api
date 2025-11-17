using LL.Infrastructure.Integrations.DataAPI;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace LL.Application;
public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton(x => configuration.GetSection(nameof(DataAPIConfig)));
        services.AddMediatR(new[] { Assembly.GetExecutingAssembly() });
        return services;
    }
}
