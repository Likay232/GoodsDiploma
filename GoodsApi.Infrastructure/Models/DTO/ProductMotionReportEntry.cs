using GoodsApi.Infrastructure.Models.Enums;

namespace GoodsApi.Infrastructure.Models.DTO;

public class ProductMotionReportEntry
{
    public DateTime OperationDate { get; set; }
    public string OperationType { get; set; } = string.Empty;
    public string ProductName { get; set; } = string.Empty;
    public string Article { get; set; } = string.Empty;
    public decimal Size { get; set; }
    public int Quantity { get; set; }
    public string CounterAgent { get; set; } = string.Empty;
}