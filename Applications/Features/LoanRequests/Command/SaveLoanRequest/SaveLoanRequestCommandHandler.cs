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

        try
        {
            var req = request.MapToLoanRequestSave();

            req.LoanRequestStatusRefId = (int)RequestStatus.New;

            return await _loanRequestService.CreateOrSaveLoanRequestAsync(req, cancellationToken);
        }
        catch (Exception ex)
        {

            throw new Exception("An error occurred while saving the loan request.", ex);
        }

    }
}