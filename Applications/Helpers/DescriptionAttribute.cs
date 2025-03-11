using System.Reflection;

namespace Applications.Helpers
{
    public class DescriptionAttribute : Attribute, IDescription
    {
        public string Description { get; set; }

        public DescriptionAttribute(string description)
        {
            Description = description;
        }
    }


    public static class DisplayAttributeExtensions
    {

        public static TAttribute GetAttribute<TAttribute>(this Enum enumValue)
           where TAttribute : Attribute
        {
            var value = enumValue.GetType()
                            .GetMember(enumValue.ToString())
                            .First()
                            .GetCustomAttribute<TAttribute>();

            return value!;
        }
    }



}
