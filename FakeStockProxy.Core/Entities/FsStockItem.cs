using FakeStockProxy.Core.Entities.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace FakeStockProxy.Core.Entities;

public class FsStockItem : Entity
{
    public string? Code { get; set; }
    public string? Title { get; set; }
    public string? Manufacturer { get; set; }
    public string? Description { get; set; }
    public string? Price { get; set; }
    public int Stock { get; set; }
    public Guid FsStockId { get; set; }
    public FsStock? FsStock { get; set; }
}
