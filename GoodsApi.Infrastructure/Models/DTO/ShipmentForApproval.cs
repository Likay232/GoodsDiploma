namespace GoodsApi.Infrastructure.Models.DTO;

public class ShipmentForApproval
{
    public int SaleOperationId { get; set; }
    public string Article { get; set; } = string.Empty;
    public decimal Size { get; set; }
    public string Color { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;

    public bool IsShipped { get; set; }

    public int Amount { get; set; }
}