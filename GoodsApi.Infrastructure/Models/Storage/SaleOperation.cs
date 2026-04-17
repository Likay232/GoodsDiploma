using System.ComponentModel.DataAnnotations.Schema;

namespace GoodsApi.Infrastructure.Models.Storage;

public class SaleOperation : BaseEntity
{
    public int ProductInfoId { get; set; }
    public int UserId { get; set; }
    
    public bool IsShipped { get; set; }

    public int Amount { get; set; }
    public decimal PricePerUnit { get; set; }

    [NotMapped]
    public decimal TotalPrice => Amount * PricePerUnit;
    
    public DateTime SaleDate { get; set; }

    public string? PathToFile { get; set; } = string.Empty;
    
    public virtual ProductInfo? ProductInfo { get; set; }
    public virtual User? User { get; set; }
}