namespace GoodsApi.Infrastructure.Models.DTO;

public class SupplyOperation
{
    public int ProductInfoId { get; set; }
    public int ProviderId { get; set; }
    public int UserId { get; set; }
    
    public int Amount { get; set; }
    public decimal PurchasePrice { get; set; }
    public DateTime AcceptanceDate { get; set; }
}