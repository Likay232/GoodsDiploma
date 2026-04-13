using GoodsApi.Infrastructure.Models.DTO;

namespace GoodsApi.Infrastructure.ViewModels;

public class ProductInfosViewModel
{
    public Product Product { get; set; } = new();

    public List<ProductInfoCardViewModel> ProductInfos { get; set; } = new();
}

public class ProductInfoCardViewModel
{
    public int Id { get; set; }

    public string Article { get; set; } = string.Empty;
    public decimal Price { get; set; }

    public decimal Size { get; set; }
    public string Color { get; set; } = string.Empty;

    public int Amount { get; set; }
    public string Location { get; set; } = string.Empty;
}