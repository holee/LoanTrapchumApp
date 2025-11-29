using Applications.Features.Loans.Mappers;
using Applications.Features.Loans.Responses;
using Applications.Interfaces.Loans;
using Applications.Utils;
using ErrorOr;

namespace Applications.Features.Loans.Command.CreateLoan;

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

        return await _loanService.CreateAsync(model);
       
    }
}
