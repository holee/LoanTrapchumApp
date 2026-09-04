using Application.Interfaces.Customers;
using Application.Utils;
using ErrorOr;

namespace Application.Features.Incomes.Command.DeleteIncome;

public sealed class DeleteIncomeCommandHandler(ICustomerIncomeExpendService expendService)
    : IRequestHandler<DeleteIncomeCommand, ErrorOr<bool>>
{

    public Task<ErrorOr<bool>> Handle(DeleteIncomeCommand request, CancellationToken cancellationToken = default)
    {
        return expendService.DeleteIncomeAsync(request.Id,request.CustomerId, cancellationToken);
    }
}