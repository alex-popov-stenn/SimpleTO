using Microsoft.Extensions.DependencyInjection;
using Outbox.Internal;

namespace Outbox;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddOutbox(this IServiceCollection services)
    {
        services.AddScoped<IOutbox, Internal.Outbox>();
        services.AddScoped<IRelay, Relay>();
        services.AddScoped<IOutboxInitializer, OutboxInitializer>();

        services.AddHostedService<PublishOutboxJob>();
        services.AddHostedService<CleanupOutboxJob>();

        return services;
    }
}