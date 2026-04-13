namespace GoodsApi.Infrastructure.Models.Storage;

public class InventoryOperation : BaseEntity
{
    public int ProductInfoId { get; set; }
    public int UserId { get; set; }
    
    public int FactAmount { get; set; }
    public int MismatchAmount { get; set; }
    
    public DateTime InventoryOperationDate { get; set; }
    
    public virtual ProductInfo? ProductInfo { get; set; }
    public virtual User? User { get; set; }
}