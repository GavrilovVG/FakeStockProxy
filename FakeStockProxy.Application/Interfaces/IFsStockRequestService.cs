using FakeStockProxy.Application.DataTransferObjects;

namespace FakeStockProxy.Application.Interfaces;

public interface IFsStockRequestService
{
    /// <summary>
    /// Get stock data from service or cache when service is down
    /// </summary>
    /// <param name="fsStockRequestModel">Input data from request</param>
    /// <returns></returns>
    Task<FsStockRequestDto?> GetStockAsync(FsStockRequestDto fsStockRequestModel);
}
