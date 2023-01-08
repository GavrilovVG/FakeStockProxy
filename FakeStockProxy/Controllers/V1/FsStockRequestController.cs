using FakeStockProxy.Application.DataTransferObjects;
using FakeStockProxy.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FakeStockProxy.Web.Controllers.V1
{
    [Route("api/v{version:apiVersion}/[controller]/[action]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class FsStockRequestController : ControllerBase
    {
        private IFsStockRequestService _iFsStockRequestService;

        public FsStockRequestController(IFsStockRequestService iFsStockRequestService)
        {
            _iFsStockRequestService = iFsStockRequestService;
        }

        [Authorize]
        [HttpGet]
        public async Task<FsStockRequestDto?> GetStock([FromQuery] FsStockRequestDto fsStockRequestModel)
        {
            var result = await _iFsStockRequestService.GetStockAsync(fsStockRequestModel);

            return result;
        }
    }
}
