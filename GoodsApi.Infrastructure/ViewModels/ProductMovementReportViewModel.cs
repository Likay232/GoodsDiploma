using System.ComponentModel.DataAnnotations;
using GoodsApi.Infrastructure.Models.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GoodsApi.Infrastructure.ViewModels;

public class ProductMovementReportViewModel : IValidatableObject
{
    [Required(ErrorMessage = "Укажите начальную дату")]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime StartDate { get; set; } = DateTime.Now;

    [Required(ErrorMessage = "Укажите конечную дату")]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime EndDate { get; set; } = DateTime.Now;

    public List<string> SelectedOperationTypes { get; set; } = [];

    [StringLength(100)]
    public string? Category { get; set; }

    [StringLength(100)]
    public string? Article { get; set; }

    [StringLength(200)]
    public string? Name { get; set; }

    public List<SelectListItem> Categories { get; set; } = [];
    public List<string> OperationTypes { get; set; } = [];

    public List<ProductMotionReportEntry> ProductMotionReportEntries { get; set; } = [];

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (StartDate > EndDate)
        {
            yield return new ValidationResult(
                "Начальная дата не может быть больше конечной",
                new[] { nameof(StartDate), nameof(EndDate) });
        }

        if (EndDate > DateTime.Now.AddMinutes(1))
        {
            yield return new ValidationResult(
                "Конечная дата не может быть в будущем",
                new[] { nameof(EndDate) });
        }
    }
}