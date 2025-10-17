using Applications.Responses.ViewModels.Customers;
using Applications.Utils;

namespace Applications.Features.Customers.Commands.CustomerIncomeQuery;

public sealed class GetCustomerIncomeQuery : IRequest<IEnumerable<VCustomerIncomes>> 
{
    public string? CustomerId { get; set; }

}
