using Microsoft.AspNetCore.Components;

namespace Applications.Extensions.Maths
{
    public static class NumerFormatExtensions
    {

        public static string FormatN(this decimal? d, string formatter)
        {
            var formatStringValue = d ?? 0;
            return string.Format($"{{0:{formatter}}}", formatStringValue);

        }
    }
}
