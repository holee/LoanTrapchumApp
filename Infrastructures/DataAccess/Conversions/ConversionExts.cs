using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infrastructures.DataAccess.Conversions
{
    public static class ConversionExts
    {
        public static ValueConverter<Guid, string> GuidConversion => new ValueConverter<Guid, string>(v => v.ToString(), x => string.IsNullOrEmpty(x) ? Guid.Empty : Guid.Parse(x));
    
    }

    public static class IntExtensions
    {
        public static ValueConverter<int, string> GuidConversion => new ValueConverter<int, string>(v => v.ToString(), x => string.IsNullOrEmpty(x) ? 0 : int.Parse(x));

    }

}
