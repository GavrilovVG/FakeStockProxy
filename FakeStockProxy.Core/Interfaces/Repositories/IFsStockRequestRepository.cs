using Ardalis.Specification;
using FakeStockProxy.Core.Entities;

namespace FakeStockProxy.Core.Interfaces.Repositories;

public interface IFsStockRequestRepository : IRepositoryBase<FsStockRequest>
{
    /// <summary>
    /// Updates cached data in a background thread (fire and forget)
    /// </summary>
    /// <param name="fsStockRequestChached">Cached data from database</param>
    /// <param name="fsStockRequestSource">Current version of data from the remote service</param>
    Task UpdateChachedDataAsync(CancellationToken token, FsStockRequest fsStockRequestChached, FsStockRequest fsStockRequestSource);

    /// <summary>
    /// Returns the only element of a sequence, or a default value if the sequence is empty; 
    /// this method throws an exception if there is more than one element in the sequence.
    /// </summary>
    /// <param name="fsStockRequest">FsStockRequest instanse with calculated Hashsum property</param>
    /// <param name="hasEFTracking">If false then AsNoTracking() called in specification instance</param>
    /// <returns>A task that represents the asynchronous operation.
    /// The task result contains the FsStockRequest.</returns>
    Task<FsStockRequest?> TryGetFromCacheAsync(FsStockRequest fsStockRequest, bool hasEFTracking = true);
}
