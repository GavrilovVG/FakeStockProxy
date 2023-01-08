using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace FakeStockProxy.UnitTests.Mocks;

public class IServiceProviderMock : Mock<IServiceProvider>
{
    public IServiceProviderMock(params (Type @interface, object service)[] services)
    {
        var scopedServiceProvider = new Mock<IServiceProvider>();

        foreach (var (@interface, service) in services)
        {
            scopedServiceProvider.Setup(s => s.GetService(@interface)).Returns(service);
        }

        var scope = new Mock<IServiceScope>();
        scope.SetupGet(s => s.ServiceProvider)
            .Returns(scopedServiceProvider.Object);

        var serviceScopeFactory = new Mock<IServiceScopeFactory>();
        serviceScopeFactory.Setup(s => s.CreateScope()).Returns(scope.Object);

        Setup(s => s.GetService(typeof(IServiceScopeFactory))).Returns(serviceScopeFactory.Object);
    }
}
