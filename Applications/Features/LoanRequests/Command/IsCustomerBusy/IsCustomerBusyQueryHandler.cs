using Applications.Interfaces.Loans;
using Applications.Utils;
using ErrorOr;

namespace Applications.Features.LoanRequests.Command.IsCustomerBusy;

public sealed class IsCustomerBusyQueryHandler(ILoanRequestService _loanRequestService) : IRequestHandler<IsCustomerBusyQuery, ErrorOr<bool>>
{
    public async Task<ErrorOr<bool>> Handle(IsCustomerBusyQuery request, CancellationToken cancellationToken)
    {
        var isBusy = await _loanRequestService.IsCustomerBusy(request.CustomerId, cancellationToken);
        return isBusy;
    }
}