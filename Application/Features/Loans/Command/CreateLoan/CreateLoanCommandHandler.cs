using Application.Features.Loans.Dtos.Responses;
using Application.Features.Loans.Mappers;
using Application.Interfaces.Loans;
using Application.Utils;
using ErrorOr;

namespace Application.Features.Loans.Command.CreateLoan;

public sealed class CreateLoanCommandHandler : IRequestHandler<CreateLoanCommand, ErrorOr<DisburseResult>>
{
    private readonly ILoanService _loanService;
    public CreateLoanCommandHandler(ILoanService loanService)
    {
        _loanService = loanService;
    }
    public async Task<ErrorOr<DisburseResult>> Handle(CreateLoanCommand request, CancellationToken cancellationToken)
    {

        var model = request.Req.MapLoanCreate();

        return await _loanService.CreateLoanDisbursementAsync(model);
       
    }
}
