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
        CreateMap<Models.Storage.SupplyOperation, Models.DTO.DeliveryReportEntry>()
            .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.AcceptanceDate.ToLocalTime()))
            .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => 
                src.ProductInfo != null && src.ProductInfo.Product != null 
                    ? src.ProductInfo.Product.Name 
                    : ""))
            .ForMember(dest => dest.Article, opt => opt.MapFrom(src => 
                src.ProductInfo != null 
                    ? src.ProductInfo.Article 
                    : ""))
            .ForMember(dest => dest.Size, opt => opt.MapFrom(src => 
                src.ProductInfo != null 
                    ? src.ProductInfo.Size 
                    : 0))
            .ForMember(dest => dest.Color, opt => opt.MapFrom(src => 
                src.ProductInfo != null 
                    ? src.ProductInfo.Color 
                    : ""))
            .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount))
            .ForMember(dest => dest.PurchasePrice, opt => opt.MapFrom(src => src.PurchasePrice))
            .ForMember(dest => dest.Provider, opt => opt.MapFrom(src => 
                src.Provider != null 
                    ? src.Provider.Name 
                    : ""))
            .ForMember(dest => dest.StorekeeperUsername, opt => opt.MapFrom(src => 
                src.User != null 
                    ? src.User.Username 
                    : ""));
        
        CreateMap<Models.Storage.ProductInfo, Models.DTO.RemainsReportEntry>()
            .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => 
                src.Product != null 
                    ? src.Product.Name 
                    : ""))
            .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount))
            .ForMember(dest => dest.Color, opt => opt.MapFrom(src => src.Color))
            .ForMember(dest => dest.Size, opt => opt.MapFrom(src => src.Size))
            .ForMember(dest => dest.Article, opt => opt.MapFrom(src => src.Article))
            .ForMember(dest => dest.MinimumRemain, opt => opt.MapFrom(src => 
                src.Product != null 
                    ? src.Product.MinimumRemain 
                    : 0));

        CreateMap<ViewModels.RefundViewModel, Models.Storage.RefundOperation>()
            .ForMember(dest => dest.RefundDate, opt => opt.MapFrom(src => src.RefundDate.ToUniversalTime()));

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