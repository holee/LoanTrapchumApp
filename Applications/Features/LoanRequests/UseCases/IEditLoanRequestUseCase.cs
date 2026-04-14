using Applications.Features.LoanRequests.Requests;
using Applications.Features.Loans.Dtos.Responses;
using Applications.Responses;

namespace Applications.Features.LoanRequests.UseCases
{
    public interface IEditLoanRequestUseCase
    {
        Task<Result<RequestResult>> ExecuteAsync(LoanRequestDtoCreate request, CancellationToken cancellation = default);
    }
}