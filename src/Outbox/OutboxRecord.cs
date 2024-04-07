namespace Outbox
{
    public sealed class OutboxRecord
    {
        public Guid Id { get; init; }
        public DateTimeOffset Timestamp { get; init; }
        public string MessageType { get; init; } = default!;
        public string Topic { get; init; } = default!;
        public string? PartitionBy { get; init; }
        public string JsonRawData { get; init; } = default!;
        public bool IsProcessed { get; init; }
        public Dictionary<string, string>? Metadata { get; init; }
    }
}