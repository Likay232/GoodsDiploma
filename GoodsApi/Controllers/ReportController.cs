using GoodsApi.Infrastructure.Models.DTO;
using GoodsApi.Infrastructure.Services;
using GoodsApi.Infrastructure.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GoodsApi.Controllers;

[Route("[controller]/[action]")]
public class ReportController(ReportService reportService) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        return View();
    }

    [HttpGet]
    [Authorize(Roles = "Storekeeper,Admin")]
    public async Task<IActionResult> ProductMovementsReport()
    {
        var viewModel = await reportService.GetProductMovementViewModel();
        
        return View(viewModel);
    }

    [HttpPost]
    [Authorize(Roles = "Storekeeper,Admin")]
    public async Task<IActionResult> GetProductMovementsReport(ProductMovementReportViewModel viewModel)
    {
        await reportService.GetFiltersForProductMovementsReport(viewModel);
        
        if (!ModelState.IsValid) return View("ProductMovementsReport", viewModel);

        viewModel.ProductMotionReportEntries = await reportService.GetProductMovementsReport(viewModel);
        
        return View("ProductMovementsReport", viewModel);
    }

    [HttpGet]
    [Authorize(Roles = "Manager,Admin")]
    public async Task<IActionResult> SalesReport()
    {
        var viewModel = await reportService.GetSalesReportViewModel();
        
        return View(viewModel);
    }

    [HttpPost]
    [Authorize(Roles = "Manager,Admin")]
    public async Task<IActionResult> GetSalesReport(SalesReportViewModel viewModel)
    {
        viewModel.Categories = await reportService.GetCategoriesSelectList();
        viewModel.SaleReport = await reportService.GetSalesReport(viewModel);
        
        if (!ModelState.IsValid) return View("SalesReport", viewModel);

        viewModel.ChartData = await reportService.GetSalesReportChartData(viewModel);
        
        return View("SalesReport", viewModel);
    }

    [HttpGet]
    [Authorize(Roles = "Storekeeper,Admin")]
    public async Task<IActionResult> DeliveriesReport()
    {
        var viewModel = await reportService.GetDeliveryReportViewModel();
        return View(viewModel);
    }

    [HttpPost]
    [Authorize(Roles = "Storekeeper,Admin")]
    public async Task<IActionResult> GetDeliveryReport(DeliveryReportViewModel viewModel)
    {
        viewModel.Categories = await reportService.GetCategoriesSelectList();
        viewModel.Providers = await reportService.GetProvidersSelectList();
        
        if (!ModelState.IsValid) return View("DeliveriesReport", viewModel);
        
        viewModel.DeliveryReport = await reportService.GetDeliveryReport(viewModel);

        return View("DeliveriesReport", viewModel);
    }

    [HttpGet]
    [Authorize(Roles = "Storekeeper,Admin")]
    public async Task<IActionResult> RemainsReport()
    {
        var  viewModel = await reportService.GetRemainReportViewModel();
        
        return View(viewModel);
    }

    [HttpPost]
    [Authorize(Roles = "Storekeeper,Admin")]
    public async Task<IActionResult> GetRemainsReport(RemainsReportViewModel viewModel)
    {
        if (!ModelState.IsValid) return View("RemainsReport", viewModel);
        
        viewModel.Categories = await reportService.GetCategoriesSelectList();
        viewModel.Brands = await reportService.GetBrandsSelectList();
        viewModel.Statuses = await reportService.GetRemainStatusesSelectList();

        viewModel.RemainReport = await reportService.GetRemainsReport(viewModel);

        return View("RemainsReport", viewModel);
    }

    [HttpPost]
    [Authorize(Roles = "Manager,Admin")]
    public async Task<IActionResult> GetSalesReportDoc([FromBody] List<SalesReportEntry> data)
    {
        var file = await reportService.GetExcelFileForList(data);

        return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
    } 
    
    [HttpPost]
    [Authorize(Roles = "Storekeeper,Admin")]
    public async Task<IActionResult> GetRemainsReportDoc([FromBody] List<RemainsReportEntry> data)
    {
        var file = await reportService.GetExcelFileForList(data);

        return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
    } 

    [HttpPost]
    [Authorize(Roles = "Storekeeper,Admin")]
    public async Task<IActionResult> GetProductMovementReportDoc([FromBody] List<ProductMotionReportEntry> data)
    {
        var file = await reportService.GetExcelFileForList(data);

        return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
    } 
    
    [HttpPost]
    [Authorize(Roles = "Storekeeper,Admin")]
    public async Task<IActionResult> GetDeliveriesReportDoc([FromBody] List<DeliveryReportEntry> data)
    {
        var file = await reportService.GetExcelFileForList(data);

        return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
    } 

}