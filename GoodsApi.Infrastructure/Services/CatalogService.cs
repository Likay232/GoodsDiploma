using System.Globalization;
using GoodsApi.Infrastructure.Models.Database;
using GoodsApi.Infrastructure.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace GoodsApi.Infrastructure.Services;

public class CatalogService(DataComponent component)
{
    public async Task<List<Models.DTO.ProductInfo>> GetProductInfos(int productId)
    {
        return await component.ProductInfos
            .Where(info => info.ProductId == productId)
            .Select(p => new Models.DTO.ProductInfo
            {
                Id = p.Id,
                ProductId = p.ProductId,
                Amount = p.Amount,
                Article = p.Article,
                Color = p.Color,
                Location = p.Location,
                Size = p.Size,
                Price = p.Price,
            })
            .ToListAsync();
    }

    public async Task<Models.DTO.ProductInfo> GetProductInfo(int id)
    {
        var productEntry = await component.ProductInfos
            .FirstOrDefaultAsync(info => info.Id == id);

        if (productEntry is null) throw new Exception();

        return productEntry.Convert<Models.Storage.ProductInfo, Models.DTO.ProductInfo>();
    }

    public async Task<ProductInfosViewModel> GetProductInfosViewModel(int productId)
    {
        var product = await GetProduct(productId);
        var productInfos = await GetProductInfos(productId);
        
        var model = new ProductInfosViewModel
        {
            Product = product,
            ProductInfos = productInfos.Select(x => new ProductInfoCardViewModel
            {
                Id = x.Id,
                Article = x.Article,
                Price = x.Price,
                Size = x.Size,
                Color = x.Color,
                Amount = x.Amount,
                Location = x.Location
            }).ToList()
        };
        return model;
    }

    public async Task<ProductInfoEditViewModel> GetProductInfoEditViewModel(int productInfoId, int productId)
    {
        var products = await GetProducts();
        var productInfo = new Models.DTO.ProductInfo();

        if (productInfoId != 0)
            productInfo = await GetProductInfo(productInfoId);
        
        var model = productInfo.Convert<Models.DTO.ProductInfo, ProductInfoEditViewModel>();
        model.ProductId = productId;
        
        model.Products = products.Select(p => new SelectListItem
        {
            Value = p.Id.ToString(),
            Text = p.Name
        }).ToList();
        
        return model;
    }

    public async Task<bool> SaveProductInfo(ProductInfoEditViewModel viewModel)
    {
        if (viewModel.Id != 0) return await ChangeProductInfo(viewModel);
        
        return await AddProductInfo(viewModel);
    }

    private async Task<bool> AddProductInfo(ProductInfoEditViewModel viewModel)
    {
        var converted = viewModel
            .Convert<ProductInfoEditViewModel, Models.DTO.ProductInfo>()
            .Convert<Models.DTO.ProductInfo, Models.Storage.ProductInfo>();

        return await component.Insert(converted);
    }
    
    private async Task<bool> ChangeProductInfo(ProductInfoEditViewModel viewModel)
    {
        var converted = viewModel
            .Convert<ProductInfoEditViewModel, Models.DTO.ProductInfo>()
            .Convert<Models.DTO.ProductInfo, Models.Storage.ProductInfo>();

        return await component.Update(converted);
    }
    
    public async Task<bool> DeleteProductInfo(int productInfoId)
    {
        return await component.Delete<Models.Storage.ProductInfo>(productInfoId);
    }

    public async Task<List<Models.DTO.Product>> GetProducts()
    {
        return await component.Products
            .Select(p => new Models.DTO.Product
            {
                Id = p.Id,
                Brand = p.Brand,
                MinimumRemain = p.MinimumRemain,
                Category = p.Category,
                Name = p.Name,
            })
            .ToListAsync();
    }
    
    public async Task<ProductsViewModel> GetProductsViewModel(string? query)
    {
        var products = await component.Products
            .Include(p => p.ProductInfos)
            .ToListAsync();

        if (!string.IsNullOrWhiteSpace(query))
        {
            query = query.ToLower();
            products = products
                .Where(p => p.Name.ToLower().Contains(query)
                            || p.ProductInfos.Any(pi => pi.Article.ToLower().Contains(query)
                                                        || pi.Color.ToLower().Contains(query)
                                                        || pi.Size.ToString(CultureInfo.InvariantCulture).Contains(query)))
                .ToList();
        }

        var productsViewModel = new ProductsViewModel();

        productsViewModel.Products = products
            .Convert<Models.Storage.Product, Models.DTO.Product>()
            .Convert<Models.DTO.Product, ProductViewModel>();
        productsViewModel.SearchQuery = query;

        return productsViewModel;
    }
    
