using FakeStockProxy.Application.DataTransferObjects;
using FakeStockProxy.Core.Interfaces;
using System.Net.Http.Json;

namespace FakeStockProxy.Application.Services
{
    public class HttpClientStockRequestService : IHttpClientTypedService<FsResponseDataDto>
    {
        private readonly HttpClient _httpClient;
        private readonly IAppLogger<FsStockRequestService> _logger;

        public HttpClientStockRequestService(HttpClient httpClient, IAppLogger<FsStockRequestService> logger)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Get data from FakeStock service
        /// </summary>
        /// <param name="queryString">Get request input parameters</param>
        /// <returns></returns>
        public async Task<FsResponseDataDto?> GetJson(string queryString)
        {
            try
            {
                var response = await _httpClient.GetAsync(queryString);

                _logger.LogInformation("HttpClientStockRequestService.GetStock: service response recieved - {StatusCode}", response.StatusCode);

                if (response.IsSuccessStatusCode)
                {
                    var retval = await response.Content.ReadFromJsonAsync<FsResponseDataDto>();

                    return retval; 
                }
            }
            catch (Exception ex) 
            {
                _logger.LogError("HttpClientStockRequestService.GetStock: HttpRequestException caught - {message}", ex.Message);
            }

            return null;
        }

    }
}
