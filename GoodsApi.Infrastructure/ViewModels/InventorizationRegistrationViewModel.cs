using System.ComponentModel.DataAnnotations;
using GoodsApi.Infrastructure.Models.DTO;

namespace GoodsApi.Infrastructure.ViewModels;

public class InventorizationRegistrationViewModel : IValidatableObject
{
    public ProductInfo ProductInfo { get; set; } = new();
    public int UserId { get; set; }
    
    [Required(ErrorMessage = "Введите фактическое количество товара на складе.")]
    [Range(0, int.MaxValue, ErrorMessage = "Фактическое количество не может быть отрицательным.")]
    public int FactAmount { get; set; }
    public int MismatchAmount { get; set; }
    
    [Required(ErrorMessage = "Укажите дату инвентаризации.")]
    public DateTime InventoryOperationDate { get; set; }
    
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (InventoryOperationDate > DateTime.Now)
        {
            yield return new ValidationResult(
                "Дата не может быть в будущем",
                [nameof(InventoryOperationDate)]);
        }

    }
}