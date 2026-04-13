using System.ComponentModel.DataAnnotations;
using GoodsApi.Infrastructure.Models.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GoodsApi.Infrastructure.ViewModels;

public class SupplyRegistrationViewModel
{
    public List<SelectListItem> Providers { get; set; } = new();

    public ProductInfo ProductInfo { get; set; } = new();

    public int UserId { get; set; }

    [Display(Name = "Поставщик")]
    [Required(ErrorMessage = "Выберите поставщика")]
    public int ProviderId { get; set; }

    [Display(Name = "Количество")]
    [Required(ErrorMessage = "Введите количество")]
    [Range(1, int.MaxValue, ErrorMessage = "Количество должно быть больше 0")]
    public int Amount { get; set; }

    [Display(Name = "Закупочная цена")]
    [Required(ErrorMessage = "Введите закупочную цену")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Цена должна быть больше 0")]
    public decimal PurchasePrice { get; set; }

    [Display(Name = "Дата поставки")]
    [Required(ErrorMessage = "Укажите дату")]
    public DateTime AcceptanceDate { get; set; }
}