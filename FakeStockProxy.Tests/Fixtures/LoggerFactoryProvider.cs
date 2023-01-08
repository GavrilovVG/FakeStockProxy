using Microsoft.Extensions.Logging;

namespace FakeStockProxy.UnitTests.Fixtures;

public class LoggerFactoryProvider
{
    public static readonly ILoggerFactory LoggerFactoryInstance = Microsoft.Extensions.Logging.LoggerFactory.Create(builder =>
    {
        builder.AddFilter(l => l == LogLevel.Debug);
        builder.AddConsole();
    });
}
