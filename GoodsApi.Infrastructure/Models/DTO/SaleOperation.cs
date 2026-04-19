namespace GoodsApi.Infrastructure.Models.DTO;

public class SaleOperation
{
    public ProductInfo ProductInfo { get; set; } = new();
    public int Id { get; set; }
    public string ManagerName { get; set; } = string.Empty;
    
    public int Amount { get; set; }
    public int RefundedAmount { get; set; }
    public decimal PricePerUnit { get; set; }
    public decimal TotalPrice => Amount * PricePerUnit;
    public DateTime SaleDate { get; set; }
    public string? PathToFile { get; set; } = string.Empty;

}