namespace Applications.Enumerations;
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

    public static string[] GetValueOnly<TEnum>(this TEnum _, params TEnum[] only) where TEnum : struct, Enum
    {
        var excludeNames = only.Select(e => e.ToString()).ToHashSet(); 

        return Enum.GetValues<TEnum>()
                   .Select(e => e.ToString())
                   .Where(name => excludeNames.Contains(name))
                   .ToArray();
    }
}

