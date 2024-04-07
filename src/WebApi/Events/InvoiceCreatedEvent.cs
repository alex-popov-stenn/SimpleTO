namespace WebApi.Events
{
    public sealed class InvoiceCreatedEvent
    {
        public const string Topic = "invoices";
        public Guid InvoiceId { get; init; }
    }
}