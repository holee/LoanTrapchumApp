using Applications.Enumerations;
using Microsoft.AspNetCore.Components;
using System.Globalization;

namespace Applications.Extensions.Maths
{
    public static class MathExtension
    {
        private static readonly CultureInfo Culture = CultureInfo.InvariantCulture;

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

        public static MarkupString FormatUp(this decimal? number, int decimalPlaces, int? currencyId)
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
        public static MarkupString Format(this decimal d, int? currencyId)
        {
            return currencyId switch
            {
                1 => (MarkupString)string.Format("{0:#,##0.00} {1}", d, "&#6107;"),
                2 => (MarkupString)string.Format("{0:#,##0.00} {1}", d, "&dollar;"),
                _ => (MarkupString)string.Format("{0:#,##0.00} {1}", d, "&#6107;"),
            };
        }
        public static MarkupString Format(this decimal d, CurrenciesType type = CurrenciesType.Khr)
        {
            return type switch
            {
                CurrenciesType.Khr => (MarkupString)string.Format("{0:#,##0.00} {1}", d, "&#6107;"),
                CurrenciesType.Usd => (MarkupString)string.Format("{0:#,##0.00} {1}", d, "&dollar;"),
                _ => (MarkupString)string.Format("{0:#,##0.00} {1}", d, "&#6107;"),
            };
        }
        public static MarkupString Format(this decimal? d, int? currencyId = 1)
        {

            return currencyId switch
            {
                1 => (MarkupString)string.Format("{0:##,0.##} {1}", d, "&#6107;"),
                2 => (MarkupString)string.Format("{0:##,0.00} {1}", d, "&dollar;"),
                _ => (MarkupString)string.Format("{0:##,0.##} {1}", d, "&#6107;"),
            };
        }


        public static string ToFormattedNumber(this decimal value, int decimalPlaces = 2)
        {
            var format = $"#,##0.{new string('0', decimalPlaces)}";
            return value.ToString(format, Culture);
        }

        public static string ToFormattedNumber(this decimal? value, int decimalPlaces = 2)
        {
            return (value ?? 0m).ToFormattedNumber(decimalPlaces);
        }

        public static MarkupString Format(this decimal? d, CurrenciesType type)
        {
            return type switch
            {
                CurrenciesType.Khr => (MarkupString)string.Format("{0:#,00.00} {1}", d, "&#6107;"),
                CurrenciesType.Usd => (MarkupString)string.Format("{0:#,00.00} {1}", d, "&dollar;"),
                _ => (MarkupString)string.Format("{0:#,00.00} {1}", d, "&#6107;"),
            };
        }

        public static string Formats(this decimal? d, int? currencyId)
        {
            return currencyId switch
            {
                1 => string.Format(UserCulture(CurrenciesType.Khr), "{0:C}", d),
                2 => string.Format(UserCulture(CurrenciesType.Usd), "{0:C}", d),
                _ => string.Format(UserCulture(CurrenciesType.Usd), "{0:C}", d),
            };
        }

        public static string AsPercent(this decimal? d)
        {
            return $"{d:N2}%";
        }

        public static string SetFormat(this decimal? d, int? currencyId)
        {
            return currencyId switch
            {
                1 => string.Format(UserCulture(CurrenciesType.Khr), "{0:C}", d),
                2 => string.Format(UserCulture(CurrenciesType.Usd), "{0:C}", d),
                _ => string.Format(UserCulture(CurrenciesType.Khr), "{0:C}", d),
            };
        }
        private static CultureInfo UserCulture(CurrenciesType type)
        {
            return type switch
            {
                CurrenciesType.Khr => new CultureInfo("km-KH"),
                CurrenciesType.Usd => new CultureInfo("en-US"),
                CurrenciesType.Euro => new CultureInfo("en-GB"),
                CurrenciesType.China => new CultureInfo("zh-CN"),
                CurrenciesType.Thai => new CultureInfo("th-TH"),
                _ => new CultureInfo("km-KH")
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

