using Applications.Responses.ViewModels.Customers;
using Applications.Utils;
using ErrorOr;

namespace Applications.Features.Incomes.Queries.GetIncomeById;

public sealed record GetIncomeByIdQuery(long Id, string CustomerId) : IRequest<ErrorOr<VCustomerIncomes>>;
