using System.ComponentModel.DataAnnotations;

namespace GoodsApi.Infrastructure.ViewModels;

public class RefundViewModel : IValidatableObject
{
    [Required]
    public int SaleOperationId { get; set; }

    [Required]
    public int SoldAmount { get; set; }
    
    [Required]
    public int RefundedAmount { get; set; }

    [Required]
    public int UserId { get; set; }

    [Required(ErrorMessage = "Укажите количество для возврата")]
    [Range(1, int.MaxValue, ErrorMessage = "Количество должно быть больше 0")]
    public int Amount { get; set; }

    [Required(ErrorMessage = "Укажите причину возврата")]
    [StringLength(500, ErrorMessage = "Максимум 500 символов")]
    public string? Description { get; set; }

    [Required(ErrorMessage = "Укажите дату возврата")]
    public DateTime RefundDate { get; set; }
    
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (RefundedAmount + Amount > SoldAmount)
        {
            yield return new ValidationResult(
                "Количество возвращаемого товара превышает количество купленного",
                [nameof(Amount)]);
        }
        
        if (RefundDate > DateTime.Now)
        {
            yield return new ValidationResult(
                "Дата не может быть в будущем",
                [nameof(RefundDate)]);
        }

    }
}