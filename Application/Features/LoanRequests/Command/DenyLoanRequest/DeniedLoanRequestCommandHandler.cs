using Application.Features.LoanRequests.Responses;
using Application.Features.Loans.Dtos.Responses;
using Application.Interfaces.Loans;
using Application.Responses;
using Application.Utils;
using ErrorOr;

namespace Application.Features.LoanRequests.Command.DenyLoanRequest;

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
