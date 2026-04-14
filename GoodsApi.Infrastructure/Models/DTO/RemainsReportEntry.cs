using GoodsApi.Infrastructure.Models.Enums;

namespace GoodsApi.Infrastructure.Models.DTO;

public class RemainsReportEntry
{
    public string ProductName { get; set; } = string.Empty;
    public string Article { get; set; } = string.Empty;
    public decimal Size { get; set; }
    public string Color { get; set; } = string.Empty;
    public int Amount { get; set; }
    public int MinimumRemain { get; set; }

    public RemainStatus Status
    {
        get
        {
            if (Amount <= MinimumRemain) return RemainStatus.Low;
            if (Amount > MinimumRemain && Amount < 2 * MinimumRemain) return RemainStatus.Medium;
            
            return RemainStatus.High;
        }
        set;
    }

}

