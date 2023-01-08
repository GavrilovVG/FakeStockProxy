namespace FakeStockProxy.UnitTests.Builders.Services;

public class FsStockRequestServiceBuilder 
{
    public HttpClientStockRequestServiceMock HttpClientStockRequestServiceMock { get; init; }
    public IFsBackgroundTasksServiceMock IFsBackgroundTasksServiceMock { get;init; }
    public FsStockRequestService FsStockRequestService { get;init; }

    public FsStockRequestServiceBuilder(FsResponseDataDto fsResponseDataDto, FsDataWrapper fsDataWrapper)
    {
        HttpClientStockRequestServiceMock = new HttpClientStockRequestServiceMock(fsResponseDataDto);
        IFsBackgroundTasksServiceMock = new IFsBackgroundTasksServiceMock();
        LoggerAdapter<FsStockRequestService> _logger = new LoggerAdapter<FsStockRequestService>(LoggerFactoryProvider.LoggerFactoryInstance);

        FsStockRequestService = new(HttpClientStockRequestServiceMock.Object, fsDataWrapper.fsStockRequestRepositoryMock.Object,
            _logger, IFsBackgroundTasksServiceMock.Object);
    }
}
