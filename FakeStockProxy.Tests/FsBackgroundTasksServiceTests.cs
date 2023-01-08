namespace FakeStockProxy.UnitTests;

public class FsBackgroundTasksServiceTests 
{
    FsDataWrapper _fsStockDataWrapper;

    public FsBackgroundTasksServiceTests()
    {
        _fsStockDataWrapper = new FsDataWrapper();
    }

    [Fact]
    public async void AddOrUpdateCacheAsync_Add()
    {
        var fsBackgroundTasksServiceBuilder = new FsBackgroundTasksServiceBuilder(_fsStockDataWrapper);

        var fsStockRequestDtoNonCached = _fsStockDataWrapper.fsTestDatasource.GetRandomFsStockRequestDtoNonCached();
        var fsStockRequestNonCached = ObjectMapper.Mapper.Map<FsStockRequest>(fsStockRequestDtoNonCached);

        await fsBackgroundTasksServiceBuilder.fsBackgroundTasksService.AddOrUpdateCacheAsync(new CancellationToken(), fsStockRequestDtoNonCached);

        //there must be a try to get data from cache
        _fsStockDataWrapper.fsStockRequestRepositoryMock.Verify(m => m.TryGetFromCacheAsync(fsStockRequestNonCached, true), Times.Once());
        //there must be one AddAsync call in the repository
        _fsStockDataWrapper.fsStockRequestRepositoryMock.Verify(m => m.AddAsync(fsStockRequestNonCached, It.IsAny<CancellationToken>()), Times.Once());
        //the repository shouldn't have UpdateChachedDataAsync calls
        _fsStockDataWrapper.fsStockRequestRepositoryMock.Verify(m => m.UpdateChachedDataAsync(It.IsAny<CancellationToken>(), It.IsAny<FsStockRequest>(),
            It.IsAny<FsStockRequest>()), Times.Never());
    }

    [Fact]
    public async void AddOrUpdateCacheAsync_Update()
    {
        var fsBackgroundTasksServiceBuilder = new FsBackgroundTasksServiceBuilder(_fsStockDataWrapper);

        var fsStockRequestDtoCached = _fsStockDataWrapper.fsTestDatasource.GetRandomFsStockRequestDtoCached();
        var fsStockRequestCached = ObjectMapper.Mapper.Map<FsStockRequest>(fsStockRequestDtoCached);

        await fsBackgroundTasksServiceBuilder.fsBackgroundTasksService.AddOrUpdateCacheAsync(new CancellationToken(), fsStockRequestDtoCached);

        //there must be a try to get data from cache
        _fsStockDataWrapper.fsStockRequestRepositoryMock.Verify(m => m.TryGetFromCacheAsync(fsStockRequestCached, true), Times.Once());
        //there shouldn't be AddAsync call in the repository
        _fsStockDataWrapper.fsStockRequestRepositoryMock.Verify(m => m.AddAsync(fsStockRequestCached, It.IsAny<CancellationToken>()), Times.Never());
        //there must be one UpdateChachedDataAsync call in the repository
        _fsStockDataWrapper.fsStockRequestRepositoryMock.Verify(m => m.UpdateChachedDataAsync(It.IsAny<CancellationToken>(), fsStockRequestCached,
            fsStockRequestCached), Times.Once());
    }
}
