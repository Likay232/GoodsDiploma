namespace GoodsApi.Infrastructure.Models.Storage;

public class WriteOffOperation : BaseEntity
{
    public int ProductInfoId { get; set; }
    public int UserId { get; set; }
    
    public int Amount { get; set; }
    public string Description { get; set; } = string.Empty;
    public DateTime WriteOffDate { get; set; }
    
    public virtual ProductInfo? ProductInfo { get; set; }
    public virtual User? User { get; set; }
}