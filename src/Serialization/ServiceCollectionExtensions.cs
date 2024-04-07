using Microsoft.Extensions.DependencyInjection;
using Serialization.Internal;

namespace Serialization;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSerialization(this IServiceCollection services)
    {
        services.AddSingleton<ISerializer, Serializer>();
        return services;
    }
}