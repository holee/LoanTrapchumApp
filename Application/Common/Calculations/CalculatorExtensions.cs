using Application.Extensions.Maths;
using Domains.Enumerations;
using Microsoft.VisualBasic;
namespace Application.Calculations;

internal static class CalculatorExtensions
{
    internal static decimal TotalPay(decimal? principal, decimal? interestRate, decimal? collectionFee = 0)
    {
        var p = principal ?? 0;

        var i = interestRate ?? 0;

        var c = collectionFee ?? 0;

        return Math.Round(p + i + c, 3);
    }
    internal static decimal TotalPay(this decimal principal, decimal? interestRate = 0, decimal? collectionFee = 0)
    {
        var i = interestRate ?? 0;

        var c = collectionFee ?? 0;

        return Math.Round(principal + i + c, 3);
    }
    internal static decimal TotalPaid(this decimal? principal, decimal? interestRate, decimal? collectionFee = 0)
    {
        var p = principal ?? 0;

        var i = interestRate ?? 0;

        var c = collectionFee ?? 0;

        return Math.Round(p + i + c, 3);
    }
    internal static decimal PaidPrincipal(int? currencyId, decimal? amount, int? nper)
    {

        if (currencyId == 1) return (amount / nper).RoundDown(-3);

        return (amount / nper).RoundDown(3);

    }
    internal static decimal PaidPrincipal(this decimal amount, int? currencyId, int? nper)
    {

        if (currencyId == (int)CurrencyEnum.KHR) return (amount / nper).RoundDown(-3);

        return (amount / nper).RoundDown(2);

    }
    internal static decimal LastBalance(decimal? beginBalance, decimal? principalPaid)
    {

        var begin = beginBalance ?? 0;

        var principal = principalPaid ?? 0;

        return Math.Round(begin - principal, 3);
    }
    internal static decimal LastBalance(this decimal beginBalance, decimal? principalPaid)
    {

        var principal = principalPaid ?? 0;

        return Math.Round(beginBalance - principal, 3);
    }
    internal static decimal CollectionFee(int? currencyId, decimal? amount, decimal? effectiveRate, int? days = 28)
    {

        var rate = effectiveRate ?? 0;

        var ratePerday = Math.Round(rate / 28 / 100, 5);

        var collectionAmount = amount * ratePerday * days;

        if (currencyId == (int)CurrencyEnum.KHR) return collectionAmount.RoundUp(-2);

        return collectionAmount.RoundUp(5);
    }
    internal static decimal CollectionFee(LoanDtoCreate? arg, decimal? amount, int? days = 28)
    {

        decimal rate = arg!.CollectionFeeRate ?? 0;

        decimal ratePerday = Math.Round(rate / 28 / 100, 5);

        decimal collectionAmount = (amount * ratePerday * days) ?? 0;

        if (arg.CurrencyId == (int)CurrencyEnum.KHR) return collectionAmount.RoundUp(-2);

        return collectionAmount.RoundUp(5);
    }
    internal static decimal CollectionFee(this decimal? amount, LoanDtoCreate? arg, int? days = 28)
    {

        decimal rate = arg!.CollectionFeeRate ?? 0;

        decimal ratePerday = Math.Round(rate / 28 / 100, 5);

        decimal collectionAmount = (amount * ratePerday * days) ?? 0;

        if (arg.CurrencyId == (int)CurrencyEnum.KHR) return collectionAmount.RoundUp(-2);

        return collectionAmount.RoundUp(5);
    }
    internal static decimal CollectionFee(this decimal amount, LoanDtoCreate? arg, int? days = 28)
    {

        decimal rate = arg!.CollectionFeeRate ?? 0;

        decimal ratePerday = Math.Round(rate / 28 / 100, 5);

        decimal collectionAmount = (amount * ratePerday * days) ?? 0;

        if (arg.CurrencyId == (int)CurrencyEnum.KHR) return collectionAmount.RoundUp(-2);

        return collectionAmount.RoundUp(5);
    }
    internal static decimal Pmt(int? currencyId, int? nperiod, decimal? rateInMonth, decimal? amount)
    {
        if (amount < 0 || rateInMonth < 0)

            throw new ArgumentException("Amount and Rate must greater than Zero");

        var r = rateInMonth / 100;

        var denominator = Convert.ToDecimal(Math.Pow(1 + (double)r!.Value, nperiod!.Value) - 1);

        var pmt = (r + r / denominator) * amount;

        if (currencyId == 1) return pmt.RoundUp(-2);

        return Math.Round(pmt!.Value, 2);
    }
    internal static decimal Interest(int? currencyId, decimal? amount, decimal? rate, int? numOfDay, int? period = 28)
    {
        var r = rate ?? 0;

        var p = period ?? 0;

        decimal ratePerDay = r / p;

        var totalPay = amount * (ratePerDay / 100) * numOfDay ?? 0;

        if (currencyId == (int)CurrencyEnum.KHR) return totalPay.RoundUp(-2);

        return Math.Round(totalPay, 5);
    }
    internal static decimal Interest(this decimal amount, int? currencyId, decimal? rate, int? numOfDay, int? period = 28)
    {
        var r = rate ?? 0;

        var p = period ?? 0;

        decimal ratePerDay = r / p;

        var totalPay = amount * (ratePerDay / 100) * numOfDay ?? 0;

        if (currencyId == (int)CurrencyEnum.KHR) return totalPay.RoundUp(-2);

        return Math.Round(totalPay, 5);
    }
    internal static decimal CalcPmt(int? currencyId, int? period, decimal? rate, decimal? amount)
    {
        var pv = Convert.ToDouble(amount);

        var rateATime = Convert.ToDouble(rate) / 100;

        var principle = Financial.Pmt(rateATime, period ?? 0, pv);

        var principleDouble = Convert.ToDecimal(principle);

        if (currencyId == 1)
        {
            return principleDouble.RoundDown(-2);
        }
        return principleDouble.RoundDown(2);
    }

