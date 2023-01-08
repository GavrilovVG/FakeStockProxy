using Ardalis.Specification;
using FakeStockProxy.Core.Entities;

namespace AspnetRun.Core.Specifications;

public class FsStockRequestSpecification : Specification<FsStockRequest>, ISingleResultSpecification<FsStockRequest>
{
    public FsStockRequestSpecification(string Hashsum)
    {
        Query.Where(sr => sr.Hashsum == Hashsum)
            .Include(sr => sr.FsStock)
            .ThenInclude(s => s.FsStockItems)
            .OrderByDescending(sr => sr.LastUpdate)
            .AsNoTracking();
    }
}   
