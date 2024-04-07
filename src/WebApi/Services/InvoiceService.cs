using Outbox;
using WebApi.Domain;
using WebApi.Events;
using WebApi.Models;

namespace WebApi.Services
{
    internal sealed class InvoiceService : IInvoiceService
    {
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IOutbox _outbox;

        public InvoiceService(IInvoiceRepository invoiceRepository, IOutbox outbox)
        {
            _invoiceRepository = invoiceRepository;
            _outbox = outbox;
        }

        public async Task<Guid> CreateInvoice(InvoiceModel model, CancellationToken cancellationToken)
        {
            var invoice = Invoice.Create(model.Amount, model.DueDate);

            await _invoiceRepository.AddEntityAsync(invoice, cancellationToken);
            await _invoiceRepository.SaveChangesAsync(cancellationToken);

            var operationId = Guid.NewGuid();

            await _outbox.AddAsync(
                data: new InvoiceCreatedEvent
                {
                    InvoiceId = invoice.Id
                }, 
                topic: InvoiceCreatedEvent.Topic,
                partitionBy: i => i.InvoiceId.ToString(),
                isSequential: false,
                metadata: new Dictionary<string, string>
                {
                    { "OperationId", operationId.ToString() }
                }, 
                cancellationToken);

            return invoice.Id;
        }
    }
}