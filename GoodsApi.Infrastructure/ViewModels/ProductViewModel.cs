namespace GoodsApi.Infrastructure.ViewModels;

public class ProductsViewModel
{
    public List<ProductViewModel> Products { get; set; } = [];
    public string? SearchQuery { get; set; } = string.Empty;
}

public class ProductViewModel
{
    public int Id { get; set; }
    
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Brand { get; set; } = string.Empty;
    
    public decimal MinimumRemain { get; set; }
}