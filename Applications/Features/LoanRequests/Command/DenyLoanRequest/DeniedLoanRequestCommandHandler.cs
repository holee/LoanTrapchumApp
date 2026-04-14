using Applications.Features.LoanRequests.Responses;
using Applications.Features.Loans.Dtos.Responses;
using Applications.Interfaces.Loans;
using Applications.Responses;
using Applications.Utils;
using ErrorOr;

namespace Applications.Features.LoanRequests.Command.DenyLoanRequest;

public sealed class DeniedLoanRequestCommandHandler : IRequestHandler<DeniedLoanRequestCommand, ErrorOr<RequestResult>>
{
    private readonly ILoanRequestService _requestService;

    public DeniedLoanRequestCommandHandler(ILoanRequestService requestService)
    {
        _requestService = requestService;
    }

    public async Task<ErrorOr<RequestResult>> Handle(DeniedLoanRequestCommand request, CancellationToken cancellationToken)
    {
        return await _requestService.DeniedLoanRequestAsync(request.Request,cancellationToken);
    }
}
