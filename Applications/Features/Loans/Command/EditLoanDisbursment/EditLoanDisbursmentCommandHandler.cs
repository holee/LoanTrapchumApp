using Applications.Features.Loans.Dtos.Responses;
using Applications.Features.Loans.Mappers;
using Applications.Interfaces.Loans;
using Applications.Utils;
using ErrorOr;

namespace Applications.Features.Loans.Command.EditLoanDisbursment;

public sealed class EditLoanDisbursmentCommandHandler : IRequestHandler<EditLoanDisbursmentCommand, ErrorOr<DisburseResult>>
{
    private readonly ILoanService _loanService;
    public EditLoanDisbursmentCommandHandler(ILoanService loanService)
    {
        _loanService = loanService;
    }
    public async Task<ErrorOr<DisburseResult>> Handle(EditLoanDisbursmentCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var model = request.Req.MapLoanCreate();

            return await _loanService.EditLoanDisbursementAsync(model, cancellationToken);

        }
        catch (Exception ex)
        {
            return Error.Unexpected(description: ex.Message);
        }

    }
}
