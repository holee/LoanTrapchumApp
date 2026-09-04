using System.ComponentModel.DataAnnotations;

namespace Application.Requests.Validations
{
    public class PasteValidationAttribute : ValidationAttribute
    {

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            return base.IsValid(value, validationContext);
        }
    }
}
