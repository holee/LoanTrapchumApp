using Applications.Enumerations;
using Applications.Responses;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Loans.Schedules;

namespace Applications.Services.Loans
{
    public interface ILoanScheduleService
    {
        Task<Result<VSchedule>> GetSchedulesByIdAsync(Guid? loanId, int? branchId, long? scheduleId);
        Task<Pager<VSchedule>> GetLoanScheduleByIdAsync(Guid? loanId, int? branchId, ScheduleStatus status = ScheduleStatus.All, QueryParam? param = null);
        Task<Pager<VSchedule>> GetLoanSchedulesByPaymentDateAsync(int? branchId, DateTimeOffset? paymentDate, bool isIncludeLate = false, bool isLateOnly = false, QueryParam? param = null);

    }

}
// Task<Result<IEnumerable<VSchedule>>> GetLoanPaymentSchedules(DateTime paymentDate, int branchId, bool includeLateSchedule = false, string? filter = null);

//Task<Pager<VSchedule>> GetLoanSchedules(int? branchId, ScheduleStatus schedule, DateTimeOffset? paymentDate = null, QueryParam? param = null);

//Task<Result<IEnumerable<VPaymentLines>>> GetPayementScheduleLines(int branchId, long loanId, int installmentNo);

//Task<Result<VSchedule>> GetLoanScheduleByIdAsync(long loanId, long loanScheduleId, int branchId, bool related);