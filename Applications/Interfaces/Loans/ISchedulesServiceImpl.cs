using Applications.Responses;
using Applications.Responses.ViewModels.Loans.Schedules;

namespace Applications.Interfaces.Loans
{
    public interface ISchedulesServiceImpl
    {
        Task<Result<VCustomerSchedules>> GetCustomerSechedule(long loanId, long customerId, int branchId);
    }
}
