using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infrastructures.DataAccess.Conversions
{
    public static class ConversionExtensions
    {
        public static ValueConverter<Guid, string> GuidConversion => new ValueConverter<Guid, string>(v => v.ToString(), x => string.IsNullOrEmpty(x) ? Guid.Empty : Guid.Parse(x));

        //public static ValueConverter<DateTime?, DateTimeOffset?> DateTimeConversion =>
        //    new ValueConverter<DateTime?, DateTimeOffset?>(
        //        v => v,                                                       // To provider conversion
        //        x => x.HasValue ? new DateTimeOffset(x, TimeSpan.Zero) : null      // From provider conversion
        //    );

    }

    public static class IntExtensions
    {
        public static ValueConverter<int, string> GuidConversion => new ValueConverter<int, string>(v => v.ToString(), x => string.IsNullOrEmpty(x) ? 0 : int.Parse(x));

    }

}
