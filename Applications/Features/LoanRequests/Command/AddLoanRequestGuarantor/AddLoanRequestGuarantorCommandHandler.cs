using Applications.Commons.Mappers;
using Applications.Interfaces.Loans;
using Applications.Utils;
using ErrorOr;

namespace Applications.Features.LoanRequests.Command.AddLoanRequestGuarantor;

public sealed class AddLoanRequestGuarantorCommandHandler(ILoanRequestService loanRequestService) : IRequestHandler<AddLoanRequestGuarantorCommand, ErrorOr<string>>
{
    public async Task<ErrorOr<string>> Handle(AddLoanRequestGuarantorCommand command, CancellationToken cancellationToken)
    {
        var req = command.Request.MapTo();

        return await loanRequestService.AddLoanRequestGuarantorAsync(req);
    }
}
