namespace Kafka;

public interface IKafkaStructureInitializer
{
    Task InitializeAsync(CancellationToken cancellationToken);
}