using System.Collections.Immutable;
using Kafka;
using UnitOfWork;

namespace Outbox.Internal;

internal sealed class Relay : IRelay
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IOutbox _outbox;
    private readonly IKafkaMessageSender _kafkaMessageSender;
    private const int BatchSize = 100;
    private static readonly TimeSpan ReservationTimeout = TimeSpan.FromSeconds(5);

    public Relay(IUnitOfWork unitOfWork, IOutbox outbox, IKafkaMessageSender kafkaMessageSender)
    {
        _unitOfWork = unitOfWork;
        _outbox = outbox;
        _kafkaMessageSender = kafkaMessageSender;
    }

    public async Task PublishAsync(CancellationToken cancellationToken)
    {
        await using var transaction = await _unitOfWork.BeginSnapshotTransactionAsync(cancellationToken);

        try
        {
            var records = await _outbox.ReserveTopByMessageTypesAsync(BatchSize, ReservationTimeout, cancellationToken);

            var builder = ImmutableArray.CreateBuilder<MessageEnvelope>();

            foreach (var record in records)
            {
                var message = new MessageEnvelope
                {
                    PayloadType = record.MessageType,
                    Payload = record.JsonRawData,
                    Topic = record.Topic,
                    Key = record.PartitionBy,
                    Created = record.Timestamp,
                    Metadata = record.Metadata
                };
                builder.Add(message);
            }

            await _kafkaMessageSender.SendAsync(builder.ToImmutable(), cancellationToken);

            await _outbox.MarkAsProcessedAsync(records, cancellationToken);
            await transaction.CommitAsync(cancellationToken);
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }

    public async Task CleanupAsync(CancellationToken cancellationToken)
    {
        await using var transaction = await _unitOfWork.BeginSnapshotTransactionAsync(cancellationToken);

        try
        {
            await _outbox.DeleteProcessedAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }
}