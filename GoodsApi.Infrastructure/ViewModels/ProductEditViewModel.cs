using System.ComponentModel.DataAnnotations;

namespace GoodsApi.Infrastructure.ViewModels;

public class ProductEditViewModel
{
    public int Id { get; set; }

    [Display(Name = "Название товара")]
    [Required(ErrorMessage = "Введите название товара.")]
    [StringLength(200, ErrorMessage = "Название товара не должно превышать 200 символов.")]
    public string Name { get; set; } = string.Empty;

    [Display(Name = "Категория")]
    [Required(ErrorMessage = "Введите категорию.")]
    [StringLength(100, ErrorMessage = "Категория не должна превышать 100 символов.")]
    public string Category { get; set; } = string.Empty;

    [Display(Name = "Бренд")]
    [Required(ErrorMessage = "Введите бренд.")]
    [StringLength(100, ErrorMessage = "Бренд не должен превышать 100 символов.")]
    public string Brand { get; set; } = string.Empty;

    [Display(Name = "Минимальный остаток")]
    [Required(ErrorMessage = "Введите минимальный остаток.")]
    [Range(0, 999999999, ErrorMessage = "Минимальный остаток не может быть отрицательным.")]
    public decimal MinimumRemain { get; set; }

    public bool IsEdit => Id != 0;

    public string PageTitle => IsEdit
        ? "Редактирование товара"
        : "Добавление товара";

    public string SubmitButtonText => IsEdit
        ? "Сохранить изменения"
        : "Создать товар";
}