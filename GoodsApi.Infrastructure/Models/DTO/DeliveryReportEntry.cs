namespace GoodsApi.Infrastructure.Models.DTO;

public class DeliveryReportEntry
{
    public DateTime Date { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string Article { get; set; } = string.Empty;
    
    public decimal Size { get; set; }
    public string Color { get; set; } = string.Empty;
    
    public int Amount { get; set; }
    public decimal PurchasePrice { get; set; }
    public decimal TotalPrice => Amount * PurchasePrice;
    
    public string Provider { get; set; } =  string.Empty;
    public string StorekeeperUsername { get; set; } = string.Empty;
}