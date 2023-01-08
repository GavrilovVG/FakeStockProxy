using FakeStockProxy.Application.DataTransferObjects;

namespace FakeStockProxy.Application.Interfaces;

/// <summary>
/// Executes tasks in background thread based on fire-and-forget principle
/// </summary>
public interface IFsBackgroundTasksService
{
    /// <summary>
    /// Tries to find FsStockRequest by hashsum and if it's exists updates it or inserts new if it's not in a background thread
    /// </summary>
    /// <param name="fsStockRequestModel">Data recieved from the remote service</param>
    Task AddOrUpdateCacheAsync(FsStockRequestDto fsStockRequestModel);
    /// <summary>
    /// Tries to find FsStockRequest by hashsum and if it's exists updates it or inserts new if it's not in a background thread
    /// </summary>
    /// <param name="fsStockRequestModel">Data recieved from the remote service</param>
    Task AddOrUpdateCacheAsync(CancellationToken cancellationToken, FsStockRequestDto fsStockRequestModel);
}