    public async Task<Models.DTO.Product> GetProduct(int id)
    {
        var productEntry = await component.Products
            .FirstOrDefaultAsync(info => info.Id == id);
        
        if (productEntry is null) throw new Exception();
        
        return productEntry.Convert<Models.Storage.Product, Models.DTO.Product>();
    }
    
    public async Task<ProductEditViewModel> GetProductViewModel(int id)
    {
        var viewModel = new ProductEditViewModel();

        if (id != 0)
        {
            var product = await GetProduct(id);

            viewModel = product.Convert<Models.DTO.Product, ProductEditViewModel>();
        }

        return viewModel;
    }


    public async Task<bool> SaveProduct(ProductEditViewModel viewModel)
    {
        var converted = viewModel
            .Convert<ProductEditViewModel, Models.DTO.Product>();

        if (viewModel.Id != 0) return await ChangeProduct(converted);
        
        return await AddProduct(converted);
    }

    private async Task<bool> AddProduct(Models.DTO.Product product)
    {
        var converted = product.Convert<Models.DTO.Product, Models.Storage.Product>();

        return await component.Insert(converted);
    }
    
    private async Task<bool> ChangeProduct(Models.DTO.Product changedProduct)
    {
        var converted = changedProduct.Convert<Models.DTO.Product, Models.Storage.Product>();

        return await component.ChangeEntity(converted);
    }

    public async Task<bool> DeleteProduct(int productId)
    {
        return await component.Delete<Models.Storage.Product>(productId);
    }

    public async Task<List<Models.DTO.Provider>> GetProviders()
    {
        return await component.Providers
            .Select(p => new Models.DTO.Provider
            {
                Id = p.Id,
                Name = p.Name,
                ContactFullName = p.ContactFullName,
                PhoneNumber = p.PhoneNumber,
                Email = p.Email,
                Address = p.Address,
            })
            .ToListAsync();
    }
    
    public async Task<ProvidersViewModel> GetProvidersViewModel()
    {
        var providers = await GetProviders();
        var viewModel = new ProvidersViewModel();

        viewModel.Providers = providers
            .Convert<Models.DTO.Provider, ProviderViewModel>();

        return viewModel;
    }


    public async Task<Models.DTO.Provider> GetProvider(int id)
    {
        var providerEntry = await component.Providers
            .FirstOrDefaultAsync(info => info.Id == id);
        
        if (providerEntry is null) throw new Exception();
        
        return providerEntry.Convert<Models.Storage.Provider, Models.DTO.Provider>();
    }

    public async Task<ProviderEditViewModel> GetProviderEditViewModel(int providerId)
    {
        var viewModel = new ProviderEditViewModel();
        if (providerId != 0)
        {
            viewModel = (await GetProvider(providerId))
                .Convert<Models.DTO.Provider, ProviderEditViewModel>();
        }

        return viewModel;
    }

    public async Task<bool> SaveProvider(ProviderEditViewModel viewModel)
    {
        if (viewModel.Id == 0)
            return await AddProvider(viewModel.Convert<ProviderEditViewModel, Models.DTO.Provider>());
        
        return await ChangeProvider(viewModel.Convert<ProviderEditViewModel, Models.DTO.Provider>());
    }

    public async Task<bool> AddProvider(Models.DTO.Provider provider)
    {
        var converted = provider.Convert<Models.DTO.Provider, Models.Storage.Provider>();

        return await component.Insert(converted);
    }

    public async Task<bool> ChangeProvider(Models.DTO.Provider changedProvider)
    {
        var converted = changedProvider.Convert<Models.DTO.Provider, Models.Storage.Provider>();
        
        return await component.ChangeEntity(converted);
    }

    public async Task<bool> DeleteProvider(int providerId)
    {
        return await component.Delete<Models.Storage.Provider>(providerId);
    }

    public async Task<ProductRemainsViewModel> GetProductRemainsViewModel()
    {
        return new ProductRemainsViewModel()
        {
            ProductRemains = await GetProductRemains()
        };
    }
    
    public async Task<List<Models.DTO.ProductRemain>> GetProductRemains()
    {
        return await component.ProductInfos
            .Include(info => info.Product)
            .Where(info => info.Product != null)
            .GroupBy(info => new
            {
                info.ProductId, info.Product!.Name, info.Product.Brand, info.Product.Category,
                info.Product.MinimumRemain, info.Size, info.Color
            })
            .Select(g => new Models.DTO.ProductRemain
            {
                ProductId = g.Key.ProductId,
                Name = g.Key.Name,
                Category = g.Key.Category,
                Brand = g.Key.Brand,
                Size = g.Key.Size,
                Color = g.Key.Color,
                MinimumRemain = g.Key.MinimumRemain,
                TotalRemain = g.Sum(info => info.Amount),
            })
            .ToListAsync();
    }
}