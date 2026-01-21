using Applications.Features.Loans.Responses;
using Applications.Responses;

namespace Applications.Interfaces
{
    public interface ILoanScheduleRepo 
    {
        Task<Result<IEnumerable<LoanScheduleResponse>>> GetLoanDisbursments(int? branchId, string? loanId, CancellationToken cancellation = default);
     
        Task<Result<LoanScheduleResponse>> CreatdPdf(int? branchId, string? loanId, int? scheduleNum, CancellationToken cancellation = default);
        Task<Result<IEnumerable<LoanScheduleResponse>>> GetLoanWithSchedules2(int? branchId, string? loanId, CancellationToken cancellation = default);
        Task<Result<IEnumerable<LoanScheduleResponse>>> GetLoanWithSchedules(int? branchId, string? loanId, CancellationToken cancellation = default);
    }
}

//Task<Result<IEnumerable<LoanScheduleResponse>>> GetLoanWithSchedules(int? branchId, string? loanId, CancellationToken cancellation = default);
