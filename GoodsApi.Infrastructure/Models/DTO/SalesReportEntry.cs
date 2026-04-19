using System.ComponentModel;

namespace GoodsApi.Infrastructure.Models.DTO;

public class SalesReportEntry
{
    [DisplayName("Дата")]
    public DateTime Date { get; set; }
    [DisplayName("Товар")]
    public string Name { get; set; } = string.Empty;
    [DisplayName("Артикул")]
    public string Article { get; set; } = string.Empty;
    [DisplayName("Размер")]
    public decimal Size { get; set; }
    
    [DisplayName("Проданное количество")]
    public int SoldAmount { get; set; }
    [DisplayName("Цена за единицу")]
    public decimal PricePerUnit { get; set; }
    [DisplayName("Итоговая сумма")]
    public decimal TotalPrice { get; set; }
    [DisplayName("Менеджер")]
    public string ManagerName { get; set; } = string.Empty;
}