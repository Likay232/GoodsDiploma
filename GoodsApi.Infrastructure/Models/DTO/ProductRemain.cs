namespace GoodsApi.Infrastructure.Models.DTO;

public class ProductRemain
{
    public int ProductId { get; set; }
    
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Brand { get; set; } = string.Empty;
    
    public decimal Size { get; set; }
    public string Color { get; set; } = string.Empty;
    
    public decimal MinimumRemain { get; set; }
    public decimal TotalRemain { get; set; }
}