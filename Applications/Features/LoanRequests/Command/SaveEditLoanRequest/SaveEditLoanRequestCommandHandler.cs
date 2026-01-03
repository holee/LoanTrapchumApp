using Applications.Commons.Mappers.LoanRequests;
using Applications.Enumerations;
using Applications.Features.Loans.Responses;
using Applications.Interfaces.Loans;
using Applications.Responses;
using Applications.Utils;
using ErrorOr;

namespace Applications.Features.LoanRequests.Command.SaveEditLoanRequest;

public sealed class SaveEditLoanRequestCommandHandler : IRequestHandler<SaveEditLoanRequestCommand, ErrorOr<RequestResult>>
{
    private readonly ILoanRequestService _loanRequestService;
    public SaveEditLoanRequestCommandHandler(ILoanRequestService loanRequestService) 
    {
        _loanRequestService = loanRequestService;
    }
    public async Task<ErrorOr<RequestResult>> Handle(SaveEditLoanRequestCommand request, CancellationToken cancellationToken)
    {

        var req = request.MapToLoanRequestSave();

        return await _loanRequestService.EditLoanRequestAsync(req, cancellationToken);

    }
}