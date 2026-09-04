using Application.Commons.Mappers.LoanRequests;
using Application.Features.Loans.Dtos.Responses;
using ErrorOr;

namespace Application.Features.LoanRequests.Command.CreateLoanRequest;

public sealed class CreateLoanRequestCommandHandler : IRequestHandler<CreateLoanRequestCommand, ErrorOr<RequestResult>>
{
    private readonly ILoanRequestService _requestService;
    public CreateLoanRequestCommandHandler(ILoanRequestService createNewLoanRequestUseCase)
    {
        _requestService = createNewLoanRequestUseCase;
    }
    public async Task<ErrorOr<RequestResult>> Handle(CreateLoanRequestCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var req = request.MapToLoanCreateNew();

            return await _requestService.CreateNewOrSaveLoanRequestAsync(req, cancellationToken);
        }
        catch (Exception ex)
        {

            throw new Exception("An error occurred while creating the loan request.", ex);
        }

      

    }
}