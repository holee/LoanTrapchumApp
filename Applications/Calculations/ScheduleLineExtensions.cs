using Applications.Enumerations;
using Applications.Requests.Loans.Disbursements;
using Domains.Models.Loans;

namespace Applications.Calculations
{
    public static class ScheduleLineExtensions
    {
        public static IEnumerable<LoanScheduleDtoCreate> MapToLoanSchedule(this IEnumerable<ScheduleLine> source, int? BranchId, string? creditAgentId, int? repaymentModeId = 2, ScheduleStatus status = ScheduleStatus.Unpaid)
        {

            ArgumentNullException.ThrowIfNull(source, nameof(source));
            ArgumentNullException.ThrowIfNull(creditAgentId, nameof(creditAgentId));
            ArgumentNullException.ThrowIfNull(repaymentModeId, nameof(repaymentModeId));
            return source.Select(x => new LoanScheduleDtoCreate
            {
                BranchId = BranchId,
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


        public static IEnumerable<LoanScheduleDtoCreate> MapToLoanSchedule(this IEnumerable<ScheduleLine> source, LoanDtoCreate req)
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
                AgentRefId = req.CreditAgentRefId ?? string.Empty,
                RepaymentModeRefId = req.RepaymentModeRefId ?? 1,
                ScheduleStatusRefId = (int)ScheduleStatus.Unpaid,
            });
        }

        public static IEnumerable<LoanScheduleDtoCreate> MapToLoanSchedule(this IEnumerable<ScheduleLine> source, string? creditAgentId, int repaymentModeId = 1)
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
                AgentRefId = creditAgentId ?? string.Empty,
                RepaymentModeRefId = repaymentModeId,
                ScheduleStatusRefId = (int)ScheduleStatus.Unpaid,
            });
        }


        public static IEnumerable<LoanScheduleDtoCreate> MapToLoanSchedule(this IEnumerable<ScheduleLine> source)
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
                AgentRefId = x.CreditAgentRefId ?? string.Empty,
                RepaymentModeRefId = x.RepaymentModeRefId ?? 1,
                ScheduleStatusRefId = x.ScheduleStatusRefId,
            });
        }

        public static IEnumerable<LoanScheduleDtoCreate> MapTo(this IEnumerable<ScheduleLine> source, int repaymentModeId = 2, ScheduleStatus status = ScheduleStatus.Unpaid)
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
}
