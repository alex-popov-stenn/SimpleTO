using System.Collections.Immutable;
using System.Text;
using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serialization;

namespace Kafka.Internal;

internal sealed class KafkaTopicMessageSender : IKafkaMessageSender
{
    private readonly ISerializer _serializer;
    private readonly ILogger<KafkaTopicMessageSender> _logger;
    private readonly IProducer<string, string> _producer;
    private readonly string _defaultKey = Guid.NewGuid().ToString();

    public KafkaTopicMessageSender(IConfiguration configuration, ISerializer serializer, ILogger<KafkaTopicMessageSender> logger)
    {
        _serializer = serializer;
        _logger = logger;
        var producerConfig = new ProducerConfig
        {
            BootstrapServers = configuration["BootstrapServers"],

            SecurityProtocol = SecurityProtocol.SaslSsl,
            SaslMechanism = SaslMechanism.Plain,

            SaslUsername = configuration["SaslUsername"],
            SaslPassword = configuration["SaslPassword"],

            LingerMs = 200,
            BatchSize = 10 * 1024,
            MessageTimeoutMs = 10000,

            // Enable receiving delivery reports
            EnableDeliveryReports = true,

            // Receive acknowledgement from all sync replicas
            //Acks = Acks.All,

            // Number of times to retry before giving up
            MessageSendMaxRetries = 3,
            // Duration to retry before next attempt
            RetryBackoffMs = 1000,

            // Set to true if you don't want to reorder messages on retry
            //EnableIdempotence = true
        };

        _producer = new ProducerBuilder<string, string>(producerConfig)
            .Build();
    }

    public Task SendAsync(ImmutableArray<MessageEnvelope> messages, CancellationToken cancellationToken)
    {
        foreach (var message in messages)
        {
            try
            {
                var kafkaPayload = _serializer.Serialize(message);

                _producer.Produce(
                    message.Topic,
                    new Message<string, string> { Key = message.Key ?? _defaultKey, Value = kafkaPayload, Headers = PrepareHeaders(message.Metadata, message.Created, message.PayloadType) }, report =>
                    {
                        if (report.Status != PersistenceStatus.Persisted)
                            //throw new Exception($"Failed to send message to Kafka, Id: {message.Id}, Topic: {topic}");
                            _logger.LogError("Failed kafka message producing with Key {Key}, Error: {error}", report.Message.Key, report.Error.Code);
                    });

                _logger.LogInformation("Message sent to Kafka, Id: {Id}, Topic: {Topic}", message.Key, message.Topic);
            }
            catch (ProduceException<Null, string> ex)
            {
                throw new Exception($"Failed to send message to Kafka, Id: {message.Key}, Topic: {message.Topic}", ex);
            }
        }

        _producer.Flush(cancellationToken);
        return Task.CompletedTask;
    }

    private Headers PrepareHeaders(Dictionary<string, string>? metadata, DateTimeOffset messageTimestamp, string messageType)
    {
        var headers = new Headers
        {
            { "timestamp", BitConverter.GetBytes(messageTimestamp.ToUnixTimeMilliseconds()) }, { "type", Encoding.UTF8.GetBytes(messageType) }
        };

        if (metadata is null)
            return headers;

        foreach (var (key, value) in metadata)
            headers.Add(key, Encoding.UTF8.GetBytes(value));

        return headers;
    }
}