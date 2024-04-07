using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Outbox.Internal;

internal sealed class CleanupOutboxJob(IServiceProvider serviceProvider, ILogger<CleanupOutboxJob> logger) : BackgroundService
{
    private readonly TimeSpan _cleanupJobInterval = TimeSpan.FromSeconds(5);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await BackgroundProcessing(stoppingToken);
    }

    private async Task BackgroundProcessing(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(_cleanupJobInterval, stoppingToken);
            await RelayExecutionAsync(stoppingToken);
        }
    }

    public override async Task StopAsync(CancellationToken stoppingToken)
    {
        await base.StopAsync(stoppingToken);
    }

    private async Task RelayExecutionAsync(CancellationToken cancellationToken)
    {
        try
        {
            await using var scope = serviceProvider.CreateAsyncScope();
            var relay = scope.ServiceProvider.GetRequiredService<IRelay>();
            await relay.CleanupAsync(cancellationToken);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Failed to cleanup outbox");
        }
    }
}