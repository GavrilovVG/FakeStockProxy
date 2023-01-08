using AutoMapper;
using FakeStockProxy.Application.DataTransferObjects;
using FakeStockProxy.Core.Entities;

namespace FakeStockProxy.Application.Mapper;

// The best implementation of AutoMapper for class libraries - https://stackoverflow.com/questions/26458731/how-to-configure-auto-mapper-in-class-library-project
public class ObjectMapper
{
    private static readonly Lazy<IMapper> Lazy = new Lazy<IMapper>(() =>
    {
        var config = new MapperConfiguration(cfg =>
        {
            // This line ensures that internal properties are also mapped over.
            cfg.ShouldMapProperty = p => p.GetMethod!.IsPublic || p.GetMethod.IsAssembly;
            cfg.AddProfile<AspnetRunDtoMapper>();
        });
        var mapper = config.CreateMapper();
        return mapper;
    });
    public static IMapper Mapper => Lazy.Value;
    
    public class AspnetRunDtoMapper : Profile
    {
        public AspnetRunDtoMapper()
        {
            CreateMap<FsStock, FsStockDto>().ReverseMap();
            CreateMap<FsStockItem, FsStockItemDto>().ReverseMap();
            CreateMap<FsStockRequest, FsStockRequestDto>().ReverseMap();
            CreateMap<FsResponseDataDto, FsStockRequestDto>().ReverseMap();
            CreateMap<FsStockRequest, FsStockRequest>();
        }
    }
}
