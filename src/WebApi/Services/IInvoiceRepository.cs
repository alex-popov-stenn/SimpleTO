using WebApi.Domain;

namespace WebApi.Services;

public interface IInvoiceRepository
{
    Task AddEntityAsync(Invoice invoice, CancellationToken cancellationToken);
    Task SaveChangesAsync(CancellationToken token);
}