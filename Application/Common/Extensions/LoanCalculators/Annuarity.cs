using Domains.Models.Loans;
using Microsoft.VisualBasic;
namespace Application.Extensions.LoanCalculators;

public sealed class Annuarity : LoanCaloulator, ICalculator
{
    public Annuarity() : base()
    {

    }
    public Annuarity(int payEvery, decimal effectiveRate, int collectionFeeMode, int day,
        int currencyId, int installmentTypeId, DateTime firstPayment, DateTime startDate)
        : base(payEvery, effectiveRate, collectionFeeMode, day, currencyId, installmentTypeId, firstPayment, startDate)
    {

    }
    private decimal CalcPmt(int? period, decimal? rate, decimal? amount)
    {
        var pv = Convert.ToDouble(amount);
        var rateATime = Convert.ToDouble(rate) / 100;
        var principle = Financial.Pmt(rateATime, period ?? 0, pv);
        var principleDouble = Convert.ToDecimal(principle);
        if (CurrencyId == 1)
        {
            return RoundDown(principleDouble, -2);
        }
        return RoundDown(principleDouble, 2);
    }
    public override List<ScheduleLine> Calculate(int period, decimal rate, decimal amount)
    {
        List<ScheduleLine> loanScheduleLines = [];

        var principleAndInterestAmount = CalcPmt(period, rate, -amount);    //Pmt(period,rate,amount); 

        decimal beginBalance = 0, lastBalance = 0;

        DateTime? paymentDate = FirstPayment;

        beginBalance = lastBalance = amount;

        var effAmount = 0M;

        for (int i = 1; i <= period; i++)
        {
            var totalPay = 0.00M;

            var interest = 0.00M;

            var totalDays = 0;

            var principal = 0.00M;

            beginBalance = lastBalance;

            DateTime? previusDate = null;

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
            if (CollectionFeeMode == 1)
            {
                effAmount = CalcEffective(amount, EffectiveRate, totalDays);
            }
            if (CollectionFeeMode == 2)
            {
                effAmount = CalcEffective(beginBalance, EffectiveRate, totalDays);
            }

            interest = CalcInterest(beginBalance, rate, totalDays);

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
            ScheduleLine line = new ScheduleLine()
            {
                InstallmentNo = i,
                PaymentDate = paymentDate,
                TotalDay = totalDays,
                Principal = principal,
                Interest = interest,
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

        var principleAndInterestAmount = CalcPmt(periodInMonth, monthlyRate, -amount);

        //Pmt(period,rate,amount); 

        decimal beginBalance = 0, lastBalance = 0;

        DateTime? paymentDate = FirstPayment;

        beginBalance = lastBalance = amount!.Value;

        var effAmount = 0M;

        for (int i = 1; i <= periodInMonth; i++)
        {
            var totalPay = 0.00M;
            var interest = 0.00M;
            var totalDays = 0;
            var principal = 0.00M;
            beginBalance = lastBalance;
            DateTime? previusDate;
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
            if (CollectionFeeMode == 1)
            {
                effAmount = CalcEffective(amount!.Value, EffectiveRate, totalDays);
            }
            if (CollectionFeeMode == 2)
            {
                effAmount = CalcEffective(beginBalance, EffectiveRate, totalDays);
            }

            interest = CalcInterest(beginBalance, monthlyRate!.Value, totalDays);

            totalPay = TotalPay(principleAndInterestAmount, effAmount);

            if (i < periodInMonth)
            {
                principal = principleAndInterestAmount - interest;

                lastBalance = LastBalance(beginBalance, principal);
            }
            if (i >= periodInMonth)
            {
                principal = beginBalance;

                lastBalance = 0;
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
                CollectionFeeAmount = effAmount

            };
            loanScheduleLines.Add(line);
        }

        return loanScheduleLines;
    }


    public override List<ScheduleLine> Calculate(int? periodInMonth, decimal? monthlyRate, decimal? amount, DateTime? firstPayment)
    {
        List<ScheduleLine> loanScheduleLines = [];

        var principleAndInterestAmount = CalcPmt(periodInMonth, monthlyRate, -amount);

        decimal beginBalance = 0, lastBalance = 0;

        DateTime? paymentDate = firstPayment;

        beginBalance = lastBalance = amount!.Value;

        var effAmount = 0M;

        for (int i = 1; i <= periodInMonth; i++)
        {
            var totalPay = 0.00M;

            var interest = 0.00M;

            var totalDays = 0;

            var principal = 0.00M;

            beginBalance = lastBalance;

            DateTime? previusDate = null;

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
            if (CollectionFeeMode == 1)
            {
                effAmount = CalcEffective(amount!.Value, EffectiveRate, totalDays);
            }
            if (CollectionFeeMode == 2)
            {
                effAmount = CalcEffective(beginBalance, EffectiveRate, totalDays);
            }

            interest = CalcInterest(beginBalance, monthlyRate!.Value, totalDays);

            totalPay = TotalPay(principleAndInterestAmount, effAmount);

            if (i < periodInMonth)
            {
                principal = principleAndInterestAmount - interest;

                lastBalance = LastBalance(beginBalance, principal);
            }
            if (i >= periodInMonth)
            {
                principal = beginBalance;
                lastBalance = 0;
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
                CollectionFeeAmount = effAmount

            };
            loanScheduleLines.Add(line);
        }

        return loanScheduleLines;
    }



}
