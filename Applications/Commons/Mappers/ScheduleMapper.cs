using Applications.Calculations;
using Applications.Requests.Loans.Payments;
using Applications.Responses.ViewModels.Loans.Schedules;

namespace Applications.Commons.Mappers
{
    public static class ScheduleMapper
    {

        public static LoanPaymentLineDtoCreate MapTo(this VSchedule s) => new LoanPaymentLineDtoCreate
        {
            LoanRefId = s.LoanId,
            LoanScheduleRefId = s!.LoanScheduleId,
            InstallmentNo = s!.InstallmentNo,
            NumberOfDay = s!.TotalDay,
            DayArrea = s.DayArrea,
            PayDate = s!.PaymentDate,
            PaidPrinciple = s.LeftPrinciple,
            PaidInterest = s.LeftInterestAmount,
            PaidCollectionFee = s.LeftCollectionFeeAmount,
            PaidPenaltyAmount = s.PenaltyAmount,
            BranchRefId = s.BranchId,
            SubTotal = s.LeftAmount
        };

        public static List<LoanPaymentLineDtoCreate> MapsTo(this IEnumerable<VSchedule> source)
        {
            return source.Select(x => x.MapTo()).ToList();
        }

    }
}
