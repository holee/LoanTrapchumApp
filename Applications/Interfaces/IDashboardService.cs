using Applications.Features.Dashbaord.Accountants.Dtos;
using Applications.Features.Dashbaord.CreditOfficers.Dtos;
using Applications.Features.Dashbaord.Managers.Dto;
using ErrorOr;

namespace Applications.Interfaces
{
    public interface IDashboardService
    {
        Task<Pager<AccountantLoanSummary>> GetAccountantLoanSummaries(int? branchId, DateTime? dueDate, CancellationToken cancellation = default);
        Task<ErrorOr<int>> GetApprovedLoanRequest(int? branchId, CancellationToken cancellation = default);
        Task<Pager<DbCoLoanSummary>> GetDbCreditOfficerSummaries(int? branchId, string? UserId, DateTime? dueDate, CancellationToken cancellation = default);
        Task<ErrorOr<DbManagerLoanSummary>> GetDbManagerLoanSummary(int? branchId, string? UserId, CancellationToken cancellation = default);
        Task<ErrorOr<NumerLoanRequest>> GetNumberDueCustomerByCo(int? branchId, string? UserId, CancellationToken cancellation = default);
        Task<ErrorOr<NumerLoanRequest>> GetNumberLoanRequestByCo(int? branchId, string? UserId, CancellationToken cancellation = default);
        Task<ErrorOr<int>> GetNumberLoanRequestByManager(int? branchId, string? UserId, CancellationToken cancellation = default);
    }
}
