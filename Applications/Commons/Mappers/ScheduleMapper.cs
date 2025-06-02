using Applications.Calculations;
using Applications.Requests.Loans.Payments;
using Applications.Responses.ViewModels.Loans;
using Applications.Responses.ViewModels.Loans.Schedules;
using Applications.Responses.ViewModels.Reports;

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


        //public static ScheduleRpt MapToScheduleRpt(this VSchedule s) => new ScheduleRpt
        //{
        //    InstallmentNo = s!.InstallmentNo,
        //    TotalDay = s!.TotalDay,
        //    PaymentDate = s!.PaymentDate,
        //    Principal = s.Principle,
        //    Interest = s.Interest,
        //    CollectionFee = s.CollectionFee,
        //    TotalPay = s.TotalPay,
        //    LoanOutstanding = s.LoanOutstanding
        //};

        //public static List<ScheduleRpt> MapsToScheduleRpt(this IEnumerable<VSchedule> source)
        //{
        //    return source.Select(x => x.MapToScheduleRpt()).ToList();
        //}

        //public static List<ScheduleRpt> MapsToScheduleRpt(this VLoanDisbursments? source)
        //{
        //    if (source == null || (source.Schedules is null)) return new List<ScheduleRpt>();

        //    return source.Schedules.Select(x => x.MapToScheduleRpt()).ToList();

        //}


    }
}
