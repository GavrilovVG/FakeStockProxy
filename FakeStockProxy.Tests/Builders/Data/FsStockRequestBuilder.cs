using FakeStockProxy.Application.Mapper;
using FakeStockProxy.Core.Entities;

namespace FakeStockProxy.UnitTests.Builders.Data;

internal class FsStockRequestBuilder
{
    public static List<FsStockRequest> BuildList(int itemsCount)
    {
        var dtos = new FsStockRequestDtoBuilder().BuildList(itemsCount);

        return BuildList(dtos);
    }

    public static List<FsStockRequest> BuildList(List<FsStockRequestDto> FsStockRequestDtos)
    {
        List<FsStockRequest> retval = new();
        FsStockRequestDtoBuilder fsStockRequestDtoBuilder = new();

        Parallel.ForEach(FsStockRequestDtos, (item) =>
        {
            FsStockRequest fsStockRequest = ObjectMapper.Mapper.Map<FsStockRequest>(item);
            fsStockRequest.Id = Guid.NewGuid();
            fsStockRequest.FsStock = ObjectMapper.Mapper.Map<FsStock>(fsStockRequestDtoBuilder.DataExampleResponseSuccess.FsStock);
            fsStockRequest.FsStock.Id = Guid.NewGuid();
            fsStockRequest.FsStock.FsStockRequestId = fsStockRequest.Id;
            fsStockRequest.FsStock.FsStockRequest = fsStockRequest;
            fsStockRequest.FsStock.FsStockItems = new List<FsStockItem>();

            Parallel.ForEach(fsStockRequestDtoBuilder.DataExampleResponseSuccess.FsStock!.FsStockItems!, (stockItemDto) =>
            {
                FsStockItem fsStockItem = ObjectMapper.Mapper.Map<FsStockItem>(stockItemDto);
                fsStockItem.Id = Guid.NewGuid();
                fsStockItem.FsStockId = fsStockRequest.FsStock.Id;
                fsStockItem.FsStock = fsStockRequest.FsStock;

                fsStockRequest.FsStock!.FsStockItems.Add(fsStockItem);
            });

            retval.Add(fsStockRequest);
        });

        return retval;
    }
}
