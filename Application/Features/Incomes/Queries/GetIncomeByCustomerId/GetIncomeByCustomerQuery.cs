using Application.Responses.ViewModels.Customers;
using Application.Utils;
using ErrorOr;

namespace Application.Features.Incomes.Queries.GetIncomeByCustomerId;

public sealed record GetIncomeByCustomerQuery(string CustomerId) : IRequest<ErrorOr<IEnumerable<VCustomerIncomes>>>;
