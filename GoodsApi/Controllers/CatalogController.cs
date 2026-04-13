using GoodsApi.Infrastructure.Models.DTO;
using GoodsApi.Infrastructure.Services;
using GoodsApi.Infrastructure.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GoodsApi.Controllers;

[Authorize]
[Route("[controller]/[action]")]
public class CatalogController(CatalogService service) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Products(string? query = null)
    {
        var products = await service.GetProductsViewModel(query);
        
        return View(products);
    }       
    
    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> ProductEditPage(int productId = 0)
    {
        var  product = await service.GetProductViewModel(productId);
        return View(product);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> SaveProduct(ProductEditViewModel viewModel)
    {
        if (!ModelState.IsValid)
            return View("ProductEditPage", viewModel);
        
        await service.SaveProduct(viewModel);

        return RedirectToAction(nameof(Products));
    }

    [HttpPost]
    public async Task<IActionResult> DeleteProduct(int productId)
    {
        await service.DeleteProduct(productId);
        
        return RedirectToAction(nameof(Products));
    }
    
    [HttpGet]
    public async Task<IActionResult> ProductInfos(int productId)
    {
        var productInfos = await service.GetProductInfosViewModel(productId);
        
        return View(productInfos);
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> ProductInfoEditPage(int productInfoId = 0, int productId = 0)
    {
        var productInfo = await service.GetProductInfoEditViewModel(productInfoId, productId);
        return View(productInfo);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> SaveProductInfo(ProductInfoEditViewModel cardViewModel)
    {
        if (!ModelState.IsValid)
        {
            var products = await service.GetProducts();

            cardViewModel.Products = products.Select(p => new SelectListItem
            {
                Value = p.Id.ToString(),
                Text = p.Name
            }).ToList();

            return View("ProductInfoEditPage", cardViewModel);
        }

        await service.SaveProductInfo(cardViewModel);
        
        return RedirectToAction(nameof(ProductInfos), new { productId = cardViewModel.ProductId });
    }
    
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteProductInfo(int productInfoId, int productId)
    {
        await service.DeleteProductInfo(productInfoId);

        return RedirectToAction(nameof(ProductInfos),  new { productId });
    }

    [HttpGet]
    [Authorize(Roles = "Admin,Storekeeper")]
    public async Task<IActionResult> Providers()
    {
        var providers = await service.GetProvidersViewModel();
        return View(providers);
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> ProviderEditPage(int providerId)
    {
        var provider = await service.GetProviderEditViewModel(providerId);
        return View(provider);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> SaveProvider(ProviderEditViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            return View("ProviderEditPage", viewModel);
        }
        
        await service.SaveProvider(viewModel);
        
        return RedirectToAction(nameof(Providers));
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteProvider(int providerId)
    {
        await service.DeleteProvider(providerId);
        
        return RedirectToAction(nameof(Providers));
    }

    [HttpGet]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> ProductRemains()
    {
        var remains = await service.GetProductRemainsViewModel();
        return View(remains);
    }
}