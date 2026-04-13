using GoodsApi.Infrastructure.Models.DTO;

namespace GoodsApi.Infrastructure.ViewModels;

public class SaleOperationsViewModel
{
    public List<ShipmentForApproval> ShipmentsForApproval { get; set; } = [];
}