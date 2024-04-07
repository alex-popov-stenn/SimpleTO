using Confluent.Kafka;
using Confluent.Kafka.Admin;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Kafka.Internal;

internal sealed class KafkaStructureInitializer : IKafkaStructureInitializer
{
    private readonly ILogger<KafkaStructureInitializer> _logger;
    private readonly IConfiguration _configuration;
    private readonly Task _initializationTask;

    public KafkaStructureInitializer(ILogger<KafkaStructureInitializer> logger, IConfiguration configuration, params string[] topics)
    {
        _logger = logger;
        _configuration = configuration;
        _initializationTask = InitializeCoreAsync(topics);
    }

    public async Task InitializeAsync(CancellationToken cancellationToken)
    {
        await _initializationTask;
    }

    private async Task InitializeCoreAsync(string[] topics)
    {
        using var adminClient = new AdminClientBuilder(new AdminClientConfig
        {
            BootstrapServers = _configuration["BootstrapServers"],
            SaslUsername = _configuration["SaslUsername"],
            SaslPassword = _configuration["SaslPassword"],
            SecurityProtocol = SecurityProtocol.SaslSsl,
            SaslMechanism = SaslMechanism.Plain,
        }).Build();
        var metadata = adminClient.GetMetadata(TimeSpan.FromMinutes(1));

        foreach (var topic in topics)
        {
            try
            {
                if (metadata.Topics.All(p => p.Topic != topic))
                    await adminClient.CreateTopicsAsync(new TopicSpecification[]
                    {
                        new TopicSpecification { Name = topic, ReplicationFactor = 3, NumPartitions = 3 }
                    });
            }
            catch (CreateTopicsException e)
            {
                _logger.LogError(
                    $"An error occurred creating topic {e.Results[0].Topic}: {e.Results[0].Error.Reason}");
            }
        }
    }
}