using ErrorOr;

namespace TrapchumApp.Helpers.Extensions
{
    public static class BlazorErrorOrExtensions
    {
        public static T GetValueOrThrow<T>(this ErrorOr<T> errorOr)
        {
            if (errorOr.IsError)
            {
                throw new InvalidOperationException(string.Join(", ", errorOr.Errors.Select(e => e.Description)));
            }
            return errorOr.Value;
        }

        public static bool IsSuccess<T>(this ErrorOr<T> errorOr)
        {
            return !errorOr.IsError;
        }

        public static string GetMessage<T>(this ErrorOr<T> errorOr)
        {
            if (errorOr.IsError)
            {
                return errorOr.FirstError.Description;
            }
            return string.Empty; 
        }

    }
}
