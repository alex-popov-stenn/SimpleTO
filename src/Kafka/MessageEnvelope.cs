namespace Kafka
{
    public sealed class MessageEnvelope
    {
        public string Topic { get; init; } = default!;
        public string? Key { get; init; }
        public string Payload { get; init; } = default!;
        public string PayloadType { get; init; } = default!;
        public Dictionary<string, string>? Metadata { get; init; }
        public DateTimeOffset Created { get; init; }
    }
}