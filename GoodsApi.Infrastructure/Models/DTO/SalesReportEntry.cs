namespace GoodsApi.Infrastructure.Models.DTO;

public class SalesReportEntry
{
    public DateTime Date { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Article { get; set; } = string.Empty;
    public decimal Size { get; set; }
    
    public int SoldAmount { get; set; }
    public decimal PricePerUnit { get; set; }
    public decimal TotalPrice { get; set; }
    public string ManagerName { get; set; } = string.Empty;
}