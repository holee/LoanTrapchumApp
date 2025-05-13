using Applications.Enumerations;
using Applications.Interfaces.Loans;
using Applications.Responses;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Loans.Schedules;

namespace Applications.Services.Loans
{
    public interface ILoanScheduleServiceImpl
    {
        Task<Result<VSchedule>> GetSchedulesByIdAsync(string? loanId, int? branchId, long? scheduleId);
        Task<Pager<VSchedule>> GetLoanScheduleByIdAsync(string? loanId, int? branchId, ScheduleStatus status = ScheduleStatus.All, QueryParam? param = null);
        Task<Pager<VSchedule>> GetLoanSchedulesByPaymentDateAsync(int? branchId, DateTimeOffset? paymentDate, bool isIncludeLate = false, bool isLateOnly = false, QueryParam? param = null);
        Task<Result<LoanScheduleVm>> GetLoanSchedulesAsync(int? branchId, string? loanId, string? customerId);
        Task<Result<IEnumerable<VSchedule>>> GetLoanScheduleByIdAsync(string? loanId, int? branchId, ScheduleStatus status = ScheduleStatus.All);
    }

    public sealed class LoanScheduleService(ILoanScheduleService loanSchedule) : ILoanScheduleServiceImpl
    {
        public async Task<Pager<VSchedule>> GetLoanScheduleByIdAsync(string? loanId, int? branchId, ScheduleStatus status = ScheduleStatus.All, QueryParam? param = null)
        {
            return await loanSchedule.GetLoanScheduleByIdAsync(loanId, branchId, status,param);
        }

        public async Task<Result<IEnumerable<VSchedule>>> GetLoanScheduleByIdAsync(string? loanId, int? branchId, ScheduleStatus status = ScheduleStatus.All)
        {
            return await loanSchedule.GetLoanScheduleByIdAsync(loanId, branchId, status);
        }

        public async Task<Pager<VSchedule>> GetLoanSchedulesByPaymentDateAsync(int? branchId, DateTimeOffset? paymentDate, bool isIncludeLate = false, bool isLateOnly = false, QueryParam? param = null)
        {
            return await loanSchedule.GetLoanSchedulesByPaymentDateAsync(paymentDate,branchId,isIncludeLate,isLateOnly,param);
        }

        public async Task<Result<VSchedule>> GetSchedulesByIdAsync(string? loanId, int? branchId, long? scheduleId)
        {
            return await loanSchedule.GetSchedulesByIdAsync(loanId, branchId, scheduleId);
        }

        public async Task<Result<LoanScheduleVm>> GetLoanSchedulesAsync(int? branchId,string? loanId,string? customerId)
        {
            return await loanSchedule.GetLoanSchedulesAsync(branchId,loanId, customerId);
        }


    }

}
