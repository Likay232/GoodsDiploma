namespace GoodsApi.Infrastructure.ViewModels;

public class ProvidersViewModel
{
    public List<ProviderViewModel> Providers { get; set; } = [];
}

public class ProviderViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string ContactFullName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
}