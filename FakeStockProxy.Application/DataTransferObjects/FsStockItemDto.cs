using System.Text.Json.Serialization;

namespace FakeStockProxy.Application.DataTransferObjects;

public class FsStockItemDto
{
    [JsonPropertyName("code")]
    public string? Code { get; set; }
    [JsonPropertyName("title")]
    public string? Title { get; set; }
    [JsonPropertyName("manufacturer")]
    public string? Manufacturer { get; set; }
    [JsonPropertyName("description")]
    public string? Description { get; set; }
    [JsonPropertyName("price")]
    public string? Price { get; set; }
    [JsonPropertyName("stock")]
    public int Stock { get; set; }
    [JsonIgnore]
    public Guid FsStockId { get; set; }
    [JsonIgnore]
    public FsStockDto? FsStock { get; set; }
}
