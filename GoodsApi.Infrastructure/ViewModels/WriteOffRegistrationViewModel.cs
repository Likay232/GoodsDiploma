using System.ComponentModel.DataAnnotations;
using GoodsApi.Infrastructure.Models.DTO;

namespace GoodsApi.Infrastructure.ViewModels;

public class WriteOffRegistrationViewModel : IValidatableObject
{
    public ProductInfo ProductInfo { get; set; } = new ();
    
    public int UserId { get; set; }
    
    [Range(1, int.MaxValue, ErrorMessage = "Списываемое количество должно быть больше 0.")]
    public int Amount { get; set; }

    [Required(ErrorMessage = "Укажите причину списания.")]
    [StringLength(500, ErrorMessage = "Описание не должно превышать 500 символов.")]
    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "Укажите дату списания.")]
    public DateTime WriteOffDate { get; set; }
    
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (WriteOffDate > DateTime.Now)
        {
            yield return new ValidationResult(
                "Дата не может быть в будущем",
                [nameof(WriteOffDate)]);
        }
    }
}
    
    