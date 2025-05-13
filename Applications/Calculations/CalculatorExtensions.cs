using Applications.Extensions.Maths;
using Microsoft.VisualBasic;
namespace Applications.Calculations;

internal class CalculatorExtensions
{
    internal static decimal TotalPay(decimal? principal, decimal? interestRate, decimal? collectionFee = 0)
    {
        var p = principal ?? 0;

        var i = interestRate ?? 0;

        var c = collectionFee ?? 0;

        return Math.Round(p + i + c, 3);
    }
    internal static decimal PaidPrincipal(int? currencyId, decimal? amount, int? nper)
    {

        if (currencyId == 1) return (amount / nper).RoundDown(-2);

        return (amount / nper).RoundDown(3);

    }
    internal static decimal CalcLastBalance(decimal? beginBalance, decimal? principalPaid)
    {

        var begin = beginBalance ?? 0;

        var principal = principalPaid ?? 0;

        return Math.Round(begin - principal, 3);
    }
    internal static decimal CalcCollectionFee(int? currencyId, decimal? amount, decimal? effectiveRate, int? days = 30)
    {

        var rate = effectiveRate ?? 0;

        var ratePerday = Math.Round(rate / 28 / 100, 3);

        var collectionAmount = amount * ratePerday * days;

        if (currencyId == 1) return collectionAmount.RoundUp(-2);

        return collectionAmount.RoundUp(2);
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

    internal static decimal CalcInterest(int? currencyId, decimal? amount, decimal? rate, int? numOfDay, int? period = 28)
    {
        var r = rate ?? 0;

        var p = period ?? 0;

        decimal ratePerDay = r / p;

        var totalPay = amount * (ratePerDay / 100) * numOfDay ?? 0;

        if (currencyId == 1) return totalPay.RoundUp(-2);

        return Math.Round(totalPay, 2);
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

    internal static int TotalDays(DateTimeOffset? start, DateTimeOffset? end)
    {
        TimeSpan dateDiff = end!.Value.Subtract(start!.Value);

        return dateDiff.Days;
    }
}