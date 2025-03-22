using Applications.Extensions.Maths;
using Microsoft.VisualBasic;
namespace TCLMS.Shared.Entities.LoanCalculators;

internal class CalculatorExtensions
{
    internal static decimal TotalPay(decimal principal, decimal interestRate, decimal collectionFee = 0)
    {
        return Math.Round((principal + interestRate + collectionFee), 3);
    }
    internal static decimal PaidPrincipal(int currencyId, decimal? amount, int? nper)
    {

        if (currencyId == 1) return MathExtension.RoundDown((amount / nper), -2);

        return MathExtension.RoundDown((amount / nper), 3);

    }
    internal static decimal LastBalance(decimal beginBalance, decimal principalPaid)
    {
        return Math.Round((beginBalance - principalPaid), 3);
    }
    internal static decimal CalcEffective(int currencyId, decimal? amount, decimal? effectiveRate, int? days = 30)
    {
        var ratePerday = Math.Round((effectiveRate!.Value / 28) / 100, 3);

        var effective = (amount * ratePerday) * days;

        if (currencyId == 1) return MathExtension.RoundUp(effective, -2);

        return MathExtension.RoundUp(effective, 2);
    }


    internal static decimal Pmt(int currencyId, int nperiod, decimal rateInMonth, decimal amount)
    {
        if (amount < 0 || rateInMonth < 0)
            throw new ArgumentException("Amount and Rate must greater than Zero");
        var r = rateInMonth / 100;
        var denominator = Convert.ToDecimal(Math.Pow((1 + (double)r), nperiod) - 1);
        var pmt = (r + (r / denominator)) * amount;
        if (currencyId == 1) return MathExtension.RoundUp(pmt, -2);
        return Math.Round(pmt, 2);
    }

    internal static decimal CalcInterest(int currencyId, decimal amount, decimal rate, int numOfDay, int calPeriod = 28)
    {
        decimal ratePerDay = rate / calPeriod;

        var totalPay = (amount * (ratePerDay / 100)) * numOfDay;

        if (currencyId == 1) return MathExtension.RoundUp(totalPay, -2);

        return Math.Round(totalPay, 2);
    }

    internal static decimal CalcPmt(int currencyId, int? period, decimal? rate, decimal? amount)
    {
        var pv = Convert.ToDouble(amount);

        var rateATime = Convert.ToDouble(rate) / 100;

        var principle = Financial.Pmt(rateATime, period ?? 0, pv);

        var principleDouble = Convert.ToDecimal(principle);

        if (currencyId == 1)
        {
            return MathExtension.RoundDown(principleDouble, -2);
        }
        return MathExtension.RoundDown(principleDouble, 2);
    }

    internal static int TotalDays(DateTime start, DateTime end)
    {
        TimeSpan dateDiff = end.Subtract(start);
        return dateDiff.Days;
    }
}