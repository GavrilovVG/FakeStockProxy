using FakeStockProxy.Core.Entities;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace FakeStockProxy.Application.DataTransferObjects;

public class FsStockDto 
{
    [JsonPropertyName("totalItems")]
    public int TotalItems { get; set; }
    //[JsonIgnore]
    //public Guid FsStockRequestId { get; set; }
    //[JsonIgnore]
    //public FsStockRequest? FsStockRequest { get; set; }
    [JsonPropertyName("items")]
    public List<FsStockItemDto>? FsStockItems { get; set; }
}