    internal static decimal CalcPmt(this decimal? amount, int? currencyId, int? period, decimal? rate)
    {
        var pv = Convert.ToDouble(amount);

        var rateATime = Convert.ToDouble(rate) / 100;

        var principle = Financial.Pmt(rateATime, period ?? 0, -pv);

        var principleDouble = Convert.ToDecimal(principle);

        if (currencyId == 1)
        {
            return principleDouble.RoundDown(-2);
        }
        return principleDouble.RoundDown(2);
    }



    internal static decimal LastScheduleAmount(this decimal baseAmount, int? currencyId, int? period, decimal? principalPerInstallment)
    {

        var lastAmount = (baseAmount - (principalPerInstallment * (period - 1))) ?? 0.00M;

        if (currencyId == (int)CurrencyEnum.KHR) return lastAmount.RoundUp(-2);

        return Math.Round(lastAmount, 2);
    }

    internal static decimal LastScheduleAmount(this decimal? baseAmount, int? currencyId, int? period, decimal? principalPerInstallment)
    {
         
        decimal lastAmount = (baseAmount - (principalPerInstallment * (period - 1))) ?? 0.00M;

        if (currencyId == (int)CurrencyEnum.KHR) return lastAmount.RoundUp(-2);

        return Math.Round(lastAmount, 2);
    }

    internal static int TotalDays(DateTimeOffset? start, DateTimeOffset? end)
    {
        var start1 = start ?? DateTime.Now;

        var end1 = end ?? DateTime.Now;

        TimeSpan dateDiff = end1.Subtract(start1);

        return dateDiff.Days;
    }
    internal static int TotalDays(this DateTime? start, DateTime? end)
    {
        var start1 = start ?? DateTime.Now;

        var end1 = end ?? DateTime.Now;

        TimeSpan dateDiff = end1.Subtract(start1);

        return dateDiff.Days;
    }
    internal static int TotalDays(this DateTime start, DateTime? end)
    {

        var end1 = end ?? DateTime.Now;

        TimeSpan dateDiff = end1.Subtract(start);

        return dateDiff.Days;
    }
}