using FakeStockProxy.UnitTests.Builders.Data;

namespace FakeStockProxy.UnitTests.Fixtures;

public sealed class FsTestDatasource
{
    private readonly List<FsStockRequest> fsStockRequestsCached;
    private readonly List<FsStockRequestDto> fsStockRequestDtosCached;
    public FsStockRequestDtoBuilder fsStockRequestDtoBuilder { get; init; }
    /// <summary>
    /// Predefined FsStockRequests list to emulate cached data
    /// </summary>
    public List<FsStockRequest> FsStockRequestsCached => fsStockRequestsCached;
    /// <summary>
    /// Predefined FsStockRequestDtos list to emulate requests for cached data
    /// </summary>
    public List<FsStockRequestDto> FsStockRequestDtosCached => fsStockRequestDtosCached;

    public FsTestDatasource(int fsStockRequestItemsCount)
    {
        fsStockRequestDtoBuilder = new FsStockRequestDtoBuilder();
        fsStockRequestDtosCached = fsStockRequestDtoBuilder.BuildList(fsStockRequestItemsCount);
        fsStockRequestsCached = FsStockRequestBuilder.BuildList(fsStockRequestDtosCached);
    }

    /// <summary>
    /// Get non-cached FsStockRequestDto
    /// </summary>
    /// <returns></returns>
    public FsStockRequestDto GetRandomFsStockRequestDtoNonCached()
    {
        var fsStockRequestDto = fsStockRequestDtoBuilder.Build();

        //check if generated data exists in cache
        if (FsStockRequestDtosCached.Any(sr => sr.Hashsum == fsStockRequestDto.Hashsum))
        {
            return GetRandomFsStockRequestDtoNonCached();
        }

        return fsStockRequestDto;
    }

    /// <summary>
    /// Get cached FsStockRequestDto
    /// </summary>
    /// <returns></returns>
    public FsStockRequestDto GetRandomFsStockRequestDtoCached()
    {
        Random random = new Random(FsStockRequestDtosCached.Count);

        return FsStockRequestDtosCached[random.Next(FsStockRequestDtosCached.Count - 1)];
    }
}
