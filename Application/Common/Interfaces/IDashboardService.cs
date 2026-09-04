using Application.Features.Dashbaord.Accountants.Dtos;
using Application.Features.Dashbaord.Dto;
using Application.Features.Dashbaord.Managers.Dto;
using ErrorOr;

namespace Application.Interfaces
{
    public interface IDashboardService
    {
        Task<Pager<AccountantLoanSummary>> GetAccountantLoanSummaries(int? branchId, DateTime? dueDate, CancellationToken cancellation = default);
        Task<AccountSummary> GetAccountantSummaries(int? branchId, DateTime? dueDate, CancellationToken cancellation = default);
        Task<ErrorOr<int>> GetApprovedLoanRequest(int? branchId, CancellationToken cancellation = default);
        Task<CoLoanSummary> GetCreditOfficerSummaries(int? branchId, string? UserId, DateTime? dueDate, CancellationToken cancellation = default);
        Task<Pager<DbCoLoanSummary>> GetDbCreditOfficerSummaries(int? branchId, string? UserId, DateTime? dueDate, CancellationToken cancellation = default);
        Task<ErrorOr<DbManagerLoanSummary>> GetDbManagerLoanSummary(int? branchId, string? UserId, CancellationToken cancellation = default);
        Task<ErrorOr<NumerLoanRequest>> GetNumberDueCustomerByCo(int? branchId, string? UserId, CancellationToken cancellation = default);
        Task<ErrorOr<NumerLoanRequest>> GetNumberLoanRequestByCo(int? branchId, string? UserId, CancellationToken cancellation = default);
        Task<ErrorOr<int>> GetNumberLoanRequestByManager(int? branchId, string? UserId, CancellationToken cancellation = default);
    }
}
