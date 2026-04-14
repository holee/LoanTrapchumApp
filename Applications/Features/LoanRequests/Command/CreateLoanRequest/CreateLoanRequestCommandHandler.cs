using Applications.Commons.Mappers.LoanRequests;
using Applications.Features.Loans.Dtos.Responses;
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