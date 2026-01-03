namespace Applications.Enumerations;

public record BaseEnum(int Value, string Name);
public static class EnumExtension
{
    public static string[] GetNamesExcluding<TEnum>(this TEnum _, params TEnum[] excluded) where TEnum : struct, Enum
    {
        var excludeNames = excluded.Select(e => e.ToString()).ToHashSet();

        return Enum.GetValues<TEnum>()
                   .Select(e => e.ToString())
                   .Where(name => !excludeNames.Contains(name))
                   .ToArray();
    }

    public static string[] Only<TEnum>(this TEnum _, params TEnum[] source) where TEnum : struct, Enum
    {
        var excludeNames = source.Select(e => e.ToString()).ToHashSet();

        return Enum.GetValues<TEnum>()
                   .Select(e => e.ToString())
                   .Where(name => excludeNames.Contains(name))
                   .ToArray();
    }


    public static string[] GetValueOnly<TEnum>(this TEnum _, params TEnum[] only) where TEnum : struct, Enum
    {
        var excludeNames = only.Select(e => e.ToString()).ToHashSet();

        return Enum.GetValues<TEnum>()
                   .Select(e => e.ToString())
                   .Where(name => excludeNames.Contains(name))
                   .ToArray();
    }

    public static BaseEnum[] ToEnumArray<TEnum>(this TEnum _, params TEnum[] exclude)
            where TEnum : struct, Enum
    {
        return Enum.GetValues<TEnum>()
                   .Where(e => !exclude.Contains(e))
                   .Select(e => new BaseEnum(ConvertToInt(e), e.ToString()))
                   .OrderBy(be => be.Name)
                   .ToArray();
    }

    private static int ConvertToInt<TEnum>(TEnum enumValue) where TEnum : struct, Enum
    {
        return Convert.ToInt32(enumValue);
    }


    public static TEnum ToEnum<TEnum>(this string value) where TEnum : struct, Enum
    {
        if (Enum.TryParse<TEnum>(value, ignoreCase: true, out var result))
            return result;

        throw new ArgumentException(
            $"'{value}' is not a valid value for enum '{typeof(TEnum).Name}'.");
    }


    public static bool TryToEnum<TEnum>(this string? value, out TEnum result) where TEnum : struct, Enum
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            result = default;
            return false;
        }

        return Enum.TryParse(value, ignoreCase: true, out result);
    }
}

