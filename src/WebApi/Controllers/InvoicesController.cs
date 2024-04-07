using Microsoft.AspNetCore.Mvc;
using UnitOfWork;
using WebApi.Models;
using WebApi.Services;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InvoicesController : ControllerBase
    {
        private readonly IInvoiceService _invoiceService;
        private readonly IUnitOfWork _unitOfWork;

        public InvoicesController(IInvoiceService invoiceService, IUnitOfWork unitOfWork)
        {
            _invoiceService = invoiceService;
            _unitOfWork = unitOfWork;
        }

        [HttpPost]
        public async Task<InvoicePayload> CreateInvoice([FromBody] InvoiceModel model, CancellationToken cancellationToken)
        {
            await using var transaction = await _unitOfWork.BeginSnapshotTransactionAsync(cancellationToken);

            try
            {
                var invoiceId = await _invoiceService.CreateInvoice(model, cancellationToken);
                await transaction.CommitAsync(cancellationToken);
                return new InvoicePayload
                {
                    Id = invoiceId
                };
            }
            catch
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        }
    }
}