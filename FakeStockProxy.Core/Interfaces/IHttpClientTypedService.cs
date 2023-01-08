namespace FakeStockProxy.Core.Interfaces;

public interface IHttpClientTypedService<T>
{
    Task<T?> GetJson(string queryString);
}
