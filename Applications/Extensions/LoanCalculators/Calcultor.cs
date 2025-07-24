using Applications.Responses.ViewModels.Loans.Payments;
using Domains.Models.Loans;
using Microsoft.VisualBasic;
namespace Applications.Extensions.LoanCalculators;
public interface ILoanCalculatorInterface
{
    public int PayEvery { get; set; }
    public decimal EffectiveRate { get; set; }
    public DateTimeOffset? FirstPayment { get; set; }
    public DateTimeOffset? StartDate { get; set; }
    public int SelectedCollectionFee { get; set; }
    public int Day { get; set; }
    public int CurrencyRefId { get; set; }
    public int InstallmentType { get; set; }
    public IList<ScheduleLine> Calculate(int? periodInMonth, decimal? monthlyRate, decimal? amount, LoanType type);
}
public sealed class Calcultor : ILoanCalculatorInterface
{
    public int PayEvery { get; set; } = 1;
    public decimal EffectiveRate { get; set; } = 0;
    public DateTimeOffset? FirstPayment { get; set; } = DateTime.Now;
    public DateTimeOffset? StartDate { get; set; } = DateTime.Now;
    public int SelectedCollectionFee { get; set; } = 1;
    public int Day { get; set; } = 30;
    public int CurrencyRefId { get; set; } = 1;
    public int InstallmentType { get; set; } = 1;
    private List<DateTimeOffset> Holidays { get; set; } = [];
    public IList<ScheduleLine> Calculate(int? periodInMonth, decimal? monthlyRate, decimal? amount, LoanType type = LoanType.Flat)
    {
        return type switch
        {

            LoanType.Flat => Flat(periodInMonth, monthlyRate, amount),
            LoanType.Decline => Decline(periodInMonth, monthlyRate, amount),
            LoanType.Annuarity => Annuarity(periodInMonth, monthlyRate, amount),
            LoanType.Balloon => Balloon(periodInMonth, monthlyRate, amount),
            _ => Flat(periodInMonth, monthlyRate, amount),
        };
    }

