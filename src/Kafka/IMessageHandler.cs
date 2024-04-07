namespace Kafka
{
    public interface IMessageHandler
    {
        Task HandleAsync(MessageEnvelope message, CancellationToken cancellationToken);
    }
}