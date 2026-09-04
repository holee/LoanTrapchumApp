using Application.Commons.Mappers.LoanRequests;
using Application.Enumerations;
using Application.Features.Loans.Dtos.Responses;
using Application.Interfaces.Loans;
using Application.Responses;
using Application.Utils;
using ErrorOr;

namespace Application.Features.LoanRequests.Command.SaveEditLoanRequest;

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