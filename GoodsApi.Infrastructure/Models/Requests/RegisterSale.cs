namespace GoodsApi.Infrastructure.Models.Requests;

public class RegisterSale
{
    public int ProductInfoId { get; set; }
    
    public int Amount { get; set; }
    public decimal PricePerUnit { get; set; }
    public DateTime SaleDate { get; set; }
}