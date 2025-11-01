using Applications.Responses.ViewModels.Customers;
using Applications.Utils;
using ErrorOr;

namespace Applications.Features.Incomes.Queries.GetIncomeByCustomerId;

public sealed record GetIncomeByCustomerQuery(string CustomerId) : IRequest<ErrorOr<IEnumerable<VCustomerIncomes>>>;
