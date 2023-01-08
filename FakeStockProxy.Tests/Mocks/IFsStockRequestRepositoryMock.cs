using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using FakeStockProxy.UnitTests.Fixtures;
using FakeStockProxy.Core.Interfaces.Repositories;
using Moq;

namespace FakeStockProxy.UnitTests.Mocks;

public class IFsStockRequestRepositoryMock : Mock<IFsStockRequestRepository>
{
    public List<FsStockRequest> FsStockRequests { get; init; }
    private SpecificationEvaluator specificationEvaluator = new SpecificationEvaluator();

    public IFsStockRequestRepositoryMock(FsTestDatasource fsStockRequestTestData)
    {
        FsStockRequests = fsStockRequestTestData.FsStockRequestsCached;

        Setup(m => m.AddAsync(It.IsAny<FsStockRequest>(), It.IsAny<CancellationToken>()))
            .Callback(() => { return; });

        Setup(m => m.UpdateChachedDataAsync(It.IsAny<CancellationToken>(), It.IsAny<FsStockRequest>(), It.IsAny<FsStockRequest>()))
            .Callback(() => { return; });

        Setup(m => m.TryGetFromCacheAsync(It.IsAny<FsStockRequest>(), It.IsAny<bool>()))
            .ReturnsAsync((FsStockRequest fsStockRequest, bool hasEFTracking) =>
            {
                return fsStockRequestTestData.FsStockRequestsCached.SingleOrDefault(sr => sr.Hashsum == fsStockRequest.Hashsum);
            });

    }
}
