using WebApi.Models;

namespace WebApi.Services
{
    public interface IInvoiceService
    {
        Task<Guid> CreateInvoice(InvoiceModel model, CancellationToken cancellationToken);
    }
}