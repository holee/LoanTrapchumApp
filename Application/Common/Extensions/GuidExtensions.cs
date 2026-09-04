namespace Application.Extensions
{
    public static class StringExtensions
    {
        public static bool IsNotNullOrEmpty(this string? value)
        {
            return !string.IsNullOrEmpty(value) || !string.IsNullOrWhiteSpace(value);
        }
        public static bool IsNullOrEmpty(this string? value)
        {
            return string.IsNullOrEmpty(value);
        }
    }
    public static class GuidExtensions
    {
        public static bool NotEmpty(this Guid? value)
        {
            return value.HasValue && value != Guid.Empty;
        }

        public static bool IsNullOrEmpty(this Guid? value)
        {
            return !value.HasValue || value == Guid.Empty;
        }


        public static bool IsNotNullOrEmpty(this Guid? value)
        {
            return value != null && value != Guid.Empty;
        }

        public static bool NotEmpty(this Guid value)
        {
            return !value.NotEmpty();
        }


        public static Guid ToGuid(this object value)
        {
            if (Guid.TryParse(value.ToString(), out Guid result))
            {
                return result;
            }

            return Guid.Empty;
        }

        public static Guid ToGuid(this string value)
        {
            if (string.IsNullOrEmpty(value)) return Guid.Empty;

            if (Guid.TryParse(value, out Guid result))
            {
                return result;
            }

            return Guid.Empty;
        }
    }
}