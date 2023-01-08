using FakeStockProxy.Application.DataTransferObjects;
using FakeStockProxy.Application.Interfaces;
using FakeStockProxy.Core.Entities;
using FakeStockProxy.Core.Interfaces;
using FakeStockProxy.Core.Interfaces.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace FakeStockProxy.Application.Services;

public class FsBackgroundTasksService : IFsBackgroundTasksService
{
    private readonly IFsBackgroundTaskQueue _taskQueue;
    private readonly IAppLogger<FsBackgroundTasksService> _logger;
    private readonly IServiceProvider _serviceProvider;

    public FsBackgroundTasksService(IAppLogger<FsBackgroundTasksService> logger,
        IFsBackgroundTaskQueue taskQueue, IServiceProvider serviceProvider)
    {
        _taskQueue = taskQueue ?? throw new ArgumentNullException(nameof(taskQueue));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
    }

    /// <inheritdoc/>
    public async Task AddOrUpdateCacheAsync(FsStockRequestDto fsStockRequestModel)
    {
        await _taskQueue.QueueBackgroundWorkItemAsync(async cancellationToken =>
            await AddOrUpdateCacheAsync(cancellationToken, fsStockRequestModel));
    }

    /// <inheritdoc/>
    public async Task AddOrUpdateCacheAsync(CancellationToken cancellationToken, FsStockRequestDto fsStockRequestModel)
    {
        _logger.LogInformation("FsBackgroundTasksService.AddOrUpdateCache: job is started.");

        if (fsStockRequestModel is null) throw new ArgumentNullException(nameof(fsStockRequestModel));
        var fsStockRequest = Mapper.ObjectMapper.Mapper.Map<FsStockRequest>(fsStockRequestModel);

        using var scope = _serviceProvider.CreateScope();
        var service = scope.ServiceProvider.GetService<IFsStockRequestRepository>();

        var dataFromCache = await service!.TryGetFromCacheAsync(fsStockRequest);

        if (dataFromCache is not null)
        {
            await service.UpdateChachedDataAsync(cancellationToken, dataFromCache, fsStockRequest);
        }
        else
        {
            await service.AddAsync(fsStockRequest, cancellationToken);
        }
        
        _logger.LogInformation("FsBackgroundTasksService.AddOrUpdateCache: job is completed.");
    }

    
}
