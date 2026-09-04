using Application.Extensions.Attrs;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Reflection;

namespace Application.Extensions.Enums;



public static class EnumExtensions
{


    public static string GetDisplayName(this Enum enumValue)
    {
        string name = enumValue.GetType()!
                        .GetMember(enumValue.ToString())
                        .First()
                        .GetCustomAttribute<DisplayAttribute>()!
                        .GetName() ?? string.Empty;

        return name;
    }

    public static int AsInt(this Enum enumValue)
    {
        return Convert.ToInt32(enumValue);
    }
    public static string GetDescription<U>(this IConvertible e) where U : class, IDescription
    {
        if (e is Enum)
        {
            Type type = e.GetType();
            Array values = Enum.GetValues(type);

            foreach (int val in values)
            {
                if (val == e.ToInt32(CultureInfo.InvariantCulture))
                {
                    var memInfo = type.GetMember(type.GetEnumName(val)!);
                    var descriptionAttribute = memInfo[0]
                        .GetCustomAttributes(typeof(U), false)
                        .FirstOrDefault() as U;

                    if (descriptionAttribute != null)
                    {
                        return descriptionAttribute.Description;
                    }
                }
            }
        }
        return string.Empty; // could also return string.Empty
    }


    public static bool TryToEnum<TEnum>(string? value, out TEnum result) where TEnum : struct, Enum
    {
        return Enum.TryParse(value, ignoreCase: true, out result);
    }


    public static string ReadDescription<U>(this Enum e) where U : class, IDescription
    {
        Type type = e.GetType();

        Array values = Enum.GetValues(type);

        foreach (int val in values)
        {
            if (val == e.AsInt())
            {
                var memInfo = type.GetMember(type.GetEnumName(val)!);
                var descriptionAttribute = memInfo[0]
                    .GetCustomAttributes(typeof(U), false)
                    .FirstOrDefault() as U;
                if (descriptionAttribute != null)
                {
                    return descriptionAttribute.Description;
                }
            }
        }
        return string.Empty; // could also return string.Empty
    }

    public static string Description(this Enum value)
    {

        var type = value.GetType();

        var name = Enum.GetName(type, value);

        if (name == null)
            return value.ToString(); // fallback for invalid values

        var field = type.GetField(name);

        if (field == null)
            return name;

        var attr = field.GetCustomAttribute<System.ComponentModel.DescriptionAttribute>();

        return attr?.Description ?? name;
    }

}
