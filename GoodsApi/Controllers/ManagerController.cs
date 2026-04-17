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
        var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
        var fileName = Path.GetFileName(filePath);

        return File(stream, "application/vnd.openxmlformats-officedocument.wordprocessingml.document", fileName);
    }
}