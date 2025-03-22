using Applications.Extensions.Maths;
using Applications.Requests.Loans.Payments;
using Applications.Responses.ViewModels.Loans.Schedules;

namespace TrapchumApp.Components.Pages.Transactions
{
    public static class LoanExtensions
    {

        public static PaymentDtoCreate ToPaymentDtoCreate(this VSchedule s) => new PaymentDtoCreate(s.LeftPrinciple, s.LeftInterestAmount, s.LeftCollectionFeeAmount, s.PenaltyAmount);
        public static LoanRepaymentLineDtoCreate ToPaymentLine(this VSchedule s, PaymentDtoCreate p) => new LoanRepaymentLineDtoCreate
        {
            LoanRefId = s.LoanId,
            LoanScheduleRefId = s!.LoanScheduleId,
            InstallmentNo = s!.InstallmentNo,
            NumberOfDay = s!.TotalDay,
            DayArrea = s.DayArrea,
            PayDate = s!.PaymentDate,
            PaidPrinciple = p.RecievePrincipleAmount,
            PaidInterest = p.RecieveInterestAmount,
            PaidCollectionFee = p.RecieveCollectionFeeAmount,
            PenaltyAmount = p.PenaltyAmountAmount,
            BranchRefId = s.BranchId,
            SubTotal = p.SubTotal()
        };
        public static decimal? SubTotal(decimal? principle = 0, decimal? interest = 0, decimal? collectionfee = 0, decimal? penalty = 0)
        {
            var value = principle + interest + collectionfee + penalty;
            return value ?? 0;
        }
        public static decimal? SubTotal(this VSchedule? s, bool includePenalty = true)
        {
            if (s is null) return 0;

            if (includePenalty)
            {
                var total = s.LeftPrinciple + s.LeftInterestAmount + s.LeftCollectionFeeAmount + s.PenaltyAmount;

                return total ?? 0;
            }

            var total1 = s.LeftPrinciple + s.LeftInterestAmount + s.LeftCollectionFeeAmount;

            return total1 ?? 0;
        }
        public static decimal SubTotal(this PaymentDtoCreate p, bool includePenalty = true)
        {
            decimal? total = 0.00M;
            if (includePenalty)
            {
                total = p.RecievePrincipleAmount + p.RecieveInterestAmount + p.RecieveCollectionFeeAmount + p.PenaltyAmountAmount;
                return total ?? 0;
            }
            total = p.RecievePrincipleAmount + p.RecieveInterestAmount + p.RecieveCollectionFeeAmount;
            return total ?? 0;
        }
        public static decimal? SubTotal(LoanRepaymentLineDtoCreate s, bool includePenalty)
        {
            decimal? total = 0.00M;

            if (includePenalty)
            {
                total = s.PaidPrinciple + s.PaidInterest + s.PaidCollectionFee + s.PenaltyAmount;
                return total ?? 0;
            }
            total = s.PaidPrinciple + s.PaidInterest + s.PaidCollectionFee;
            return total ?? 0;
        }
        public static decimal? Total(this IEnumerable<LoanRepaymentLineDtoCreate> s)
        {
            var total1 = s.Sum(x => x.SubTotal);
            return total1 ?? 0;
        }
        public static decimal? TotalAmountWithPenalty(this IEnumerable<LoanRepaymentLineDtoCreate> s)
        {
            decimal? total = 0.00M;

            foreach (var item in s)
            {
                total += SubTotal(item, true);
            }
            return total ?? 0;
        }
        public static decimal? TotalAmountNoPenalty(this IEnumerable<LoanRepaymentLineDtoCreate> s)
        {
            decimal? total = 0.00M;

            foreach (var item in s)
            {
                total += SubTotal(item, false);
            }
            return total ?? 0;
        }
        public static decimal? Total(this IEnumerable<LoanRepaymentLineDtoCreate> s, bool includePenalty = true)
        {
            decimal? total = 0.00M;

            foreach (var item in s)
            {
                total += SubTotal(item, includePenalty);
            }
            return total ?? 0;
        }
        public static decimal? Total(this IEnumerable<VSchedule> s, bool includePenalty)
        {
            decimal? total = 0.00M;
            foreach (var item in s)
            {
                total += LoanExtensions.SubTotal(item, includePenalty);
            }
            return total ?? 0;
        }
        public static decimal? Total(decimal? principle, decimal? interest = 0, decimal? collectionfee = 0, decimal? penalty = 0)
        {
            principle = principle + interest + collectionfee + penalty;
            return principle ?? 0;
        }
        public static decimal? TotalPaidPrincipleAmount(this IEnumerable<LoanRepaymentLineDtoCreate> s)
        {
            var total1 = s.Sum(x => x.PaidPrinciple);
            return total1 ?? 0;
        }
        public static decimal? TotalPaidInterestAmount(this IEnumerable<LoanRepaymentLineDtoCreate> s)
        {
            var total1 = s.Sum(x => x.PaidInterest);
            return total1 ?? 0;
        }
        public static decimal? TotalPaidCollectionFeeAmount(this IEnumerable<LoanRepaymentLineDtoCreate> s)
        {
            var total1 = s.Sum(x => x.PaidCollectionFee);
            return total1 ?? 0;
        }
        public static decimal? TotalPaidPenaltyAmount(this IEnumerable<LoanRepaymentLineDtoCreate> s)
        {
            var total1 = s.Sum(x => x.PenaltyAmount);
            return total1 ?? 0;
        }
        public static decimal? CalculatePenalty(int CurrencyId = 1, decimal? totalPayment = 0, decimal? rate = 0, int? totalDay = 0)
        {
            decimal? totalPenalty = 0.00M;

            if (CurrencyId == 1 && totalDay > 0)
            {
                totalPenalty = totalPayment * (rate / 100) * totalDay;
                return totalPenalty.RoundUpKhr(2);

            }
            if (CurrencyId == 2 && totalDay > 0)
            {
                totalPenalty = totalPayment * (rate / 100) * totalDay;
                return totalPenalty.RoundUsd(2);
            }

            return totalPenalty;
        }

    }
}

