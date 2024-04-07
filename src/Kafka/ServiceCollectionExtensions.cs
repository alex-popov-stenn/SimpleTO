using Kafka.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serialization;

namespace Kafka;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddKafka(this IServiceCollection services, params string[] topics)
    {
        services.AddSingleton<IKafkaMessageSender, KafkaTopicMessageSender>();
        services.AddSingleton<IKafkaStructureInitializer>(sp =>
            new KafkaStructureInitializer(sp.GetRequiredService<ILogger<KafkaStructureInitializer>>(),
                sp.GetRequiredService<IConfiguration>(), topics));

        foreach (var topic in topics)
        {
            services.AddSingleton<IHostedService>(sp =>
                new KafkaConsumer(sp.GetRequiredService<IConfiguration>(),
                    sp.GetRequiredService<ILogger<KafkaConsumer>>(),
                    sp.GetRequiredService<ISerializer>(),
                    sp.GetRequiredService<IEnumerable<IMessageHandler>>(),
                    topic));
        }

        return services;
    }

    public static IServiceCollection HandleMessage<TMessageHandler>(this IServiceCollection services)
        where TMessageHandler : class, IMessageHandler
    {
        services.AddSingleton<IMessageHandler, TMessageHandler>();
        return services;
    }
}