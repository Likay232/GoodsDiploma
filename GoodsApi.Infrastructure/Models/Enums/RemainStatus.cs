using System.ComponentModel.DataAnnotations;

namespace GoodsApi.Infrastructure.Models.Enums;

public enum RemainStatus
{
    [Display(Name = "Дефицит")]
    Low,
    [Display(Name = "В норме")]
    Medium,
    [Display(Name = "Избыток")]
    High
}