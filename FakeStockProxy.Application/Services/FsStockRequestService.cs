using FakeStockProxy.Application.DataTransferObjects;
using FakeStockProxy.Application.Interfaces;
using FakeStockProxy.Application.Miscellaneous.Enums;
using FakeStockProxy.Core.Entities;
using FakeStockProxy.Core.Interfaces;
using FakeStockProxy.Core.Interfaces.Repositories;
using System.Web;

namespace FakeStockProxy.Application.Services;

public class FsStockRequestService : IFsStockRequestService
{
    private readonly IHttpClientTypedService<FsResponseDataDto> _httpClientStockRequestService;
    private readonly IFsStockRequestRepository _fsStockRequestRepository;
    private readonly IAppLogger<FsStockRequestService> _logger;
    private readonly IFsBackgroundTasksService _fsBackgroundTasksService;

    public FsStockRequestService(IHttpClientTypedService<FsResponseDataDto> httpClientStockRequestService,
        IFsStockRequestRepository fsStockRequestRepository, IAppLogger<FsStockRequestService> logger, 
        IFsBackgroundTasksService fsBackgroundTasksService)
    {
        _httpClientStockRequestService = httpClientStockRequestService ?? throw new ArgumentNullException(nameof(httpClientStockRequestService));
        _fsStockRequestRepository = fsStockRequestRepository ?? throw new ArgumentNullException(nameof(fsStockRequestRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _fsBackgroundTasksService = fsBackgroundTasksService ?? throw new ArgumentNullException(nameof(fsBackgroundTasksService));
    }

    /// <inheritdoc/>
    public async Task<FsStockRequestDto?> GetStockAsync(FsStockRequestDto fsStockRequestModel)
    {
        var queryString = GetQueryString(fsStockRequestModel);
        _logger.LogInformation("[FsStockRequestService].GetStockAsync: Querystring parsed - {queryString}", queryString);

        var fsResponseData = await _httpClientStockRequestService.GetJson(queryString);
        
        //if the service has successfuly returned stock data, update cache and return results
        if(IsRemoteServiceResponseValid(fsResponseData))
        {
            Mapper.ObjectMapper.Mapper.Map(fsResponseData, fsStockRequestModel);
            fsStockRequestModel.StockRequestResultCode = StockRequestResultEnum.Success;
            await _fsBackgroundTasksService.AddOrUpdateCacheAsync(fsStockRequestModel);

            return fsStockRequestModel;
        }

        //try to return data from cache or return failure status to caller
        var fsStockRequestFromCache = await _fsStockRequestRepository
            .TryGetFromCacheAsync(Mapper.ObjectMapper.Mapper.Map<FsStockRequest>(fsStockRequestModel), false);

        if (fsStockRequestFromCache is not null)
        {
            //return cached data
            Mapper.ObjectMapper.Mapper.Map(fsStockRequestFromCache, fsStockRequestModel);
            fsStockRequestModel.StockRequestResultCode = StockRequestResultEnum.FailureCachedData;

            return fsStockRequestModel;
        }
        else
        {
            fsStockRequestModel.StockRequestResultCode = StockRequestResultEnum.FailureNoData;
            return fsStockRequestModel;
        }
    }

    /// <summary>
    /// Validates remote service response
    /// </summary>
    /// <param name="fsStockRequestModel">Deserialized object recieved in remote service response</param>
    /// <returns>True if response is valid</returns>
    private bool IsRemoteServiceResponseValid(FsResponseDataDto? fsResponseData)
    {
        if (fsResponseData is not null)
        {
            Guid _guid;
            Guid.TryParse(fsResponseData.RequestId, out _guid);

            //the service returns RequestId when it works ordinally
            if (_guid != Guid.Empty)
            {
                return true;
            }
        }

        return false;
    }

    private string GetQueryString(FsStockRequestDto fsStockRequestModel)
    {
        var query = HttpUtility.ParseQueryString(string.Empty);

        if (fsStockRequestModel.Skip is not null) query["Skip"] = fsStockRequestModel.Skip.ToString();
        if (fsStockRequestModel.Take is not null) query["Take"] = fsStockRequestModel.Take.ToString();
        if (fsStockRequestModel.Filter is not null) query["Filter"] = fsStockRequestModel.Filter;
        if (fsStockRequestModel.OrderBy is not null) query["OrderBy"] = fsStockRequestModel.OrderBy;
        if (fsStockRequestModel.OrderDirection is not null) query["OrderDirection"] = fsStockRequestModel.OrderDirection;
        string queryString = "?" + query.ToString();

        return queryString;
    }
}
