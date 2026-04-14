using GoodsApi.Infrastructure.Models.Database;
using GoodsApi.Infrastructure.Models.DTO;
using GoodsApi.Infrastructure.Models.Enums;
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

    public async Task<List<SelectListItem>> GetBrandsSelectList()
    {
        var products = await catalogService.GetProducts();

        var brands = products
            .GroupBy(pm => pm.Brand)
            .Select(p => p.Key)
            .ToList();

        return brands.Select(c => new SelectListItem()
            {
                Value = c,
                Text = c
            })
            .ToList();
    }

    public async Task<List<SelectListItem>> GetRemainStatusesSelectList()
    {
        return Enum.GetValues(typeof(RemainStatus))
            .Cast<RemainStatus>()
            .Select(x => new SelectListItem
            {
                Value = x.ToString(),
                Text = x switch
                {
                    RemainStatus.Medium => "В норме",
                    RemainStatus.Low => "Дефицит",
                    RemainStatus.High => "Избыток",
                    _ => x.ToString()
                }
            }).ToList();
    }

    public async Task<List<SelectListItem>> GetProvidersSelectList()
    {
        var providers = (await catalogService.GetProviders())
            .Select(pm => new SelectListItem()
            {
                Text = pm.Name,
                Value = pm.Name
            });

        return providers.ToList();
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

    public async Task<DeliveryReportViewModel> GetDeliveryReportViewModel()
    {
        var viewModel = new DeliveryReportViewModel();

        var t = component.SupplyOperations.ToList();

        viewModel.Categories = await GetCategoriesSelectList();
        viewModel.Providers = await GetProvidersSelectList();
        viewModel.MaximumSupplyTotalPriceInDb = component.SupplyOperations.Max(s => s.Amount * s.PurchasePrice);
        viewModel.MaximumSupplyTotalPrice = viewModel.MaximumSupplyTotalPriceInDb;
        return viewModel;
    }

    public async Task<List<DeliveryReportEntry>> GetDeliveryReport(DeliveryReportViewModel viewModel)
    {
        var deliveries = component.SupplyOperations
            .Include(u => u.User)
            .Include(u => u.Provider)
            .Include(o => o.ProductInfo)
            .ThenInclude(o => o!.Product)
            .Where(s =>
                s.AcceptanceDate >= viewModel.StartDate.ToUniversalTime() &&
                s.AcceptanceDate <= viewModel.EndDate.ToUniversalTime());

        if (viewModel.Category is not null)
            deliveries = deliveries.Where(o => o.ProductInfo!.Product!.Category == viewModel.Category);

        if (viewModel.SelectedProviders is not null && viewModel.SelectedProviders.Any())
            deliveries = deliveries.Where(d => viewModel.SelectedProviders.Contains(d.Provider!.Name));

        var deliveriesList = deliveries.ToList();

        deliveriesList = deliveriesList.Where(d =>
                d.TotalPrice >= viewModel.MinimumSupplyTotalPrice &&
                d.TotalPrice <= viewModel.MaximumSupplyTotalPrice)
            .ToList();

        return deliveriesList.Convert<Models.Storage.SupplyOperation, DeliveryReportEntry>();
    }

    public async Task<RemainsReportViewModel> GetRemainReportViewModel()
    {
        var viewModel = new RemainsReportViewModel();

        viewModel.Categories = await GetCategoriesSelectList();
        viewModel.Brands = await GetBrandsSelectList();
        viewModel.Statuses = await GetRemainStatusesSelectList();

        return viewModel;
    }

    public async Task<List<RemainsReportEntry>> GetRemainsReport(RemainsReportViewModel viewModel)
    {
        var remains = component.ProductInfos
            .Include(product => product.Product).ToList();

        if (viewModel.Category is not null)
            remains = remains.Where(r => r.Product!.Category == viewModel.Category).ToList();

        if (viewModel.Brand is not null)
            remains = remains.Where(r => r.Product!.Brand == viewModel.Brand).ToList();

        if (viewModel.StatusFilter is not null)
        {
            remains = remains.Where(r =>
            {
                var status =
                    r.Amount <= r.Product!.MinimumRemain
                        ? RemainStatus.Low
                        : (r.Amount < 2 * r.Product.MinimumRemain
                            ? RemainStatus.Medium
                            : RemainStatus.High);

                return status == viewModel.StatusFilter;
            }).ToList();
        }

        return remains.Convert<Models.Storage.ProductInfo, RemainsReportEntry>();
    }
}