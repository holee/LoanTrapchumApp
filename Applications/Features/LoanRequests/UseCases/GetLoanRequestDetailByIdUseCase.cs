using Applications.Features.LoanRequests.Interfaces;
using Applications.Interfaces.Loans;
using Applications.Responses;
using Applications.Responses.ViewModels.Loans.Requests;

namespace Applications.Features.LoanRequests.UseCases;

public sealed class GetLoanRequestDetailByIdUseCase : IGetLoanRequestDetailByIdUseCase
{
    private readonly ILoanRequestService _loanRequestService;
    public GetLoanRequestDetailByIdUseCase(ILoanRequestService loanRequestService)
    {
        _loanRequestService = loanRequestService;
    }

    public async Task<Result<VLoanRequests>> ExecuteAsync(int? branchId, string? customerId, string? loanRequestId, bool include)
    {
        if(!branchId.HasValue || string.IsNullOrEmpty(customerId) || string.IsNullOrEmpty(loanRequestId))
        {
            return Result.Failure<VLoanRequests>(ErrorResponse.Validation("BranchId,CustomerId and LoanRequestId is required."));
        }

        return await _loanRequestService.GetLoanRequestAsync(branchId, customerId, loanRequestId, include);
    }
}
