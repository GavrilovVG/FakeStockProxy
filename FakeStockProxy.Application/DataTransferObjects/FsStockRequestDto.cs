using FakeStockProxy.Application.Extensions;
using FakeStockProxy.Application.Miscellaneous.Enums;
using FakeStockProxy.Application.Miscellaneous.Json.Attributes;
using System.Text.Json.Serialization;

namespace FakeStockProxy.Application.DataTransferObjects;

public class FsStockRequestDto
{
    private string? hashsum;

    /// <summary>
    /// Hashsum based on properties marked with UserInputProperty attribute. Use FsStockRequestModel.SetHashsum() to set it
    /// </summary>
    [SwaggerJsonIgnore]
    [JsonIgnore]
    public string? Hashsum
    {
        get
        {
            if(string.IsNullOrEmpty(hashsum)) SetHashsum();
            return hashsum;
        }
        set => hashsum = value;
    }
    [UserInputProperty]
    public int? Skip { get; set; }
    [UserInputProperty]
    public int? Take { get; set; }
    [SwaggerJsonIgnore]
    [JsonIgnore]
    public string? Expand { get; set; }
    [UserInputProperty]
    public string? Filter { get; set; }
    [UserInputProperty]
    public string? OrderBy { get; set; }
    [UserInputProperty]
    public string? OrderDirection { get; set; }
    [SwaggerJsonIgnore]
    public DateTime LastUpdate { get; init; } = DateTime.Now;
    [SwaggerJsonIgnore]
    public StockRequestResultEnum StockRequestResultCode { get; set; }
    [SwaggerJsonIgnore]
    public string StockRequestResultName { get => StockRequestResultCode.ToString(); }
    [SwaggerJsonIgnore]
    public FsStockDto? FsStock { get; set; }
    /// <summary>
    /// Set hashsum based on properties marked with UserInputProperty attribute
    /// </summary>
    public void SetHashsum()
    {
        hashsum = this.GetHashsum<UserInputProperty>();
    }

    #region System.Object overrides

    public override int GetHashCode()
    {
        return hashsum!.GetHashCode();
    }

    public override bool Equals(object? obj)
    {
        if (obj == null) return false;
        if (obj.GetType() != GetType()) return false;
        if (ReferenceEquals(this, obj)) return true;

        return Equals((FsStockRequestDto)obj);
    }

    public bool Equals(FsStockRequestDto fsStockRequestDto)
    {
        return fsStockRequestDto.Hashsum == Hashsum;
    }

    public static bool operator ==(FsStockRequestDto a, FsStockRequestDto b)
    {
        if (ReferenceEquals(a, b))
        {
            return true;
        }

        if (((object)a == null) || ((object)b == null))
        {
            return false;
        }

        return a.Hashsum == b.Hashsum;
    }

    public static bool operator !=(FsStockRequestDto a, FsStockRequestDto b)
    {
        return !(a == b);
    }

    #endregion
}
