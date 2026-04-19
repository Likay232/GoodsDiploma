using GoodsApi.Infrastructure.Models.DTO;

namespace GoodsApi.Infrastructure.ViewModels;

public class ShipmentApprovalViewModel
{
    public List<ShipmentForApproval> ShipmentsForApproval { get; set; } = [];
}