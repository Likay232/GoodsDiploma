using System.ComponentModel.DataAnnotations;
using GoodsApi.Infrastructure.CustomAttributes;

namespace GoodsApi.Infrastructure.ViewModels;

public class ProviderEditViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Введите название поставщика.")]
    [StringLength(150, ErrorMessage = "Название не должно превышать 150 символов.")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Введите контактное ФИО.")]
    [StringLength(150, ErrorMessage = "ФИО не должно превышать 150 символов.")]
    public string ContactFullName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Введите номер телефона.")]
    [PhoneNumber]
    public string PhoneNumber { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Введите адрес электронной почты.")]
    [EmailAddress(ErrorMessage = "Введите корректный email.")]
    [StringLength(120, ErrorMessage = "Email не должен превышать 120 символов.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Введите адрес.")]
    [StringLength(300, ErrorMessage = "Адрес не должен превышать 300 символов.")]
    public string Address { get; set; } = string.Empty;

    public bool IsEdit => Id != 0;
}
