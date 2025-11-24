using Applications.Commons.Mappers.LoanRequests;
using Applications.Features.Loans.Responses;
using Applications.Interfaces.Loans;
using Applications.Responses;
using Applications.Utils;
using ErrorOr;

namespace Applications.Features.LoanRequests.Command.CreateLoanRequest;

public sealed class CreateLoanRequestCommandHandler : IRequestHandler<CreateLoanRequestCommand, ErrorOr<RequestResult>>
{
    private readonly ILoanRequestService _requestService;
    public CreateLoanRequestCommandHandler(ILoanRequestService createNewLoanRequestUseCase)
    {
        _requestService = createNewLoanRequestUseCase;
    }
    public async Task<ErrorOr<RequestResult>> Handle(CreateLoanRequestCommand request, CancellationToken cancellationToken)
    {

        var req = request.MapToLoanCreateNew();

        return await _requestService.CreateNewLoanRequestAsync(req, cancellationToken);

    }
}