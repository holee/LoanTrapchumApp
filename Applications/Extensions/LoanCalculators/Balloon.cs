using Domains.Models.Loans;
namespace Applications.Extensions.LoanCalculators;

public sealed class Balloon : LoanCaloulator, ICalculator
{

    public Balloon() : base()
    {

    }
    public Balloon(int payEvery, decimal effectiveRate, int collectionFeeMode, int day,
        int currencyId, int installmentTypeId, DateTime firstPayment, DateTime startDate)
        : base(payEvery, effectiveRate, collectionFeeMode, day, currencyId, installmentTypeId, firstPayment, startDate)
    {

    }


    public override List<ScheduleLine> Calculate(int periodInMonth, decimal monthlyRate, decimal amount)
    {
        var schedules = new List<ScheduleLine>();
        var circles = periodInMonth / PayEvery;
        var beginBalance = 0.00M;
        var lastBalance = 0.00M;
        var paymentDate = FirstPayment;
        beginBalance = lastBalance = amount;
        for (int i = 1; i <= periodInMonth; i++)
        {
            beginBalance = lastBalance;
            var interestAmount = 0.00M;
            var totalPayAmount = 0.00M;
            var effAmount = 0.00M;
            var totalDays = 0;
            var principal = PaidPrincipal(amount, circles);
            DateTime previusDate;
            if (i == 1)
            {
                previusDate = StartDate;
                totalDays = TotalDays(previusDate, FirstPayment);
            }
            if (i > 1)
            {
                previusDate = schedules[i - 2].PaymentDate!.Value;
                paymentDate = SetPaymentDate(previusDate); //StartDate.AddMonths(i);
                SetHoliday(ref paymentDate);
                totalDays = TotalDays(previusDate, paymentDate);
            }

            if (i % PayEvery == 0)
            {
                if (CollectionFeeMode == 1)
                {
                    effAmount = CalcEffective(amount, EffectiveRate, totalDays);
                }
                else
                {
                    effAmount = CalcEffective(beginBalance, EffectiveRate, totalDays);
                }

                interestAmount = CalcInterest(beginBalance, monthlyRate, totalDays);
                totalPayAmount = TotalPay(principal, interestAmount, effAmount);
                if (i < periodInMonth)
                {
                    lastBalance = LastBalance(beginBalance, principal);
                }
                else
                {
                    principal = beginBalance;
                    lastBalance = 0;
                }

            }
            if (i % PayEvery != 0)
            {
                principal = 0;
                if (CollectionFeeMode == 1)
                {
                    effAmount = CalcEffective(amount, EffectiveRate, totalDays);
                }
                else
                {
                    effAmount = CalcEffective(beginBalance, EffectiveRate, totalDays);
                }
                interestAmount = CalcInterest(beginBalance, monthlyRate, totalDays);
                totalPayAmount = TotalPay(principal, interestAmount, effAmount);
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
                OutStanding = lastBalance
            };
            schedules.Add(line);
        }

        return schedules;

    }
    public override List<ScheduleLine> Calculate(int? periodInMonth, decimal? monthlyRate, decimal? amount)
    {
        var schedules = new List<ScheduleLine>();

        decimal _monthlyRate = monthlyRate ?? 0.00M;

        decimal _amount = amount ?? 0.00M;

        int _periodInMonth = periodInMonth ?? 0;

        int circles = _periodInMonth / PayEvery;

        var beginBalance = 0.00M;

        var lastBalance = 0.00M;

        var paymentDate = FirstPayment;

        beginBalance = lastBalance = _amount;

        for (int i = 1; i <= periodInMonth; i++)
        {
            beginBalance = lastBalance;
            var interestAmount = 0.00M;
            var totalPayAmount = 0.00M;
            var effAmount = 0.00M;
            var totalDays = 0;
            var principal = PaidPrincipal(_amount, circles);
            DateTime previusDate;
            if (i == 1)
            {
                previusDate = StartDate;
                totalDays = TotalDays(previusDate, FirstPayment);
            }
            if (i > 1)
            {
                previusDate = schedules[i - 2].PaymentDate!.Value;
                paymentDate = SetPaymentDate(previusDate); //StartDate.AddMonths(i);
                SetHoliday(ref paymentDate);
                totalDays = TotalDays(previusDate, paymentDate);
            }

            if (i % PayEvery == 0)
            {
                if (CollectionFeeMode == 1)
                {
                    effAmount = CalcEffective(_amount, EffectiveRate, totalDays);
                }
                else
                {
                    effAmount = CalcEffective(beginBalance, EffectiveRate, totalDays);
                }

                interestAmount = CalcInterest(beginBalance, _monthlyRate, totalDays);
                totalPayAmount = TotalPay(principal, interestAmount, effAmount);
                if (i < periodInMonth)
                {
                    lastBalance = LastBalance(beginBalance, principal);
                }
                else
                {
                    principal = beginBalance;
                    lastBalance = 0;
                }

            }
            if (i % PayEvery != 0)
            {
                principal = 0;
                if (CollectionFeeMode == 1)
                {
                    effAmount = CalcEffective(_amount, EffectiveRate, totalDays);
                }
                else
                {
                    effAmount = CalcEffective(beginBalance, EffectiveRate, totalDays);
                }
                interestAmount = CalcInterest(beginBalance, _monthlyRate, totalDays);
                totalPayAmount = TotalPay(principal, interestAmount, effAmount);
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
                OutStanding = lastBalance
            };
            schedules.Add(line);
        }

        return schedules;
    }
   
