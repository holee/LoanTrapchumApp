using Applications.Responses.ViewModels.Customers;
using Applications.Utils;

namespace Applications.Features.Customers.Commands.CustomerExpenseQuery;

public sealed class GetCustomerExpenseQuery : IRequest<IEnumerable<VCustomerExpenses>>
{
    public string? CustomerId { get; set; }

}
