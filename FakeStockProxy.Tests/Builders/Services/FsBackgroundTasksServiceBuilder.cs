namespace FakeStockProxy.UnitTests.Builders.Services;

public class FsBackgroundTasksServiceBuilder
{
    public FsBackgroundTasksService fsBackgroundTasksService { get; init; }
    public IFsBackgroundTaskQueue iFsBackgroundTaskQueue { get; init; }
    public IServiceProvider iServiceProvider { get; init; }

    public FsBackgroundTasksServiceBuilder(FsDataWrapper fsDataWrapper)
    {
        iFsBackgroundTaskQueue = new FsBackgroundTaskQueue(10);
        iServiceProvider = new IServiceProviderMock((typeof(IFsStockRequestRepository), fsDataWrapper.fsStockRequestRepositoryMock.Object)).Object;
        LoggerAdapter<FsBackgroundTasksService> _logger = new LoggerAdapter<FsBackgroundTasksService>(LoggerFactoryProvider.LoggerFactoryInstance);

        fsBackgroundTasksService = new FsBackgroundTasksService(_logger, iFsBackgroundTaskQueue, iServiceProvider);

    }
}
