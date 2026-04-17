using System.Globalization;
using GoodsApi.Infrastructure.Models.Database;
using GoodsApi.Infrastructure.Models.Enums;
using GoodsApi.Infrastructure.Models.Requests;
using GoodsApi.Infrastructure.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace GoodsApi.Infrastructure.Services;

public class ManagerService(DataComponent component, CatalogService catalogService, DocumentGenerationService documentGenerationService)
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
            .Include(pi => pi.Product)
            .FirstOrDefault(pi => pi.Id == viewModel.ProductInfo.Id);

        if (productInfo is null) return false;
        
        productInfo.Amount -= newSaleOperation.Amount;
        await component.Update(productInfo);
        
        await component.Insert(newSaleOperation);

        var fieldValues = new List<string>();
        
        fieldValues.Add(newSaleOperation.Id.ToString());
        fieldValues.Add(newSaleOperation.SaleDate.Date.ToString("yyyy-MM-dd"));
        fieldValues.Add(productInfo.Product!.Name);
        fieldValues.Add(productInfo.Amount.ToString());
        fieldValues.Add(newSaleOperation.PricePerUnit.ToString(CultureInfo.InvariantCulture));
        
        decimal totalPrice = newSaleOperation.TotalPrice;
        int rubles = (int)Math.Floor(totalPrice);
        int pennies = (int)((totalPrice - rubles) * 100);
        
        fieldValues.Add(newSaleOperation.TotalPrice.ToString(CultureInfo.InvariantCulture));
        fieldValues.Add(rubles.ToString(CultureInfo.InvariantCulture));
        fieldValues.Add(pennies.ToString(CultureInfo.InvariantCulture));

        var userName = component.Users.First(u => u.Id == userId).Username;
        
        fieldValues.Add(userName);

        var fileName = await documentGenerationService.GenerateDoc(DocumentType.Check, fieldValues);

        if (!string.IsNullOrWhiteSpace(fileName))
        {
            newSaleOperation.PathToFile = fileName;
            await component.Update(newSaleOperation);
        }
        
        return true;
    }
}