using System.ComponentModel;
using GoodsApi.Infrastructure.Models.Enums;

namespace GoodsApi.Infrastructure.Models.DTO;

public class RemainsReportEntry
{
    [DisplayName("Товар")]
    public string ProductName { get; set; } = string.Empty;
    [DisplayName("Артикул")]
    public string Article { get; set; } = string.Empty;
    [DisplayName("Размер")]
    public decimal Size { get; set; }
    [DisplayName("Цвет")]
    public string Color { get; set; } = string.Empty;
    [DisplayName("Остаток")]
    public int Amount { get; set; }
    [DisplayName("Минимальный остаток")]
    public int MinimumRemain { get; set; }

    [DisplayName("Статус")]
    public RemainStatus Status
    {
        get
        {
            if (Amount <= MinimumRemain) return RemainStatus.Low;
            if (Amount > MinimumRemain && Amount < 2 * MinimumRemain) return RemainStatus.Medium;
            
            return RemainStatus.High;
        }
    }
}

