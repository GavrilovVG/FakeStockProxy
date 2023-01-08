using FakeStockProxy.UnitTests.Fixtures;
using FakeStockProxy.Core.Interfaces;
using Moq;

namespace FakeStockProxy.UnitTests.Mocks;

public class HttpClientStockRequestServiceMock : Mock<IHttpClientTypedService<FsResponseDataDto>>
{
    public HttpClientStockRequestServiceMock(FsResponseDataDto fsResponseDataDto)
    {
        Setup(m => m.GetJson(It.IsAny<string>()))
            .ReturnsAsync(fsResponseDataDto);
    }
}