    public override List<ScheduleLine> Calculate(int? periodInMonth, decimal? monthlyRate, decimal? amount, DateTime firstPayment)
    {
        var schedules = new List<ScheduleLine>();

        decimal _monthlyRate = monthlyRate ?? 0.00M;

        decimal _amount = amount ?? 0.00M;

        int _periodInMonth = periodInMonth ?? 0;

        int circles = _periodInMonth / PayEvery;

        var beginBalance = 0.00M;

        var lastBalance = 0.00M;

        var paymentDate = firstPayment;

        beginBalance = lastBalance = _amount;

        for (int i = 1; i <= periodInMonth; i++)
        {
            beginBalance = lastBalance;
            var interestAmount = 0.00M;
            var totalPayAmount = 0.00M;
            var effAmount = 0.00M;
            var totalDays = 0;
            var principal = PaidPrincipal(_amount, circles);
            DateTime previusDate;
            if (i == 1)
            {
                previusDate = StartDate;
                totalDays = TotalDays(previusDate, FirstPayment);
            }
            if (i > 1)
            {
                previusDate = schedules[i - 2].PaymentDate!.Value;
                paymentDate = SetPaymentDate(previusDate); //StartDate.AddMonths(i);
                SetHoliday(ref paymentDate);
                totalDays = TotalDays(previusDate, paymentDate);
            }

            if (i % PayEvery == 0)
            {
                if (CollectionFeeMode == 1)
                {
                    effAmount = CalcEffective(_amount, EffectiveRate, totalDays);
                }
                else
                {
                    effAmount = CalcEffective(beginBalance, EffectiveRate, totalDays);
                }

                interestAmount = CalcInterest(beginBalance, _monthlyRate, totalDays);
                totalPayAmount = TotalPay(principal, interestAmount, effAmount);
                if (i < periodInMonth)
                {
                    lastBalance = LastBalance(beginBalance, principal);
                }
                else
                {
                    principal = beginBalance;
                    lastBalance = 0;
                }

            }
            if (i % PayEvery != 0)
            {
                principal = 0;
                if (CollectionFeeMode == 1)
                {
                    effAmount = CalcEffective(_amount, EffectiveRate, totalDays);
                }
                else
                {
                    effAmount = CalcEffective(beginBalance, EffectiveRate, totalDays);
                }
                interestAmount = CalcInterest(beginBalance, _monthlyRate, totalDays);
                totalPayAmount = TotalPay(principal, interestAmount, effAmount);
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
                OutStanding = lastBalance
            };
            schedules.Add(line);
        }

        return schedules;
    }

    private bool IsEven(int x, int y)
    {
        return x % y == 0;
    }

}
