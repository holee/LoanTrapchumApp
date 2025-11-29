using Applications.Commons.Mappers;
using Applications.Interfaces.Loans;
using Applications.Utils;
using ErrorOr;

namespace Applications.Features.LoanRequests.Command.AddDescription;

public sealed class AddDescriptionCommandHandler(ILoanRequestService loanRequestService) : IRequestHandler<AddDescriptionCommand, ErrorOr<string>>
{
    public async Task<ErrorOr<string>> Handle(AddDescriptionCommand command, CancellationToken cancellationToken)
    {

        var model = command.Request.MapToDescriptionCreate();

        return await loanRequestService.AddDescription(model);

    }
}
