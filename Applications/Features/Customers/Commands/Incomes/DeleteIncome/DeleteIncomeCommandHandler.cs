using Applications.Interfaces.Customers;
using Applications.Utils;
using ErrorOr;

namespace Applications.Features.Customers.Commands.Incomes.DeleteIncome;

public sealed class DeleteIncomeCommandHandler(ICustomerIncomeExpendService expendService)
    : IRequestHandler<DeleteIncomeCommand, ErrorOr<bool>>
{

    public Task<ErrorOr<bool>> Handle(DeleteIncomeCommand request, CancellationToken cancellationToken = default)
    {
        return expendService.DeleteIncomeAsync(request.Id,request.CustomerId, cancellationToken);
    }
}