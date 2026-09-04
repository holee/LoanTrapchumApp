using Application.Commons.Mappers;
using Application.Interfaces.Loans;
using Application.Utils;
using ErrorOr;

namespace Application.Features.LoanRequests.Command.AddLoanRequestDescription;

public sealed class AddLoanRequestDescriptionCommandHandler(ILoanRequestService reqService)
    : IRequestHandler<AddLoanRequestDescriptionCommand, ErrorOr<string>>
{
    public async Task<ErrorOr<string>> Handle(AddLoanRequestDescriptionCommand request, CancellationToken cancellationToken)
    {
        var model = request.Req.MapToDescriptionCreate();

        return await reqService.AddDescription(model, cancellationToken);
    }
}
