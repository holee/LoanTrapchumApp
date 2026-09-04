using Application.Extensions.Maths;
using Application.Features.LoanPayments.Requests;
using Application.Responses.ViewModels.Loans.Schedules;

namespace Application.Calculations
{
    public static class LoanExtensions
    {

        public static PaymentDtoCreate ToPaymentDtoCreate(this VSchedule s) => new PaymentDtoCreate(s.LeftPrinciple, s.LeftInterestAmount, s.LeftCollectionFeeAmount, s.PenaltyAmount);

        public static LoanPaymentLineDtoCreate ToPaymentLine(this VSchedule s, PaymentDtoCreate p) => new LoanPaymentLineDtoCreate
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
            PaidPenaltyAmount = p.PenaltyAmountAmount,
            BranchRefId = s.BranchId,
            SubTotal = p.SubTotal()
        };


        public static LoanPaymentLineDtoCreate ToPaymentLine(this VSchedule s,
            decimal? RecievePrincipleAmount,
            decimal RecieveInterestAmount,
            decimal? RecieveCollectionFeeAmount,
            decimal? PenaltyAmountAmount) => new LoanPaymentLineDtoCreate
            {
                LoanRefId = s.LoanId,
                LoanScheduleRefId = s!.LoanScheduleId,
                InstallmentNo = s!.InstallmentNo,
                NumberOfDay = s!.TotalDay,
                DayArrea = s.DayArrea,
                PayDate = s!.PaymentDate,
                BranchRefId = s.BranchId,
                SubTotal = (RecievePrincipleAmount + RecieveInterestAmount + RecieveInterestAmount + PenaltyAmountAmount),
                PaidPrinciple = RecievePrincipleAmount,
                PaidInterest = RecieveInterestAmount,
                PaidCollectionFee = RecieveInterestAmount,
                PaidPenaltyAmount = PenaltyAmountAmount,
            };

        public static List<LoanPaymentLineDtoCreate> ToPaymentLines(this IEnumerable<VSchedule> source, PaymentDtoCreate p)
        {
            return source.Select(x => x.ToPaymentLine(p)).ToList();
        }



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
        public static decimal? SubTotal(LoanPaymentLineDtoCreate s, bool includePenalty)
        {
            decimal? total = 0.00M;

            if (includePenalty)
            {
                total = s.PaidPrinciple + s.PaidInterest + s.PaidCollectionFee + s.PaidPenaltyAmount;
                return total ?? 0;
            }
            total = s.PaidPrinciple + s.PaidInterest + s.PaidCollectionFee;
            return total ?? 0;
        }
        public static decimal? Total(this IEnumerable<LoanPaymentLineDtoCreate> s)
        {
            var total1 = s.Sum(x => x.SubTotal);
            return total1 ?? 0;
        }
        public static decimal? TotalAmountWithPenalty(this IEnumerable<LoanPaymentLineDtoCreate> s)
        {
            decimal? total = 0.00M;

            foreach (var item in s)
            {
                total += SubTotal(item, true);
            }
            return total ?? 0;
        }
        public static decimal? TotalAmountNoPenalty(this IEnumerable<LoanPaymentLineDtoCreate> s)
        {
            decimal? total = 0.00M;

            foreach (var item in s)
            {
                total += SubTotal(item, false);
            }
            return total ?? 0;
        }
        public static decimal? Total(this IEnumerable<LoanPaymentLineDtoCreate> s, bool includePenalty = true)
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
                total += item.SubTotal(includePenalty);
            }
            return total ?? 0;
        }
        public static decimal? Total(decimal? principle, decimal? interest = 0, decimal? collectionfee = 0, decimal? penalty = 0)
        {
            principle = principle + interest + collectionfee + penalty;
            return principle ?? 0;
        }
        public static decimal? TotalPaidPrinciple(this IEnumerable<LoanPaymentLineDtoCreate> source)
        {
            if (!source.Any()) return 0.00M;

            var total1 = source.Sum(x => x.PaidPrinciple);

            return total1 ?? 0.00M;
        }
        public static decimal? TotalPaidInterest(this IEnumerable<LoanPaymentLineDtoCreate> source)
        {
            if (!source.Any()) return 0.00M;

            var total1 = source.Sum(x => x.PaidInterest);

            return total1 ?? 0.00M;
        }
        public static decimal? TotalPaidCollectionFee(this IEnumerable<LoanPaymentLineDtoCreate> source)
        {
            if (!source.Any()) return 0.00M;

            var total1 = source.Sum(x => x.PaidCollectionFee);

            return total1 ?? 0;
        }
        public static decimal? TotalPaidPenalty(this IEnumerable<LoanPaymentLineDtoCreate> source)
        {
            if (!source.Any()) return 0.00M;

            var total1 = source.Sum(x => x.PaidPenaltyAmount);

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

