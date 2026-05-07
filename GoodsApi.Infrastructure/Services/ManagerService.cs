using System.Globalization;
using GoodsApi.Infrastructure.Models;
using GoodsApi.Infrastructure.Models.Database;
using GoodsApi.Infrastructure.Models.DTO;
using GoodsApi.Infrastructure.Models.Enums;
using GoodsApi.Infrastructure.Models.Requests;
using GoodsApi.Infrastructure.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace GoodsApi.Infrastructure.Services;

public class ManagerService(DataComponent component, CatalogService catalogService, DocumentGenerationService documentGenerationService, INotificationService notificationService)
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

        var userId = int.Parse(userIdStr);
        newSaleOperation.UserId = userId;

        var productInfo = component.ProductInfos
            .Include(pi => pi.Product)
            .FirstOrDefault(pi => pi.Id == viewModel.ProductInfo.Id);

        if (productInfo is null) return false;

        productInfo.Amount -= newSaleOperation.Amount;

        if (productInfo.Amount < productInfo.Product?.MinimumRemain)
            await notificationService.CreateAdminLowRemainsNotifications(productInfo.Article);

        await component.Update(productInfo);

        await component.Insert(newSaleOperation);

        decimal totalPrice = newSaleOperation.TotalPrice;
        int rubles = (int)Math.Floor(totalPrice);
        int pennies = (int)((totalPrice - rubles) * 100);
        var userName = component.Users.First(u => u.Id == userId).Username;

        var fieldDictionary = new Dictionary<string, string>()
        {
            { "check-", newSaleOperation.Id.ToString() },
            { "date", newSaleOperation.SaleDate.Date.ToString("yyyy-MM-dd") },
            { "product-name", productInfo.Product!.Name },
            { "amount", request.Amount.ToString() },
            { "price-for-unit", newSaleOperation.PricePerUnit.ToString(CultureInfo.InvariantCulture) },
            { "total-sum", totalPrice.ToString(CultureInfo.InvariantCulture) },
            { "rubles", rubles.ToString(CultureInfo.InvariantCulture) },
            { "pennies", pennies.ToString(CultureInfo.InvariantCulture) },
            { "manager-name", userName },
        };

        var fileName = await documentGenerationService.GenerateDoc(DocumentType.Check, fieldDictionary);

        if (!string.IsNullOrWhiteSpace(fileName))
        {
            newSaleOperation.PathToFile = fileName;
            await component.Update(newSaleOperation);
        }
        
        return true;
    }

    public async Task<SaleOperationsViewModel> GetSaleOperationsViewModel()
    {
        var sales = await component.SaleOperations
            .Include(s => s.RefundOperations)
            .Include(s => s.ProductInfo)
            .Include(s => s.User)
            .Where(s => s.ProductInfo != null && s.User != null)
            .Select(s => new SaleOperation()
            {
                Id = s.Id,
                ManagerName = s.User!.Username,
                Amount = s.Amount,
                PricePerUnit = s.PricePerUnit,
                RefundedAmount = s.RefundOperations != null ? s.RefundOperations.Sum(r => r.Amount) : 0,
                SaleDate = s.SaleDate,
                PathToFile = s.PathToFile,
                ProductInfo = new ProductInfo
                {
                    Id = s.ProductInfoId,
                    ProductId = s.ProductInfo!.ProductId,
                    Article = s.ProductInfo.Article,
                    Price = s.ProductInfo.Price,
                    Size = s.ProductInfo.Size,
                    Color = s.ProductInfo.Color,
                    Amount = s.ProductInfo.Amount,
                    Location = s.ProductInfo.Location,
                }
            })
            .ToListAsync();

        var viewModel = new SaleOperationsViewModel()
        {
            SaleOperations = sales
                .Where(so => so.Amount > so.RefundedAmount || (!string.IsNullOrEmpty(so.PathToFile) && File.Exists(so.PathToFile)))
                .ToList()

        };

        return viewModel;
    }
    
    public async Task<RefundViewModel> GetRefundViewModel(int saleOperationId)
    {
        var saleOperation = await component.SaleOperations
            .FirstAsync(s => s.Id == saleOperationId);
        
        var refundedAmountForSaleOperation = component.RefundOperations
            .Where(r => r.SaleOperationId == saleOperationId)
            .Sum(r => r.Amount);

        var viewModel = new RefundViewModel()
        {
            SaleOperationId = saleOperationId,
            SoldAmount = saleOperation.Amount,
            RefundedAmount = refundedAmountForSaleOperation,
        };

        return viewModel;
    }
    
    public async Task<bool> RegisterRefundOperation(RefundViewModel viewModel, string userIdStr)
    {
        var userId = int.TryParse(userIdStr, out int result) ? result : 0;
        
        if (!component.Users.Any(u => u.Id == userId)) return false;
        
        var saleOperation = component.SaleOperations
            .Include(s => s.ProductInfo)
            .FirstOrDefault(s => s.Id == viewModel.SaleOperationId);
        
        if (saleOperation is null) return false;
     
        var refundedAmountForSaleOperation = component.RefundOperations
            .Where(r => r.SaleOperationId == viewModel.SaleOperationId)
            .Sum(r => r.Amount);

        if (refundedAmountForSaleOperation + viewModel.Amount > saleOperation.Amount) return false;

        var productInfo = (await catalogService.GetProductInfo(saleOperation.ProductInfoId))
            .Convert<ProductInfo, Models.Storage.ProductInfo>();
        
        productInfo.Amount += viewModel.Amount;
        
        var refundOperation = viewModel
            .Convert<RefundViewModel, Models.Storage.RefundOperation>();
        refundOperation.UserId = userId;

        var tasks = new List<Task> { component.Update(productInfo), component.Insert(refundOperation) };
        
        await Task.WhenAll(tasks);
        
        return true;
    }
}