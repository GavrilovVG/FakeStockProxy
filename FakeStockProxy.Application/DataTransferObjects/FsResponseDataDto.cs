using System.Text.Json.Serialization;

namespace FakeStockProxy.Application.DataTransferObjects;

public class FsResponseDataDto 
{
    [JsonPropertyName("status")]
    public string? Status { get; set; }
    [JsonPropertyName("requestId")]
    public string? RequestId { get; set; }
    [JsonPropertyName("result")]
    public FsStockDto? FsStock { get; set; }
}
