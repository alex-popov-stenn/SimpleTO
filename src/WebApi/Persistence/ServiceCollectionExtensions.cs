using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace WebApi.Persistence;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPersistence<TContext>(this IServiceCollection services, string connectionString)
        where TContext : DbContext
    {
        services.AddApplicationDbContext<TContext>(connectionString);
        services.TryAddTransient<DbContext>(sp => sp.GetRequiredService<TContext>());
        return services;
    }

    private static void AddApplicationDbContext<TContext>(this IServiceCollection services, string connectionString)
        where TContext : DbContext
    {
        services.AddDbContext<TContext>((sp, options) =>
        {
            options
                .UseSqlServer(
                    connectionString,
                    cfg => cfg.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName))
                .UseQueryTrackingBehavior(QueryTrackingBehavior.TrackAll);
        });
    }
}