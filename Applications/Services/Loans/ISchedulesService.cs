using Applications.Responses;
using Applications.Responses.ViewModels.Loans.Schedules;

namespace Applications.Services.Loans
{
    public interface ISchedulesService
    {
        Task<Result<LoanScheduleVm>> GetCustomerSechedule(long loanId, long customerId, int branchId);
    }
}
