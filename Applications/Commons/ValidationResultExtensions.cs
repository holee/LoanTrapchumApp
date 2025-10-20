using System.Linq;
using ErrorOr;
using FluentValidation.Results;

namespace Applications.Extensions;

public static class ValidationResultExtensions
{
    public static ErrorOr<T> ToErrorOr<T>(this ValidationResult validationResult)
    {
        var errors = validationResult.Errors
            .Select(f => Error.Validation(f.PropertyName, f.ErrorMessage))
            .ToList();

        return errors;
    }
}