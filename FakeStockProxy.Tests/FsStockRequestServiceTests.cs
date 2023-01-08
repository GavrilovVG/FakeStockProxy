

namespace FakeStockProxy.UnitTests;

public class FsStockRequestServiceTests
{
    FsDataWrapper _fsStockDataWrapper;

    public FsStockRequestServiceTests() 
    {
        _fsStockDataWrapper = new FsDataWrapper();
    }

    [Fact]
    public async void ValidJsonRecieved()
    {
        FsStockRequestServiceBuilder fsStockRequestServiceBuilder = 
            new FsStockRequestServiceBuilder(_fsStockDataWrapper.fsTestDatasource.fsStockRequestDtoBuilder.DataExampleResponseSuccess,
            _fsStockDataWrapper);

        var fsStockRequestDtoNonCached = _fsStockDataWrapper.fsTestDatasource.GetRandomFsStockRequestDtoNonCached();
        var fsStockRequestNonCached = ObjectMapper.Mapper.Map<FsStockRequest>(fsStockRequestDtoNonCached);
        var result = await fsStockRequestServiceBuilder.FsStockRequestService.GetStockAsync(fsStockRequestDtoNonCached);

        Assert.NotNull(result);
        Assert.Equal(result.Hashsum, fsStockRequestDtoNonCached.Hashsum);
        Assert.True(result.StockRequestResultCode == Application.Miscellaneous.Enums.StockRequestResultEnum.Success);

        //request sent to httpClient
        fsStockRequestServiceBuilder.HttpClientStockRequestServiceMock.Verify(m => m.GetJson(It.IsAny<string>()), Times.Once());
        //cache update called in background tasks service
        fsStockRequestServiceBuilder.IFsBackgroundTasksServiceMock.Verify(m => m.AddOrUpdateCacheAsync(fsStockRequestDtoNonCached), Times.Once());
    }

    [Fact]
    public async void InvalidJsonRecieved_NonCachedData()
    {
        FsStockRequestServiceBuilder fsStockRequestServiceBuilder =
            new FsStockRequestServiceBuilder(_fsStockDataWrapper.fsTestDatasource.fsStockRequestDtoBuilder.DataExampleResponseFailure,
            _fsStockDataWrapper);

        var fsStockRequestDtoNonCached = _fsStockDataWrapper.fsTestDatasource.GetRandomFsStockRequestDtoNonCached();

        var result = await fsStockRequestServiceBuilder.FsStockRequestService.GetStockAsync(fsStockRequestDtoNonCached);

        Assert.NotNull(result);
        Assert.Equal(result.Hashsum, fsStockRequestDtoNonCached.Hashsum);
        Assert.True(result.StockRequestResultCode == Application.Miscellaneous.Enums.StockRequestResultEnum.FailureNoData);

        //request sent to httpClient
        fsStockRequestServiceBuilder.HttpClientStockRequestServiceMock.Verify(m => m.GetJson(It.IsAny<string>()), Times.Once());
        //cache update never called
        fsStockRequestServiceBuilder.IFsBackgroundTasksServiceMock.Verify(m => m.AddOrUpdateCacheAsync(fsStockRequestDtoNonCached), Times.Never());
        //there was a try to get data from cache
        _fsStockDataWrapper.fsStockRequestRepositoryMock.Verify(m => m.TryGetFromCacheAsync(It.IsAny<FsStockRequest>(), It.IsAny<bool>()), Times.Once());
    }

    [Fact]
    public async void InvalidJsonRecieved_CachedData()
    {
        FsStockRequestServiceBuilder fsStockRequestServiceBuilder =
            new FsStockRequestServiceBuilder(_fsStockDataWrapper.fsTestDatasource.fsStockRequestDtoBuilder.DataExampleResponseFailure,
            _fsStockDataWrapper);

        var fsStockRequestDtoCached = _fsStockDataWrapper.fsTestDatasource.GetRandomFsStockRequestDtoCached();

        var result = await fsStockRequestServiceBuilder.FsStockRequestService.GetStockAsync(fsStockRequestDtoCached);

        Assert.NotNull(result);
        Assert.Equal(result.Hashsum, fsStockRequestDtoCached.Hashsum);
        Assert.True(result.StockRequestResultCode == Application.Miscellaneous.Enums.StockRequestResultEnum.FailureCachedData);

        //request sent to httpClient
        fsStockRequestServiceBuilder.HttpClientStockRequestServiceMock.Verify(m => m.GetJson(It.IsAny<string>()), Times.Once());
        //cache update never called
        fsStockRequestServiceBuilder.IFsBackgroundTasksServiceMock.Verify(m => m.AddOrUpdateCacheAsync(fsStockRequestDtoCached), Times.Never());
        //there was a try to get data from cache
        _fsStockDataWrapper.fsStockRequestRepositoryMock.Verify(m => m.TryGetFromCacheAsync(It.IsAny<FsStockRequest>(), It.IsAny<bool>()), Times.Once());
    }
}
