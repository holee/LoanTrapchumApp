using Applications.Commons.Mappers.LoanRequests;
using Applications.Enumerations;
using Applications.Features.Loans.Dtos.Responses;
using ErrorOr;

namespace Applications.Features.LoanRequests.Command.SaveLoanRequest;

public sealed class SaveLoanRequestCommandHandler : IRequestHandler<SaveLoanRequestCommand, ErrorOr<RequestResult>>
{
    private readonly ILoanRequestService _loanRequestService;
    public SaveLoanRequestCommandHandler(ILoanRequestService loanRequestService)
    {
        _loanRequestService = loanRequestService;
    }
    public async Task<ErrorOr<RequestResult>> Handle(SaveLoanRequestCommand request, CancellationToken cancellationToken)
    {

        var req = request.MapToLoanRequestSave();

        req.LoanRequestStatusRefId = (int)RequestStatus.New;

        return await _loanRequestService.CreateNewOrSaveLoanRequestAsync(req, cancellationToken);
    }
}