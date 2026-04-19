using GoodsApi.Infrastructure.Models.Database;
using GoodsApi.Infrastructure.Models.DTO;
using GoodsApi.Infrastructure.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace GoodsApi.Infrastructure.Services;

public class StorekeeperService(DataComponent component, CatalogService catalogService)
{
    public async Task<bool> ConfirmShipmentForSaleOperation(int saleOperationId)
    {
        var saleOperation = component.SaleOperations
            .FirstOrDefault(x => x.Id == saleOperationId);

        if (saleOperation == null) throw new Exception();

        saleOperation.IsShipped = true;
        return await component.Update(saleOperation);
    }

    public async Task<bool> RegisterWriteOffOperation(WriteOffRegistrationViewModel viewModel, string? userIdStr)
    {
        if (userIdStr is null) return false;

        var newWriteOffOperation = viewModel.Convert<WriteOffRegistrationViewModel, Models.Storage.WriteOffOperation>();
        var productInfo = viewModel.ProductInfo.Convert<ProductInfo, Models.Storage.ProductInfo>();

        var userId = int.Parse(userIdStr);
        newWriteOffOperation.UserId = userId;

        productInfo.Amount -= viewModel.Amount;
        await component.Update(productInfo);

        return await component.Insert(newWriteOffOperation);
    }

    public async Task<ShipmentApprovalViewModel> GetSaleOperationsViewModel()
    {
        var shipmentsForApproval = component.SaleOperations
            .Include(operation => operation.ProductInfo)
            .Where(operation => operation.ProductInfo != null)
            .OrderBy(x => x.IsShipped)
            .ThenBy(x => x.Id)
            .Select(operation => new ShipmentForApproval()
            {
                Amount = operation.Amount,
                IsShipped = operation.IsShipped,
                SaleOperationId = operation.Id,
                Article = operation.ProductInfo!.Article,
                Color = operation.ProductInfo.Color,
                Location = operation.ProductInfo.Location,
                Size = operation.ProductInfo.Size,
            })
            .ToList();

        return new ShipmentApprovalViewModel
        {
            ShipmentsForApproval = shipmentsForApproval
        };
    }

    public async Task<WriteOffRegistrationViewModel> GetWriteOffRegistrationViewModel(int productInfoId)
    {
        var productInfo = await catalogService.GetProductInfo(productInfoId);

        return new WriteOffRegistrationViewModel
        {
            ProductInfo = productInfo,
        };
    }

    public async Task<InventorizationRegistrationViewModel> GetInventorizationViewModel(int productInfoId)
    {
        var productInfo = await catalogService.GetProductInfo(productInfoId);

        return new InventorizationRegistrationViewModel()
        {
            ProductInfo = productInfo,
        };
    }

    public async Task<bool> RegisterInventorizationOperation(InventorizationRegistrationViewModel viewModel, string? userIdStr)
    {
        if (userIdStr is null) return false;

        var newInventoryOperation = viewModel.Convert<InventorizationRegistrationViewModel, Models.Storage.InventoryOperation>();
        var productInfo = viewModel.ProductInfo.Convert<ProductInfo, Models.Storage.ProductInfo>();

        var userId = int.Parse(userIdStr);
        newInventoryOperation.UserId = userId;

        productInfo.Amount = viewModel.FactAmount;
        await component.Update(productInfo);

        return await component.Insert(newInventoryOperation);
    }

    public async Task<SupplyRegistrationViewModel> GetSupplyRegistrationViewModel(int productInfoId, string userIdStr)
    {
        var providers = await catalogService.GetProviders();
        var productInfo = await catalogService.GetProductInfo(productInfoId);

        int.TryParse(userIdStr, out int userId);
        
        var viewModel = new SupplyRegistrationViewModel
        {
            UserId = userId,
            ProductInfo = productInfo,
            Providers = providers.Select(p => new SelectListItem()
                {
                    Value = p.Id.ToString(),
                    Text = p.Name
                })
                .ToList()
        };

        return viewModel;
    }

    public async Task<bool> RegisterSupplyOperation(SupplyRegistrationViewModel viewModel)
    {
        var converted = viewModel
            .Convert<SupplyRegistrationViewModel, SupplyOperation>()
            .Convert<SupplyOperation, Models.Storage.SupplyOperation>();

        var productInfo = viewModel.ProductInfo.Convert<ProductInfo, Models.Storage.ProductInfo>();
        
        productInfo.Amount += viewModel.Amount;
        await component.Update(productInfo);
        
        return await component.Insert(converted);
    }
}