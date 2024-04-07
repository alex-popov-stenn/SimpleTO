using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Outbox.Internal;

internal sealed class PublishOutboxJob(IServiceProvider serviceProvider, ILogger<PublishOutboxJob> logger) : BackgroundService
{
    private readonly TimeSpan _publishJobInterval = TimeSpan.FromMilliseconds(500);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await BackgroundProcessing(stoppingToken);
    }

    private async Task BackgroundProcessing(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(_publishJobInterval, stoppingToken);
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
            await relay.PublishAsync(cancellationToken);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Failed to publish outbox");
        }
    }
}