using Application.Commons.Mappers;
using Application.Interfaces.Loans;
using Application.Utils;
using ErrorOr;

namespace Application.Features.LoanRequests.Command.AddDescription;

public sealed class AddDescriptionCommandHandler(ILoanRequestService loanRequestService) : IRequestHandler<AddDescriptionCommand, ErrorOr<string>>
{
    public async Task<ErrorOr<string>> Handle(AddDescriptionCommand command, CancellationToken cancellationToken)
    {

        var model = command.Request.MapToDescriptionCreate();

        return await loanRequestService.AddDescription(model);

    }
}
