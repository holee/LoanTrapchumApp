using Applications.Responses;
using Applications.Responses.ViewModels.Loans.Schedules;

namespace Applications.Services.Loans
{
    public interface ISchedulesService
    {
        Task<Result<VCustomerSchedules>> GetCustomerSechedule(long loanId, long customerId, int branchId);
    }
}
