namespace WebApi.Models;

public sealed class InvoiceModel
{
    public decimal Amount { get; set; }
    public DateTime DueDate { get; set; }
}