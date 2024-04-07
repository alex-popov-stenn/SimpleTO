using UnitOfWork;
using WebApi.Domain;
using WebApi.Services;

namespace WebApi.Persistence;

internal sealed class InvoiceRepository : IInvoiceRepository
{
    private readonly IUnitOfWork _unitOfWork;

    public InvoiceRepository(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task AddEntityAsync(Invoice invoice, CancellationToken token)
    {
        await _unitOfWork.AddAsync(invoice, token);
    }

    public async Task SaveChangesAsync(CancellationToken token)
    {
        await _unitOfWork.CommitAsync(token);
    }
}