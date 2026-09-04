using Application.Commons.Mappers;
using Application.Interfaces.Loans;
using Application.Utils;
using ErrorOr;

namespace Application.Features.LoanRequests.Command.AddLoanRequestGuarantor;

public sealed class AddLoanRequestGuarantorCommandHandler(ILoanRequestService loanRequestService) : IRequestHandler<AddLoanRequestGuarantorCommand, ErrorOr<string>>
{
    public async Task<ErrorOr<string>> Handle(AddLoanRequestGuarantorCommand command, CancellationToken cancellationToken)
    {
        var req = command.Request.MapTo();

        return await loanRequestService.AddLoanRequestGuarantorAsync(req);
    }
}