    public void SetHoliday(ref DateTimeOffset? paymentDate)
    {
        foreach (var holiday in Holidays)
        {
            if (IsSunday(paymentDate) || IsHoliday(holiday, paymentDate))
            {

                paymentDate = paymentDate!.Value.AddDays(1);

            }
        }
    }
    public void SetHolidays(List<DateTimeOffset> holidays)
    {
        this.Holidays = holidays;
    }
    /// <summary>
    /// Private fields and methods
    /// </summary>
    /// <param name="nper"></param>
    /// <param name="rate"></param>
    /// <param name="amount"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    decimal Pmt(int nper, decimal rate, decimal amount)
    {
        if (amount < 0 || rate < 0)
            throw new ArgumentException("Amount and Rate must greater than Zero");
        var r = rate / 100;
        var denominator = Convert.ToDecimal(Math.Pow(1 + (double)r, nper) - 1);
        var pmt = (r + r / denominator) * amount;
        if (CurrencyRefId == 1) return RoundUp(pmt, -2);
        return Math.Round(pmt, 2);
    }
    decimal CalcInterest(decimal amount, decimal rate, int day)
    {
        if (amount < 0 || rate < 0)
            throw new ArgumentException($"Amount={amount} and Rate={rate} must greater than Zero");
        decimal ratePerDay = rate / Day;
        var totalPay = amount * (ratePerDay / 100) * day;
        if (CurrencyRefId == 1) return RoundUp(totalPay, -2);
        return Math.Round(totalPay, 2);
    }
    decimal TotalPay(decimal principal, decimal interestRate, decimal collectionFee = 0)
    {
        return Math.Round(principal + interestRate + collectionFee, 2);
    }
    decimal PaidPrincipal(decimal amount, int nper)
    {
        if (CurrencyRefId == 1) return RoundDown(amount / nper, -2);
        return RoundDown(amount / nper, 2);
    }
    decimal LastBalance(decimal beginBalance, decimal principalPaid)
    {
        return Math.Round(beginBalance - principalPaid, 2);
    }
    decimal CalcEffective(decimal amount, decimal effectiveRate, int days = 30)
    {
        var per = effectiveRate / Day;
        var effective = amount * (per / 100) * days;
        if (CurrencyRefId == 1) return RoundUp(effective, -2);
        return RoundUp(effective, 2);
    }
    decimal RoundDown(decimal number, int decimalPlaces)
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
    internal int TotalDays(DateTimeOffset? start, DateTimeOffset? end)
    {
        TimeSpan dateDiff = end!.Value.Subtract(start!.Value);

        return dateDiff.Days;
    }
    DateTimeOffset? SetPaymentDate(DateTimeOffset? date)
    {
        return InstallmentType switch
        {
            1 => date!.Value.AddMonths(1),
            2 => date!.Value.AddDays(14),
            3 => date!.Value.AddDays(7),
            4 => date!.Value.AddDays(1),
            _ => DateTime.UtcNow,
        };
    }
    private bool IsSunday(DateTimeOffset? date)
    {
        return date!.Value.DayOfWeek == DayOfWeek.Sunday;
    }
    private bool IsHoliday(DateTimeOffset? holiday, DateTimeOffset? paymentDate)
    {
        var days = (holiday - paymentDate)!.Value.TotalDays;
        return days == 0;
    }
    private decimal CalcPmt(int period, decimal rate, decimal amount)
    {
        var pv = Convert.ToDouble(amount);
        var rateATime = Convert.ToDouble(rate) / 100;
        var principle = Financial.Pmt(rateATime, period, pv);
        var principleDouble = Convert.ToDecimal(principle);
        if (CurrencyRefId == 1)
        {
            return RoundDown(principleDouble, -2);
        }
        return RoundDown(principleDouble, 2);
    }
    private IList<ScheduleLine> Balloon(int? periodInMonth, decimal? monthlyRate, decimal? amount)
    {
        var schedules = new List<ScheduleLine>();
        var circles = periodInMonth / PayEvery;
        decimal? beginBalance = 0.00M;
        decimal? lastBalance = 0.00M;
        DateTimeOffset? paymentDate = FirstPayment;
        beginBalance = lastBalance = amount;
        int scheduleNum = 1;
        for (int i = 1; i <= periodInMonth; i++)
        {
            beginBalance = lastBalance;

            var interestAmount = 0.00M;

            var totalPayAmount = 0.00M;

            var effAmount = 0.00M;

            var totalDays = 0;

            var principal = PaidPrincipal(amount!.Value, circles!.Value);

            DateTimeOffset? previusDate;

            if (i == 1)
            {
                previusDate = StartDate;

                totalDays = TotalDays(previusDate, FirstPayment);

            }
            if (i > 1)
            {
                previusDate = schedules[i - 2].PaymentDate!.Value;

                paymentDate = SetPaymentDate(previusDate.Value); //StartDate.AddMonths(i);

                SetHoliday(ref paymentDate);

                totalDays = TotalDays(previusDate, paymentDate);
            }

            if (i % PayEvery == 0)
            {
                if (SelectedCollectionFee == 1)
                {
                    effAmount = CalcEffective(amount!.Value, EffectiveRate, totalDays);
                }
                else
                {
                    effAmount = CalcEffective(beginBalance!.Value, EffectiveRate, totalDays);
                }

                interestAmount = CalcInterest(beginBalance!.Value, monthlyRate!.Value, totalDays);

                totalPayAmount = TotalPay(principal, interestAmount, effAmount);

                if (i < periodInMonth)
                {
                    lastBalance = LastBalance(beginBalance!.Value, principal);
                }
                else
                {
                    principal = beginBalance!.Value;
                    lastBalance = 0;
                }

            }
            if (i % PayEvery != 0)
            {
                principal = 0;
                if (SelectedCollectionFee == 1)
                {
                    effAmount = CalcEffective(amount!.Value, EffectiveRate, totalDays);
                }
                else
                {
                    effAmount = CalcEffective(beginBalance!.Value, EffectiveRate, totalDays);
                }
                interestAmount = CalcInterest(beginBalance!.Value, monthlyRate!.Value, totalDays);

                totalPayAmount = TotalPay(principal, interestAmount, effAmount);
            }

            if(i % 5 == 0)
            {
                scheduleNum++;
            }

            ScheduleLine line = new ScheduleLine()
            {
                InstallmentNo = i,
                TotalDay = totalDays,
                PaymentDate = paymentDate,
                Principal = principal,
                Interest = interestAmount,
                CollectionFeeAmount = effAmount,
                TotalPay = totalPayAmount,
                OutStanding = lastBalance,
                ScheduleNum = scheduleNum,
            };
            schedules.Add(line);
        }

        return schedules;

    }
    private IList<ScheduleLine> Decline(int? period, decimal? rate, decimal? amount)
    {
        List<ScheduleLine> loanScheduleLines = [];
        var principalPaid = PaidPrincipal(amount!.Value, period!.Value);
        var effAmount = 0M;
        decimal beginBalance = 0, lastBalance = 0;
        DateTimeOffset? paymentDate = FirstPayment;
        beginBalance = lastBalance = amount!.Value;
        int scheduleNum = 1;
        for (int i = 1; i <= period; i++)
        {
            var totalPay = 0.00M;

            var interest = 0.00M;

            var totalDays = 0;

            beginBalance = lastBalance;

            DateTimeOffset? previusDate;

            if (i == 1)
            {
                previusDate = StartDate;
                totalDays = TotalDays(previusDate, FirstPayment);
            }
            if (i > 1)
            {
                previusDate = loanScheduleLines[i - 2].PaymentDate!.Value;

                paymentDate = SetPaymentDate(previusDate); //StartDate.AddMonths(i);

                SetHoliday(ref paymentDate);

                totalDays = TotalDays(previusDate, paymentDate);

            }
            if (SelectedCollectionFee == 1)
            {
                effAmount = CalcEffective(amount!.Value, EffectiveRate, totalDays);
            }
            if (SelectedCollectionFee == 2)
            {
                effAmount = CalcEffective(beginBalance, EffectiveRate, totalDays);
            }
            interest = CalcInterest(beginBalance, rate!.Value, totalDays);
            totalPay = TotalPay(principalPaid, interest, effAmount);
            if (i < period)
            {
                lastBalance = LastBalance(beginBalance, principalPaid);
            }
            if (i >= period)
            {
                principalPaid = beginBalance;
                lastBalance = 0;
            }

            if(i % 5== 0)
            {
                scheduleNum++;
            }

            ScheduleLine line = new ScheduleLine()
            {
                InstallmentNo = i,
                PaymentDate = paymentDate,
                Principal = principalPaid,
                Interest = interest,
                TotalDay = totalDays,
                TotalPay = totalPay,
                OutStanding = lastBalance,
                CollectionFeeAmount = effAmount,
                ScheduleNum = scheduleNum,
            };
            loanScheduleLines.Add(line);
        }

        return loanScheduleLines;
    }
    private IList<ScheduleLine> Flat(int? periodInMonth, decimal? monthlyRate, decimal? amount)
    {
        List<ScheduleLine> scheduleLines = [];
        var principalPaid = PaidPrincipal(amount!.Value, periodInMonth!.Value);
        decimal beginBalance = 0, lastBalance = 0;
        DateTimeOffset? paymentDate = FirstPayment;
        beginBalance = lastBalance = amount!.Value;
        var effAmount = 0M;
        DateTimeOffset? previusDate;
        int scheduleNum = 1;
        for (int i = 1; i <= periodInMonth; i++)
        {
            beginBalance = lastBalance;

            var totalDays = 0;

            var totalPay = 0.00M;

            var interest = 0.00M;

            if (i < periodInMonth)
            {
                lastBalance = LastBalance(beginBalance, principalPaid);
            }
            if (i >= periodInMonth)
            {
                principalPaid = beginBalance;

                lastBalance = 0;
            }
            if (i == 1)
            {
                previusDate = StartDate;

                totalDays = TotalDays(previusDate, FirstPayment);
            }
            if (i > 1)
            {
                previusDate = scheduleLines[i - 2].PaymentDate!.Value;

                paymentDate = SetPaymentDate(previusDate);

                SetHoliday(ref paymentDate);

                totalDays = TotalDays(previusDate, paymentDate);
            }
            if (SelectedCollectionFee == 1)
            {
                effAmount = CalcEffective(amount!.Value, EffectiveRate, totalDays);
            }
            if (SelectedCollectionFee == 2)
            {
                effAmount = CalcEffective(beginBalance, EffectiveRate, totalDays);
            }
            interest = CalcInterest(amount!.Value, monthlyRate!.Value, totalDays);
            totalPay = TotalPay(principalPaid, interest, effAmount);


            if (i % 5 == 0)
            {
                scheduleNum++;
            }

            ScheduleLine line = new ScheduleLine()
            {
                InstallmentNo = i,
                PaymentDate = paymentDate,
                TotalDay = totalDays,
                Principal = principalPaid,
                Interest = interest,
                TotalPay = totalPay,
                OutStanding = lastBalance,
                CollectionFeeAmount = effAmount
            };
            scheduleLines.Add(line);
        }

        return scheduleLines;
    }
    private IList<ScheduleLine> Annuarity(int? period, decimal? rate, decimal? amount)
    {
        List<ScheduleLine> loanScheduleLines = [];
        var principleAndInterestAmount = CalcPmt(period!.Value, rate!.Value, -amount!.Value);    //Pmt(period,rate,amount); 
        decimal beginBalance = 0, lastBalance = 0;
        DateTimeOffset? paymentDate = FirstPayment;
        beginBalance = lastBalance = amount.Value;
        var effAmount = 0M;
        int scheduleNum = 1;
        for (int i = 1; i <= period; i++)
        {
            var totalPay = 0.00M;

            var interest = 0.00M;

            var totalDays = 0;

            var principal = 0.00M;

            beginBalance = lastBalance;

            DateTimeOffset? previusDate;
            if (i == 1)
            {
                previusDate = StartDate;

                totalDays = TotalDays(previusDate, FirstPayment);
            }
            if (i > 1)
            {
                previusDate = loanScheduleLines[i - 2].PaymentDate!.Value;

                paymentDate = SetPaymentDate(previusDate);

                SetHoliday(ref paymentDate);

                totalDays = TotalDays(previusDate, paymentDate);
            }
            if (SelectedCollectionFee == 1)
            {
                effAmount = CalcEffective(amount!.Value, EffectiveRate, totalDays);
            }
            if (SelectedCollectionFee == 2)
            {
                effAmount = CalcEffective(beginBalance, EffectiveRate, totalDays);
            }
            interest = CalcInterest(beginBalance, rate!.Value, totalDays);
            totalPay = TotalPay(principleAndInterestAmount, effAmount);

            if (i < period)
            {
                principal = principleAndInterestAmount - interest;
                lastBalance = LastBalance(beginBalance, principal);
            }
            if (i >= period)
            {
                principal = beginBalance;
                lastBalance = 0;
            }

            if (i % 5 == 0)
            {
                scheduleNum++;
            }

            ScheduleLine line = new ScheduleLine()
            {
                InstallmentNo = i,
                PaymentDate = paymentDate,
                TotalDay = totalDays,
                Principal = principal,
                Interest = interest,
                TotalPay = totalPay,
                OutStanding = lastBalance,
                CollectionFeeAmount = effAmount,
                ScheduleNum= scheduleNum

            };
            loanScheduleLines.Add(line);
        }

        return loanScheduleLines;
    }

}
