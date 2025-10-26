using Applications.Responses.ViewModels.Customers;
using Applications.Utils;
using ErrorOr;

namespace Applications.Features.Customers.Commands.IncomeCommands.Queries.GetIncomeByCustomerId;

public sealed record GetIncomeByCustomerQuery(string CustomerId) : IRequest<ErrorOr<IEnumerable<VCustomerIncomes>>>;
