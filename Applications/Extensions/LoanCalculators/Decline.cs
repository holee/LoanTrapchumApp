using Domains.Models.Loans;
namespace Applications.Extensions.LoanCalculators;

public sealed class Decline : LoanCaloulator, ICalculator
{

    public Decline() : base()
    {

    }
    public Decline(int payEvery, decimal effectiveRate, int collectionFeeMode, int day,
        int currencyId, int installmentTypeId, DateTime firstPayment, DateTime startDate)
        : base(payEvery, effectiveRate, collectionFeeMode, day, currencyId, installmentTypeId, firstPayment, startDate)
    {
         
    }


    public override List<ScheduleLine> Calculate(int period, decimal rate, decimal amount)
    {
        List<ScheduleLine> loanScheduleLines = [];

        var principalPaid = PaidPrincipal(amount, period);

        var effAmount = 0M;

        decimal beginBalance = 0, lastBalance = 0;

        DateTime paymentDate = FirstPayment;

        beginBalance = lastBalance = amount;

        for (int i = 1; i <= period; i++)
        {
            var totalPay = 0.00M;
            var interest = 0.00M;
            var totalDays = 0;
            beginBalance = lastBalance;
            DateTime previusDate;
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
            if (CollectionFeeMode == 1)
            {
                effAmount = CalcEffective(amount, EffectiveRate, totalDays);
            }
            if (CollectionFeeMode == 2)
            {
                effAmount = CalcEffective(beginBalance, EffectiveRate, totalDays);
            }
            interest = CalcInterest(beginBalance, rate, totalDays);

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
            ScheduleLine line = new ScheduleLine()
            {
                InstallmentNo = i,
                PaymentDate = paymentDate,
                Principal = principalPaid,
                Interest = interest,
                TotalDay = totalDays,
                TotalPay = totalPay,
                OutStanding = lastBalance,
                CollectionFeeAmount = effAmount
            };
            loanScheduleLines.Add(line);
        }

        return loanScheduleLines;
    }

    public override List<ScheduleLine> Calculate(int? periodInMonth, decimal? monthlyRate, decimal? amount)
    {
        List<ScheduleLine> loanScheduleLines = [];

        int period = periodInMonth ?? 0;

        decimal _amount = amount ?? 0.00M;

        decimal rate = monthlyRate ?? 0;

        var principalPaid = PaidPrincipal(_amount, period);

        var effAmount = 0M;

        decimal beginBalance = 0, lastBalance = 0;

        DateTime paymentDate = FirstPayment;

        beginBalance = lastBalance = _amount;

        for (int i = 1; i <= period; i++)
        {
            var totalPay = 0.00M;
            var interest = 0.00M;
            var totalDays = 0;
            beginBalance = lastBalance;
            DateTime previusDate;
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
            if (CollectionFeeMode == 1)
            {
                effAmount = CalcEffective(_amount, EffectiveRate, totalDays);
            }
            if (CollectionFeeMode == 2)
            {
                effAmount = CalcEffective(beginBalance, EffectiveRate, totalDays);
            }
            interest = CalcInterest(beginBalance, rate, totalDays);
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
            ScheduleLine line = new ScheduleLine()
            {
                InstallmentNo = i,
                PaymentDate = paymentDate,
                Principal = principalPaid,
                Interest = interest,
                TotalDay = totalDays,
                TotalPay = totalPay,
                OutStanding = lastBalance,
                CollectionFeeAmount = effAmount
            };
            loanScheduleLines.Add(line);
        }

        return loanScheduleLines;
    }

    public override List<ScheduleLine> Calculate(int? periodInMonth, decimal? monthlyRate, decimal? amount, DateTime firstPayment)
    {
        List<ScheduleLine> loanScheduleLines = [];

        int period = periodInMonth ?? 0;

        decimal _amount = amount ?? 0.00M;

        decimal rate = monthlyRate ?? 0;

        var principalPaid = PaidPrincipal(_amount, period);

        var effAmount = 0M;

        decimal beginBalance = 0, lastBalance = 0;

        DateTime paymentDate = firstPayment;

        beginBalance = lastBalance = _amount;

        for (int i = 1; i <= period; i++)
        {
            var totalPay = 0.00M;
            var interest = 0.00M;
            var totalDays = 0;
            beginBalance = lastBalance;
            DateTime previusDate;
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
            if (CollectionFeeMode == 1)
            {
                effAmount = CalcEffective(_amount, EffectiveRate, totalDays);
            }
            if (CollectionFeeMode == 2)
            {
                effAmount = CalcEffective(beginBalance, EffectiveRate, totalDays);
            }
            interest = CalcInterest(beginBalance, rate, totalDays);
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
            ScheduleLine line = new ScheduleLine()
            {
                InstallmentNo = i,
                PaymentDate = paymentDate,
                Principal = principalPaid,
                Interest = interest,
                TotalDay = totalDays,
                TotalPay = totalPay,
                OutStanding = lastBalance,
                CollectionFeeAmount = effAmount
            };
            loanScheduleLines.Add(line);
        }

        return loanScheduleLines;
    }
}
