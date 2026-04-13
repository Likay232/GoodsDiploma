namespace GoodsApi.Infrastructure.Models.Storage;

public class Provider : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string ContactFullName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
}