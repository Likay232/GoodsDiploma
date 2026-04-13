using AutoMapper;

namespace GoodsApi.Infrastructure.AutoMapperProfiles;

public class ModelsMappingProfile : Profile
{
    public ModelsMappingProfile()
    {
        CreateMap<Models.DTO.ProductInfo, Models.Storage.ProductInfo>().ReverseMap();
        CreateMap<Models.DTO.SupplyOperation, Models.Storage.SupplyOperation>().ReverseMap();
        CreateMap<Models.DTO.Product, Models.Storage.Product>().ReverseMap();
        CreateMap<Models.DTO.User, Models.Storage.User>().ReverseMap();
        CreateMap<Models.DTO.Provider, Models.Storage.Provider>().ReverseMap();
        CreateMap<ViewModels.SaleRegistrationViewModel, Models.Requests.RegisterSale>()
            .ForMember(dest => dest.ProductInfoId, opt => 
                opt.MapFrom(src => src.ProductInfo.Id))
            .ForMember(dest => dest.SaleDate, opt => 
                opt.MapFrom(src => src.SaleDate.ToUniversalTime()))
            .ReverseMap();

        CreateMap<Models.Requests.RegisterSale, Models.Storage.SaleOperation>().ReverseMap();
        CreateMap<Models.Requests.RegisterWriteOff, Models.Storage.WriteOffOperation>().ReverseMap();
        
        CreateMap<ViewModels.UserEditViewModel, Models.DTO.User>().ReverseMap();
        CreateMap<ViewModels.RoleItemViewModel, Models.DTO.Role>().ReverseMap();
        CreateMap<ViewModels.ProductViewModel, Models.DTO.Product>().ReverseMap();
        CreateMap<ViewModels.ProductInfoEditViewModel, Models.DTO.ProductInfo>().ReverseMap();
        CreateMap<ViewModels.ProductEditViewModel, Models.DTO.Product>().ReverseMap();
        CreateMap<ViewModels.ProviderEditViewModel, Models.DTO.Provider>().ReverseMap();
        CreateMap<ViewModels.ProviderViewModel, Models.DTO.Provider>().ReverseMap();
        CreateMap<ViewModels.WriteOffRegistrationViewModel, Models.Storage.WriteOffOperation>()
            .ForMember(dest => dest.ProductInfo, opt => opt.Ignore())
            .ForMember(dest => dest.WriteOffDate, opt => 
                opt.MapFrom(src => src.WriteOffDate.ToUniversalTime()))
            .ReverseMap();
        CreateMap<ViewModels.InventorizationRegistrationViewModel, Models.Storage.InventoryOperation>()
            .ForMember(dest => dest.ProductInfo, opt => opt.Ignore())
            .ForMember(dest => dest.InventoryOperationDate, opt => 
                opt.MapFrom(src => src.InventoryOperationDate.ToUniversalTime()))
            .ReverseMap();
        
        CreateMap<ViewModels.ProductViewModel, Models.DTO.Product>().ReverseMap(); 
        CreateMap<ViewModels.SupplyRegistrationViewModel, Models.DTO.SupplyOperation>()
            .ForMember(dest => dest.AcceptanceDate, opt => 
                opt.MapFrom(src => src.AcceptanceDate.ToUniversalTime()))
            .ReverseMap();

        CreateMap<Models.Storage.ProductMovement, Models.DTO.ProductMotionReportEntry>()
            .ForMember(dest => dest.OperationType, opt => 
                opt.MapFrom(src => MapOperationType(src.OperationType)));

        CreateMap<Models.Storage.SaleOperation, Models.DTO.SalesReportEntry>()
            .ForMember(dest => dest.SoldAmount, opt => opt.MapFrom(src => src.Amount))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => 
                src.ProductInfo != null && src.ProductInfo.Product != null 
                    ? src.ProductInfo.Product.Name 
                    : ""))
            .ForMember(dest => dest.Article, opt => opt.MapFrom(src => 
                src.ProductInfo != null 
                    ? src.ProductInfo.Article 
                    : ""))
            .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.SaleDate.ToLocalTime()))
            .ForMember(dest => dest.Size, opt => opt.MapFrom(src => 
                src.ProductInfo != null 
                    ? src.ProductInfo.Size 
                    : 0))
            .ForMember(dest => dest.ManagerName, opt => opt.MapFrom(src => 
                src.User != null 
                    ? src.User.Username 
                    : ""));
        CreateMap<Models.DTO.SalesReportEntry, Models.DTO.SaleReportChartData>();
    }

    private string MapOperationType(string operationType)
    {
        return operationType switch
        {
            "Supply" => "Поставка",
            "Sale" => "Продажа",
            "WriteOff" => "Списание",
            "Inventory" => "Инвентаризация",
            _ => "Неизвестно"
        };
    }
}