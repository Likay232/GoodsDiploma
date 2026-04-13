using GoodsApi.Infrastructure.Models.DTO;

namespace GoodsApi.Infrastructure.ViewModels;

public class SaleRegistrationViewModel
{
    public ProductInfo ProductInfo { get; set; } = new();
    public string Name { get; set; } =  string.Empty;
    public int Amount { get; set; }
    public decimal PricePerUnit { get; set; }
    public DateTime SaleDate { get; set; }
}