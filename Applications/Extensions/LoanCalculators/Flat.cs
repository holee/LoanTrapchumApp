using Domains.Models.Loans;
namespace Applications.Extensions.LoanCalculators;

public sealed class Flat : LoanCaloulator, ICalculator
{

    public Flat() : base()
    {

    }
    public Flat(int payEvery, decimal effectiveRate, int collectionFeeMode, int day,
        int currencyId, int installmentTypeId, DateTime firstPayment, DateTime startDate)
        : base(payEvery, effectiveRate, collectionFeeMode, day, currencyId, installmentTypeId, firstPayment, startDate)
    {

    }
    public override List<ScheduleLine> Calculate(int periodInMonth, decimal monthlyRate, decimal amount)
    {
        List<ScheduleLine> scheduleLines = [];
        var principalPaid = PaidPrincipal(amount, periodInMonth);
        decimal beginBalance = 0, lastBalance = 0;
        DateTime paymentDate = FirstPayment;
        beginBalance = lastBalance = amount;
        var effAmount = 0M;
        DateTime previusDate;
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
            if (CollectionFeeMode == 1)
            {
                effAmount = CalcEffective(amount, EffectiveRate, totalDays);
            }
            if (CollectionFeeMode == 2)
            {
                effAmount = CalcEffective(beginBalance, EffectiveRate, totalDays);
            }
            interest = CalcInterest(amount, monthlyRate, totalDays);
            totalPay = TotalPay(principalPaid, interest, effAmount);
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
    public override List<ScheduleLine> Calculate(int? periodInMonth, decimal? monthlyRate, decimal? amount)
    {
        List<ScheduleLine> scheduleLines = [];
        var principalPaid = PaidPrincipal(amount!.Value, periodInMonth!.Value);
        decimal beginBalance = 0, lastBalance = 0;
        DateTime paymentDate = FirstPayment;
        beginBalance = lastBalance = amount!.Value;
        var effAmount = 0M;
        DateTime previusDate;
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
            if (CollectionFeeMode == 1)
            {
                effAmount = CalcEffective(amount.Value, EffectiveRate, totalDays);
            }
            if (CollectionFeeMode == 2)
            {
                effAmount = CalcEffective(beginBalance, EffectiveRate, totalDays);
            }
            interest = CalcInterest(amount.Value, monthlyRate!.Value, totalDays);
            totalPay = TotalPay(principalPaid, interest, effAmount);
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
    public override List<ScheduleLine> Calculate(int? periodInMonth, decimal? monthlyRate, decimal? amount, DateTime firstPayment)
    {
        List<ScheduleLine> scheduleLines = [];
        var principalPaid = PaidPrincipal(amount!.Value, periodInMonth!.Value);
        decimal beginBalance = 0, lastBalance = 0;
        DateTime paymentDate = FirstPayment;
        beginBalance = lastBalance = amount!.Value;
        var effAmount = 0M;
        DateTime previusDate;
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
            if (CollectionFeeMode == 1)
            {
                effAmount = CalcEffective(amount.Value, EffectiveRate, totalDays);
            }
            if (CollectionFeeMode == 2)
            {
                effAmount = CalcEffective(beginBalance, EffectiveRate, totalDays);
            }
            interest = CalcInterest(amount.Value, monthlyRate!.Value, totalDays);
            totalPay = TotalPay(principalPaid, interest, effAmount);
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

}
