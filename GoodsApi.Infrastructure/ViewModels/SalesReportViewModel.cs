using System.ComponentModel.DataAnnotations;
using GoodsApi.Infrastructure.Models.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GoodsApi.Infrastructure.ViewModels;

public class SalesReportViewModel
{
    [Required(ErrorMessage = "Укажите начальную дату")]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-ddTHH:mm}", ApplyFormatInEditMode = true)]
    public DateTime StartDate { get; set; } = DateTime.Now;

    [Required(ErrorMessage = "Укажите конечную дату")]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-ddTHH:mm}", ApplyFormatInEditMode = true)]
    public DateTime EndDate { get; set; } = DateTime.Now;

    [StringLength(100)]
    public string? Category { get; set; }

    [StringLength(100)]
    public string? Article { get; set; }

    [StringLength(200)]
    public string? Name { get; set; }

    public List<SelectListItem> Categories { get; set; } = [];

    public decimal MinimumTotalPrice { get; set; }
    public decimal MaximumTotalPrice { get; set; }
    public decimal MaximumTotalPriceInDb { get; set; }

    public List<SalesReportEntry>? SaleReport { get; set; }
    public List<SaleReportChartData>? ChartData { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (StartDate > EndDate)
        {
            yield return new ValidationResult(
                "Начальная дата не может быть больше конечной",
                [nameof(StartDate), nameof(EndDate)]);
        }

        if (EndDate > DateTime.Now.AddMinutes(1))
        {
            yield return new ValidationResult(
                "Конечная дата не может быть в будущем",
                [nameof(EndDate)]);
        }

        if (MaximumTotalPrice < MinimumTotalPrice)
        {
            yield return new ValidationResult(
                "Максимальная цена не может быть меньше минимальной",
                [nameof(MaximumTotalPrice), nameof(MinimumTotalPrice)]);
        }
    }
}