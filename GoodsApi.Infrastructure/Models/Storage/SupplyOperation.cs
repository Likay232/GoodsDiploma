using System.ComponentModel.DataAnnotations.Schema;

namespace GoodsApi.Infrastructure.Models.Storage;

public class SupplyOperation : BaseEntity
{
    public int ProductInfoId { get; set; }
    public int ProviderId { get; set; }
    public int UserId { get; set; }
    
    public int Amount { get; set; }
    public decimal PurchasePrice { get; set; }
    
    [NotMapped]
    public decimal TotalPrice => PurchasePrice * Amount;
    
    public DateTime AcceptanceDate { get; set; }
    
    public virtual ProductInfo? ProductInfo { get; set; }
    public virtual Provider? Provider { get; set; }
    public virtual User? User { get; set; }
}