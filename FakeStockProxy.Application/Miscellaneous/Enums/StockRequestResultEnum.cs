namespace FakeStockProxy.Application.Miscellaneous.Enums;

public enum StockRequestResultEnum
{
    None,
    Success,
    /// <summary>
    /// Fake stock service connection failure and there is no data in cache
    /// </summary>
    FailureNoData,
    /// <summary>
    /// Fake stock service connection failure and data loaded from cache
    /// </summary>
    FailureCachedData
}
