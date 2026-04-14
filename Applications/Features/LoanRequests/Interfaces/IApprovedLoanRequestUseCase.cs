using Applications.Features.LoanRequests.Requests;
using Applications.Features.Loans.Dtos.Responses;
using Applications.Responses;

namespace Applications.Features.LoanRequests.Interfaces
{
    public interface IApprovedLoanRequestUseCase
    {
        Task<Result<RequestResult>> ExecuteAsync(AproveLoanDtoCreate req);
    }
}