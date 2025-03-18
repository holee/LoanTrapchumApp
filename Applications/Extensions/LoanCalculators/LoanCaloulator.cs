using Domains.Models.Loans;

namespace Applications.Extensions.LoanCalculators;

public abstract class LoanCaloulator : ICalculator
{
    public LoanCaloulator()
    {
        PayEvery = 1;
        EffectiveRate = 0;
        FirstPayment = DateTime.UtcNow;
        StartDate = DateTime.UtcNow;
        CollectionFeeMode = 1;
        Day = 30;
        CurrencyId = 1;
        InstallmentTypeId = 1;
    }
    public LoanCaloulator(int payEvery, decimal effectiveRate, int collectionFeeMode, int day,
        int currencyId, int installmentTypeId, DateTime firstPayment, DateTime startDate)
    {
        PayEvery = payEvery;
        EffectiveRate = effectiveRate;
        FirstPayment = firstPayment;
        StartDate = startDate;
        CollectionFeeMode = collectionFeeMode;
        Day = day;
        CurrencyId = currencyId;
        InstallmentTypeId = installmentTypeId;
    }

    public int PayEvery { get; set; }
    public decimal EffectiveRate { get; set; } = 0;
    public DateTime FirstPayment { get; set; }
    public DateTime StartDate { get; set; }
    public int CollectionFeeMode { get; set; } = 1;
    public int Day { get; set; } = 30;
    public int CurrencyId { get; set; } = 1;
    public int InstallmentTypeId { get; set; } = 1;
    private List<DateTime> Holidays { get; set; } = [];
    protected decimal Pmt(int nper, decimal rate, decimal amount)
    {
        if (amount < 0 || rate < 0)
            throw new ArgumentException("Amount and Rate must greater than Zero");
        var r = rate / 100;
        var denominator = Convert.ToDecimal(Math.Pow(1 + (double)r, nper) - 1);
        var pmt = (r + r / denominator) * amount;
        if (CurrencyId == 1) return RoundUp(pmt, -2);
        return Math.Round(pmt, 2);
    }
    protected decimal CalcInterest(decimal amount, decimal rate, int day)
    {
        //if (amount < 0 || rate < 0)
        //    throw new ArgumentException($"Amount={amount} and Rate={rate} must greater than Zero");
        decimal ratePerDay = rate / Day;
        var totalPay = amount * (ratePerDay / 100) * day;
        if (CurrencyId == 1) return RoundUp(totalPay, -2);
        return Math.Round(totalPay, 2);
    }
    protected decimal TotalPay(decimal principal, decimal interestRate, decimal collectionFee = 0)
    {
        return Math.Round(principal + interestRate + collectionFee, 2);
    }
    protected decimal PaidPrincipal(decimal amount, int nper)
    {
        if (CurrencyId == 1) return RoundDown(amount / nper, -2);
        return RoundDown(amount / nper, 2);
    }
    protected decimal LastBalance(decimal beginBalance, decimal principalPaid)
    {
        return Math.Round(beginBalance - principalPaid, 2);
    }
    protected decimal CalcEffective(decimal amount, decimal effectiveRate, int days = 30)
    {
        var per = effectiveRate / Day;
        var effective = amount * (per / 100) * days;
        if (CurrencyId == 1) return RoundUp(effective, -2);
        return RoundUp(effective, 2);
    }
    internal decimal RoundDown(decimal number, int decimalPlaces)
    {
        var v1 = (decimal)Math.Pow(10, decimalPlaces);
        var v2 = (decimal)Math.Pow(10, decimalPlaces);
        return Math.Floor(number * v1) / v2;
    }
    public static decimal RoundUp(decimal number, int decimalPlaces)
    {
        return Math.Ceiling(number * (decimal)Math.Pow(10, decimalPlaces)) / (decimal)Math.Pow(10, decimalPlaces);
    }
    internal int GetHolidays(IEnumerable<DateTime> holidaies, DateTime holiday)
    {
        return holidaies.Where(x => x == holiday).Count();
    }
    internal int TotalDays(DateTime start, DateTime end)
    {
        TimeSpan dateDiff = end.Subtract(start);
        return dateDiff.Days;
    }
    protected DateTime SetPaymentDate(DateTime date)
    {
        return InstallmentTypeId switch
        {
            1 => date.AddMonths(1),
            2 => date.AddDays(14),
            3 => date.AddDays(7),
            4 => date.AddDays(1),
            _ => DateTime.Now,
        };
    }
    private bool IsSunday(DateTime date)
    {
        return date.DayOfWeek == DayOfWeek.Sunday;
    }
    private bool IsHoliday(DateTime holiday, DateTime paymentDate)
    {
        var days = (holiday - paymentDate).TotalDays;
        return days == 0;
    }
    public void SetHoliday(ref DateTime paymentDate)
    {
        foreach (var holiday in Holidays)
        {
            if (IsSunday(paymentDate) || IsHoliday(holiday, paymentDate))
            {

                paymentDate = paymentDate.AddDays(1);

            }
        }
    }
    public void SetHolidays(List<DateTime> Holidays)
    {
        this.Holidays = Holidays;
    }
    public abstract List<ScheduleLine> Calculate(int periodInMonth, decimal monthlyRate, decimal amount);
    public abstract List<ScheduleLine> Calculate(int? periodInMonth, decimal? monthlyRate, decimal? amount);
    public abstract List<ScheduleLine> Calculate(int? periodInMonth, decimal? monthlyRate, decimal? amount, DateTime firstPayment);

}
