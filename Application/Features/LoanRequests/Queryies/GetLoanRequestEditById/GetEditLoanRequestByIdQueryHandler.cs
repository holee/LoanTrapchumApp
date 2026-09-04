using Application.Interfaces.Loans;
using Application.Responses.ViewModels.Loans.Requests;
using Application.Utils;
using Domains.Models.Loans;
using ErrorOr;

namespace Application.Features.LoanRequests.Queryies.GetLoanRequestById;

public sealed class GetEditLoanRequestByIdQueryHandler : IRequestHandler<GetEditLoanRequestByIdQuery, ErrorOr<LoanRequest>>
{
    private readonly ILoanRequestService _loanRequestService;

    public GetEditLoanRequestByIdQueryHandler(ILoanRequestService loanRequestService)
    {
        _loanRequestService = loanRequestService;
    }

    public async Task<ErrorOr<LoanRequest>> Handle(GetEditLoanRequestByIdQuery request, CancellationToken cancellationToken)
    {
        return await _loanRequestService.GetEditLoanRequestAsync(request.BranchId, request.CustomerId, request.LoanRequestId, cancellationToken);
    }
}
