using FakeStockProxy.Core.Interfaces;
using Microsoft.Extensions.Hosting;

namespace FakeStockProxy.Infrastracture.HostedServices;

public class FsBackgroundService : BackgroundService
{
    private readonly IAppLogger<FsBackgroundService> _logger;
    public IFsBackgroundTaskQueue TaskQueue { get; }

    public FsBackgroundService(IFsBackgroundTaskQueue taskQueue, IAppLogger<FsBackgroundService> logger)
    {
        _logger = logger;
        TaskQueue = taskQueue;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("FsBackgroundService.ExecuteAsync: running.");

        await BackgroundProcessing(stoppingToken);
    }

    private async Task BackgroundProcessing(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var workItem = await TaskQueue.DequeueAsync(stoppingToken);

            try
            {
                await workItem(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError("FsBackgroundService.BackgroundProcessing: exception caught - {message}.", ex.Message);
            }
        }
    }

    public override async Task StopAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("FsBackgroundService.StopAsync: Queued Hosted Service is stopping.");

        await base.StopAsync(stoppingToken);
    }
}
