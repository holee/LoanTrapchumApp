using FluentValidation.Results;

namespace Applications.Exceptions;

public sealed class MediatorException : Exception
{
    private readonly List<string> _errorMessages = new();

    public List<string> ErrorMessages => _errorMessages;

    public IDictionary<string, string[]> Errors { get; } = new Dictionary<string, string[]>(StringComparer.OrdinalIgnoreCase);

    public MediatorException(string message) : base(message)
    {
        if (!string.IsNullOrWhiteSpace(message))
        {
            ErrorMessages.Add(message);
        }
    }
    
    
    public MediatorException(ValidationResult validationResult)
    {
        if (validationResult == null)
            throw new ArgumentNullException(nameof(validationResult));

        if (!validationResult.IsValid)
        {
            Errors = validationResult.ToDictionary();

            foreach (var error in validationResult.Errors)
            {
                if (!string.IsNullOrWhiteSpace(error.ErrorMessage))
                    ErrorMessages.Add(error.ErrorMessage);
            }
        }
    }
    
}
