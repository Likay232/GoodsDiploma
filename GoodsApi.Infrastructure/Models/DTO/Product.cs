namespace GoodsApi.Infrastructure.Models.DTO;

public class Product
{
    public int Id { get; set; }
    
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Brand { get; set; } = string.Empty;
    
    public decimal MinimumRemain { get; set; }
}