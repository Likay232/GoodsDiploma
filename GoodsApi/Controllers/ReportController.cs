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

    [HttpGet]
    public async Task<IActionResult> DeliveriesReport()
    {
        var viewModel = await reportService.GetDeliveryReportViewModel();
        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> GetDeliveryReport(DeliveryReportViewModel viewModel)
    {
        viewModel.DeliveryReport = await reportService.GetDeliveryReport(viewModel);
        viewModel.Categories = await reportService.GetCategoriesSelectList();
        viewModel.Providers = await reportService.GetProvidersSelectList();

        return View("DeliveriesReport", viewModel);
    }

    [HttpGet]
    public async Task<IActionResult> RemainsReport()
    {
        var  viewModel = await reportService.GetRemainReportViewModel();
        
        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> GetRemainsReport(RemainsReportViewModel viewModel)
    {
        viewModel.Categories = await reportService.GetCategoriesSelectList();
        viewModel.Brands = await reportService.GetBrandsSelectList();
        viewModel.Statuses = await reportService.GetRemainStatusesSelectList();

        viewModel.RemainReport = await reportService.GetRemainsReport(viewModel);

        return View("RemainsReport", viewModel);
    }
}