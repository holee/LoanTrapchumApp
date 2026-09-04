using Application.Features.Loans.Dtos.Responses;
using ErrorOr;

namespace Application.Interfaces
{
    public interface IScheduleService 
    {
        Task<ErrorOr<IEnumerable<LoanResponse>>> GetLoanDisbursments(int? branchId, string? loanId, CancellationToken cancellation = default);   
        Task<ErrorOr<LoanResponse>> CreatdPdf(int? branchId, string? loanId, int? scheduleNum, CancellationToken cancellation = default);
        Task<ErrorOr<IEnumerable<LoanResponse>>> GetLoanWithSchedules2(int? branchId, string? loanId, CancellationToken cancellation = default);
        Task<ErrorOr<IEnumerable<LoanResponse>>> GetLoanWithSchedules(int? branchId, string? loanId, CancellationToken cancellation = default);
        Task<ErrorOr<LoanResponse>> CreatePdf(int? branchId, string? loanId, CancellationToken cancellation = default);
        Task<ErrorOr<LoanResponse>> GetLoanWithSchedules(int? branchId, string? loanId, int? scheduleNum, CancellationToken cancellation = default);
    }
}

//Task<Result<IEnumerable<LoanScheduleResponse>>> GetLoanWithSchedules(int? branchId, string? loanId, CancellationToken cancellation = default);
