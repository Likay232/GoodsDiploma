using GoodsApi.Infrastructure.Models.Database;
using GoodsApi.Infrastructure.Models.DTO;
using GoodsApi.Infrastructure.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace GoodsApi.Infrastructure.Services;

public class ReportService(DataComponent component, CatalogService catalogService)
{
    public async Task<ProductMovementReportViewModel> GetProductMovementViewModel()
    {
        var viewModel = new ProductMovementReportViewModel();

        await GetFiltersForProductMovementsReport(viewModel);

        return viewModel;
    }

    public async Task GetFiltersForProductMovementsReport(ProductMovementReportViewModel viewModel)
    {
        var operationTypes = component.ProductMovements
            .GroupBy(p => p.OperationType)
            .Select(p => p.Key)
            .ToList();

        viewModel.Categories = await GetCategoriesSelectList();

        viewModel.OperationTypes = operationTypes;
    }
    
    public async Task<List<SelectListItem>> GetCategoriesSelectList()
    {
        var products = await catalogService.GetProducts();

        var categories = products
            .GroupBy(pm => pm.Category)
            .Select(p => p.Key)
            .ToList();

        return categories.Select(c => new SelectListItem()
            {
                Value = c,
                Text = c
            })
            .ToList();
    }

    public async Task<List<ProductMotionReportEntry>> GetProductMovementsReport(
        ProductMovementReportViewModel viewModel)
    {
        var productMovements = component.ProductMovements.Where(pm =>
            pm.OperationDate >= viewModel.StartDate.ToUniversalTime() &&
            pm.OperationDate <= viewModel.EndDate.ToUniversalTime());

        if (viewModel.SelectedOperationTypes.Any())
            productMovements =
                productMovements.Where(pm => viewModel.SelectedOperationTypes.Contains(pm.OperationType));

        if (viewModel.Article is not null)
            productMovements = productMovements.Where(pm => pm.Article == viewModel.Article);

        if (viewModel.Name is not null)
            productMovements = productMovements.Where(pm => pm.ProductName == viewModel.Name);

        return productMovements.ToList().Convert<Models.Storage.ProductMovement, ProductMotionReportEntry>();
    }

    public async Task<SalesReportViewModel> GetSalesReportViewModel()
    {
        var viewModel = new SalesReportViewModel();

        var sales = component.SaleOperations.ToList();

        var maximumTotalPrice = sales.Max(operation => operation.PricePerUnit * operation.Amount);

        viewModel.Categories = await GetCategoriesSelectList();

        viewModel.MinimumTotalPrice = 0;
        viewModel.MaximumTotalPrice = maximumTotalPrice;
        viewModel.MaximumTotalPriceInDb = maximumTotalPrice;

        return viewModel;
    }

    public async Task<List<SalesReportEntry>> GetSalesReport(SalesReportViewModel viewModel)
    {
        var salesQuery = component.SaleOperations
            .Include(s => s.User)
            .Include(s => s.ProductInfo)
            .ThenInclude(pi => pi!.Product)
            .Where(s =>
                s.SaleDate >= viewModel.StartDate.ToUniversalTime() &&
                s.SaleDate <= viewModel.EndDate.ToUniversalTime());

        if (viewModel.Article is not null)
            salesQuery = salesQuery.Where(saleOperation => saleOperation.ProductInfo!.Article == viewModel.Article);

        if (viewModel.Name is not null)
            salesQuery = salesQuery.Where(saleOperation => saleOperation.ProductInfo!.Product!.Name == viewModel.Name);

        var salesList = salesQuery.ToList();

        salesList = salesList
            .Where(sale => sale.TotalPrice >= viewModel.MinimumTotalPrice &&
                           sale.TotalPrice <= viewModel.MaximumTotalPrice)
            .ToList();

        var salesReport = salesList.Convert<Models.Storage.SaleOperation, SalesReportEntry>();

        return salesReport;
    }

    public async Task<List<SaleReportChartData>?> GetSalesReportChartData(SalesReportViewModel viewModel)
    {
        return viewModel.SaleReport?
            .GroupBy(s => s.Date.Date)
            .Select(g => new SaleReportChartData()
            {
                Date = g.Key,
                TotalPrice = g.Sum(s => s.TotalPrice)
            })
            .ToList();
    }
}