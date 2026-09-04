using FluentValidation.Results;

namespace Application.Exceptions
{
    public sealed class BadRequestExceptions : Exception
    {
        public List<string> ErrorMessage { get; } = new();

        public IDictionary<string, string[]> Errors { get; } = new Dictionary<string, string[]>(StringComparer.OrdinalIgnoreCase);

        public bool HasError => Errors.Any() || ErrorMessage.Any();

        public BadRequestExceptions(string message) : base(message)
        {
            if (!string.IsNullOrWhiteSpace(message))
            {
                ErrorMessage.Add(message);
            }
        }

        public BadRequestExceptions(ValidationResult validationResult)
        {
            if (validationResult == null)
                throw new ArgumentNullException(nameof(validationResult));

            if (!validationResult.IsValid)
            {
                Errors = validationResult.ToDictionary();

                foreach (var error in validationResult.Errors)
                {
                    if (!string.IsNullOrWhiteSpace(error.ErrorMessage))
                        ErrorMessage.Add(error.ErrorMessage);
                }
            }
        }

        public string GetError(int index)
        {
            if (index >= 0 && index < ErrorMessage.Count)
                return ErrorMessage[index];

            return string.Empty;
        }

        public string GetError(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                return string.Empty;

            if (Errors.TryGetValue(key, out var errors) && errors.Any())
                return errors.First();

            return string.Empty;
        }

        public string[] GetErrors(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                return Array.Empty<string>();

            if (Errors.TryGetValue(key, out var errors))
                return errors;

            return Array.Empty<string>();
        }
    }

}
