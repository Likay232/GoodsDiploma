namespace GoodsApi.Infrastructure.Models.Storage;

public class RefundOperation : BaseEntity
{
    public int SaleOperationId { get; set; }
    public int UserId { get; set; }
    
    public int Amount { get; set; }
    public string Description { get; set; } = string.Empty;
    public DateTime RefundDate { get; set; }
    
    public virtual SaleOperation? SaleOperation { get; set; }
    public virtual User? User { get; set; }
}