using System.Globalization;
using System.Numerics;

namespace Application.Extensions.NumberExtensions
{
    public static class NumberExtension
    {
        public static T ToNumer<T>(this string type) where T : IConvertible, INumber<T>
        {
            var testValue = type.Length == 0 ? "0" : type;

            var value = (T)Convert.ChangeType(testValue, typeof(T));

            return value;
        }

        public static T ToNumber<T>(this object input) where T : struct, IConvertible, INumber<T>
        {

            var testValue = input.ToString()?.Length == 0 ? "0" : input.ToString()!;

            var value = (T)Convert.ChangeType(testValue, typeof(T));

            return value;

            //if (input == null || string.IsNullOrWhiteSpace(input.ToString()))
            //{
            //    throw new ArgumentNullException(nameof(input), "Input cannot be null or empty");
            //}

            //Type targetType = typeof(T);

            //if (!typeof(IConvertible).IsAssignableFrom(targetType))
            //{
            //    throw new InvalidCastException($"Target type {targetType.FullName} is not convertible.");
            //}

            //try
            //{
            //    return (T)Convert.ChangeType(input, targetType, CultureInfo.InvariantCulture);
            //}
            //catch (Exception ex)
            //{
            //    throw new InvalidCastException($"Cannot convert value '{input}' to type {targetType.Name}.", ex);
            //}

        }

    }
}
