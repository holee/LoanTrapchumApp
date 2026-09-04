using Application.Enumerations;
using Application.Features.Schedules.Dto;

namespace Application.Interfaces.Loans
{
    public interface ILoanScheduleService
    {
        //Task<VSchedule?> GetSchedulesByIdAsync(string? loanId, int? branchId, long? scheduleId);
        Task<Pager<VSchedule>> GetLoanScheduleByIdAsync(string? loanId, int? branchId, ScheduleStatus status = ScheduleStatus.All, QueryParam? param = null);
        Task<Pager<VSchedule>> GetLoanSchedulesByPaymentDateAsync(DateTimeOffset? paymentDate, int? branchId, bool isIncludeLate = false, bool isLateOnly = false, QueryParam? param = null);
        //Task<Result<LoanScheduleVm>> GetLoanSchedulesAsync(int? branchId,string? loanId, string? customerId);
        //Task<Result<IEnumerable<VSchedule>>> GetLoanScheduleByIdAsync(string? loanId, int? branchId, ScheduleStatus status = ScheduleStatus.All);
        Task<Pager<VSchedule>> GetLateSchedulesAsync(int? branchId, string? loanId, DateTimeOffset? paymentDate, CancellationToken cancellation = default);
        Task<Pager<VSchedule>> GetLoanSchedulesMonthToDateAsync(int? branchId, DateTimeOffset? startDate, DateTimeOffset? endDate, QueryParam? Param, CancellationToken token = default);
        Task<VSchedule?> GetScheduleByIdAsync(long? scheduleId, int? branchId, string? loanId, CancellationToken cancellation = default);
        Task<ScheduleDetailsDto?> GetScheduleDetailsById(int? branchId, string? loanId, long? scheduleId, CancellationToken cancellation = default);
        Task<IEnumerable<VSchedule>> GetLoanScheduleByIdAsync(string? loanId, int? branchId, ScheduleStatus status = ScheduleStatus.All, CancellationToken cancellation = default);
    }

}
// Task<Result<IEnumerable<VSchedule>>> GetLoanPaymentSchedules(DateTime paymentDate, int branchId, bool includeLateSchedule = false, string? filter = null);

//Task<Pager<VSchedule>> GetLoanSchedules(int? branchId, ScheduleStatus schedule, DateTimeOffset? paymentDate = null, QueryParam? param = null);

//Task<Result<IEnumerable<VPaymentLines>>> GetPayementScheduleLines(int branchId, long loanId, int installmentNo);

//Task<Result<VSchedule>> GetLoanScheduleByIdAsync(long loanId, long loanScheduleId, int branchId, bool related);