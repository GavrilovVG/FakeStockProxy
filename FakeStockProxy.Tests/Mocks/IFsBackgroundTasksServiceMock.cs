using FakeStockProxy.Application.Interfaces;

namespace FakeStockProxy.UnitTests.Mocks;

public class IFsBackgroundTasksServiceMock : Mock<IFsBackgroundTasksService>
{
    public IFsBackgroundTasksServiceMock()
    {
        Setup(m => m.AddOrUpdateCacheAsync(It.IsAny<FsStockRequestDto>()))
            .Returns(Task.CompletedTask);
    }
}
