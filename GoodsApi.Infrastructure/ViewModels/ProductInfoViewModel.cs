using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GoodsApi.Infrastructure.ViewModels;

public class ProductInfoEditViewModel
{
    public int Id { get; set; }

    [Display(Name = "Товар")]
    [Required(ErrorMessage = "Выберите товар.")]
    public int ProductId { get; set; }

    [Display(Name = "Артикул")]
    [Required(ErrorMessage = "Введите артикул.")]
    [StringLength(100, ErrorMessage = "Артикул не должен превышать 100 символов.")]
    public string Article { get; set; } = string.Empty;

    [Display(Name = "Цена")]
    [Required(ErrorMessage = "Введите цену.")]
    [Range(0.01, 999999999, ErrorMessage = "Цена должна быть больше 0.")]
    public decimal Price { get; set; }

    [Display(Name = "Размер")]
    [Required(ErrorMessage = "Введите размер.")]
    [Range(0.01, 999999999, ErrorMessage = "Размер должен быть больше 0.")]
    public decimal Size { get; set; }

    [Display(Name = "Цвет")]
    [Required(ErrorMessage = "Введите цвет.")]
    [StringLength(100, ErrorMessage = "Цвет не должен превышать 100 символов.")]
    public string Color { get; set; } = string.Empty;

    [Display(Name = "Количество")]
    [Required(ErrorMessage = "Введите количество.")]
    [Range(0, int.MaxValue, ErrorMessage = "Количество не может быть отрицательным.")]
    public int Amount { get; set; }

    [Display(Name = "Расположение")]
    [Required(ErrorMessage = "Введите расположение.")]
    [StringLength(200, ErrorMessage = "Расположение не должно превышать 200 символов.")]
    public string Location { get; set; } = string.Empty;

    public List<SelectListItem> Products { get; set; } = new();

    public bool IsEdit => Id != 0;

    public string PageTitle => IsEdit
        ? "Редактирование товара"
        : "Добавление товара";

    public string SubmitButtonText => IsEdit
        ? "Сохранить"
        : "Создать";
}