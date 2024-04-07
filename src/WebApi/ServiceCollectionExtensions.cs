using WebApi.Persistence;
using WebApi.Services;

namespace WebApi;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInvoices(this IServiceCollection services)
    {
        services.AddScoped<IInvoiceService, InvoiceService>();
        services.AddScoped<IInvoiceRepository, InvoiceRepository>();
        return services;
    }
}