using System.ComponentModel.DataAnnotations;

namespace Applications.Helpers.CustomValidations;

public sealed class NotAllowZeroAttribute:ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var input=(double)Convert.ChangeType(value!, typeof(double));

        if (input > 0)
        {
            return ValidationResult.Success;
        }

        return new ValidationResult("Value Not Allow Zero value.");
    }

}
