using Applications.Enumerations;
using Applications.Features.Loans.Requests;
using Applications.Features.Loans.Requests;
using Domains.Models.Loans;
namespace Applications.Responses;

public static class ScheduleLineExtensions
{
    public static IEnumerable<LoanScheduleDtoCreate> ToSchedules(this IEnumerable<ScheduleLine> source,
        int? branchId,string? creditAgentId,int? repaymentModeId = 2, 
        ScheduleStatus status = ScheduleStatus.Unpaid)
    {

        ArgumentNullException.ThrowIfNull(source, nameof(source));
        ArgumentNullException.ThrowIfNull(creditAgentId, nameof(creditAgentId));
        ArgumentNullException.ThrowIfNull(repaymentModeId, nameof(repaymentModeId));
        return source.Select(x => new LoanScheduleDtoCreate
        {
            BranchId = branchId,
            InstallmentNo = x.InstallmentNo,
            PaymentDate = x.PaymentDate,
            Principal = x.Principal,
            Interest = x.Interest,
            CollectionFee = x.CollectionFeeAmount,
            LoanOutstanding = x.OutStanding,
            TotalDay = x.TotalDay,
            TotalPay = x.TotalPay,
            AgentRefId = creditAgentId ?? string.Empty,
            RepaymentModeRefId = repaymentModeId ?? 1,
            ScheduleStatusRefId = (int)status,
        });
    }

    public static IEnumerable<LoanScheduleDtoCreate> ToSchedules(this IEnumerable<ScheduleLine> source, int repaymentModeId = 2, ScheduleStatus status = ScheduleStatus.Unpaid)
    {

        ArgumentNullException.ThrowIfNull(source, nameof(source));
        return source.Select(x => new LoanScheduleDtoCreate
        {
            InstallmentNo = x.InstallmentNo,
            PaymentDate = x.PaymentDate,
            Principal = x.Principal,
            Interest = x.Interest,
            CollectionFee = x.CollectionFeeAmount,
            LoanOutstanding = x.OutStanding,
            TotalDay = x.TotalDay,
            TotalPay = x.TotalPay,
            AgentRefId = string.Empty,
            RepaymentModeRefId = repaymentModeId,
            ScheduleStatusRefId = (int)status,
        });
    }
}
