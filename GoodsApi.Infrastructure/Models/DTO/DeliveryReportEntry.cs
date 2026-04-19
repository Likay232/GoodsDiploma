using System.ComponentModel;

namespace GoodsApi.Infrastructure.Models.DTO;

public class DeliveryReportEntry
{
    [DisplayName("Дата")]
    public DateTime Date { get; set; }
    [DisplayName("Товар")]
    public string ProductName { get; set; } = string.Empty;
    [DisplayName("Артикул")]
    public string Article { get; set; } = string.Empty;
    
    [DisplayName("Размер")]
    public decimal Size { get; set; }
    [DisplayName("Цвет")]
    public string Color { get; set; } = string.Empty;
    [DisplayName("Количество")]
    public int Amount { get; set; }
    [DisplayName("Закупочная цена")]
    public decimal PurchasePrice { get; set; }
    [DisplayName("Итоговая сумма")]
    public decimal TotalPrice => Amount * PurchasePrice;
    
    [DisplayName("Поставщик")]
    public string Provider { get; set; } =  string.Empty;
    [DisplayName("Кладовщик")]
    public string StorekeeperUsername { get; set; } = string.Empty;
}