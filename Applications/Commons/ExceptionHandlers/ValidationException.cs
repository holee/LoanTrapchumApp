using FluentValidation.Results;

namespace Applications.Commons.ExceptionHandlers;

public sealed class ValidationException : Exception
{
    private const string DefaultMessage = "One or more validation failures have occurred.";

    public IReadOnlyList<ValidationFailure> Errors { get; }

    public ValidationException(IEnumerable<ValidationFailure> errors)
        : base(DefaultMessage)
    {
        ArgumentNullException.ThrowIfNull(errors);
        Errors = [.. errors];
    }

    public ValidationException(ValidationResult validationResult)
        : this((validationResult ?? throw new ArgumentNullException(nameof(validationResult))).Errors)
    {
    }
}
