using System.Security.Claims;
using GoodsApi.Infrastructure.Services;
using GoodsApi.Infrastructure.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GoodsApi.Controllers;

[Route("[controller]/[action]")]
[Authorize(Roles = "Manager")]
public class ManagerController(ManagerService managerService) : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return RedirectToAction("Products", "Catalog");
    }
    
    [HttpGet]
    public async Task<IActionResult> SaleRegistrationPage(int productInfoId)
    {
        var viewModel = await managerService.GetSaleRegistrationViewModel(productInfoId);
        
        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> RegisterSale(SaleRegistrationViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View("SaleRegistrationPage", model);
        }
        
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;        
        
        await managerService.RegisterSale(model, userId);
        
        return RedirectToAction("ProductInfos", "Catalog", new {productId = model.ProductInfo.ProductId});
    }

    [HttpGet]
    public async Task<IActionResult> GetDocument(string filePath)
    {
        if (!Path.Exists(filePath)) return StatusCode(404);
        
        var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
        var fileName = Path.GetFileName(filePath);

        return File(stream, "application/vnd.openxmlformats-officedocument.wordprocessingml.document", fileName);
    }

    [HttpGet]
    public async Task<IActionResult> SaleOperations()
    {
        var viewModel = await managerService.GetSaleOperationsViewModel();
        return View(viewModel);
    }

    [HttpGet]
    public async Task<IActionResult> RefundPage(int saleOperationId)
    {
        var viewModel = await managerService.GetRefundViewModel(saleOperationId);
        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> RegisterRefund(RefundViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View("RefundPage", model);
        }
        
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;        

        if (!await managerService.RegisterRefundOperation(model, userId))
        {
            ModelState.AddModelError("", "Количество товара на возврат превышает купленное количество. ");
            
            return View("RefundPage", model);
        }

        return RedirectToAction(nameof(SaleOperations));
    }
        
}