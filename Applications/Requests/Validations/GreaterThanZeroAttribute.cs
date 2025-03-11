using System.ComponentModel.DataAnnotations;

namespace Applications.Requests.Validations;


[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public class GreaterThanZeroAttribute: ValidationAttribute
{
    private bool _allowNegativeNumber = false; 

    public GreaterThanZeroAttribute(bool AllowNegativeNumber=false)
    {
        _allowNegativeNumber = AllowNegativeNumber;
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if ( !_allowNegativeNumber && value is int id && id > 0) 
            return ValidationResult.Success;

        if(_allowNegativeNumber && value is int p && p != 0)
            return ValidationResult.Success;

        var errorMessage = string.Format("{0} must be greater than 0.", validationContext.MemberName!);
        var result = new ValidationResult(errorMessage);

        return result;
    }
}
