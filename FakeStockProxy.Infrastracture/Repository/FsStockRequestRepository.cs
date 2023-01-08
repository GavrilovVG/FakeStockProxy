using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using AspnetRun.Core.Specifications;
using FakeStockProxy.Core.Entities;
using FakeStockProxy.Core.Interfaces;
using FakeStockProxy.Core.Interfaces.Repositories;
using FakeStockProxy.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FakeStockProxy.Infrastructure.Repository;

public class FsStockRequestRepository : RepositoryBase<FsStockRequest>, IFsStockRequestRepository
{
    private readonly FakeStockProxyContext dbContext;
    private readonly IAppLogger<FsStockRequestRepository> _logger;

    public FsStockRequestRepository(FakeStockProxyContext dbContext, IAppLogger<FsStockRequestRepository> logger) : base(dbContext)
    {
        this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc/>
    public async Task<FsStockRequest?> TryGetFromCacheAsync(FsStockRequest fsStockRequest, bool hasEFTracking = true)
    {
        if (fsStockRequest is null) throw new ArgumentNullException(nameof(fsStockRequest));
        if (string.IsNullOrEmpty(fsStockRequest.Hashsum)) throw new ArgumentException(nameof(fsStockRequest.Hashsum));

        var spec = new FsStockRequestSpecification(fsStockRequest.Hashsum);

        if (!hasEFTracking)
        {
            spec.Query.AsNoTracking();
        }

        return await SingleOrDefaultAsync(spec);
    }

    /// <inheritdoc/>
    public async Task UpdateChachedDataAsync(CancellationToken token, FsStockRequest fsStockRequestChached, FsStockRequest fsStockRequestSource)
    {
        if (token.IsCancellationRequested) return;

        dbContext.Attach(fsStockRequestChached).State = EntityState.Modified;

        dbContext.FsStocks.Remove(fsStockRequestChached.FsStock!);
        
        fsStockRequestChached.FsStock = fsStockRequestSource.FsStock;
        fsStockRequestChached.LastUpdate = fsStockRequestSource.LastUpdate;

        await dbContext.SaveChangesAsync(token);

        _logger.LogInformation("FsStockRequestRepository.UpdateChachedData: job is completed.");
    }

}
