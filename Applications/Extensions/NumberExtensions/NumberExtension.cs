using System.Numerics;

namespace Applications.Extensions.NumberExtions
{
    public static class NumberExtension
    {
        public static T ToNumer<T>(this string type) where T : IConvertible, INumber<T>
        {
            var testValue = type.Length == 0 ? "0" : type;

            var value = (T)Convert.ChangeType(testValue, typeof(T));

            return value;
        }

        public static T ToNumer<T>(this object type) where T : IConvertible, INumber<T>
        {
            var testValue = type.ToString()?.Length == 0 ? "0" : type.ToString()!;

            var value = (T)Convert.ChangeType(testValue, typeof(T));

            return value;
        }

    
    }
}
