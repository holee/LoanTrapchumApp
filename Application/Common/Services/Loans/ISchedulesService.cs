using Application.Responses;
using Application.Responses.ViewModels.Loans.Schedules;

namespace Application.Services.Loans
{
    public interface ISchedulesService
    {
        Task<Result<LoanScheduleVm>> GetCustomerSechedule(long loanId, long customerId, int branchId);
    }
}
