using System.ComponentModel.DataAnnotations;
using GoodsApi.Infrastructure.Models.DTO;

namespace GoodsApi.Infrastructure.ViewModels;

public class SaleRegistrationViewModel : IValidatableObject
{
    public ProductInfo ProductInfo { get; set; } = new();
    public string Name { get; set; } =  string.Empty;
    public int Amount { get; set; }
    public decimal PricePerUnit { get; set; }
    public DateTime SaleDate { get; set; }
    
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (SaleDate > DateTime.Now)
        {
            yield return new ValidationResult(
                "Дата не может быть в будущем",
                [nameof(SaleDate)]);
        }
    }
}