using Applications.Enumerations;
using Domains.Models.Loans;
using Applications.Features.Loans.Dtos.Requests;

namespace Applications.Features.Loans.Mappers
{
    public static class ScheduleLineMapper
    {
        public static IEnumerable<LoanScheduleDtoCreate> MapToLoanSchedule(this IEnumerable<ScheduleLine> source,
            LoanDtoCreate req)
        {

            ArgumentNullException.ThrowIfNull(source, nameof(source));

            return source.Select(x => new LoanScheduleDtoCreate
            {
                BranchId = x.BranchId,
                InstallmentNo = x.InstallmentNo,
                PaymentDate = x.PaymentDate,
                Principal = x.Principal,
                Interest = x.Interest,
                CollectionFee = x.CollectionFeeAmount,
                LoanOutstanding = x.OutStanding,
                TotalDay = x.TotalDay,
                TotalPay = x.TotalPay,
                ScheduleNum = x.ScheduleNum,
                AgentRefId = req.CreditAgentRefId ?? string.Empty,
                RepaymentModeRefId = req.RepaymentModeRefId ?? 1,
                ScheduleStatusRefId = (int)ScheduleStatus.Unpaid,
            });
        }

        public static IEnumerable<LoanScheduleDtoCreate> MapToLoanSchedule(this IEnumerable<ScheduleLine> source,
            string? creditAgentId, int repaymentModeId = 1)
        {

            ArgumentNullException.ThrowIfNull(source, nameof(source));

            return source.Select(x => new LoanScheduleDtoCreate
            {
                BranchId = x.BranchId,
                InstallmentNo = x.InstallmentNo,
                PaymentDate = x.PaymentDate,
                Principal = x.Principal,
                Interest = x.Interest,
                CollectionFee = x.CollectionFeeAmount,
                LoanOutstanding = x.OutStanding,
                TotalDay = x.TotalDay,
                TotalPay = x.TotalPay,
                ScheduleNum = x.ScheduleNum,
                AgentRefId = creditAgentId ?? string.Empty,
                RepaymentModeRefId = repaymentModeId,
                ScheduleStatusRefId = (int)ScheduleStatus.Unpaid,
            });
        }

        public static IEnumerable<LoanScheduleDtoCreate> MapToLoanSchedules(this IEnumerable<ScheduleLine> source)
        {

            ArgumentNullException.ThrowIfNull(source, nameof(source));

            return source.Select(x => x.MapToLoanSchedule());

        }

        public static LoanScheduleDtoCreate MapToLoanSchedule(this ScheduleLine source)
        {

            ArgumentNullException.ThrowIfNull(source, nameof(source));

            return new LoanScheduleDtoCreate
            {
                BranchId = source.BranchId,
                InstallmentNo = source.InstallmentNo,
                PaymentDate = source.PaymentDate,
                Principal = source.Principal,
                Interest = source.Interest,
                CollectionFee = source.CollectionFeeAmount,
                LoanOutstanding = source.OutStanding,
                TotalDay = source.TotalDay,
                TotalPay = source.TotalPay,
                ScheduleNum = source.ScheduleNum,
                AgentRefId = source.CreditAgentRefId ?? string.Empty,
                RepaymentModeRefId = source.RepaymentModeRefId ?? (int)RepaymentOption.Declined,
                ScheduleStatusRefId = (int)ScheduleStatus.Unpaid
            };
        }
    }
}
