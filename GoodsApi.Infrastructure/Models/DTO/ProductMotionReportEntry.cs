using System.ComponentModel;
using GoodsApi.Infrastructure.Models.Enums;

namespace GoodsApi.Infrastructure.Models.DTO;

public class ProductMotionReportEntry
{
    [DisplayName("Дата")]
    public DateTime OperationDate { get; set; }
    [DisplayName("Тип операции")]
    public string OperationType { get; set; } = string.Empty;
    [DisplayName("Товар")]
    public string ProductName { get; set; } = string.Empty;
    [DisplayName("Артикул")]
    public string Article { get; set; } = string.Empty;
    [DisplayName("Размер")]
    public decimal Size { get; set; }
    [DisplayName("Количество")]
    public int Quantity { get; set; }
    [DisplayName("Контрагент")]
    public string CounterAgent { get; set; } = string.Empty;
}