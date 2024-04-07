using System.Collections.Immutable;

namespace Kafka;

public interface IKafkaMessageSender
{
    Task SendAsync(ImmutableArray<MessageEnvelope> messages, CancellationToken cancellationToken);
}