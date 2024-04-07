namespace Outbox;

public interface IRelay
{      
    Task PublishAsync(CancellationToken cancellationToken);
    Task CleanupAsync(CancellationToken cancellationToken);
}