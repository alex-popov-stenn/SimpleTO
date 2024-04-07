using Microsoft.Extensions.DependencyInjection;

namespace UnitOfWork;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddUnitOfWork(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, Internal.UnitOfWork>();
        return services;
    }
}