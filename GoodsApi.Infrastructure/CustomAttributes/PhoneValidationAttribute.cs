using System.ComponentModel.DataAnnotations;
using PhoneNumbers;

namespace GoodsApi.Infrastructure.CustomAttributes;

public class PhoneNumberAttribute : ValidationAttribute
{
    private readonly string _defaultRegion;

    public PhoneNumberAttribute(string defaultRegion = "RU")
    {
        _defaultRegion = defaultRegion;
        ErrorMessage = "Введите корректный номер телефона.";
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null)
            return ValidationResult.Success;

        var phoneUtil = PhoneNumberUtil.GetInstance();

        try
        {
            var number = phoneUtil.Parse(value.ToString(), _defaultRegion);

            if (!phoneUtil.IsValidNumber(number))
                return new ValidationResult(ErrorMessage);

            return ValidationResult.Success;
        }
        catch
        {
            return new ValidationResult(ErrorMessage);
        }
    }
}