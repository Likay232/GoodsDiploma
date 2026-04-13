namespace GoodsApi.Infrastructure.Models.Storage;

public class ProductInfo : BaseEntity
{
    public int ProductId { get; set; }
    public decimal Size { get; set; }
    public string Color { get; set; } = string.Empty;
    public int Amount { get; set; }
    public string Location { get; set; } = string.Empty;
    
    public decimal Price { get; set; }
    public string Article { get; set; } = string.Empty;

    
    public Product? Product { get; set; }
}