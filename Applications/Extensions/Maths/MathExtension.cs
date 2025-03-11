using Microsoft.AspNetCore.Components;

namespace Applications.Extensions.Maths
{
    public static class MathExtension
    {

        public static decimal RoundUsd(this decimal? number, int decimalPlaces)
        {
            return Math.Round(number!.Value, decimalPlaces);
        }
        public static decimal RoundUsd(this decimal number, int decimalPlaces)
        {
            return Math.Round(number, decimalPlaces);
        }
        public static double RoundDown(this double number, int decimalPlaces)
        {
            return Math.Floor(number * Math.Pow(10, decimalPlaces)) / Math.Pow(10, decimalPlaces);
        }
        public static decimal RoundDown(this decimal number, int decimalPlaces)
        {
            return Math.Floor(number * (decimal)Math.Pow(10, decimalPlaces)) / (decimal)Math.Pow(10, decimalPlaces);
        }
        public static decimal RoundDown(this decimal? number, int decimalPlaces)
        {
            number ??= 0;
            return Math.Floor(number.Value * (decimal)Math.Pow(10, decimalPlaces)) / (decimal)Math.Pow(10, decimalPlaces);
        }
        public static decimal RoundDownKhr(this decimal number, int decimalPlaces)
        {
            return number.RoundDown(-decimalPlaces);
        }
        public static decimal RoundDownKhr(this decimal? number, int decimalPlaces)
        {
            number ??= 0;
            return number.RoundDown(-decimalPlaces);
        }
        public static decimal RoundUpKhr(this decimal number, int decimalPlaces)
        {
            return number.RoundUp(-decimalPlaces);
        }
        public static decimal RoundUpKhr(this decimal? number, int decimalPlaces)
        {
            return number!.Value.RoundUp(-decimalPlaces);
        }
        public static double RoundUp(this double number, int decimalPlaces)
        {
            return Math.Ceiling(number * Math.Pow(10, decimalPlaces)) / Math.Pow(10, decimalPlaces);
        }
        public static decimal RoundUp(this decimal number, int decimalPlaces)
        {
            return Math.Ceiling(number * (decimal)Math.Pow(10, decimalPlaces)) / (decimal)Math.Pow(10, decimalPlaces);
        }
        public static decimal RoundUp(this decimal? number, int decimalPlaces)
        {
            number ??= 0;
            return Math.Ceiling(number.Value * (decimal)Math.Pow(10, decimalPlaces)) / (decimal)Math.Pow(10, decimalPlaces);
        }
        public static decimal RoundUp(this decimal number, int decimalPlaces, int currencyId)
        {
            if (currencyId == 1)
            {
                return number.RoundUp(-decimalPlaces);
            }
            else
            {
                return number.RoundUp(decimalPlaces);
            }

        }
        public static decimal RoundUp(this decimal? number, int decimalPlaces, int currencyId)
        {
            var arg = number ?? 0;

            if (currencyId == 1)
            {
                return arg.RoundUp(-decimalPlaces);
            }
            else
            {
                return arg.RoundUp(decimalPlaces);
            }
        }
        public static decimal RoundDown(this decimal number, int decimalPlaces, int currencyId)
        {
            if (currencyId == 1)
            {
                return number.RoundDown(-decimalPlaces);
            }
            else
            {
                return number.RoundDown(decimalPlaces);
            }
        }
        public static decimal RoundDown(this decimal? number, int decimalPlaces, int currencyId)
        {
            var arg = number ?? 0;

            if (currencyId == 1)
            {
                return arg.RoundDown(-decimalPlaces);
            }
            else
            {
                return arg.RoundDown(decimalPlaces);
            }
        }
        public static decimal Round(decimal? value, int decimalPlaces)
        {
            if (value is null) return 0;
            return Math.Round(value!.Value, decimalPlaces);

        }

        /// <summary>
        /// Format Currency
        /// </summary>
        /// <param name="number"></param>
        /// <param name="decimalPlaces"></param>
        /// <param name="currencyId"></param>
        /// <returns></returns>

        public static MarkupString FormatDown(this decimal? number, int decimalPlaces, int currencyId)
        {
            var arg = number ?? 0;

            var rounding = 0.0M;

            if (currencyId == 1)
            {
                rounding = arg.RoundDown(-decimalPlaces);
            }
            else
            {
                rounding = arg.RoundDown(decimalPlaces);
            }

            return rounding.Format(currencyId);

        }
        public static MarkupString FormatUp(this decimal? number, int decimalPlaces, int currencyId)
        {
            var arg = number ?? 0;

            var rounding = 0.0M;

            if (currencyId == 1)
            {
                rounding = arg.RoundUp(-decimalPlaces);
            }
            else
            {
                rounding = arg.RoundUp(decimalPlaces);
            }

            return rounding.Format(currencyId);
        }
        public static MarkupString FormatDown(this decimal number, int decimalPlaces, int currencyId)
        {
            var rounding = 0.0M;

            if (currencyId == 1)
            {
                rounding = number.RoundDown(-decimalPlaces);
            }
            else
            {
                rounding = number.RoundDown(decimalPlaces);
            }

            return rounding.Format(currencyId);


        }
        public static MarkupString FormatUp(this decimal number, int decimalPlaces, int currencyId)
        {
            var rounding = 0.0M;

            if (currencyId == 1)
            {
                rounding = number.RoundUp(-decimalPlaces);
            }
            else
            {
                rounding = number.RoundUp(decimalPlaces);
            }

            return rounding.Format(currencyId);
        }
        private static MarkupString Format(this decimal d, int? currencyRefId)
        {
            return currencyRefId switch
            {
                1 => (MarkupString)string.Format("{0:#,00.00} {1}", d, "&#6107;"),
                2 => (MarkupString)string.Format("{0:#,00.00} {1}", d, "&dollar;"),
                _ => (MarkupString)string.Format("{0:#,00.00} {1}", d, "&#6107;"),
            };
        }
        private static MarkupString Format(this decimal? d, int? currencyRefId)
        {
            return currencyRefId switch
            {
                1 => (MarkupString)string.Format("{0:#,00.00} {1}", d, "&#6107;"),
                2 => (MarkupString)string.Format("{0:#,00.00} {1}", d, "&dollar;"),
                _ => (MarkupString)string.Format("{0:#,00.00} {1}", d, "&#6107;"),
            };
        }
        public static string Usd(int? value)
        {
            value = value ?? 0;

            return string.Format("{0:#,0.00}", value);
        }
        public static string Usd(decimal? value)
        {
            value = value ?? 0;
            return string.Format("{0:#,0.00}", value);
        }
        public static string Khr(int? value)
        {
            value ??= 0;

            return string.Format("{0:#,0}", value);
        }
        public static string Khr(decimal? value)
        {
            value ??= 0;
            return string.Format("{0:#,0}", value);
        }

        public static string Seperator(this decimal? value, int? currencyId = 1)
        {

            value ??= 0;

            return currencyId switch
            {
                1 => Khr(value),
                2 => Usd(value),
                _ => Khr(value),
            };
        }

        public static string Seperator(this int? value, int? currencyId = 1)
        {
            value ??= 0;

            return currencyId switch
            {
                1 => Khr(value),
                2 => Usd(value),
                _ => Khr(value),
            };
        }

    }
}

