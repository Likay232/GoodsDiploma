namespace GoodsApi.Infrastructure.Models.Storage;

public class Product : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Brand { get; set; } = string.Empty;
    
    public decimal MinimumRemain { get; set; }
    
    public virtual List<ProductInfo> ProductInfos { get; set; }
}