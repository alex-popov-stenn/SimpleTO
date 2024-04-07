using Kafka;
using Serialization;
using WebApi.Events;

namespace WebApi.MessageHandlers
{
    internal sealed class RootMessageHandler : IMessageHandler
    {
        private readonly ILogger<RootMessageHandler> _logger;
        private readonly ISerializer _serializer;

        public RootMessageHandler(ILogger<RootMessageHandler> logger, ISerializer serializer)
        {
            _logger = logger;
            _serializer = serializer;
        }

        public Task HandleAsync(MessageEnvelope message, CancellationToken cancellationToken)
        {
            var type = Type.GetType(message.PayloadType);

            if (type == typeof(InvoiceCreatedEvent))
            {
                var invoiceCreatedEvent = _serializer.Deserialize<InvoiceCreatedEvent>(message.Payload);
                _logger.LogInformation("Received invoice created event with ID {InvoiceId}", invoiceCreatedEvent.InvoiceId);
            }

            return Task.CompletedTask;
        }
    }
}