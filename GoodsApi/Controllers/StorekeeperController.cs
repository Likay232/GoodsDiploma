using System.Security.Claims;
using GoodsApi.Infrastructure.Services;
using GoodsApi.Infrastructure.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GoodsApi.Controllers;

[Route("[controller]/[action]")]
[Authorize(Roles = "Storekeeper")]
public class StorekeeperController(StorekeeperService storekeeperService, CatalogService catalogService) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        return RedirectToAction("Products", "Catalog");
    }
    
    [HttpGet]
    public async Task<IActionResult> SaleOperationsPage()
    {
        var saleOperationsViewModel = await storekeeperService.GetSaleOperationsViewModel();
        
        return View(saleOperationsViewModel);
    }

    [HttpPost]
    public async Task<IActionResult> ConfirmShipment(int saleOperationId)
    {
        await storekeeperService.ConfirmShipmentForSaleOperation(saleOperationId);
        
        return RedirectToAction(nameof(SaleOperationsPage));
    }

    [HttpGet]
    public async Task<IActionResult> InventorizationPage(int productInfoId)
    {
        var viewModel = await storekeeperService.GetInventorizationViewModel(productInfoId);
        
        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> RegisterInventorizationOperation(InventorizationRegistrationViewModel viewModel)
    {
        if (!ModelState.IsValid)
            return View("InventorizationPage", viewModel);
        
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        await storekeeperService.RegisterInventorizationOperation(viewModel, userId);
        
        return RedirectToAction("ProductInfos", "Catalog", new {productId = viewModel.ProductInfo.ProductId});
    }
    
    [HttpGet]
    public async Task<IActionResult> WriteOffRegistrationPage(int productInfoId)
    {
        var viewModel = await storekeeperService.GetWriteOffRegistrationViewModel(productInfoId);
        
        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> RegisterWriteOffOperation(WriteOffRegistrationViewModel viewModel)
    {
        if (!ModelState.IsValid)
            return View("WriteoffRegistrationPage", viewModel);
        
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        if (viewModel.Amount > viewModel.ProductInfo.Amount)
        {
            ModelState.AddModelError(nameof(viewModel.ProductInfo.Amount), "Недостаточно товара для списания.");
            return View("WriteOffRegistrationPage", viewModel);
        }
        
        await storekeeperService.RegisterWriteOffOperation(viewModel, userId);

        return RedirectToAction("ProductInfos", "Catalog", new {productId = viewModel.ProductInfo.ProductId});
    }

    [HttpGet]
    public async Task<IActionResult> SupplyRegistrationPage(int productInfoId, int productId)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        var viewModel = await storekeeperService.GetSupplyRegistrationViewModel(productInfoId, userId!);
        
        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> RegisterSupplyOperation(SupplyRegistrationViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            var providers = await catalogService.GetProviders();
            viewModel.Providers = providers.Select(p => new SelectListItem()
            {
                Value = p.Id.ToString(),
                Text = p.Name,
            }).ToList();
            
            return View("SupplyRegistrationPage", viewModel);
        }
        
        await storekeeperService.RegisterSupplyOperation(viewModel);
        
        return RedirectToAction("Products", "Catalog");
    }

}