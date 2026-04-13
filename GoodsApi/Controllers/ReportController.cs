using GoodsApi.Infrastructure.Services;
using GoodsApi.Infrastructure.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GoodsApi.Controllers;

public class ReportController(ReportService reportService) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> ProductMovementsReport()
    {
        var viewModel = await reportService.GetProductMovementViewModel();
        
        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> GetProductMovementsReport(ProductMovementReportViewModel viewModel)
    {
        await reportService.GetFiltersForProductMovementsReport(viewModel);
        viewModel.ProductMotionReportEntries = await reportService.GetProductMovementsReport(viewModel);
        
        return View("ProductMovementsReport", viewModel);
    }

    [HttpGet]
    public async Task<IActionResult> SalesReport()
    {
        var viewModel = await reportService.GetSalesReportViewModel();
        
        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> GetSalesReport(SalesReportViewModel viewModel)
    {
        viewModel.Categories = await reportService.GetCategoriesSelectList();
        viewModel.SaleReport = await reportService.GetSalesReport(viewModel);
        viewModel.ChartData = await reportService.GetSalesReportChartData(viewModel);
        
        return View("SalesReport", viewModel);
    }
}