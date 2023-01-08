using FakeStockProxy.Core.Entities.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace FakeStockProxy.Core.Entities;

public class FsStock : Entity
{
    public int TotalItems { get; set; }
    public Guid FsStockRequestId { get; set; }
    public FsStockRequest? FsStockRequest { get; set; }
    public List<FsStockItem>? FsStockItems { get; set; }
}
