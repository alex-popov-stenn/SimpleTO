using Confluent.Kafka;

namespace Kafka.Internal;

public static class ConsumerExtensions
{
    public static IReadOnlyCollection<ConsumeResult<TKey, TValue>> ConsumeBatch<TKey, TValue>(
        this IConsumer<TKey, TValue> consumer, TimeSpan consumeTimeout, int maxBatchSize, CancellationToken stoppingToken)
    {
        var message = consumer.Consume(consumeTimeout);

        if (message?.Message is null)
            return Array.Empty<ConsumeResult<TKey, TValue>>();

        var messageBatch = new List<ConsumeResult<TKey, TValue>> { message };

        while (messageBatch.Count < maxBatchSize)
        {
            message = consumer.Consume(TimeSpan.Zero);
            if (message?.Message is null)
                break;

            messageBatch.Add(message);
        }

        return messageBatch;
    }
}