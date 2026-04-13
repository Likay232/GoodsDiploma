using GoodsApi.Infrastructure.Models.Database;
using GoodsApi.Infrastructure.Models.Requests;
using GoodsApi.Infrastructure.ViewModels;

namespace GoodsApi.Infrastructure.Services;

public class ManagerService(DataComponent component, CatalogService catalogService)
{
    public async Task<SaleRegistrationViewModel> GetSaleRegistrationViewModel(int productInfoId)
    {
        var productInfo = await catalogService.GetProductInfo(productInfoId);
        var product = await catalogService.GetProduct(productInfo.ProductId);
        
        var viewModel = new SaleRegistrationViewModel()
        {
            ProductInfo = productInfo,
            Name = product.Name,
        };

        return viewModel;
    }
    
    public async Task<bool> RegisterSale(SaleRegistrationViewModel viewModel, string? userIdStr)
    {
        if (userIdStr is null) return false;

        var request = viewModel.Convert<SaleRegistrationViewModel, RegisterSale>();
        var newSaleOperation = request.Convert<RegisterSale, Models.Storage.SaleOperation>();
        
        var userId =  int.Parse(userIdStr);
        newSaleOperation.UserId = userId;

        var productInfo = component.ProductInfos
            .FirstOrDefault(pi => pi.Id == viewModel.ProductInfo.Id);

        if (productInfo is null) return false;
        
        productInfo.Amount -= newSaleOperation.Amount;
        await component.Update(productInfo);
        
        return await component.Insert(newSaleOperation);
    }
}