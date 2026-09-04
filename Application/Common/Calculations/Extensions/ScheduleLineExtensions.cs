namespace Application.Calculations.Extensions
{
    public static class ScheduleLineExtensions
    {

        //public static IEnumerable<LoanScheduleDtoCreate> MapToLoanSchedule(this IEnumerable<ScheduleLine> source, LoanDtoCreate req)
        //{

        //    ArgumentNullException.ThrowIfNull(source, nameof(source));

        //    return source.Select(x => new LoanScheduleDtoCreate
        //    {
        //        BranchId = x.BranchId,
        //        InstallmentNo = x.InstallmentNo,
        //        PaymentDate = x.PaymentDate,
        //        Principal = x.Principal,
        //        Interest = x.Interest,
        //        CollectionFee = x.CollectionFeeAmount,
        //        LoanOutstanding = x.OutStanding,
        //        TotalDay = x.TotalDay,
        //        TotalPay = x.TotalPay,
        //        ScheduleNum = x.ScheduleNum,
        //        AgentRefId = req.CreditAgentRefId ?? string.Empty,
        //        RepaymentModeRefId = req.RepaymentModeRefId ?? 1,
        //        ScheduleStatusRefId = (int)ScheduleStatus.Unpaid,
        //    });
        //}

        //public static IEnumerable<LoanScheduleDtoCreate> MapToLoanSchedule(this IEnumerable<ScheduleLine> source, string? creditAgentId, int repaymentModeId = 1)
        //{

        //    ArgumentNullException.ThrowIfNull(source, nameof(source));

        //    return source.Select(x => new LoanScheduleDtoCreate
        //    {
        //        BranchId = x.BranchId,
        //        InstallmentNo = x.InstallmentNo,
        //        PaymentDate = x.PaymentDate,
        //        Principal = x.Principal,
        //        Interest = x.Interest,
        //        CollectionFee = x.CollectionFeeAmount,
        //        LoanOutstanding = x.OutStanding,
        //        TotalDay = x.TotalDay,
        //        TotalPay = x.TotalPay,
        //        ScheduleNum= x.ScheduleNum,
        //        AgentRefId = creditAgentId ?? string.Empty,
        //        RepaymentModeRefId = repaymentModeId,
        //        ScheduleStatusRefId = (int)ScheduleStatus.Unpaid,
        //    });
        //}


        //public static IEnumerable<LoanScheduleDtoCreate> MapToLoanSchedules(this IEnumerable<ScheduleLine> source)
        //{

        //    ArgumentNullException.ThrowIfNull(source, nameof(source));

        //    return source.Select(x => x.MapToLoanSchedule());

        //}

        //public static LoanScheduleDtoCreate MapToLoanSchedule(this ScheduleLine source)
        //{

        //    ArgumentNullException.ThrowIfNull(source, nameof(source));

        //    return new LoanScheduleDtoCreate
        //    {
        //        BranchId = source.BranchId,
        //        InstallmentNo = source.InstallmentNo,
        //        PaymentDate = source.PaymentDate,
        //        Principal = source.Principal,
        //        Interest = source.Interest,
        //        CollectionFee = source.CollectionFeeAmount,
        //        LoanOutstanding = source.OutStanding,
        //        TotalDay = source.TotalDay,
        //        TotalPay = source.TotalPay,
        //        ScheduleNum = source.ScheduleNum,
        //        AgentRefId = source.CreditAgentRefId ?? string.Empty,
        //        RepaymentModeRefId = source.RepaymentModeRefId ?? 1,
        //        ScheduleStatusRefId = (int)ScheduleStatus.Unpaid,
        //    };
        //}




    }


}

//public static IEnumerable<LoanScheduleDtoCreate> MapToLoanSchedule(this IEnumerable<ScheduleLine> source, int? BranchId, string? creditAgentId, int? repaymentModeId = 2, ScheduleStatus status = ScheduleStatus.Unpaid)
//{

//    ArgumentNullException.ThrowIfNull(source, nameof(source));
//    ArgumentNullException.ThrowIfNull(creditAgentId, nameof(creditAgentId));
//    ArgumentNullException.ThrowIfNull(repaymentModeId, nameof(repaymentModeId));
//    return source.Select(x => new LoanScheduleDtoCreate
//    {
//        BranchId = BranchId,
//        InstallmentNo = x.InstallmentNo,
//        PaymentDate = x.PaymentDate,
//        Principal = x.Principal,
//        Interest = x.Interest,
//        CollectionFee = x.CollectionFeeAmount,
//        LoanOutstanding = x.OutStanding,
//        TotalDay = x.TotalDay,
//        TotalPay = x.TotalPay,
//        AgentRefId = creditAgentId ?? string.Empty,
//        RepaymentModeRefId = repaymentModeId ?? 1,
//        ScheduleStatusRefId = (int)status,
//    });
//}

//public static IEnumerable<LoanScheduleDtoCreate> MapTo(this IEnumerable<ScheduleLine> source, int repaymentModeId = 2, ScheduleStatus status = ScheduleStatus.Unpaid)
//{
//    ArgumentNullException.ThrowIfNull(source, nameof(source));
//    return source.Select(x => new LoanScheduleDtoCreate
//    {
//        InstallmentNo = x.InstallmentNo,
//        PaymentDate = x.PaymentDate,
//        Principal = x.Principal,
//        Interest = x.Interest,
//        CollectionFee = x.CollectionFeeAmount,
//        LoanOutstanding = x.OutStanding,
//        TotalDay = x.TotalDay,
//        TotalPay = x.TotalPay,
//        AgentRefId = string.Empty,
//        RepaymentModeRefId = repaymentModeId,
//        ScheduleStatusRefId = (int)status,
//    });