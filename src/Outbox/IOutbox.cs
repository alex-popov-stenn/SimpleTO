using System.Collections.Immutable;

namespace Outbox;

public interface IOutbox
{
    Task AddAsync<T>(T data, string topic, Func<T, string>? partitionBy, bool isSequential, Dictionary<string, string>? metadata, CancellationToken cancellationToken)
        where T : class;
    Task<ImmutableArray<OutboxRecord>> ReserveTopByMessageTypesAsync(int top, TimeSpan reservationTimeout, CancellationToken cancellationToken);
    Task MarkAsProcessedAsync(ImmutableArray<OutboxRecord> data, CancellationToken cancellationToken);
    Task DeleteProcessedAsync(CancellationToken cancellationToken);
}